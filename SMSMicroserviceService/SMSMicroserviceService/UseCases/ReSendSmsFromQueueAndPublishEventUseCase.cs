using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.UseCases.Interfaces;

namespace SMSMicroService.UseCases
{
    public class ReSendSmsFromQueueAndPublishEventUseCase : IReSendSmsFromQueueAndPublishEventUseCase
    {
        private readonly IDeadLetterQueueGateway<MessageDomain> _queueGateway;

        public ReSendSmsFromQueueAndPublishEventUseCase(IDeadLetterQueueGateway<MessageDomain> queueGateway)
        {
            _queueGateway = queueGateway;
        }
        public async Task ExecuteAsync()
        {
            await _queueGateway.DeQueue().ConfigureAwait(false);
        }
    }
}