using RabbitMQ.Client.Events;

namespace SMSMicroService.Entities.Domains
{
    public class RabbitMessageReceivedArgumentDomain<T>
    {
        public T Data { get;}
        public BasicDeliverEventArgs Delivery { get;}
        public RabbitMessageReceivedArgumentDomain(T data, BasicDeliverEventArgs delivery)
        {
            Data = data;
            Delivery = delivery;
        }

    }
}
