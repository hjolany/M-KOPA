using MediatR;
using SMSMicroService.Infrastructures.Extensions;
using SMSMicroService.Notifications;
using SMSMicroService.UseCases.Interfaces;

namespace SMSMicroService.Services
{
    public class MainQueueService : BackgroundService
    {
        private readonly ISendSmsFromQueueAndPublishEventUseCase _sendSmsFromQueueAndPublishEventUseCase;
        private readonly IMediator _mediator;
        private readonly ILogger<MainQueueService> _logger;

        public MainQueueService(ISendSmsFromQueueAndPublishEventUseCase sendSmsFromQueueAndPublishEventUseCase
        , IMediator mediator
        , ILogger<MainQueueService> logger)
        {
            _sendSmsFromQueueAndPublishEventUseCase = sendSmsFromQueueAndPublishEventUseCase;
            _mediator = mediator;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _sendSmsFromQueueAndPublishEventUseCase.ExecuteAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            { 
                _logger.LogCritical($"Main service was stopped" +
                                    $"\r\n======================" +
                                    $"{e.GetFullMessage()}" +
                                    $"================================");
                await _mediator.Publish(new PromptNotification<Exception>(e), stoppingToken); 
            }
        }
    }
}
