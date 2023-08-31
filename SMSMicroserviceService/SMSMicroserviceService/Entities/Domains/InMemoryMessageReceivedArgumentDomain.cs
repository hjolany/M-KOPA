using SMSMicroService.Entities.Domains.Base;

namespace SMSMicroService.Entities.Domains
{
    public class InMemoryMessageReceivedArgumentDomain<T>: MessageReceivedArgumentDomain<T>
    {

        public InMemoryMessageReceivedArgumentDomain(T data) : base(data)
        {
        }
    }
}
