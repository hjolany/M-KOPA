using Microsoft.Extensions.Logging;
using Moq;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Handlers;
using SMSMicroService.Helpers;
using SMSMicroService.Helpers.Interfaces;
using SMSMicroService.Notifications;

namespace SMSMicroService.Tests.Handlers
{
    public class PromptStringHandlerTests
    {
        private readonly PromptStringHandler _sut;
        private readonly Mock<ILogger<PromptStringHandler>> _logger;
        private readonly Mock<IEmailHelper> _emailHelper;

        public PromptStringHandlerTests()
        {
            _emailHelper = new Mock<IEmailHelper>();
            _logger = new Mock<ILogger<PromptStringHandler>>();
            _sut = new PromptStringHandler(_logger.Object, _emailHelper.Object);
        }

        [Fact]
        public async Task HandlerSendsEmailAndWorksFine()
        {
            // Arrange
            var recipient = AppConfig.Get("Recipients:Admin").ToString();
            var sampleMessage = "Sample Message";
            var notification = new PromptNotification<string>(sampleMessage);
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
                        email.Body.Contains(sampleMessage))),
                Times.Once);
        }

        [Fact]
        public async Task HandlerLogExceptionWhenEmailHelperThrowsAnyException()
        {
            // Arrange
            var recipient = AppConfig.Get("Recipients:Admin").ToString();
            var sampleMessage = "Sample Message";
            var emailHelperException = new Exception("Internal Exception");
            var notification = new PromptNotification<string>(sampleMessage);
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
                        email.Body.Contains(sampleMessage))),
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
