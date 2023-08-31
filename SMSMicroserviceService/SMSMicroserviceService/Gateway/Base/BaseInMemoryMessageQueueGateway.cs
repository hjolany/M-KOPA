using System.Collections.Concurrent;
using RabbitMQ.Client.Events;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Entities.Domains.Interfaces;
using SMSMicroService.Gateway.Interface;

namespace SMSMicroService.Gateway.Base
{
    public class BaseInMemoryMessageQueueGateway<T>: IMessageQueueGateway<T>
    where T : class 
    {
        private readonly ConcurrentQueue<T?> _queue;
        public BaseInMemoryMessageQueueGateway()
        {
            _queue = new ConcurrentQueue<T?>();
        }

        public event EventHandler<IMessageReceivedArgumentDomain<T>>? OnMessage;

        public async Task EnQueue(T? message)
        {
            _queue.Enqueue(message);
        }

        public async Task DeQueue()
        {
            _queue.TryDequeue(out var message);
            OnMessage?.Invoke(this,new InMemoryMessageReceivedArgumentDomain<T>(message));
        }

        public Task<int> ConsumerCount()
        {
            throw new NotImplementedException();
        }

        public Task DeleteQueue()
        {
            throw new NotImplementedException();
        }
    }
}
