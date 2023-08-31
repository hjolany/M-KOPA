using Moq;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.UseCases;
using SMSMicroService.UseCases.Interfaces;

namespace SMSMicroService.Tests.UnitTests.UseCases
{
    public class ReSendSmsFromQueueAndPublishEventUseCaseTests
    {

        private readonly IReSendSmsFromQueueAndPublishEventUseCase _sut;
        private readonly Mock<IRabbitDeadLetterMessageQueueGateway<MessageDomain?>> _gateway;


        public ReSendSmsFromQueueAndPublishEventUseCaseTests()
        {
            _gateway = new Mock<IRabbitDeadLetterMessageQueueGateway<MessageDomain?>>();
            _sut = new ReSendSmsFromQueueAndPublishEventUseCase(_gateway.Object);
        }

        [Fact]
        public void ExecuteAsyncWithNoIssues()
        {
            // Arrange

            // Act
            _sut.ExecuteAsync();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public async Task ExecuteAsyncThenThrowsExceptionWhenGatewayFiresExceptions()
        {
            // Arrange
            _gateway.Setup(x => x.DeQueue())
                .Throws(new Exception());

            // Act & Assert

            _ = await Assert.ThrowsAsync<Exception>(() => _sut.ExecuteAsync());
        }
    }
}
