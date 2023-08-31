namespace SMSMicroService.Entities.Domains.Interfaces
{
    public interface IMessageReceivedArgumentDomain<out T>
    {
        public T Data { get; } 
    }
}
