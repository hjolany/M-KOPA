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
    public class FillMessageQueue
    {
        private readonly Fixture _fixture;
        private readonly IConnection _connection;
        private readonly IConnectionFactory _factory;
        private readonly RabbitMainMessageQueueGateway<MessageDomain?> mainGateway;
        private readonly Mock<ILogger<RabbitMainMessageQueueGateway<MessageDomain>>> _mainLogger;
        private readonly Mock<IMediator> _mediator;
        private readonly string uri;
        private string _queueName;

        public FillMessageQueue(string queueName)
        {
            _queueName = queueName;
            uri = AppConfig.Get("Queue:Uri");
            _fixture = new Fixture();
            _factory = new ConnectionFactory()
            {
                Uri = new Uri(uri)
            };
            _connection = _factory.CreateConnection();
            _mainLogger = new Mock<ILogger<RabbitMainMessageQueueGateway<MessageDomain>>>();
            _mediator = new Mock<IMediator>();
            mainGateway = new RabbitMainMessageQueueGateway<MessageDomain?>(_queueName
                , _connection
                , _mediator.Object
                , _mainLogger.Object);
        }

        public void FillMainQueue()
        {
            var fillMq = new SMSMicroService.Controllers.QueueApiController(mainGateway, new MessageGateway());
            fillMq.Send(_fixture.Create<MessageDomain>());
        }
    }
}
