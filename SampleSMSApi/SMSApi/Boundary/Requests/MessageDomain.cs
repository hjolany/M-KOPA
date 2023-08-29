namespace SMSApi.Boundary.Requests
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