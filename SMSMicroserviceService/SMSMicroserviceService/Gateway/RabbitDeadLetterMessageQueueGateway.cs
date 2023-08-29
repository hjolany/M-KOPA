using MediatR;
using RabbitMQ.Client;
using SMSMicroService.Gateway.Base;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Notifications;

namespace SMSMicroService.Gateway
{
    public class RabbitDeadLetterMessageQueueGateway<T> : BaseRabbitMessageQueueGateway<T>, IRabbitDeadLetterMessageQueueGateway<T>
    where T : class
    {
        public RabbitDeadLetterMessageQueueGateway(string queueName
            , IConnection connection
            , IMediator mediator
            , ILogger<RabbitDeadLetterMessageQueueGateway<T>> logger)
            : base(queueName, connection, mediator, logger)
        {
            OnMessage += RabbitDeadLetterMessageQueueGateway_OnMessage;
        }

        private void RabbitDeadLetterMessageQueueGateway_OnMessage(object? sender, Entities.Domains.RabbitMessageReceivedArgumentDomain<T> e)
        {
            Mediator.Publish(new ReSendSmsAndPublishNotification<T>(e.Data));
            Channel.BasicAck(e.Delivery.DeliveryTag, multiple: false);
        }
    }
}
