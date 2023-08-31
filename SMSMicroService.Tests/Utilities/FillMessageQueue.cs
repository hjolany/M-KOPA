#nullable enable
using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway;
using SMSMicroService.Gateway.InMemory;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Gateway.RabbitMq;
using SMSMicroService.Helpers;

namespace SMSMicroService.Tests.Utilities
{
    public class FillMessageQueue
    {
        private readonly Fixture _fixture;
        private readonly IMessageQueueGateway<MessageDomain?> _rabbitMqGateway;
        private readonly IMessageQueueGateway<MessageDomain?> _inMemoryGateway;

        public FillMessageQueue(string queueName)
        {
            var uri = AppConfig.Get("Queue:Uri");
            _fixture = new Fixture();
            IConnectionFactory factory = new ConnectionFactory()
            {
                Uri = new Uri(uri)
            };
            var connection = factory.CreateConnection();
            var rabbitLogger = new Mock<ILogger<RabbitMainMessageQueueGateway<MessageDomain>>>();
            var inMemoryLogger = new Mock<ILogger<InMemoryMessageQueueGateway<MessageDomain>>>();
            var mediator = new Mock<IMediator>();
            _inMemoryGateway = new InMemoryMessageQueueGateway<MessageDomain?>(inMemoryLogger.Object,mediator.Object);
            _rabbitMqGateway = new RabbitMainMessageQueueGateway<MessageDomain?>(queueName
                , connection
                , mediator.Object
                , rabbitLogger.Object);
        }

        public void FillMainQueue()
        {
            var fillMq = new SMSMicroService.Controllers.QueueApiController(/*_rabbitMqGateway,*/_inMemoryGateway,new MessageGateway());
            fillMq.Send(_fixture.Create<MessageDomain>());
            //rabbitMqGateway.EnQueue(_fixture.Create<MessageDomain>());
        }
    }
}
