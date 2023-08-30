using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Gateway;
using SMSMicroService.Helpers;
using SMSMicroService.Infrastructures;
using SMSMicroService.Notifications;
using SMSMicroService.Tests.Utilities;
using IConnectionFactory = RabbitMQ.Client.IConnectionFactory;

namespace SMSMicroService.Tests.UnitTests.Gateway;

public class RabbitMainMessageQueueGatewayTests
{

    private readonly Fixture _fixture;
    private readonly IConnection _connection;
    private readonly IConnectionFactory _factory;
    private readonly RabbitMainMessageQueueGateway<MessageDomain> _sut;
    private readonly string _queueName = AppConfig.Get("Queue:Main");
    private readonly Mock<ILogger<RabbitMainMessageQueueGateway<MessageDomain>>> _logger;
    private readonly Mock<IMediator> _mediator;
    private readonly string uri;
    public RabbitMainMessageQueueGatewayTests()
    {
        uri = AppConfig.Get("Queue:Uri");
        _fixture = new Fixture();
        _factory = new ConnectionFactory()
        {
            Uri = new Uri(uri)
        };
        _connection = _factory.CreateConnection();
        /*var channel = _connection.CreateModel();*/
        _logger = new Mock<ILogger<RabbitMainMessageQueueGateway<MessageDomain>>>();
        _mediator = new Mock<IMediator>();
        _sut = new RabbitMainMessageQueueGateway<MessageDomain>(_queueName
            , _connection
            , _mediator.Object
            , _logger.Object);
    }


    [Fact]
    public async Task DeQueueWithNoIssuesWhenConnectionIsOpen()
    {
        // Arrange

        // Act
        await _sut.DeQueue().ConfigureAwait(false);
        // Assert 
    }

    [Fact]
    public async Task EventHandlerIsWorking()
    {
        // Arrange
        bool eventFired = false;
        new FillMessageQueue().FillMainQueue();

        // Act & Assert
        _sut.OnMessage += async (sender, domain) =>
        {
            eventFired = true;
        };
        await _sut.DeQueue().ConfigureAwait(false);
        Thread.Sleep(2000);
        Assert.True(eventFired);
        _mediator.Verify(x =>
            x.Publish(It.IsAny<SendSmsAndPublishNotification<MessageDomain>>()
                , It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task LogExceptionWhenEventThrowsException()
    {
        // Arrange 
        new FillMessageQueue().FillMainQueue();
        var mockEventHandler = new Mock<EventHandler<RabbitMessageReceivedArgumentDomain<MessageDomain>>>();
        mockEventHandler.Setup(h =>
                h.Invoke(It.IsAny<object>(), It.IsAny<RabbitMessageReceivedArgumentDomain<MessageDomain>>()))
            .Throws(new Exception("Sample Exception"));

        //_sut.OnMessage -= async (sender, domain) => { };
        _sut.OnMessage += mockEventHandler.Object;

        // Act & Assert

        //_sut.OnMessage += async (sender, domain) => throw new Exception("SampleException");
        await _sut.DeQueue().ConfigureAwait(false);
        Thread.Sleep(2000);

        // Assert
        _logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.AtLeastOnce);
    }

    [Fact]
    public async Task LogCriticalExceptionAndPublishNotificationWhenEventThrowsCriticalException()
    {
        // Arrange 
        new FillMessageQueue().FillMainQueue();
        var mockEventHandler = new Mock<EventHandler<RabbitMessageReceivedArgumentDomain<MessageDomain>>>();
        mockEventHandler.Setup(h =>
                h.Invoke(It.IsAny<object>(), It.IsAny<RabbitMessageReceivedArgumentDomain<MessageDomain>>()))
            .Throws(new CriticalException("Sample Exception"));

        _sut.OnMessage += mockEventHandler.Object;

        // Act & Assert

        //_sut.OnMessage += async (sender, domain) => throw new Exception("SampleException");
        await _sut.DeQueue().ConfigureAwait(false);
        Thread.Sleep(2000);

        // Assert
        _logger.Verify(x => x.Log(
            LogLevel.Critical,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.AtLeastOnce);

        _mediator.Verify(x =>
            x.Publish(It.IsAny<PromptNotification<CriticalException>>()
                , It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task DeQueueWhenConnectionIsNotOpenThenThrowsConnectionAbortedException()
    {
        // Arrange
        _connection.Close();

        // Act & Assert
        _ = Assert.ThrowsAsync<ConnectionAbortedException>(async () => await _sut.DeQueue().ConfigureAwait(false));
        // Assert 
    }

    [Fact]
    public async Task EnQueueWhenConnectionIsNotOpenThenThrowsConnectionAbortedException()
    {
        // Arrange
        _connection.Close();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ConnectionAbortedException>(async () =>
            await _sut.EnQueue(It.IsAny<MessageDomain>()).ConfigureAwait(false)).ConfigureAwait(false);
        // Assert 
        Assert.IsType<ConnectionAbortedException>(exception);
    }

    [Fact]
    public async Task EnQueueWhenConnectionIsOpenThenWorksFine()
    {
        // Arrange

        // Act & Assert
        await _sut.EnQueue(It.IsAny<MessageDomain>()).ConfigureAwait(false);
        // Assert 
        Assert.True(true);
    }
}