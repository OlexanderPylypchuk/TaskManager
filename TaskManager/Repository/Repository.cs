using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Repository.IRepository;

namespace TaskManager.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        AppDbContext _db;
        internal DbSet<T> _dbSet;
        public Repository(AppDbContext db){
            _db = db;
            _dbSet = _db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return query;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
