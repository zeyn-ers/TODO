using AutoMapper;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services;

/// <summary>
/// Todo Service Implementation (ITodoService'e bire bir uyumlu)
/// </summary>
public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;
    private readonly IMapper _mapper;

    public TodoService(ITodoRepository todoRepository, IMapper mapper)
    {
        _todoRepository = todoRepository;
        _mapper = mapper;
    }

    /// <summary>Tüm todo'ları getirir</summary>
    public async Task<IEnumerable<TodoDto>> GetAllAsync()
    {
        var todos = await _todoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    /// <summary>ID'ye göre tek todo getirir</summary>
    public async Task<TodoDto?> GetByIdAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        return todo != null ? _mapper.Map<TodoDto>(todo) : null;
    }

    /// <summary>Yeni todo oluşturur</summary>
    public async Task<TodoDto> CreateAsync(CreateTodoDto dto)
    {
        // DTO → Entity (CategoryId null ise backend default'u kullan)
        var entity = new Todo
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            CategoryId = dto.CategoryId ?? 1,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _todoRepository.AddAsync(entity);
        return _mapper.Map<TodoDto>(created);
    }

    /// <summary>Mevcut todo'yu günceller</summary>
    public async Task<TodoDto?> UpdateAsync(int id, UpdateTodoDto dto)
    {
        var existing = await _todoRepository.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.IsCompleted = dto.IsCompleted;
        existing.DueDate = dto.DueDate;
        existing.Priority = dto.Priority;
        if (dto.CategoryId.HasValue) existing.CategoryId = dto.CategoryId.Value;
        existing.UpdatedAt = DateTime.UtcNow;

        await _todoRepository.UpdateAsync(existing);
        return _mapper.Map<TodoDto>(existing);
    }

    /// <summary>Todo'yu siler</summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _todoRepository.GetByIdAsync(id);
        if (existing is null) return false;

        await _todoRepository.DeleteAsync(id);
        return true;
    }

    /// <summary>Belirli kategoriye ait todo’ları listeler</summary>
    public async Task<IEnumerable<TodoDto>> GetTodosByCategoryAsync(int categoryId)
    {
        var todos = await _todoRepository.GetTodosByCategoryAsync(categoryId);
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }
}
