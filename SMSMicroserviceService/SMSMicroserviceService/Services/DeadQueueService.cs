using MediatR;
using SMSMicroService.Infrastructures.Extensions;
using SMSMicroService.Notifications;
using SMSMicroService.UseCases.Interfaces;

namespace SMSMicroService.Services
{
    public class DeadQueueService : BackgroundService
    {
        private readonly IReSendSmsFromQueueAndPublishEventUseCase _reSendSmsFromQueueAndPublishEventUseCase;
        private readonly IMediator _mediator;
        private readonly ILogger<DeadQueueService> _logger;

        public DeadQueueService(IReSendSmsFromQueueAndPublishEventUseCase reSendSmsFromQueueAndPublishEventUseCase
        , IMediator mediator
        , ILogger<DeadQueueService> logger)
        {
            _reSendSmsFromQueueAndPublishEventUseCase = reSendSmsFromQueueAndPublishEventUseCase;
            _mediator = mediator;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _reSendSmsFromQueueAndPublishEventUseCase.ExecuteAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            { 
                _logger.LogCritical($"Main service was stopped" +
                                    $"\r\n======================" +
                                    $"{e.GetFullMessage()}" +
                                    $"================================");
                await _mediator.Publish(new PromptNotification<Exception>(e)); 
            }
        }
    }
}
