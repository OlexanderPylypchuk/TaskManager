using TaskManager.Models;

namespace TaskManager.Repository.IRepository
{
    public interface IUserRepository :IRepository<User>
    {
        Task Update(User user);
    }
}
