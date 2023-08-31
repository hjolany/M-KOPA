using System.Collections.Concurrent;
using MediatR;
using SMSMicroService.Entities.Domains;
using SMSMicroService.Entities.Domains.Interfaces;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Infrastructures.Queue.InMemory;

namespace SMSMicroService.Gateway.Base
{
    public abstract class BaseInMemoryMessageQueueGateway<T>: IMessageQueueGateway<T>
    where T : class 
    {

        InMemoryQueue<T> queue;
        private readonly ILogger<IInMemoryMessageQueueGateway<T>> _logger;
        protected readonly IMediator Mediator;

        protected BaseInMemoryMessageQueueGateway(ILogger<IInMemoryMessageQueueGateway<T>> logger, IMediator mediator)
        {
            _logger = logger;
            Mediator = mediator;
            queue = new InMemoryQueue<T>();
        }

        public event EventHandler<IMessageReceivedArgumentDomain<T>>? OnMessage;

        public async Task EnQueue(T? message)
        {
            queue.Enqueue(message);
        }

        public async Task DeQueue()
        {
            //var tcs = new TaskCompletionSource();

            var message = queue.Dequeue();
            queue.Received += (object? sender, InMemoryMessageEventArgs<T> e) =>
            {
                OnMessage?.Invoke(this,new InMemoryMessageReceivedArgumentDomain<T>(e.Message));
            };

            //await tcs.Task;
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
