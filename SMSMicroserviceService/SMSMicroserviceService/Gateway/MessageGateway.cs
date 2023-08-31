using SMSMicroService.Entities.Models;
using SMSMicroService.Gateway.Base;
using SMSMicroService.Gateway.Interface;

namespace SMSMicroService.Gateway
{
    public class MessageGateway:BaseGateway<MessageModel>,IMessageTableGateway
    {
        /*public MessageGateway(SmsDbContext context) : base(context)
        {

        }*/
    }
}
