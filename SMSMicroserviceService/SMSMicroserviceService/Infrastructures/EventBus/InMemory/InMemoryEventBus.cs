namespace SMSMicroService.Infrastructures.EventBus.InMemory
{
    public class InMemoryEventBus<T>
    {
        private readonly Dictionary<Type, List<object>> _eventHandlers;

        public InMemoryEventBus()
        {
            _eventHandlers = new Dictionary<Type, List<object>>();
        }

        public void Subscribe(object handler)  
        {
            var eventType = typeof(T);

            if (!_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] = new List<object>();
            }

            _eventHandlers[eventType].Add(handler);
        }

        public void Publish(T @event)  
        {
            var eventType = typeof(T);

            if (_eventHandlers.ContainsKey(eventType))
            {
                foreach (var handler in _eventHandlers[eventType])
                {
                    var method = handler.GetType().GetMethod("Handle");
                    method?.Invoke(handler, new object[] { @event });
                }
            }
        }
    }

}
