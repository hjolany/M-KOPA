namespace SMSMicroService.Infrastructures.Queue.InMemory
{
    public class InMemoryMessageEventArgs<T>
    {
        public T Message { get; }

        public InMemoryMessageEventArgs(T message)
        {
            Message = message;
        }
    }
}
