using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using TaskManager.Models;
using TaskManager.Models.Dtos;
using TaskManager.Repository.IRepository;

namespace TaskManager.Controllers
{
    [Route("tasks")]
    [Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskController> _logger;
        private readonly IMapper _mapper;
        private ResponceDto _responceDto;
        public TaskController(ITaskRepository taskRepository, IMapper mapper, ILogger<TaskController> logger)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _responceDto = new ResponceDto();
            _logger = logger;

        }
        [HttpGet("{id}")]
        public async Task<ResponceDto> GetById(string id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(u => u.Type == SD.UserIdClaimName)?.Value;
                var task = await _taskRepository.GetAsync(u => u.Id.ToString() == id && u.UserId.ToString() == userId);
                if (task == null)
                {
                    throw new Exception("Could not find such task in db");
                }
                _responceDto.Success = true;
                _responceDto.Result = _mapper.Map<TaskOfUserDto>(task);
            }
            catch (Exception ex)
            {
                _responceDto.Success=false;
                _responceDto.Message = ex.Message;
            }
            return _responceDto;
        }
        [HttpGet]
        public async Task<ResponceDto> Get([FromQuery] StatusTypes? status = null, DateTime? dueDay = null, PriorityTypes? priority = null,
            bool sortByPriority = false, bool sortByDueDate = false, int pageSize = 5, int pageNumber = 1)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(u => u.Type == SD.UserIdClaimName)?.Value;
                var taskList = await _taskRepository.GetAllAsync(u => u.UserId.ToString() == userId, pageSize:pageSize, pageNumber: pageNumber);
                
                if (taskList == null)
                {
                    throw new Exception("Could not find such task in db");
                }
                //querying by users request
                if (status != null)
                {
                    taskList=taskList.Where(u=>u.Status== status);
                }
                if(priority != null)
                {
                    taskList = taskList.Where(u => u.Priority == priority);
                }
                if (dueDay != null)
                {
                    taskList = taskList.Where(u => u.DueDate == dueDay);
                }
                if (sortByPriority && sortByDueDate)
                {
                    taskList = taskList.OrderBy(u=>u.Priority).ThenBy(u=>u.DueDate);
                }
                else if (sortByPriority)
                {
                    taskList = taskList.OrderBy(u => u.Priority);
                }
                else if(sortByDueDate)
                {
                    taskList = taskList.OrderBy(u => u.DueDate);
                }
                _responceDto.Success = true;
                _responceDto.Result = _mapper.Map<List<TaskOfUserDto>>(taskList.ToList());
            }
            catch (Exception ex)
            {
                _responceDto.Success = false;
                _responceDto.Message = ex.Message;
            }
            return _responceDto;
        }
        [HttpPost]
        public async Task<ResponceDto> Create([FromBody] TaskOfUserDto taskDto)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(u => u.Type == SD.UserIdClaimName)?.Value;
                var task = _mapper.Map<TaskOfUser>(taskDto);
                task.UserId = Guid.Parse(userId);
                await _taskRepository.CreateAsync(task);
                _responceDto.Success = true;
                _responceDto.Result = _mapper.Map<TaskOfUserDto>(task);
                _logger.Log(LogLevel.Information, $"User {userId} created task {task.Id}");
            }
            catch (Exception ex)
            {
                _responceDto.Success = false;
                _responceDto.Message = ex.Message;
            }
            return _responceDto;
        }
        [HttpPut]
        public async Task<ResponceDto> Update([FromBody] TaskOfUserDto taskDto)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(u => u.Type == SD.UserIdClaimName)?.Value;
                var task =await _taskRepository.GetAsync(u=>u.Id == taskDto.Id);
                if (task == null)
                {
                    throw new Exception("No task to update with such id");
                }
                if (task.UserId.ToString() != userId)
                {
                    throw new Exception("Access denied");
                }
                task.Status = taskDto.Status;
                task.Title = taskDto.Title;
                task.Description = taskDto.Description;
                task.Priority = taskDto.Priority;
                task.DueDate = taskDto.DueDate;
                
                await _taskRepository.UpdateAsync(task);
                _responceDto.Success = true;
                _responceDto.Result = _mapper.Map<TaskOfUserDto>(task);
                _logger.Log(LogLevel.Information, $"User {userId} updated task {task.Id}");
            }
            catch (Exception ex)
            {
                _responceDto.Success = false;
                _responceDto.Message = ex.Message;
            }
            return _responceDto;
        }
        [HttpDelete("{id}")]
        public async Task<ResponceDto> Delete (string id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(u => u.Type == SD.UserIdClaimName)?.Value;
                var task = await _taskRepository.GetAsync(u => u.Id.ToString() == id);
                if (task == null)
                {
                    throw new Exception("No task to delete with such id");
                }
                if (task.UserId.ToString() != userId)
                {
                    throw new Exception("Access denied");
                }
                await _taskRepository.DeleteAsync(task);
                _responceDto.Success = true;
                _logger.Log(LogLevel.Information, $"User {userId} deleted task {task.Id}");
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
