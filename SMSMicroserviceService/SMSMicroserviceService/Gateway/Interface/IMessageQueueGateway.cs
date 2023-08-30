using SMSMicroService.Entities.Domains;

namespace SMSMicroService.Gateway.Interface
{
    public interface IMessageQueueGateway<T>
        where T : class
    {
        public abstract event EventHandler<RabbitMessageReceivedArgumentDomain<T>> OnMessage;
        public abstract Task EnQueue(T message);
        public abstract Task DeQueue();
        public abstract Task<int> ConsumerCount();
    }
}
