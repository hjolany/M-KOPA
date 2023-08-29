namespace SMSMicroService.Entities.Domains
{
    public class MessageDomain
    {
        public string PhoneNumber { get; set; }
        public string Content { get; set; }
        public override string ToString()
        {
            return $"{PhoneNumber}=>\t{Content}";
        }
    }
}