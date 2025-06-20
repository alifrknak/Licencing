using Domain;
using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure;

// PersistentRepository is a generic repository that implements the IPersistencePort interface.
public class PersistentRespository<T>(DbContext dbContext) : IPersistencePort<T> where T : BaseEntity
{
    public Task AddAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        dbContext.Set<T>().Add(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Expression<Func<T, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression, nameof(expression));
        var entity = dbContext.Set<T>().SingleOrDefaultAsync(expression);
        if (entity == null)
        {
            throw new InvalidOperationException("Entity not found");
        }
        dbContext.Set<T>().Remove(entity.Result);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression, nameof(expression));
        return dbContext.Set<T>().AnyAsync(expression);
    }

    public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression, nameof(expression));
        return dbContext.Set<T>().Where(expression).ToListAsync().ContinueWith(task => task.Result.AsEnumerable());
    }

    public async Task<T> Single(Expression<Func<T, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression, nameof(expression));
        return await dbContext.Set<T>().SingleOrDefaultAsync(expression);
    }

    public Task UpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        dbContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }
}