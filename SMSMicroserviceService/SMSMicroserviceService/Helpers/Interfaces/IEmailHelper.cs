using SMSMicroService.Entities.Domains;

namespace SMSMicroService.Helpers.Interfaces
{
    public interface IEmailHelper
    {
        public Task Send(EmailDomain model);
    }
}
