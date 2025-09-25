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
    private readonly ITodoNoteRepository _todoNoteRepository;
    private readonly IMapper _mapper;

    public TodoService(ITodoRepository todoRepository, ITodoNoteRepository todoNoteRepository, IMapper mapper)
    {
        _todoRepository = todoRepository;
        _todoNoteRepository = todoNoteRepository;
        _mapper = mapper;
    }

    /// <summary>Tüm todo'ları getirir</summary>
    public async Task<IEnumerable<TodoDto>> GetAllAsync(bool includeRelations = false)
    {
        var todos = await _todoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    /// <summary>ID'ye göre tek todo getirir</summary>
    public async Task<TodoDto?> GetByIdAsync(int id, bool includeRelations = false)
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
        
        // InitialNote varsa ekle
        if (!string.IsNullOrWhiteSpace(dto.InitialNote))
        {
            var note = new TodoNote
            {
                TodoId = created.Id,
                Content = dto.InitialNote,
                CreatedAt = DateTime.UtcNow
            };
            await _todoNoteRepository.AddAsync(note);
        }
        
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

    /// <summary>Sayfalama ile todo'ları getirir</summary>
    public async Task<PagedResult<TodoDto>> GetPagedTodosAsync(PaginationParameters parameters)
    {
        var (items, totalCount) = await _todoRepository.GetPagedAsync(parameters.PageNumber, parameters.PageSize);
        var todoDtos = _mapper.Map<IEnumerable<TodoDto>>(items);
        
        return new PagedResult<TodoDto>
        {
            Data = todoDtos,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }

    public async Task<IEnumerable<TodoDto>> GetTodosByCategoryAsync(int categoryId, bool includeRelations = false)
    {
        var todos = await _todoRepository.GetTodosByCategoryAsync(categoryId);
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    public async Task<PagedResult<TodoDto>> GetPagedTodosAsync(PaginationParameters parameters, bool includeRelations = false)
    {
        var (items, totalCount) = await _todoRepository.GetPagedAsync(parameters.PageNumber, parameters.PageSize);
        var todoDtos = _mapper.Map<IEnumerable<TodoDto>>(items);
        
        return new PagedResult<TodoDto>
        {
            Data = todoDtos,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }

    public async Task<IEnumerable<TodoDto>> GetFilteredTodosAsync(int? categoryId = null, bool? isDone = null, string? sortBy = null, bool includeRelations = false)
    {
        var todos = await _todoRepository.GetAllAsync();
        var query = todos.AsQueryable();
        
        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);
            
        if (isDone.HasValue)
            query = query.Where(t => t.IsCompleted == isDone.Value);
            
        query = sortBy?.ToLower() switch
        {
            "priority" => query.OrderByDescending(t => t.Priority),
            "duedate" => query.OrderBy(t => t.DueDate ?? DateTime.MaxValue),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };
        
        return _mapper.Map<IEnumerable<TodoDto>>(query.ToList());
    }
}
