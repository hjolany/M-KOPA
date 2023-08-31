using RabbitMQ.Client.Events;
using SMSMicroService.Entities.Domains.Base;

namespace SMSMicroService.Entities.Domains
{
    public class RabbitMessageReceivedArgumentDomain<T>: MessageReceivedArgumentDomain<T>
    {
        public BasicDeliverEventArgs Delivery { get; }

        public RabbitMessageReceivedArgumentDomain(T data, BasicDeliverEventArgs delivery) : base(data)
        {
            Delivery = delivery;
        }
    }
}
