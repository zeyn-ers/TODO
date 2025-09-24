using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories;

/// <summary>
/// Todo Repository Implementation
/// BaseRepository<Todo>'dan türetilir ve ITodoRepository interface'ini implement eder
/// Todo entity'si için özel iş mantığı metodlarını içerir
/// </summary>
public class TodoRepository : BaseRepository<Todo>, ITodoRepository
{
    /// <summary>
    /// Constructor - BaseRepository'e DbContext'i geçirir
    /// </summary>
    /// <param name="context">Entity Framework DbContext</param>
    public TodoRepository(TodoDbContext context) : base(context)
    {
    }

    /// <summary>
    /// ID'ye göre tek bir todo getirir (Category ile birlikte)
    /// </summary>
    /// <param name="id">Aranacak todo'nun ID'si</param>
    /// <returns>Bulunan todo veya null</returns>
    public override async Task<Todo?> GetByIdAsync(int id)
    {
        return await _dbSet.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Tüm todo'ları getirir (Category ile birlikte)
    /// </summary>
    /// <returns>Todo listesi</returns>
    public override async Task<IEnumerable<Todo>> GetAllAsync()
    {
        return await _dbSet.Include(t => t.Category).ToListAsync();
    }

    /// <summary>
    /// Tamamlanmış todo'ları getirir
    /// IsCompleted = true olan todo'ları filtreler
    /// </summary>
    /// <returns>Tamamlanmış todo listesi</returns>
    public async Task<IEnumerable<Todo>> GetCompletedTodosAsync()
    {
        return await _dbSet.Include(t => t.Category).Where(t => t.IsCompleted).ToListAsync();
    }

    /// <summary>
    /// Bekleyen todo'ları getirir
    /// IsCompleted = false olan todo'ları filtreler
    /// </summary>
    /// <returns>Bekleyen todo listesi</returns>
    public async Task<IEnumerable<Todo>> GetPendingTodosAsync()
    {
        return await _dbSet.Include(t => t.Category).Where(t => !t.IsCompleted).ToListAsync();
    }

    /// <summary>
    /// Belirli bir öncelik seviyesindeki todo'ları getirir
    /// Priority değeri eşleşen todo'ları filtreler
    /// </summary>
    /// <param name="priority">Öncelik seviyesi (1: Düşük, 2: Orta, 3: Yüksek)</param>
    /// <returns>Belirtilen öncelikteki todo listesi</returns>
    public async Task<IEnumerable<Todo>> GetTodosByPriorityAsync(int priority)
    {
        return await _dbSet.Include(t => t.Category).Where(t => t.Priority == priority).ToListAsync();
    }

    /// <summary>
    /// Süresi geçmiş todo'ları getirir
    /// Koşullar:
    /// - IsCompleted = false (Tamamlanmamış)
    /// - DueDate.HasValue = true (Bitiş tarihi belirlenmiş)
    /// - DueDate.Value < DateTime.UtcNow (Bitiş tarihi geçmiş)
    /// </summary>
    /// <returns>Süresi geçmiş todo listesi</returns>
    public async Task<IEnumerable<Todo>> GetOverdueTodosAsync()
    {
        var now = DateTime.UtcNow; // Şu anki zaman
        return await _dbSet.Include(t => t.Category).Where(t => 
            !t.IsCompleted && // Tamamlanmamış
            t.DueDate.HasValue && // Bitiş tarihi var
            t.DueDate.Value < now // Bitiş tarihi geçmiş
        ).ToListAsync();
    }

    /// <summary>
    /// Belirli kategoriye ait todo'ları getirir
    /// </summary>
    /// <param name="categoryId">Kategori ID'si</param>
    /// <returns>Kategoriye ait todo listesi</returns>
    public async Task<IEnumerable<Todo>> GetTodosByCategoryAsync(int categoryId)
    {
        return await _dbSet.Include(t => t.Category).Where(t => t.CategoryId == categoryId).ToListAsync();
    }
}
