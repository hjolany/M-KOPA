using AutoFixture;
using RabbitMQ.Client;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway;
using SMSMicroService.Helpers;
using SMSMicroService.Infrastructures;

namespace SMSMicroService.Tests.Gateway
{
    public class RabbitMqEventBusGatewayTests
    {

        private readonly IConnection _connection;
        private readonly Fixture _fixture;
        private readonly RabbitMqEventBusGateway<MessageDomain> _sut;

        public RabbitMqEventBusGatewayTests()
        {
            _fixture = new Fixture();
            var uri = AppConfig.Get("Queue:Uri");
            var exchangeName = AppConfig.Get("Queue:ExchangeName");
            var routingKey = AppConfig.Get("Queue:RoutingKey");
            IConnectionFactory factory = new ConnectionFactory()
            {
                Uri = new Uri(uri)
            };
            _connection = factory.CreateConnection();
            _sut = new RabbitMqEventBusGateway<MessageDomain>(exchangeName, routingKey, _connection);
        }


        [Fact]
        public async Task PublishWorksFine()
        {
            // Arrange
            var data = _fixture.Create<MessageDomain>();
            // Act
            await _sut.Publish(data).ConfigureAwait(false);

            //Assert
            Assert.True(true);
        }
        

        [Fact]
        public async Task ThrowsCriticalExceptionWhenConnectionIsClosed()
        {
            // Arrange
            var data = _fixture.Create<MessageDomain>();
            _connection.Close();
            // Act & Assert
            var exception = Assert.ThrowsAsync<CriticalException>(async () =>
            {
                await _sut.Publish(data).ConfigureAwait(false);
            });
        }
    }
}
