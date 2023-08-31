namespace SMSMicroService.UseCases.Interfaces
{
    public interface IDeadLetterEnQueueUseCase<in T>
    {
        public Task ExecuteAsync(T entity);
    }
}
