namespace SMSMicroService.Gateway.Interface
{
    public interface IRabbitMainMessageQueueGateway<T> : IMessageQueueGateway<T>
        where T : class
    {
    }
}
