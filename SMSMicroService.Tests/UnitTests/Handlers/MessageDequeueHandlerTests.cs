using System.Net;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Entities.Models;
using SMSMicroService.Factories;
using SMSMicroService.Gateway;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Handlers;
using SMSMicroService.Helpers.Interfaces;
using SMSMicroService.Infrastructures;
using SMSMicroService.Infrastructures.Enums;
using SMSMicroService.Notifications;
using SMSMicroService.UseCases;
using SMSMicroService.UseCases.Interfaces;

namespace SMSMicroService.Tests.UnitTests.Handlers
{
    public class MessageDequeueHandlerTests
    {
        private readonly Fixture _fixture = new();
        private readonly MessageDequeueHandler _sut;
        private readonly Mock<IMessageTableGateway> messageGateway;
        private readonly Mock<IEventBusGateway<string>> eventBusGateway;
        private readonly Mock<ICallApi<MessageDomain>> callApi;
        private readonly Mock<IDeadLetterEnQueueUseCase<MessageDomain?>> _deadLetterEnQueueUseCase;
        private readonly Mock<ILogger<MessageDequeueHandler>> logger;

        public MessageDequeueHandlerTests()
        {
            messageGateway = new Mock<IMessageTableGateway>();
            eventBusGateway = new Mock<IEventBusGateway<string>>();
            callApi = new Mock<ICallApi<MessageDomain>>();
            _deadLetterEnQueueUseCase = new Mock<IDeadLetterEnQueueUseCase<MessageDomain?>>();
            logger = new Mock<ILogger<MessageDequeueHandler>>();

            _sut = new MessageDequeueHandler(messageGateway.Object
                , eventBusGateway.Object
                , callApi.Object
                , _deadLetterEnQueueUseCase.Object
                , logger.Object);
        }

        [Fact]
        public async Task HandlerSavesEntityAndLogInfoWhenApiCallResponseIsNotOk()
        {
            // Arrange
            var message = _fixture.Create<MessageDomain>();
            var data = new SendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            messageGateway
                .Setup(x => x.Add(It.IsAny<MessageModel>()))
                .ReturnsAsync(entity);
            callApi.Setup(x => x.Post(It.IsAny<string>(), message))
                .ReturnsAsync(httpResponse);

            // Act
            await _sut.Handle(data, It.IsAny<CancellationToken>());

            // Assert
            messageGateway.Verify(x => x.Update(entity), Times.Once);
            logger.Verify(x => x.Log(
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
            var data = new SendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            messageGateway
                .Setup(x => x.Add(It.IsAny<MessageModel>()))
                .ReturnsAsync(entity);
            callApi.Setup(x => x.Post(It.IsAny<string>(), message))
                .ReturnsAsync(httpResponse);

            // Act
            await _sut.Handle(data, It.IsAny<CancellationToken>());

            // Assert
            messageGateway.Verify(x => x.Update(entity), Times.Once);
            logger.Verify(x => x.Log(
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
            var data = new SendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            messageGateway
                .Setup(x => x.Add(It.IsAny<MessageModel>()))
                .ThrowsAsync(new CriticalException(It.IsAny<string>()));
            callApi.Setup(x => x.Post(It.IsAny<string>(), message))
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
            var data = new SendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            messageGateway
                .Setup(x => x.Add(It.IsAny<MessageModel>()))
                .ThrowsAsync(new Exception(It.IsAny<string>()));

            callApi.Setup(x => x.Post(It.IsAny<string>(), message))
                .ReturnsAsync(httpResponse);

            // Act
            await _sut.Handle(data, It.IsAny<CancellationToken>());

            // Assert
            logger.Verify(x => x.Log(
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
            var data = new SendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            messageGateway
                .Setup(x => x.Add(It.IsAny<MessageModel>()))
                .ReturnsAsync(entity);

            callApi.Setup(x => x.Post(It.IsAny<string>(), message))
                .ReturnsAsync(httpResponse);

            // Act
            await _sut.Handle(data, It.IsAny<CancellationToken>());

            // Assert
            eventBusGateway.Verify(x => x.Publish("SmsSent"), Times.Once);
        }

        [Fact]
        public async Task HandlerSendsMessageToDeadLetterAndLogErrorWhenApiCallerDoNotReturnsResponseOk()
        {
            // Arrange
            var message = _fixture.Create<MessageDomain>();
            var data = new SendSmsAndPublishNotification<MessageDomain>(message);
            var entity = message.ToModel();
            entity.Id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            messageGateway
                .Setup(x => x.Add(It.IsAny<MessageModel>()))
                .ReturnsAsync(entity);

            callApi.Setup(x => x.Post(It.IsAny<string>(), message))
                .ReturnsAsync(httpResponse);

            // Act
            await _sut.Handle(data, It.IsAny<CancellationToken>());

            // Assert
            eventBusGateway.Verify(x => x.Publish("SmsSent"), Times.Never);
            _deadLetterEnQueueUseCase.Verify(x => 
                x.ExecuteAsync(message), Times.Once);
            logger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
    }
}
