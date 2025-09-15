using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories;

public class TodoRepository : BaseRepository<Todo>, ITodoRepository
{
    public TodoRepository(TodoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Todo>> GetCompletedTodosAsync()
    {
        return await _dbSet.Where(t => t.IsCompleted).ToListAsync();
    }

    public async Task<IEnumerable<Todo>> GetPendingTodosAsync()
    {
        return await _dbSet.Where(t => !t.IsCompleted).ToListAsync();
    }

    public async Task<IEnumerable<Todo>> GetTodosByPriorityAsync(int priority)
    {
        return await _dbSet.Where(t => t.Priority == priority).ToListAsync();
    }

    public async Task<IEnumerable<Todo>> GetOverdueTodosAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet.Where(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value < now).ToListAsync();
    }
}
