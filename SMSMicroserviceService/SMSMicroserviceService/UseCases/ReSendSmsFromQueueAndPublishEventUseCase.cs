using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.UseCases.Interfaces;

namespace SMSMicroService.UseCases
{
    public class ReSendSmsFromQueueAndPublishEventUseCase : IReSendSmsFromQueueAndPublishEventUseCase
    {
        private readonly IRabbitDeadLetterMessageQueueGateway<MessageDomain?> _queueGateway;
        private readonly IInMemoryMessageQueueGateway<MessageDomain> _inMemoryMessageQueueGateway;

        public ReSendSmsFromQueueAndPublishEventUseCase(IRabbitDeadLetterMessageQueueGateway<MessageDomain?> queueGateway,
            IInMemoryMessageQueueGateway<MessageDomain> inMemoryMessageQueueGateway)
        {
            _queueGateway = queueGateway;
            _inMemoryMessageQueueGateway = inMemoryMessageQueueGateway;
        }
        public async Task ExecuteAsync()
        {
            /*await _queueGateway.DeQueue().ConfigureAwait(false);*/
            await _inMemoryMessageQueueGateway.DeQueue().ConfigureAwait(false);
        }
    }
}