using TaskManager.Models;

namespace TaskManager.Repository.IRepository
{
    public interface ITaskRepository : IRepository<TaskOfUser>
    {
        Task Update(TaskOfUser taskOfUser);
    }
}
