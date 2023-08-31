namespace SMSMicroService.Entities.Domains.Interfaces
{
    public interface IMessageDomain
    {
        public string PhoneNumber { get; set; }
        public string Content { get; set; }
        public string ToString();
    }
}
