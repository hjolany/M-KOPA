using System.Text;
using System.Threading.Channels;
using MediatR;
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Entities.Domains.Interfaces;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Infrastructures;
using SMSMicroService.Infrastructures.Extensions;
using SMSMicroService.Notifications;

namespace SMSMicroService.Gateway.Base
{
    public abstract class BaseRabbitMessageQueueGateway<T> : IMessageQueueGateway<T>
        where T : class 
    {
        public event EventHandler<IMessageReceivedArgumentDomain<T>>? OnMessage;
        private readonly string _queueName;
        private readonly ILogger<BaseRabbitMessageQueueGateway<T>> _logger;
        protected readonly IMediator Mediator;
        protected readonly IModel Channel;
        private readonly IConnection _connection;

        protected BaseRabbitMessageQueueGateway(string queueName
            , IConnection connection
            , IMediator mediator
            , ILogger<BaseRabbitMessageQueueGateway<T>> logger)
        {
            _queueName = queueName;
            Mediator = mediator;
            _connection = connection;
            Channel = _connection.CreateModel();
            _logger = logger;
            Channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public virtual async Task DeQueue()
        {
            if (!_connection.IsOpen)
                throw new ConnectionAbortedException($"{nameof(BaseRabbitMessageQueueGateway<T>)}." +
                                                     $"{nameof(BaseRabbitMessageQueueGateway<T>.DeQueue)}: " +
                                                     $"The connection to the message queue is closed.");

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += async (sender, e) =>
            {
                try
                {
                    var body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var objResult = JsonConvert.DeserializeObject<T>(message);
                    if (objResult == null)
                        return;
                    OnMessage?.Invoke(this,new RabbitMessageReceivedArgumentDomain<T>(objResult,e));
                    /*await _mediator.Publish(new SendSmsAndPublishNotification<T>(objResult));
                    _channel.BasicAck(e.DeliveryTag, multiple: false);*/
                }
                catch (CriticalException cex)
                {
                    _logger.LogCritical(cex.GetFullMessage());
                    await Mediator.Publish(new PromptNotification<CriticalException>(cex));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.GetFullMessage());
                }
            };
            Channel.BasicConsume(queue: _queueName, consumer: consumer, autoAck: false);
        }

        public async Task<int> ConsumerCount()
        {
            if (!_connection.IsOpen)
                throw new ConnectionAbortedException($"{nameof(BaseRabbitMessageQueueGateway<T>)}." +
                                                     $"{nameof(BaseRabbitMessageQueueGateway<T>.EnQueue)}: " +
                                                     $"The connection to the message queue is closed.");

            return (int)Channel.ConsumerCount(_queueName);
        } 

        public virtual async Task EnQueue(T? message)
        {
            if (!_connection.IsOpen)
                throw new ConnectionAbortedException($"{nameof(BaseRabbitMessageQueueGateway<T>)}." +
                                                     $"{nameof(BaseRabbitMessageQueueGateway<T>.EnQueue)}: " +
                                                     $"The connection to the message queue is closed.");
            
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            Channel.BasicPublish("", _queueName, null, body);
        }
    }
}
