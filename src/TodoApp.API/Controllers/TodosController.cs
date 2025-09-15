using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;
    private readonly ILogger<TodosController> _logger;

    public TodosController(ITodoService todoService, ILogger<TodosController> logger)
    {
        _todoService = todoService;
        _logger = logger;
    }

    /// <summary>
    /// Tüm todo'ları getirir
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetAllTodos()
    {
        try
        {
            var todos = await _todoService.GetAllTodosAsync();
            return Ok(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all todos");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// ID'ye göre todo getirir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoDto>> GetTodo(int id)
    {
        try
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound($"Todo with ID {id} not found");
            }
            return Ok(todo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting todo with ID {Id}", id);
            return StatusCode(500, "Internal server error");
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
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TodoDto>> CreateTodo(CreateTodoDto createTodoDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _todoService.CreateTodoAsync(createTodoDto);
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating todo");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Todo'yu günceller
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TodoDto>> UpdateTodo(int id, UpdateTodoDto updateTodoDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _todoService.UpdateTodoAsync(id, updateTodoDto);
            return Ok(todo);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating todo with ID {Id}", id);
            return StatusCode(500, "Internal server error");
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
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodo(int id)
    {
        try
        {
            await _todoService.DeleteTodoAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting todo with ID {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
