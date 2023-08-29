using SMSMicroService.Entities.Domains;
using SMSMicroService.Entities.Models;
using SMSMicroService.Infrastructures.Enums;

namespace SMSMicroService.Factories
{
    public static class MessageFactory
    {
        public static MessageModel ToModel(this MessageDomain domain,EStatus status=EStatus.Draft,int retryCount = 2)
        {
            return new MessageModel()
            {
                Content = domain.Content,
                PhoneNumber = domain.PhoneNumber,
                Status = status,
                RetryCount = retryCount
            };
        }
    }
}
