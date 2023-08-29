using MediatR;
using RabbitMQ.Client;
using SMSMicroService.Gateway.Base;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Notifications;

namespace SMSMicroService.Gateway
{
    public class RabbitMainMessageQueueGateway<T> : BaseRabbitMessageQueueGateway<T>, IRabbitMainMessageQueueGateway<T>
        where T : class
    {
        public RabbitMainMessageQueueGateway(string queueName
            , IConnection connection
            , IMediator mediator
            , ILogger<RabbitMainMessageQueueGateway<T>> logger)
            : base(queueName, connection, mediator, logger)
        {
            this.OnMessage += RabbitMainMessageQueueGateway_OnMessage;
        }

        private void RabbitMainMessageQueueGateway_OnMessage(object? sender, Entities.Domains.RabbitMessageReceivedArgumentDomain<T> e)
        {
            Mediator.Publish(new SendSmsAndPublishNotification<T>(e.Data));
            Channel.BasicAck(e.Delivery.DeliveryTag, multiple: false);
        }
    }
}
