using RabbitMQ.Client;

namespace SMSMicroService.Infrastructures.Wrappers.Interfaces
{
    public interface IModelWrapper : IModel
    {
        new void BasicPublish(string exchange, string routingKey, IBasicProperties basicProperties, byte[] body);
    }
}
