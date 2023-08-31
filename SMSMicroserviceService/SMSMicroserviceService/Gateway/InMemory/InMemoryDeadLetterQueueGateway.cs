using MediatR;
using SMSMicroService.Gateway.Base;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Notifications;

namespace SMSMicroService.Gateway.InMemory
{
    public class InMemoryDeadLetterQueueGateway<T>: BaseInMemoryMessageQueueGateway<T>, IInMemoryDeadLetterQueueGateway<T>
        where T : class
    {
        public InMemoryDeadLetterQueueGateway(ILogger<BaseInMemoryMessageQueueGateway<T>> logger
            , IMediator mediator) : base(logger, mediator)
        {
            OnMessage += InMemoryMessageQueueGateway_OnMessage;
        }

        private void InMemoryMessageQueueGateway_OnMessage(object? sender, Entities.Domains.Interfaces.IMessageReceivedArgumentDomain<T> e)
        {
            Mediator.Publish(new ReSendSmsAndPublishNotification<T>(e.Data));
        }
    }
}
