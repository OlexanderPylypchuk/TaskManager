using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repository.IRepository;

namespace TaskManager.Repository
{
    public class TaskRepository : Repository<TaskOfUser>, ITaskRepository
    {
        AppDbContext _db;
        public TaskRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task Update(TaskOfUser taskOfUser)
        {
            taskOfUser.UpdatedAt = DateTime.Now;
            _db.Tasks.Update(taskOfUser);
            await _db.SaveChangesAsync();
        }
    }
}
