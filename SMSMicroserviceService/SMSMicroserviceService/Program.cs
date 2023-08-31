using MediatR;
using SMSMicroService.Gateway;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Infrastructures;
using SMSMicroService.Entities.Domains;
using SMSMicroService.UseCases;
using SMSMicroService.UseCases.Interfaces;
using RabbitMQ.Client;
using SMSMicroService.Helpers;
using SMSMicroService.Helpers.Interfaces;
using SMSMicroService.Services;
using SMSMicroService.Controllers;
using SMSMicroService.Gateway.RabbitMq;
using SMSMicroService.Gateway.InMemory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(l =>
{
    l.AddConsole();
});

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddHttpClient("CallApiHttpClient", c =>
{
    c.Timeout = TimeSpan.FromSeconds(int.Parse(AppConfig.Get("ExternalAPi:Timeout")));
});

builder.Services.AddSingleton<QueueApiController, QueueApiController>();

builder.Services.AddSingleton<IEmailHelper, EmailHelper>();
builder.Services.AddSingleton<ICallApi<MessageDomain>, CallApi<MessageDomain>>();
builder.Services.AddSingleton<IMessageGateway, MessageGateway>();
builder.Services.AddSingleton<ISendSmsFromQueueAndPublishEventUseCase, SendSmsFromQueueAndPublishEventUseCase>();
builder.Services.AddSingleton<IReSendSmsFromQueueAndPublishEventUseCase, ReSendSmsFromQueueAndPublishEventUseCase>();

builder.Services.AddSingleton<IInMemoryMessageQueueGateway<MessageDomain>, InMemoryMessageQueueGateway<MessageDomain>>();

builder.Services.AddSingleton<IRabbitMainMessageQueueGateway<MessageDomain>>(provider =>
{
    var uri = AppConfig.Get("Queue:Uri");
    var queueName = AppConfig.Get("Queue:Main");
    var factory = new ConnectionFactory()
    {
        Uri = new Uri(uri)
    };
    var connection = factory.CreateConnection();
    var mediator = provider.GetRequiredService<IMediator>();
    var logger = provider.GetRequiredService<ILogger<RabbitMainMessageQueueGateway<MessageDomain>>>();
    return new RabbitMainMessageQueueGateway<MessageDomain>(queueName, connection, mediator,logger);
});

builder.Services.AddSingleton<IRabbitDeadLetterMessageQueueGateway<MessageDomain>>(provider =>
{
    var uri = AppConfig.Get("Queue:Uri");
    var queueName = AppConfig.Get("Queue:DeadLetter");
    var factory = new ConnectionFactory()
    {
        Uri = new Uri(uri)
    };
    var connection = factory.CreateConnection();
    var mediator = provider.GetRequiredService<IMediator>();
    var logger = provider.GetRequiredService<ILogger<RabbitDeadLetterMessageQueueGateway<MessageDomain>>>();
    return new RabbitDeadLetterMessageQueueGateway<MessageDomain>(queueName, connection, mediator, logger);
});

builder.Services.AddSingleton<IEventBusGateway<string>>(provider =>
{
    var uri = AppConfig.Get("Queue:Uri");
    var exchangeName = AppConfig.Get("Queue:ExchangeName");
    var routingKey = AppConfig.Get("Queue:RoutingKey");
    var factory = new ConnectionFactory()
    {
        Uri = new Uri(uri)
    };
    var connection = factory.CreateConnection();
    return new RabbitMqEventBusGateway<string>(exchangeName, routingKey, connection);
}); 

builder.Services.AddHostedService<MainQueueService>();
builder.Services.AddHostedService<DeadQueueService>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("I'm Ok, don't worry....");
    }); 
    endpoints.MapHealthChecks("/health");
});

app.Run();

public partial class Program { }