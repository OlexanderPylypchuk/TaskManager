using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models.Dtos;
using TaskManager.Service.IService;

namespace TaskManager.Controllers
{
    [Route("users")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger; 
        private readonly ResponceDto _responceDto;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _responceDto = new ResponceDto();
            _logger = logger;

        }
        [HttpPost("login")]
        public async Task<ResponceDto> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                var login = await _authService.Login(loginRequestDto);
                _logger.Log(logLevel: LogLevel.Information, $"User {loginRequestDto.UserName} logged in");
                //contains token for authentication if succesful
                _responceDto.Result = login;
                _responceDto.Success = login.User==null;
            }
            catch (Exception ex)
            {
                _responceDto.Success=false;
                _responceDto.Message = ex.Message;
            }
            return _responceDto;
        }
        [HttpPost("register")]
        public async Task<ResponceDto> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            try
            {
                var registrationResult = await _authService.Register(registrationRequestDto);
                if (string.IsNullOrEmpty(registrationResult))
                {
                    _responceDto.Success = true;
                    _logger.Log(logLevel: LogLevel.Information, $"User {registrationRequestDto.Username} registered");
                }
                else
                {
                    _responceDto.Success = false;
                    _responceDto.Message = registrationResult;
                }


            }
            catch (Exception ex)
            {
                _responceDto.Success = false;
                _responceDto.Message = ex.Message;
            }
            return _responceDto;
        }
    }
}
