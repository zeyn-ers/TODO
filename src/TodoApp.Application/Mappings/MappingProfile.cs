using AutoMapper;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Mappings;

/// <summary>
/// AutoMapper Profile
/// Entity ve DTO arasındaki dönüşüm kurallarını tanımlar
/// AutoMapper, bu profile'ı kullanarak otomatik dönüşümler yapar
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Constructor - Mapping kurallarını tanımlar
    /// </summary>
    public MappingProfile()
    {
        // Todo Entity <-> TodoDto DTO dönüşümü (çift yönlü)
        // ReverseMap() sayesinde hem Todo -> TodoDto hem de TodoDto -> Todo dönüşümü yapılır
        CreateMap<Todo, TodoDto>().ReverseMap();
        
        // CreateTodoDto -> Todo Entity dönüşümü
        // Yeni todo oluştururken kullanılır
        CreateMap<CreateTodoDto, Todo>();
        
        // UpdateTodoDto -> Todo Entity dönüşümü
        // Mevcut todo güncellerken kullanılır
        CreateMap<UpdateTodoDto, Todo>();
    }
}
