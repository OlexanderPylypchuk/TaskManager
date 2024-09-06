using System.Linq.Expressions;

namespace TaskManager.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveAsync();
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> filter=null, int pageSize = 5, int pageNumber = 1);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
    }
}
