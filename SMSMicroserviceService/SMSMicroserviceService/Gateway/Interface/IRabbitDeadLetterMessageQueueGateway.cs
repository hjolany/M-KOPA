namespace SMSMicroService.Gateway.Interface
{
    public interface IRabbitDeadLetterMessageQueueGateway<T> : IDeadLetterQueueGateway<T> where T : class
    {
    }
}
