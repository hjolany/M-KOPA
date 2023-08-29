using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SMSMicroService.Gateway.Interface;
using SMSMicroService.Infrastructures;

namespace SMSMicroService.Gateway.Base
{
    public class BaseGateway<T> : IBaseGateway<T>
        where T : class
    {
        //private readonly SmsDbContext _context;

        /* public BaseGateway(SmsDbContext context)
         {
             _context = context;
         }*/

        protected virtual async Task<T?> Get(int id)
        {
            return await SmsDbContext.Create().FindAsync<T>(id).ConfigureAwait(false);
        }

        public virtual async Task<T?> Get(Expression<Func<T, bool>> expression)
        {
            var context = SmsDbContext.Create();
            return await context.Set<T>()
                .FirstOrDefaultAsync(expression)
                .ConfigureAwait(false);
        }

        private async Task<T?> Get(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var prop = entity.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == "id");
            int id = 0;
            if (prop != null)
                id = int.Parse(prop.GetValue(entity)?.ToString() ?? "0");

            var context = SmsDbContext.Create();
            var response = await context.FindAsync<T>(id).ConfigureAwait(false);
            if (response != null)
                context.Entry(response).State = EntityState.Detached;
            return response;
        }

        public virtual Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            var context = SmsDbContext.Create();
            var data =
                context.Set<T>()
                    .Where(predicate);
            return Task.FromResult(data);
        }

        public virtual Task<IQueryable<T>> GetAll()
        {
            var context = SmsDbContext.Create();
            return Task.FromResult<IQueryable<T>>(context.Set<T>());
        }

        public async Task Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var context = SmsDbContext.Create();
            context.Update(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<T> Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var context = SmsDbContext.Create();
            var result = await context.AddAsync(entity).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return result.Entity;
        }

        public async Task AddRange(List<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (entities.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(entities));
            var context = SmsDbContext.Create();
            await context.AddRangeAsync(entities).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task RemoveAll(List<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (entities.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(entities));
            var context = SmsDbContext.Create();
            context.RemoveRange(entities);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task RemoveAll(Expression<Func<T, bool>> expressions)
        {
            var context = SmsDbContext.Create();
            var data = await context.Set<T>()
                .Where(expressions).ToListAsync().ConfigureAwait(false);
            context.RemoveRange(data);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Remove(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var context = SmsDbContext.Create();
            _ = context.Remove(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task Remove(Expression<Func<T, bool>> expression)
        {
            var data = await Get(expression).ConfigureAwait(false);
            if (data != null)
            {
                await Remove(data).ConfigureAwait(false);
            }
        }
    }
}