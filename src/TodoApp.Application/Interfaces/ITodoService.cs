using TodoApp.Application.DTOs;

namespace TodoApp.Application.Interfaces;

/// <summary>
/// Todo Service Interface
/// Application katmanında bulunan iş mantığı servisinin interface'i
/// Bu interface, Todo ile ilgili tüm iş operasyonlarını tanımlar
/// Repository katmanı ile API katmanı arasında köprü görevi görür
/// </summary>
public interface ITodoService
{
    /// <summary>
    /// Tüm todo'ları getirir
    /// </summary>
    /// <returns>Todo listesi (DTO formatında)</returns>
    Task<IEnumerable<TodoDto>> GetAllTodosAsync();

    /// <summary>
    /// ID'ye göre tek bir todo getirir
    /// </summary>
    /// <param name="id">Aranacak todo'nun ID'si</param>
    /// <returns>Bulunan todo veya null</returns>
    Task<TodoDto?> GetTodoByIdAsync(int id);

    /// <summary>
    /// Tamamlanmış todo'ları getirir
    /// </summary>
    /// <returns>Tamamlanmış todo listesi</returns>
    Task<IEnumerable<TodoDto>> GetCompletedTodosAsync();

    /// <summary>
    /// Bekleyen todo'ları getirir
    /// </summary>
    /// <returns>Bekleyen todo listesi</returns>
    Task<IEnumerable<TodoDto>> GetPendingTodosAsync();

    /// <summary>
    /// Belirli bir öncelik seviyesindeki todo'ları getirir
    /// </summary>
    /// <param name="priority">Öncelik seviyesi (1: Düşük, 2: Orta, 3: Yüksek)</param>
    /// <returns>Belirtilen öncelikteki todo listesi</returns>
    Task<IEnumerable<TodoDto>> GetTodosByPriorityAsync(int priority);

    /// <summary>
    /// Süresi geçmiş todo'ları getirir
    /// </summary>
    /// <returns>Süresi geçmiş todo listesi</returns>
    Task<IEnumerable<TodoDto>> GetOverdueTodosAsync();

    /// <summary>
    /// Yeni bir todo oluşturur
    /// </summary>
    /// <param name="createTodoDto">Oluşturulacak todo'nun verileri</param>
    /// <returns>Oluşturulan todo (DTO formatında)</returns>
    Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto);

    /// <summary>
    /// Mevcut bir todo'yu günceller
    /// </summary>
    /// <param name="id">Güncellenecek todo'nun ID'si</param>
    /// <param name="updateTodoDto">Güncelleme verileri</param>
    /// <returns>Güncellenen todo (DTO formatında)</returns>
    Task<TodoDto> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto);

    /// <summary>
    /// Todo'yu siler
    /// </summary>
    /// <param name="id">Silinecek todo'nun ID'si</param>
    Task DeleteTodoAsync(int id);

    /// <summary>
    /// Todo'yu tamamlandı olarak işaretler
    /// </summary>
    /// <param name="id">İşaretlenecek todo'nun ID'si</param>
    /// <returns>Güncellenen todo (DTO formatında)</returns>
    Task<TodoDto> MarkAsCompletedAsync(int id);

    /// <summary>
    /// Todo'yu bekleyen olarak işaretler
    /// </summary>
    /// <param name="id">İşaretlenecek todo'nun ID'si</param>
    /// <returns>Güncellenen todo (DTO formatında)</returns>
    Task<TodoDto> MarkAsPendingAsync(int id);
}
