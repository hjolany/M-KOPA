using MediatR;

namespace SMSMicroService.Notifications.Interfaces
{
    public interface IPromptCommand<T>/*: IRequest<Unit>*/
    {
        public T Data { get; set; }
    }
}
