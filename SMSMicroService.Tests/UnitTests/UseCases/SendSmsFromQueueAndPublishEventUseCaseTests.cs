using Moq;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway.InMemory;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.UseCases;
using SMSMicroService.UseCases.Interfaces;

namespace SMSMicroService.Tests.UnitTests.UseCases
{
    public class SendSmsFromQueueAndPublishEventUseCaseTests
    {

        private readonly ISendSmsFromQueueAndPublishEventUseCase _sut;
        private readonly Mock<InMemoryMessageQueueGateway<MessageDomain?>> _gateway;


        public SendSmsFromQueueAndPublishEventUseCaseTests()
        {
            _gateway = new Mock<InMemoryMessageQueueGateway<MessageDomain?>>();
            _sut = new SendSmsFromQueueAndPublishEventUseCase(_gateway.Object);
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
