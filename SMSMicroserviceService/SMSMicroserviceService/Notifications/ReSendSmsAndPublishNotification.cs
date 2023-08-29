using MediatR;

namespace SMSMicroService.Notifications
{
    public class ReSendSmsAndPublishNotification<T> :INotification
    {
        public T Data { get; set; }
        public ReSendSmsAndPublishNotification(T data)
        {
            Data = data;
        }
    }
}
