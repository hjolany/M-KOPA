using Microsoft.Extensions.Logging;
using Moq;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Handlers;
using SMSMicroService.Helpers;
using SMSMicroService.Helpers.Interfaces;
using SMSMicroService.Infrastructures;
using SMSMicroService.Infrastructures.Extensions;
using SMSMicroService.Notifications;

namespace SMSMicroService.Tests.UnitTests.Handlers
{
    public class PromptCriticalExceptionHandlerTests
    {
        private readonly PromptCriticalExceptionHandler _sut;
        private readonly Mock<ILogger<PromptCriticalExceptionHandler>> _logger;
        private readonly Mock<IEmailHelper> _emailHelper;

        public PromptCriticalExceptionHandlerTests()
        {
            _emailHelper = new Mock<IEmailHelper>();
            _logger = new Mock<ILogger<PromptCriticalExceptionHandler>>();
            _sut = new PromptCriticalExceptionHandler(_logger.Object, _emailHelper.Object);
        }

        [Fact]
        public async Task HandlerSendsEmailAndWorksFine()
        {
            // Arrange
            var recipient = AppConfig.Get("Recipients:Admin").ToString();
            var exception = new CriticalException("Sample Exception");
            var notification = new PromptNotification<CriticalException>(exception);
            _emailHelper.Setup(x => x.Send(It.IsAny<EmailDomain>()))
                .ReturnsAsync(true);

            // Act
            await _sut.Handle(notification, CancellationToken.None);

            // Assert
            _emailHelper.Verify(
                emailHelper => emailHelper.Send(
                    It.Is<EmailDomain>(email =>
                        email.Subject == "Prompt" &&
                        email.Email.Contains(recipient) &&
                        email.Body.Contains(exception.GetFullMessage()))),
                Times.Once);
        }

        [Fact]
        public async Task HandlerLogExceptionWhenEmailHelperThrowsAnyException()
        {
            // Arrange
            var recipient = AppConfig.Get("Recipients:Admin").ToString();
            var exception = new CriticalException("Sample Exception");
            var emailHelperException = new Exception("Internal Exception");
            var notification = new PromptNotification<CriticalException>(exception);
            _emailHelper.Setup(x => x.Send(It.IsAny<EmailDomain>()))
                .ThrowsAsync(emailHelperException);

            // Act
            await _sut.Handle(notification, CancellationToken.None);

            // Assert
            _emailHelper.Verify(
                emailHelper => emailHelper.Send(
                    It.Is<EmailDomain>(email =>
                        email.Subject == "Prompt" &&
                        email.Email.Contains(recipient) &&
                        email.Body.Contains(exception.GetFullMessage()))),
                Times.Once);

            _logger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);

        }

    }
}
