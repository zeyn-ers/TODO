using AutoMapper;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;
    private readonly IMapper _mapper;

    public TodoService(ITodoRepository todoRepository, IMapper mapper)
    {
        _todoRepository = todoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoDto>> GetAllTodosAsync()
    {
        var todos = await _todoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    public async Task<TodoDto?> GetTodoByIdAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        return todo != null ? _mapper.Map<TodoDto>(todo) : null;
    }

    public async Task<IEnumerable<TodoDto>> GetCompletedTodosAsync()
    {
        var todos = await _todoRepository.GetCompletedTodosAsync();
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    public async Task<IEnumerable<TodoDto>> GetPendingTodosAsync()
    {
        var todos = await _todoRepository.GetPendingTodosAsync();
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    public async Task<IEnumerable<TodoDto>> GetTodosByPriorityAsync(int priority)
    {
        var todos = await _todoRepository.GetTodosByPriorityAsync(priority);
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    public async Task<IEnumerable<TodoDto>> GetOverdueTodosAsync()
    {
        var todos = await _todoRepository.GetOverdueTodosAsync();
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto)
    {
        var todo = new Todo
        {
            Title = createTodoDto.Title,
            Description = createTodoDto.Description,
            DueDate = createTodoDto.DueDate,
            Priority = createTodoDto.Priority,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        var createdTodo = await _todoRepository.AddAsync(todo);
        return _mapper.Map<TodoDto>(createdTodo);
    }

    public async Task<TodoDto> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto)
    {
        var existingTodo = await _todoRepository.GetByIdAsync(id);
        if (existingTodo == null)
        {
            throw new ArgumentException($"Todo with ID {id} not found.");
        }

        existingTodo.Title = updateTodoDto.Title;
        existingTodo.Description = updateTodoDto.Description;
        existingTodo.IsCompleted = updateTodoDto.IsCompleted;
        existingTodo.DueDate = updateTodoDto.DueDate;
        existingTodo.Priority = updateTodoDto.Priority;
        existingTodo.UpdatedAt = DateTime.UtcNow;

        await _todoRepository.UpdateAsync(existingTodo);
        return _mapper.Map<TodoDto>(existingTodo);
    }

    public async Task DeleteTodoAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null)
        {
            throw new ArgumentException($"Todo with ID {id} not found.");
        }

        await _todoRepository.DeleteAsync(id);
    }

    public async Task<TodoDto> MarkAsCompletedAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null)
        {
            throw new ArgumentException($"Todo with ID {id} not found.");
        }

        todo.IsCompleted = true;
        todo.UpdatedAt = DateTime.UtcNow;

        await _todoRepository.UpdateAsync(todo);
        return _mapper.Map<TodoDto>(todo);
    }

    public async Task<TodoDto> MarkAsPendingAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null)
        {
            throw new ArgumentException($"Todo with ID {id} not found.");
        }

        todo.IsCompleted = false;
        todo.UpdatedAt = DateTime.UtcNow;

        await _todoRepository.UpdateAsync(todo);
        return _mapper.Map<TodoDto>(todo);
    }
}
