using MediatR;
using RabbitMQ.Client;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Entities.Domains.Interfaces;
using SMSMicroService.Gateway.Base;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Notifications;

namespace SMSMicroService.Gateway.RabbitMq
{
    public class RabbitDeadLetterMessageQueueGateway<T> : BaseRabbitMessageQueueGateway<T>, IRabbitDeadLetterMessageQueueGateway<T>
    where T : class
    {
        public RabbitDeadLetterMessageQueueGateway(string queueName
            , IConnection connection
            , IMediator mediator
            , ILogger<IMessageQueueGateway<T>> logger)
            : base(queueName, connection, mediator, logger)
        {
            OnMessage += RabbitDeadLetterMessageQueueGateway_OnMessage;
        }

        private void RabbitDeadLetterMessageQueueGateway_OnMessage(object? sender, IMessageReceivedArgumentDomain<T> e)
        {
            Mediator.Publish(new ReSendSmsAndPublishNotification<T>(e.Data));
            Channel.BasicAck(((RabbitMessageReceivedArgumentDomain<T>)e).Delivery.DeliveryTag, multiple: false);
        }
    }
}
