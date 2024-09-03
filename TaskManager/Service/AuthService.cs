using System.Security.Cryptography;
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
