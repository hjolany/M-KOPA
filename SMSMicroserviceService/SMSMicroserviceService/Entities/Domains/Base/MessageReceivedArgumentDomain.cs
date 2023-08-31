using RabbitMQ.Client.Events;
using SMSMicroService.Entities.Domains.Interfaces;

namespace SMSMicroService.Entities.Domains.Base
{
    public class MessageReceivedArgumentDomain<T> : IMessageReceivedArgumentDomain<T>
    {
        protected MessageReceivedArgumentDomain(T data)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
