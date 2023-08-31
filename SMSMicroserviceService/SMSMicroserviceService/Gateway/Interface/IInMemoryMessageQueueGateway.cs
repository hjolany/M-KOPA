namespace SMSMicroService.Gateway.Interface
{
    public interface IInMemoryMessageQueueGateway<T> : IMessageQueueGateway<T> where T : class
    {
    }
}
