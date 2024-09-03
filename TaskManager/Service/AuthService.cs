using System.Security.Cryptography;
using System.Text.RegularExpressions;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IPasswordHasherService passwordHasherService, IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
            _passwordHasher = passwordHasherService;
        }
        public async Task<LoginResponceDto> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                var user = await _userRepository.GetAsync(u=>u.Username == loginRequestDto.UserName);

                if (_passwordHasher.VerifyPassword(user.PasswordHash, loginRequestDto.Password))
                {
                    var token = _jwtTokenGenerator.GenerateToken(user);
                    return new LoginResponceDto
                    {
                        User = _mapper.Map<UserDto>(user),
                        Token = token
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
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
                CheckPassword(registrationRequestDto.Password);
                string hash = _passwordHasher.HashPassword(registrationRequestDto.Password);
                user.PasswordHash = hash;
                await _userRepository.CreateAsync(user);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message+ex.InnerException?.Message;
            }
        }
        private void CheckPassword(string password)
        {
            if (password.Length < 8)
            {
                throw new Exception("Password is too short");
            }

            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                throw new Exception("Password needs at least 1 uppercase letter");
            }

            if (!Regex.IsMatch(password, "[a-z]"))
            {
                throw new Exception("Password needs at least 1 lowercase letter");
            }

            if (!Regex.IsMatch(password, "[0-9]"))
            {
                throw new Exception("Password needs at least 1 number");
            }

            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                throw new Exception("Password needs at least 1 special symbol");
            }
        }
    }
}
