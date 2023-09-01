using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway;
using SMSMicroService.Gateway.RabbitMq;
using SMSMicroService.Helpers;

namespace SMSMicroService.Tests.Utilities
{
    public class RabbitMessageQueueHelper
    {
        private readonly Fixture _fixture;
        private readonly RabbitMainMessageQueueGateway<MessageDomain> _mainGateway;

        public RabbitMessageQueueHelper(string queueName)
        {
            var uri = AppConfig.Get("Queue:Uri");
            _fixture = new Fixture();
            IConnectionFactory factory = new ConnectionFactory()
            {
                Uri = new Uri(uri)
            };
            var connection = factory.CreateConnection();
            Mock<ILogger<RabbitMainMessageQueueGateway<MessageDomain>>> mainLogger = new();
            Mock<IMediator> mediator = new();
            _mainGateway = new RabbitMainMessageQueueGateway<MessageDomain>(queueName
                , connection
                , mediator.Object
                , mainLogger.Object);
        }

        public void FillMainQueue()
        {
            var fillMq = new SMSMicroService.Controllers.QueueApiController(_mainGateway, new MessageGateway());
            fillMq.Send(_fixture.Create<MessageDomain>());
        }
    }
}
