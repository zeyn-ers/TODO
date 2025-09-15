using TodoApp.Application.DTOs;

namespace TodoApp.Application.Interfaces;

public interface ITodoService
{
    Task<IEnumerable<TodoDto>> GetAllTodosAsync();
    Task<TodoDto?> GetTodoByIdAsync(int id);
    Task<IEnumerable<TodoDto>> GetCompletedTodosAsync();
    Task<IEnumerable<TodoDto>> GetPendingTodosAsync();
    Task<IEnumerable<TodoDto>> GetTodosByPriorityAsync(int priority);
    Task<IEnumerable<TodoDto>> GetOverdueTodosAsync();
    Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto);
    Task<TodoDto> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto);
    Task DeleteTodoAsync(int id);
    Task<TodoDto> MarkAsCompletedAsync(int id);
    Task<TodoDto> MarkAsPendingAsync(int id);
}
