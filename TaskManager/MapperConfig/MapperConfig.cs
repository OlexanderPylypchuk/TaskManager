using System.Reflection.PortableExecutable;
using AutoMapper;
using TaskManager.Models;
using TaskManager.Models.Dtos;

namespace TaskManager.MapperConfig
{
    public class MapperConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapping = new MapperConfiguration(config =>
            {
                config.CreateMap<User, UserDto>().ReverseMap();
                config.CreateMap<TaskOfUser, TaskOfUserDto>().ReverseMap();
            });
            return mapping;
        }
    }
}
