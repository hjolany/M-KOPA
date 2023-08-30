using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using SMSMicroService.Notifications;
using SMSMicroService.Services;
using SMSMicroService.UseCases.Interfaces;

namespace SMSMicroService.Tests.UnitTests.Services
{

    public class MainQueueServiceTest
    {
        private readonly Mock<ISendSmsFromQueueAndPublishEventUseCase> _sendSmsFromQueueAndPublishEventUseCase;
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<MainQueueService>> _logger;
        BackgroundService _sut;

        public MainQueueServiceTest()
        {
            _sendSmsFromQueueAndPublishEventUseCase = new Mock<ISendSmsFromQueueAndPublishEventUseCase>();
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<MainQueueService>>();
            _sut = new MainQueueService(_sendSmsFromQueueAndPublishEventUseCase.Object, _mediator.Object, _logger.Object);
        }

        [Fact]
        public void ExecuteAsyncWithNoIssues()
        {
            // Arrange

            // Act
            _sut.StartAsync(CancellationToken.None);

            // Assert
            _sendSmsFromQueueAndPublishEventUseCase.Verify(x => x.ExecuteAsync(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsyncWhenThrowsExceptionThenLogAndPrompt()
        {
            // Arrange
            _sendSmsFromQueueAndPublishEventUseCase.Setup(x => x.ExecuteAsync())
                .ThrowsAsync(new Exception());

            // Act & Assert
            /*var exception = await Assert.ThrowsAsync<Exception>(() => );*/
            await _sut.StartAsync(new CancellationToken());

            _logger.Verify(x => x.Log(
                LogLevel.Critical,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);

            _mediator.Verify(x => x.Publish(It.IsAny<PromptNotification<Exception>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}