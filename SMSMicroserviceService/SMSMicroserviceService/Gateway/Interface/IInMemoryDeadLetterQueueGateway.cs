namespace SMSMicroService.Gateway.Interface
{
    public interface IInMemoryDeadLetterQueueGateway<T> : IDeadLetterQueueGateway<T> where T : class
    {
    }
}
