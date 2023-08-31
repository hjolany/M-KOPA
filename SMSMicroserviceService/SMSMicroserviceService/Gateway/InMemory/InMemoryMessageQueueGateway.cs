using MediatR;
using SMSMicroService.Gateway.Base;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Notifications;
using System.Collections.Concurrent;

namespace SMSMicroService.Gateway.InMemory
{
    public class InMemoryMessageQueueGateway<T>: BaseInMemoryMessageQueueGateway<T>, IInMemoryMessageQueueGateway<T>
        where T : class
    {
        public InMemoryMessageQueueGateway(ILogger<InMemoryMessageQueueGateway<T>> logger
            , IMediator mediator) : base(logger, mediator)
        {
            OnMessage += InMemoryMessageQueueGateway_OnMessage;
        }

        private void InMemoryMessageQueueGateway_OnMessage(object? sender, Entities.Domains.Interfaces.IMessageReceivedArgumentDomain<T> e)
        {
            Mediator.Publish(new SendSmsAndPublishNotification<T>(e.Data));
        }
    }
}
