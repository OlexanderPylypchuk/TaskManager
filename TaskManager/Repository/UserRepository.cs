using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repository.IRepository;

namespace TaskManager.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        AppDbContext _db;
        public UserRepository(AppDbContext db):base(db) 
        {
            _db = db;
        }
        public async Task Update(User user)
        {
            user.UpdatedAt = DateTime.Now;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
