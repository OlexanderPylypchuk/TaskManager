using TaskManager.Models;

namespace TaskManager.Service.IService
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(User user);
    }
}
