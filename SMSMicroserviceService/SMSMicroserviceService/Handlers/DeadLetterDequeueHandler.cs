using MediatR;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Factories;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Helpers;
using SMSMicroService.Helpers.Interfaces;
using SMSMicroService.Infrastructures;
using SMSMicroService.Infrastructures.Enums;
using SMSMicroService.Notifications;

namespace SMSMicroService.Handlers
{
    public class DeadLetterDequeueHandler : INotificationHandler<ReSendSmsAndPublishNotification<MessageDomain>>
    {
        private readonly IMessageGateway _messageGateway;
        private readonly IEventBusGateway<string> _eventBusGateway;
        private readonly ICallApi<MessageDomain> _callApi;
        private readonly ILogger<DeadLetterDequeueHandler> _logger;

        public DeadLetterDequeueHandler(IMessageGateway messageGateway
            , IEventBusGateway<string> eventBusGateway
            , ICallApi<MessageDomain> callApi
            , ILogger<DeadLetterDequeueHandler> logger)
        {
            _messageGateway = messageGateway;
            _eventBusGateway = eventBusGateway;
            _callApi = callApi;
            _logger = logger;
        }
        public async Task Handle(ReSendSmsAndPublishNotification<MessageDomain> notification, CancellationToken cancellationToken)
        {
            try
            {
                if (notification?.Data != null)
                {
                    var entity = await _messageGateway.Add(notification.Data.ToModel(retryCount: 2))
                        .ConfigureAwait(false);

                    var response = await
                        _callApi.Post(AppConfig.Get("Settings:SmsAPiUrl2").ToString(), notification.Data)
                            .ConfigureAwait(false);

                    if (response?.IsSuccessStatusCode ?? false)
                    {
                        entity.Status = EStatus.Success;
                        await _eventBusGateway.Publish("SmsSent").ConfigureAwait(false);
                    }
                    else
                    {
                        entity.Status = EStatus.Failed;
                        entity.Exception = await response?.Content.ReadAsStringAsync();
                        _logger.LogError($"External API Error: {entity.Exception}");
                    }

                    await _messageGateway.Update(entity).ConfigureAwait(false);
                    _logger.LogInformation($"{DateTime.Now:HH:mm:ss}\t{notification.Data}");
                }
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
