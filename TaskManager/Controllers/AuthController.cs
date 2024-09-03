using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models.Dtos;
using TaskManager.Service.IService;

namespace TaskManager.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ResponceDto _responceDto;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _responceDto = new ResponceDto();
        }
        [HttpPost("login")]
        public async Task<ResponceDto> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                var login = await _authService.Login(loginRequestDto);
                _responceDto.Result = login;
                _responceDto.Success = true;
                
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
                if(string.IsNullOrEmpty(registrationResult)) _responceDto.Success = true;
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
