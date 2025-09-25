using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

/// <summary>
/// TodoNote repository interface'i
/// </summary>
public interface ITodoNoteRepository : IRepository<TodoNote>
{
    Task<IEnumerable<TodoNote>> GetByTodoIdAsync(int todoId);
    Task<TodoNote?> GetByTodoIdAndNoteIdAsync(int todoId, int noteId);
}