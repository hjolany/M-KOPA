using MediatR;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Factories;
using SMSMicroService.Gateway;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Helpers;
using SMSMicroService.Helpers.Interfaces;
using SMSMicroService.Infrastructures;
using SMSMicroService.Infrastructures.Enums;
using SMSMicroService.Notifications;

namespace SMSMicroService.Handlers
{
    public class MessageDequeueHandler : INotificationHandler<SendSmsAndPublishNotification<MessageDomain>>
    {
        private readonly IMessageGateway _messageGateway;
        private readonly IEventBusGateway<string> _eventBusGateway;
        private readonly ICallApi<MessageDomain> _callApi;
        private readonly IRabbitDeadLetterMessageQueueGateway<MessageDomain?> _rabbitDeadLetterMessageQueueGateway;
        private readonly ILogger<MessageDequeueHandler> _logger;

        public MessageDequeueHandler(IMessageGateway messageGateway
            , IEventBusGateway<string> eventBusGateway
            , ICallApi<MessageDomain> callApi
            , IRabbitDeadLetterMessageQueueGateway<MessageDomain?> rabbitDeadLetterMessageQueueGateway
            , ILogger<MessageDequeueHandler> logger)
        {
            _messageGateway = messageGateway;
            _eventBusGateway = eventBusGateway;
            _callApi = callApi;
            _rabbitDeadLetterMessageQueueGateway = rabbitDeadLetterMessageQueueGateway;
            _logger = logger;
        }
        public async Task Handle(SendSmsAndPublishNotification<MessageDomain> notification, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _messageGateway.Add(notification.Data.ToModel(retryCount: 1)).ConfigureAwait(false);

                var response = await
                    _callApi.Post(AppConfig.Get("Settings:SmsAPiUrl1").ToString(), notification.Data).ConfigureAwait(false);

                if (response?.IsSuccessStatusCode ?? false)
                {
                    entity.Status = EStatus.Success;
                    await _eventBusGateway.Publish("SmsSent").ConfigureAwait(false);
                }
                else
                {
                    await _rabbitDeadLetterMessageQueueGateway.EnQueue(notification.Data).ConfigureAwait(false);
                    entity.Status = EStatus.Failed;
                    entity.Exception = await response?.Content.ReadAsStringAsync();
                    _logger.LogError($"External API Error: {entity.Exception}");
                }
                await _messageGateway.Update(entity).ConfigureAwait(false);
                _logger.LogInformation($"{DateTime.Now:HH:mm:ss}\t{notification.Data}");
            }
            catch (CriticalException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
