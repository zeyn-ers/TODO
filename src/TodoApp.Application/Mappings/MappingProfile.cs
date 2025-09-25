using AutoMapper;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Mappings
{
    /// <summary>AutoMapper eşlemeleri</summary>
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category ↔ DTO
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            // Todo → TodoDto (Category ve Notes alanlarını doldur)
            CreateMap<Todo, TodoDto>()
                .ForMember(d => d.CategoryId,   opt => opt.MapFrom(s => s.CategoryId))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.Name : null))
                .ForMember(d => d.Notes, opt => opt.MapFrom(s => s.Notes));

            // Create/Update → Todo
            CreateMap<CreateTodoDto, Todo>();
            CreateMap<UpdateTodoDto, Todo>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // TodoNote ↔ DTO
            CreateMap<TodoNote, TodoNoteDto>();
            CreateMap<CreateTodoNoteDto, TodoNote>();
            CreateMap<UpdateTodoNoteDto, TodoNote>();
        }
    }
}
