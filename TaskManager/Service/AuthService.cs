using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Models.Dtos;
using TaskManager.Service.IService;

namespace TaskManager.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasherService _passwordHasher;
        private IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(IJwtTokenGenerator jwtTokenGenerator, AppDbContext db, IPasswordHasherService passwordHasherService)
        {
            _db = db;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasherService;
        }
        public async Task<LoginResponceDto> Login(string username, string password)
        {
            try
            {
                var user = _db.Users.First(u=>u.Username == username);
                
                if(_passwordHasher.VerifyPassword(user.PasswordHash, password))
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
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
