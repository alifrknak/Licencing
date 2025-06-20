using System.Linq.Expressions;

namespace Domain.Ports;

public interface IPersistencePort<T> where T : BaseEntity
{
    Task AddAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
    Task<T> Single(Expression<Func<T, bool>> expression);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Expression<Func<T, bool>> expression);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
}