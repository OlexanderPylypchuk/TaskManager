﻿using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        private ResponceDto _responceDto;
        public TaskController(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _responceDto=new ResponceDto();
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
        public async Task<ResponceDto> Get([FromQuery] StatusTypes? status = null, PriorityTypes? priority = null, bool sortByPriority = false, bool sortByDueDate = false)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(u => u.Type == SD.UserIdClaimName)?.Value;
                var taskList = await _taskRepository.GetAllAsync(u => u.UserId.ToString() == userId);
                if (taskList == null)
                {
                    throw new Exception("Could not find such task in db");
                }
                if (status != null)
                {
                    taskList=taskList.Where(u=>u.Status== status);
                }
                if(priority != null)
                {
                    taskList = taskList.Where(u => u.Priority == priority);
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
                
                await _taskRepository.UpdateAsync(task);
                _responceDto.Success = true;
                _responceDto.Result = _mapper.Map<TaskOfUserDto>(task);
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
                    throw new Exception("No task to update with such id");
                }
                if (task.UserId.ToString() != userId)
                {
                    throw new Exception("Access denied");
                }
                await _taskRepository.DeleteAsync(task);
                _responceDto.Success = true;
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
