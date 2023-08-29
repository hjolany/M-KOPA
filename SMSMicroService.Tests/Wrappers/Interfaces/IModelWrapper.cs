using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SMSMicroService.Tests.Wrappers.Interfaces
{
    public interface IModelWrapper : IModel
    {
        /*public new void BasicPublish(string exchange, string routingKey, IBasicProperties basicProperties, byte[] body);*/
        /*public new string BasicConsume(string queue, bool autoAck, string consumerTag, bool noLocal, bool exclusive,IDictionary<string, object> arguments, EventingBasicConsumer consumer);
        public new string BasicConsume(string queue, bool autoAck, string consumerTag, bool noLocal, bool exclusive,IDictionary<string, object> arguments, IBasicConsumer consumer);
        public new string BasicConsume(string queue, bool autoAck, IBasicConsumer consumer);
        public new void BasicConsume(string queue, bool autoAck, EventingBasicConsumer consumer);*/

    }
}
