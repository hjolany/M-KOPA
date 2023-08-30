using Microsoft.Extensions.Logging;
using Moq;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Handlers;
using SMSMicroService.Helpers;
using SMSMicroService.Helpers.Interfaces;
using SMSMicroService.Infrastructures.Extensions;
using SMSMicroService.Notifications;

namespace SMSMicroService.Tests.UnitTests.Handlers
{
    public class PromptExceptionHandlerTests
    {
        private readonly PromptExceptionHandler _sut;
        private readonly Mock<ILogger<PromptExceptionHandler>> _logger;
        private readonly Mock<IEmailHelper> _emailHelper;

        public PromptExceptionHandlerTests()
        {
            _emailHelper = new Mock<IEmailHelper>();
            _logger = new Mock<ILogger<PromptExceptionHandler>>();
            _sut = new PromptExceptionHandler(_logger.Object, _emailHelper.Object);
        }

        [Fact]
        public async Task HandlerSendsEmailAndWorksFine()
        {
            // Arrange
            var recipient = AppConfig.Get("Recipients:Admin").ToString();
            var exception = new Exception("Sample Exception");
            var notification = new PromptNotification<Exception>(exception);
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
            var exception = new Exception("Sample Exception");
            var emailHelperException = new Exception("Internal Exception");
            var notification = new PromptNotification<Exception>(exception);
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
