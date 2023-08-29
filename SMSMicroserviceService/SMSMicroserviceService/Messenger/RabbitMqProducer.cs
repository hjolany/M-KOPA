using SMSMicroService.Gateway.Interface;
using SMSService.Entities.Domains;

namespace SMSMicroService.Messenger
{
    public class RabbitMqProducer
    {
        private readonly IMessageQueueGateway<MessageDomain> _messageQueue;

        public RabbitMqProducer(IMessageQueueGateway<MessageDomain> messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public void Publish(MessageDomain message)
        {
            _messageQueue.Queue(message);
            Console.WriteLine($"{message.PhoneNumber}:\t{message.Content}");
        }

        public MessageDomain? Receive()
        {
            var response = _messageQueue.DeQueue();

            return response;
        }
    }
}
