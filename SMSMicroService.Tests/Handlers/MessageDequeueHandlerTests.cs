using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Entities.Models;
using SMSMicroService.Factories;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Handlers;
using SMSMicroService.Helpers.Interfaces;
using SMSMicroService.Notifications;

namespace SMSMicroService.Tests.Handlers
{
    public class MessageDequeueHandlerTests
    {
        private readonly Fixture _fixture = new();
        private readonly MessageDequeueHandler _sut;
        private readonly Mock<IMessageGateway> messageGateway;
        private readonly Mock<IEventBusGateway<string>> eventBusGateway;
        private readonly Mock<ICallApi<MessageDomain>> callApi;
        private readonly Mock<IRabbitDeadLetterMessageQueueGateway<MessageDomain>> rabbitDeadLetterMessageQueueGateway;
        private readonly Mock<ILogger<MessageDequeueHandler>> logger;

        public MessageDequeueHandlerTests()
        {
            messageGateway = new Mock<IMessageGateway>();
            eventBusGateway = new Mock<IEventBusGateway<string>>();
            callApi = new Mock<ICallApi<MessageDomain>>();
            rabbitDeadLetterMessageQueueGateway = new Mock<IRabbitDeadLetterMessageQueueGateway<MessageDomain>>();
            logger = new Mock<ILogger<MessageDequeueHandler>>();

            _sut = new MessageDequeueHandler(messageGateway.Object
                , eventBusGateway.Object
                , callApi.Object
                , rabbitDeadLetterMessageQueueGateway.Object
                , logger.Object);
        }

        [Fact]
        public async Task HandleWorksFine()
        {
            // Arrange
            var message = _fixture.Create<MessageDomain>();
            var data = new SendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK); /*_fixture.Build<HttpResponseMessage>()
                .With(x => x.StatusCode , HttpStatusCode.OK)
                .Create();*/

            messageGateway
                .Setup(x=>x.Add(It.IsAny<MessageModel>()))
                .ReturnsAsync(entity);
            callApi.Setup(x => x.Post(It.IsAny<string>(), message))
                .ReturnsAsync(httpResponse);

            // Act
            await _sut.Handle(data, It.IsAny<CancellationToken>());

            // Assert
            logger.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
    }
}
