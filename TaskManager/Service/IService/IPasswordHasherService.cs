namespace TaskManager.Service.IService
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyPassword(string passwordHash, string inputPassword);
    }
}
