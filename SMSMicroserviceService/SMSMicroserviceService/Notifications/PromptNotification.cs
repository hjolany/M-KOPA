using MediatR;

namespace SMSMicroService.Notifications
{
    public class PromptNotification<T> : INotification
    {
        public T Data { get; set; }

        public PromptNotification(T data)
        {
            Data = data;
        }

    }
}
