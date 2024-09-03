using TaskManager.Models.Dtos;

namespace TaskManager.Service.IService
{
    public interface IAuthService
    {
        Task<LoginResponceDto> Login(LoginRequestDto loginRequestDto);
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
    }
}
