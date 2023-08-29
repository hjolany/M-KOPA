using SMSMicroService.Entities.Domains;
using SMSMicroService.Infrastructures.Enums;

namespace SMSMicroService.Entities.Models
{
    public class MessageModel : MessageDomain
    {
        public int Id { get; set; }
        public EStatus Status { get; set; }
        public bool Published { get; set; } = false;
        public int RetryCount { get; set; }
        public string? Exception { get; set; }
    }
}