using MediatR;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Helpers;
using SMSMicroService.Helpers.Interfaces;
using SMSMicroService.Infrastructures;
using SMSMicroService.Infrastructures.Extensions;
using SMSMicroService.Notifications;

namespace SMSMicroService.Handlers
{
    public class PromptExceptionHandler : INotificationHandler<PromptNotification<Exception>>
    {
        private readonly ILogger<PromptExceptionHandler> _logger;
        private readonly IEmailHelper _emailHelper;

        public PromptExceptionHandler(ILogger<PromptExceptionHandler> logger, IEmailHelper emailHelper)
        {
            _logger = logger;
            _emailHelper = emailHelper;
        }
        public async Task Handle(PromptNotification<Exception> notification, CancellationToken cancellationToken)
        {
            try
            {
                var email = new EmailDomain()
                {
                    Email = new List<string>()
                {
                    AppConfig.Get("Recipients:Admin")
                },
                    Body = notification.Data.GetFullMessage(),
                    Subject = "Prompt"
                };

                await _emailHelper.Send(email);
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
        private readonly IEmailHelper _emailHelper;

        public PromptCriticalExceptionHandler(ILogger<PromptCriticalExceptionHandler> logger, IEmailHelper emailHelper)
        {
            _logger = logger;
            _emailHelper = emailHelper;
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
                    Body = notification.Data.GetFullMessage(),
                    Subject = "Prompt"
                };

                await _emailHelper.Send(email);
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
        private readonly IEmailHelper _emailHelper;

        public PromptStringHandler(ILogger<PromptStringHandler> logger, IEmailHelper emailHelper)
        {
            _logger = logger;
            _emailHelper = emailHelper;
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

                await _emailHelper.Send(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetFullMessage());
            }
        }
    }
}
