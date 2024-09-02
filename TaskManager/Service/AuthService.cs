using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Dtos;
using TaskManager.Repository.IRepository;
using TaskManager.Service.IService;

namespace TaskManager.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasher;
        private IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IPasswordHasherService passwordHasherService)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasherService;
        }
        public async Task<LoginResponceDto> Login(string username, string password)
        {
            try
            {
                var user = await _userRepository.GetAsync(u=>u.Username == username);

                if (_passwordHasher.VerifyPassword(user.PasswordHash, password))
                {
                    var token = _jwtTokenGenerator.GenerateToken(user);
                    return new LoginResponceDto
                    {
                        User = user,
                        Token = token
                    };
                }
            }
            catch (Exception ex)
            {

            }
            return new LoginResponceDto();
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            try
            {
                User user = new User()
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Username = registrationRequestDto.Username,
                    Email = registrationRequestDto.Email
                };
                string hash = _passwordHasher.HashPassword(registrationRequestDto.Password);
                user.PasswordHash = hash;
                await _userRepository.CreateAsync(user);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
