using SMSMicroService.Entities.Domains;
using SMSMicroService.Entities.Domains.Interfaces;

namespace SMSMicroService.Gateway.Interface
{
    public interface IMessageQueueGateway<T>
        where T : class
    {
        public abstract event EventHandler<IMessageReceivedArgumentDomain<T>> OnMessage;
        public abstract Task EnQueue(T? message);
        public abstract Task DeQueue();
        public abstract Task<int> ConsumerCount();
    }
}
