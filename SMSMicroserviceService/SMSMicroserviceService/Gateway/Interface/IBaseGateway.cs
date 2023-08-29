using System.Linq.Expressions;

namespace SMSMicroService.Gateway.Interface
{
    public interface IBaseGateway<T> where T : class
    {
        public Task<T> Add(T entity);
        public Task AddRange(List<T> entities);
        public Task Remove(T entity);
        public Task Remove(Expression<Func<T, bool>> expression);
        public Task RemoveAll(List<T> entities);
        public Task RemoveAll(Expression<Func<T, bool>> expressions);
        public abstract Task<T?> Get(Expression<Func<T, bool>> expression);
        public abstract Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> predicate);
        public abstract Task<IQueryable<T>> GetAll();
        public Task Update(T entity);
    }
}