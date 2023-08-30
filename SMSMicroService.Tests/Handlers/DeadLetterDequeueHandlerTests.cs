using Microsoft.Extensions.Logging;
using Moq;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Handlers;
using SMSMicroService.Helpers.Interfaces;
using AutoFixture;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Entities.Models;
using SMSMicroService.Factories;
using SMSMicroService.Infrastructures;
using SMSMicroService.Notifications;
using System.Net;

namespace SMSMicroService.Tests.Handlers
{
    public class DeadLetterDequeueHandlerTests
    {
        private readonly Fixture _fixture = new();
        private readonly DeadLetterDequeueHandler _sut;
        private readonly Mock<IMessageGateway> _messageGateway;
        private readonly Mock<IEventBusGateway<string>> _eventBusGateway;
        private readonly Mock<ICallApi<MessageDomain>> _callApi;
        private readonly Mock<ILogger<DeadLetterDequeueHandler>> _logger;

        public DeadLetterDequeueHandlerTests()
        {
            _messageGateway = new Mock<IMessageGateway>();
            _eventBusGateway = new Mock<IEventBusGateway<string>>();
            _callApi = new Mock<ICallApi<MessageDomain>>();
            _logger = new Mock<ILogger<DeadLetterDequeueHandler>>();

            _sut = new DeadLetterDequeueHandler(_messageGateway.Object
                , _eventBusGateway.Object
                , _callApi.Object
                , _logger.Object);
        }

        [Fact]
        public async Task HandlerSavesEntityAndLogInfoWhenApiCallResponseIsNotOk()
        {
            // Arrange
            var message = _fixture.Create<MessageDomain>();
            var data = new ReSendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            _messageGateway
                .Setup(x => x.Add(It.IsAny<MessageModel>()))
                .ReturnsAsync(entity);
            _callApi.Setup(x => x.Post(It.IsAny<string>(), message))
                .ReturnsAsync(httpResponse);

            // Act
            await _sut.Handle(data, It.IsAny<CancellationToken>());

            // Assert
            _messageGateway.Verify(x => x.Update(entity), Times.Once);
            _logger.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }

        [Fact]
        public async Task HandlerSavesEntityAndLOgInfoWhenApiCallResponseIsOk()
        {
            // Arrange
            var message = _fixture.Create<MessageDomain>();
            var data = new ReSendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            _messageGateway
                .Setup(x => x.Add(It.IsAny<MessageModel>()))
                .ReturnsAsync(entity);
            _callApi.Setup(x => x.Post(It.IsAny<string>(), message))
                .ReturnsAsync(httpResponse);

            // Act
            await _sut.Handle(data, It.IsAny<CancellationToken>());

            // Assert
            _messageGateway.Verify(x => x.Update(entity), Times.Once);
            _logger.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }

        [Fact]
        public async Task HandlerThrowsCriticalExceptionsWHenMessageThrowsCriticalException()
        {
            // Arrange
            var message = _fixture.Create<MessageDomain>();
            var data = new ReSendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            _messageGateway
                .Setup(x => x.Add(It.IsAny<MessageModel>()))
                .ThrowsAsync(new CriticalException(It.IsAny<string>()));
            _callApi.Setup(x => x.Post(It.IsAny<string>(), message))
                .ReturnsAsync(httpResponse);

            // Act
            async Task Act() => await _sut.Handle(data, It.IsAny<CancellationToken>());

            // Assert
            await Assert.ThrowsAnyAsync<CriticalException>(Act);
        }

        [Fact]
        public async Task HandlerLogExceptionsWHenMessageThrowsException()
        {
            // Arrange
            var message = _fixture.Create<MessageDomain>();
            var data = new ReSendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            _messageGateway
                .Setup(x => x.Add(It.IsAny<MessageModel>()))
                .ThrowsAsync(new Exception(It.IsAny<string>()));

            _callApi.Setup(x => x.Post(It.IsAny<string>(), message))
                .ReturnsAsync(httpResponse);

            // Act
            await _sut.Handle(data, It.IsAny<CancellationToken>());

            // Assert
            _logger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }

        [Fact]
        public async Task HandlerCallEventPublisherWhenApiCallerReturnsResponseOk()
        {
            // Arrange
            var message = _fixture.Create<MessageDomain>();
            var data = new ReSendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            _messageGateway
                .Setup(x => x.Add(It.IsAny<MessageModel>()))
                .ReturnsAsync(entity);

            _callApi.Setup(x => x.Post(It.IsAny<string>(), message))
                .ReturnsAsync(httpResponse);

            // Act
            await _sut.Handle(data, It.IsAny<CancellationToken>());

            // Assert
            _eventBusGateway.Verify(x => x.Publish("SmsSent"), Times.Once);
        }
    }
}
