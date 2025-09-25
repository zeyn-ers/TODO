using TodoApp.Application.DTOs;

namespace TodoApp.Application.Interfaces;

/// <summary>TodoNote işlemleri için servis kontratı</summary>
public interface ITodoNoteService
{
    Task<IEnumerable<TodoNoteDto>> GetByTodoIdAsync(int todoId);
    Task<TodoNoteDto> CreateAsync(int todoId, CreateTodoNoteDto dto);
    Task<bool> DeleteAsync(int todoId, int noteId);
    Task<TodoNoteDto?> UpdateAsync(int todoId, int noteId, UpdateTodoNoteDto dto);
}