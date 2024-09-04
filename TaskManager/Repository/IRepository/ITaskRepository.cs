using TaskManager.Models;

namespace TaskManager.Repository.IRepository
{
    public interface ITaskRepository : IRepository<TaskOfUser>
    {
        Task UpdateAsync(TaskOfUser taskOfUser);
    }
}
