namespace SMSMicroService.Gateway.Interface
{
    public interface IRabbitDeadLetterMessageQueueGateway<T> :IMessageQueueGateway<T> where T : class
    {
    }
}
