using System.Collections.Concurrent;

namespace SMSMicroService.Infrastructures.Queue.InMemory
{
    public class InMemoryQueue<T>
    {
        private readonly ConcurrentQueue<T?> _queue;
        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private Thread _listenerThread;
        public event EventHandler<InMemoryMessageEventArgs<T>> Received;

        public InMemoryQueue()
        {
            _queue = new ConcurrentQueue<T?>();
            _listenerThread = new Thread(QueueListener);
            _listenerThread.Start();
        }

        public void Enqueue(T message)
        {
            _queue.Enqueue(message);
            _resetEvent.Set();
        }

        public T? Dequeue()
        {
            _queue.TryDequeue(out var message);
            return message;
        }

        private void QueueListener()
        {
            while (true)
            {
                _resetEvent.WaitOne();
                while (_queue.TryDequeue(out var message))
                {
                    Received?.Invoke(this,new InMemoryMessageEventArgs<T>(message));
                }
                _resetEvent.Reset();
            }
        }
    }
}
