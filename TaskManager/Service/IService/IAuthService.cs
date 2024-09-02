using TaskManager.Models.Dtos;

namespace TaskManager.Service.IService
{
    public interface IAuthService
    {
        Task<LoginResponceDto> Login(string username, string password);
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
    }
}
