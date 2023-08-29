using MediatR;

namespace SMSMicroService.Notifications
{
    public class SendSmsAndPublishNotification<T> :INotification
    {
        public T Data { get; set; }
        public SendSmsAndPublishNotification(T data)
        {
            Data = data;
        }
    }
}
