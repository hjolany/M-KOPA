using SMSMicroService.Gateway.Interface;
using SMSMicroService.UseCases.Interfaces;

namespace SMSMicroService.UseCases
{
    public class DeadLetterEnQueueUseCase<T> : IDeadLetterEnQueueUseCase<T>
        where T : class
    {
        private readonly IDeadLetterQueueGateway<T> _gateway;

        public DeadLetterEnQueueUseCase(IDeadLetterQueueGateway<T> gateway)
        {
            _gateway = gateway;
        }

        public async Task ExecuteAsync(T entity)
        {
            await _gateway.EnQueue(entity).ConfigureAwait(false);
        }
    }
}
