namespace SMSMicroService.Infrastructures.Queue.InMemory
{
    public class InMemoryMessage
    {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
