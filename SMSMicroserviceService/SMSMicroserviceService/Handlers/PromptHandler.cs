using MediatR;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Helpers;
using SMSMicroService.Infrastructures;
using SMSMicroService.Infrastructures.Extensions;
using SMSMicroService.Notifications;

namespace SMSMicroService.Handlers
{
    public class PromptExceptionHandler : INotificationHandler<PromptNotification<Exception>>
    {
        private readonly ILogger<PromptExceptionHandler> _logger;

        public PromptExceptionHandler(ILogger<PromptExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async Task Handle(PromptNotification<Exception> notification, CancellationToken cancellationToken)
        {
            try
            {
                var email = new EmailDomain()
                {
                    Email = new List<string>()
                {
                    AppConfig.Get("Recipients:Admin").ToString()
                },
                    Body = notification.Data.ToString(),
                    Subject = "Prompt"
                };

                EmailHelper.Send(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetFullMessage());
            }
        }
    }

    public class PromptCriticalExceptionHandler : INotificationHandler<PromptNotification<CriticalException>>
    {
        private readonly ILogger<PromptCriticalExceptionHandler> _logger;

        public PromptCriticalExceptionHandler(ILogger<PromptCriticalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(PromptNotification<CriticalException> notification, CancellationToken cancellationToken)
        {
            try
            {
                var email = new EmailDomain()
                {
                    Email = new List<string>()
                {
                    AppConfig.Get("Recipients:Admin").ToString()
                },
                    Body = notification.Data.ToString(),
                    Subject = "Prompt"
                };

                EmailHelper.Send(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetFullMessage());
            }
        }
    }

    public class PromptStringHandler : INotificationHandler<PromptNotification<string>>
    {
        private readonly ILogger<PromptStringHandler> _logger;

        public PromptStringHandler(ILogger<PromptStringHandler> logger)
        {
            _logger = logger;
        }
        public async Task Handle(PromptNotification<string> notification, CancellationToken cancellationToken)
        {
            try
            {
                var email = new EmailDomain()
                {
                    Email = new List<string>()
                {
                    AppConfig.Get("Recipients:Admin").ToString()
                },
                    Body = notification.Data,
                    Subject = "Prompt"
                };

                EmailHelper.Send(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetFullMessage());
            }
        }
    }
}
