using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

public interface ITodoRepository : IRepository<Todo>
{
    Task<IEnumerable<Todo>> GetCompletedTodosAsync();
    Task<IEnumerable<Todo>> GetPendingTodosAsync();
    Task<IEnumerable<Todo>> GetTodosByPriorityAsync(int priority);
    Task<IEnumerable<Todo>> GetOverdueTodosAsync();
}
