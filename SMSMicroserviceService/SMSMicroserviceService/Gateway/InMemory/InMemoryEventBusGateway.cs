using SMSMicroService.Gateway.Interface;
using SMSMicroService.Infrastructures.EventBus.InMemory;

namespace SMSMicroService.Gateway.InMemory
{
    public class InMemoryEventBusGateway<T>:IEventBusGateway<T>
    {
        private readonly InMemoryEventBus<T> _eventBus;
        public InMemoryEventBusGateway()
        {
            _eventBus = new InMemoryEventBus<T>();

            var @event = new InMemoryEventHandler<T>();
            _eventBus.Subscribe(@event);
        }  
        public async Task Publish(T @event)
        {
            _eventBus.Publish(@event);
        }
    }
}
