using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.UseCases.Interfaces;

namespace SMSMicroService.UseCases
{
    public class SendSmsFromQueueAndPublishEventUseCase: ISendSmsFromQueueAndPublishEventUseCase
    {
        private readonly IRabbitMainMessageQueueGateway<MessageDomain?> _queueGateway;

        public SendSmsFromQueueAndPublishEventUseCase(IRabbitMainMessageQueueGateway<MessageDomain?> queueGateway)
        {
            _queueGateway = queueGateway;
        }
        public async Task ExecuteAsync()
        {
            await _queueGateway.DeQueue().ConfigureAwait(false);
        }
    }
}