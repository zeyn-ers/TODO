using AutoMapper;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services;

/// <summary>
/// Todo Service Implementation
/// Application katmanında bulunan iş mantığı servisi
/// Bu sınıf, Todo ile ilgili tüm iş operasyonlarını yönetir
/// Repository katmanı ile API katmanı arasında köprü görevi görür
/// </summary>
public class TodoService : ITodoService
{
    /// <summary>
    /// Todo Repository - Veri erişim katmanı
    /// </summary>
    private readonly ITodoRepository _todoRepository;
    
    /// <summary>
    /// AutoMapper - Entity ve DTO arasında dönüşüm yapar
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor - Dependency Injection ile gerekli servisleri alır
    /// </summary>
    /// <param name="todoRepository">Todo repository servisi</param>
    /// <param name="mapper">AutoMapper servisi</param>
    public TodoService(ITodoRepository todoRepository, IMapper mapper)
    {
        _todoRepository = todoRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Tüm todo'ları getirir
    /// Repository'den entity'leri alır ve DTO'ya dönüştürür
    /// </summary>
    /// <returns>Todo listesi (DTO formatında)</returns>
    public async Task<IEnumerable<TodoDto>> GetAllTodosAsync()
    {
        var todos = await _todoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    /// <summary>
    /// ID'ye göre tek bir todo getirir
    /// </summary>
    /// <param name="id">Aranacak todo'nun ID'si</param>
    /// <returns>Bulunan todo veya null</returns>
    public async Task<TodoDto?> GetTodoByIdAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        return todo != null ? _mapper.Map<TodoDto>(todo) : null;
    }

    /// <summary>
    /// Tamamlanmış todo'ları getirir
    /// </summary>
    /// <returns>Tamamlanmış todo listesi</returns>
    public async Task<IEnumerable<TodoDto>> GetCompletedTodosAsync()
    {
        var todos = await _todoRepository.GetCompletedTodosAsync();
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    /// <summary>
    /// Bekleyen todo'ları getirir
    /// </summary>
    /// <returns>Bekleyen todo listesi</returns>
    public async Task<IEnumerable<TodoDto>> GetPendingTodosAsync()
    {
        var todos = await _todoRepository.GetPendingTodosAsync();
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    /// <summary>
    /// Belirli bir öncelik seviyesindeki todo'ları getirir
    /// </summary>
    /// <param name="priority">Öncelik seviyesi (1: Düşük, 2: Orta, 3: Yüksek)</param>
    /// <returns>Belirtilen öncelikteki todo listesi</returns>
    public async Task<IEnumerable<TodoDto>> GetTodosByPriorityAsync(int priority)
    {
        var todos = await _todoRepository.GetTodosByPriorityAsync(priority);
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    /// <summary>
    /// Süresi geçmiş todo'ları getirir
    /// </summary>
    /// <returns>Süresi geçmiş todo listesi</returns>
    public async Task<IEnumerable<TodoDto>> GetOverdueTodosAsync()
    {
        var todos = await _todoRepository.GetOverdueTodosAsync();
        return _mapper.Map<IEnumerable<TodoDto>>(todos);
    }

    /// <summary>
    /// Yeni bir todo oluşturur
    /// DTO'dan Entity'ye dönüşüm yapar ve veritabanına kaydeder
    /// </summary>
    /// <param name="createTodoDto">Oluşturulacak todo'nun verileri</param>
    /// <returns>Oluşturulan todo (DTO formatında)</returns>
    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto)
    {
        // DTO'dan Entity'ye manuel dönüşüm
        var todo = new Todo
        {
            Title = createTodoDto.Title,
            Description = createTodoDto.Description,
            DueDate = createTodoDto.DueDate,
            Priority = createTodoDto.Priority,
            IsCompleted = false, // Yeni todo her zaman bekleyen durumda
            CreatedAt = DateTime.UtcNow // Oluşturulma zamanı
        };

        var createdTodo = await _todoRepository.AddAsync(todo);
        return _mapper.Map<TodoDto>(createdTodo);
    }

    /// <summary>
    /// Mevcut bir todo'yu günceller
    /// Önce todo'yu bulur, sonra günceller
    /// </summary>
    /// <param name="id">Güncellenecek todo'nun ID'si</param>
    /// <param name="updateTodoDto">Güncelleme verileri</param>
    /// <returns>Güncellenen todo (DTO formatında)</returns>
    /// <exception cref="ArgumentException">Todo bulunamazsa fırlatılır</exception>
    public async Task<TodoDto> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto)
    {
        var existingTodo = await _todoRepository.GetByIdAsync(id);
        if (existingTodo == null)
        {
            throw new ArgumentException($"Todo with ID {id} not found.");
        }

        // Entity'yi güncelle
        existingTodo.Title = updateTodoDto.Title;
        existingTodo.Description = updateTodoDto.Description;
        existingTodo.IsCompleted = updateTodoDto.IsCompleted;
        existingTodo.DueDate = updateTodoDto.DueDate;
        existingTodo.Priority = updateTodoDto.Priority;
        existingTodo.UpdatedAt = DateTime.UtcNow; // Güncelleme zamanı

        await _todoRepository.UpdateAsync(existingTodo);
        return _mapper.Map<TodoDto>(existingTodo);
    }

    /// <summary>
    /// Todo'yu siler
    /// Önce todo'nun var olup olmadığını kontrol eder
    /// </summary>
    /// <param name="id">Silinecek todo'nun ID'si</param>
    /// <exception cref="ArgumentException">Todo bulunamazsa fırlatılır</exception>
    public async Task DeleteTodoAsync(int id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null)
        {
            throw new ArgumentException($"Todo with ID {id} not found.");
        }

        await _todoRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Todo'yu tamamlandı olarak işaretler
    /// Sadece IsCompleted ve UpdatedAt alanlarını günceller
    /// </summary>
    /// <param name="id">İşaretlenecek todo'nun ID'si</param>
    /// <returns>Güncellenen todo (DTO formatında)</returns>
    /// <exception cref="ArgumentException">Todo bulunamazsa fırlatılır</exception>
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

    /// <summary>
    /// Todo'yu bekleyen olarak işaretler
    /// Sadece IsCompleted ve UpdatedAt alanlarını günceller
    /// </summary>
    /// <param name="id">İşaretlenecek todo'nun ID'si</param>
    /// <returns>Güncellenen todo (DTO formatında)</returns>
    /// <exception cref="ArgumentException">Todo bulunamazsa fırlatılır</exception>
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
