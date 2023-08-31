namespace SMSMicroService.Gateway.Interface
{
    public interface IEventBusGateway<in T>
    {
        public Task Publish(T @event);
    }
}
