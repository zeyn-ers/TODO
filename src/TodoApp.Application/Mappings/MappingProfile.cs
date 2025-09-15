using AutoMapper;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Todo, TodoDto>().ReverseMap();
        CreateMap<CreateTodoDto, Todo>();
        CreateMap<UpdateTodoDto, Todo>();
    }
}
