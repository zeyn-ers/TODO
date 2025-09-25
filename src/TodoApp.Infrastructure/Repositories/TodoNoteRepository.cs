using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories;

/// <summary>
/// TodoNote repository implementasyonu
/// </summary>
public class TodoNoteRepository : BaseRepository<TodoNote>, ITodoNoteRepository
{
    public TodoNoteRepository(TodoDbContext context) : base(context) { }

    public async Task<IEnumerable<TodoNote>> GetByTodoIdAsync(int todoId)
    {
        return await _context.TodoNotes
            .Where(n => n.TodoId == todoId)
            .OrderBy(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<TodoNote?> GetByTodoIdAndNoteIdAsync(int todoId, int noteId)
    {
        return await _context.TodoNotes
            .FirstOrDefaultAsync(n => n.TodoId == todoId && n.Id == noteId);
    }
}