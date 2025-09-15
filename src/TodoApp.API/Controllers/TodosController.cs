using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Controllers;

/// <summary>
/// Todo API Controller
/// RESTful API endpoint'lerini tanımlar
/// Bu controller, HTTP isteklerini karşılar ve TodoService'i kullanarak işlemleri yapar
/// </summary>
[ApiController]
[Route("api/[controller]")] // Base route: /api/todos
public class TodosController : ControllerBase
{
    /// <summary>
    /// Todo Service - İş mantığı katmanı
    /// </summary>
    private readonly ITodoService _todoService;
    
    /// <summary>
    /// Logger - Hata loglama ve izleme için
    /// </summary>
    private readonly ILogger<TodosController> _logger;

    /// <summary>
    /// Constructor - Dependency Injection ile gerekli servisleri alır
    /// </summary>
    /// <param name="todoService">Todo iş mantığı servisi</param>
    /// <param name="logger">Loglama servisi</param>
    public TodosController(ITodoService todoService, ILogger<TodosController> logger)
    {
        _todoService = todoService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm todo'ları getirir
    /// HTTP GET /api/todos
    /// </summary>
    /// <returns>Todo listesi</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetAllTodos()
    {
        try
        {
            var todos = await _todoService.GetAllTodosAsync();
            return Ok(todos); // HTTP 200 OK
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all todos");
            return StatusCode(500, "Internal server error"); // HTTP 500 Internal Server Error
        }
    }

    /// <summary>
    /// ID'ye göre todo getirir
    /// HTTP GET /api/todos/{id}
    /// </summary>
    /// <param name="id">Todo ID'si</param>
    /// <returns>Todo veya 404 Not Found</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoDto>> GetTodo(int id)
    {
        try
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound($"Todo with ID {id} not found"); // HTTP 404 Not Found
            }
            return Ok(todo); // HTTP 200 OK
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting todo with ID {Id}", id);
            return StatusCode(500, "Internal server error"); // HTTP 500 Internal Server Error
        }
    }

    /// <summary>
    /// Tamamlanan todo'ları getirir
    /// </summary>
    [HttpGet("completed")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetCompletedTodos()
    {
        try
        {
            var todos = await _todoService.GetCompletedTodosAsync();
            return Ok(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting completed todos");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Bekleyen todo'ları getirir
    /// </summary>
    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetPendingTodos()
    {
        try
        {
            var todos = await _todoService.GetPendingTodosAsync();
            return Ok(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting pending todos");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Önceliğe göre todo'ları getirir
    /// </summary>
    [HttpGet("priority/{priority}")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodosByPriority(int priority)
    {
        try
        {
            if (priority < 1 || priority > 3)
            {
                return BadRequest("Priority must be between 1 and 3");
            }

            var todos = await _todoService.GetTodosByPriorityAsync(priority);
            return Ok(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting todos by priority {Priority}", priority);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Süresi geçmiş todo'ları getirir
    /// </summary>
    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetOverdueTodos()
    {
        try
        {
            var todos = await _todoService.GetOverdueTodosAsync();
            return Ok(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting overdue todos");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Yeni todo oluşturur
    /// HTTP POST /api/todos
    /// </summary>
    /// <param name="createTodoDto">Oluşturulacak todo'nun verileri</param>
    /// <returns>Oluşturulan todo veya hata mesajı</returns>
    [HttpPost]
    public async Task<ActionResult<TodoDto>> CreateTodo(CreateTodoDto createTodoDto)
    {
        try
        {
            // Model validation kontrolü (FluentValidation)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // HTTP 400 Bad Request
            }

            var todo = await _todoService.CreateTodoAsync(createTodoDto);
            // HTTP 201 Created - Yeni kaynak oluşturuldu
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating todo");
            return StatusCode(500, "Internal server error"); // HTTP 500 Internal Server Error
        }
    }

    /// <summary>
    /// Todo'yu günceller
    /// HTTP PUT /api/todos/{id}
    /// </summary>
    /// <param name="id">Güncellenecek todo'nun ID'si</param>
    /// <param name="updateTodoDto">Güncelleme verileri</param>
    /// <returns>Güncellenen todo veya hata mesajı</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<TodoDto>> UpdateTodo(int id, UpdateTodoDto updateTodoDto)
    {
        try
        {
            // Model validation kontrolü (FluentValidation)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // HTTP 400 Bad Request
            }

            var todo = await _todoService.UpdateTodoAsync(id, updateTodoDto);
            return Ok(todo); // HTTP 200 OK
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message); // HTTP 404 Not Found
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating todo with ID {Id}", id);
            return StatusCode(500, "Internal server error"); // HTTP 500 Internal Server Error
        }
    }

    /// <summary>
    /// Todo'yu tamamlandı olarak işaretler
    /// </summary>
    [HttpPatch("{id}/complete")]
    public async Task<ActionResult<TodoDto>> MarkAsCompleted(int id)
    {
        try
        {
            var todo = await _todoService.MarkAsCompletedAsync(id);
            return Ok(todo);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while marking todo as completed with ID {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Todo'yu bekleyen olarak işaretler
    /// </summary>
    [HttpPatch("{id}/pending")]
    public async Task<ActionResult<TodoDto>> MarkAsPending(int id)
    {
        try
        {
            var todo = await _todoService.MarkAsPendingAsync(id);
            return Ok(todo);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while marking todo as pending with ID {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Todo'yu siler
    /// HTTP DELETE /api/todos/{id}
    /// </summary>
    /// <param name="id">Silinecek todo'nun ID'si</param>
    /// <returns>204 No Content veya hata mesajı</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodo(int id)
    {
        try
        {
            await _todoService.DeleteTodoAsync(id);
            return NoContent(); // HTTP 204 No Content - Başarılı silme
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message); // HTTP 404 Not Found
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting todo with ID {Id}", id);
            return StatusCode(500, "Internal server error"); // HTTP 500 Internal Server Error
        }
    }
}
