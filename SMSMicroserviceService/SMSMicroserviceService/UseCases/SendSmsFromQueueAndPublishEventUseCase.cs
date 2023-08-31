using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.UseCases.Interfaces;

namespace SMSMicroService.UseCases
{
    public class SendSmsFromQueueAndPublishEventUseCase: ISendSmsFromQueueAndPublishEventUseCase
    {
        private readonly IMessageQueueGateway<MessageDomain> _messageQueueGateway;

        public SendSmsFromQueueAndPublishEventUseCase(IMessageQueueGateway<MessageDomain> messageQueueGateway)
        {
            _messageQueueGateway = messageQueueGateway;
        }
        public async Task ExecuteAsync()
        {
            await _messageQueueGateway.DeQueue().ConfigureAwait(false);
        }
    }
}