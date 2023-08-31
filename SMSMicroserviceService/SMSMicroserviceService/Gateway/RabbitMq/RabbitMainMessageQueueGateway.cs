using MediatR;
using RabbitMQ.Client;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway.Base;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Notifications;

namespace SMSMicroService.Gateway.RabbitMq
{
    public class RabbitMainMessageQueueGateway<T> : BaseRabbitMessageQueueGateway<T>, IRabbitMainMessageQueueGateway<T>
        where T : class
    {
        public RabbitMainMessageQueueGateway(string queueName
            , IConnection connection
            , IMediator mediator
            , ILogger<BaseRabbitMessageQueueGateway<T>> logger)
            : base(queueName, connection, mediator, logger)
        {
            this.OnMessage += RabbitMainMessageQueueGateway_OnMessage;
        }

        private void RabbitMainMessageQueueGateway_OnMessage(object? sender, Entities.Domains.Interfaces.IMessageReceivedArgumentDomain<T> e)
        {
            Mediator.Publish(new SendSmsAndPublishNotification<T>(e.Data));
            Channel.BasicAck(((RabbitMessageReceivedArgumentDomain<T>)e).Delivery.DeliveryTag, multiple: false);
        }
    }
}
