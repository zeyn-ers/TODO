using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

/// <summary>
/// Todo Repository Interface
/// IRepository<Todo>'dan türetilir ve Todo'ya özel ek metodlar içerir
/// Bu interface, Todo entity'si için özel iş mantığı metodlarını tanımlar
/// </summary>
public interface ITodoRepository : IRepository<Todo>
{
    /// <summary>
    /// Tamamlanmış (IsCompleted = true) todo'ları getirir
    /// </summary>
    /// <returns>Tamamlanmış todo listesi</returns>
    Task<IEnumerable<Todo>> GetCompletedTodosAsync();

    /// <summary>
    /// Bekleyen (IsCompleted = false) todo'ları getirir
    /// </summary>
    /// <returns>Bekleyen todo listesi</returns>
    Task<IEnumerable<Todo>> GetPendingTodosAsync();

    /// <summary>
    /// Belirli bir öncelik seviyesindeki todo'ları getirir
    /// </summary>
    /// <param name="priority">Öncelik seviyesi (1: Düşük, 2: Orta, 3: Yüksek)</param>
    /// <returns>Belirtilen öncelikteki todo listesi</returns>
    Task<IEnumerable<Todo>> GetTodosByPriorityAsync(int priority);

    /// <summary>
    /// Süresi geçmiş todo'ları getirir
    /// DueDate < DateTime.Now ve IsCompleted = false olan todo'lar
    /// </summary>
    /// <returns>Süresi geçmiş todo listesi</returns>
    Task<IEnumerable<Todo>> GetOverdueTodosAsync();
}
