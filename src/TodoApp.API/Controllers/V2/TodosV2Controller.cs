using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Controllers.V2;

/// <summary>
/// Todos Controller V2 - Gelişmiş özellikler ile
/// </summary>
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;
    private readonly ILogger<TodosController> _logger;

    public TodosController(ITodoService todoService, ILogger<TodosController> logger)
    {
        _todoService = todoService;
        _logger = logger;
    }

    /// <summary>Filtreleme ve sıralama ile todo'ları getirir (V2 özelliği)</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetFiltered(
        [FromQuery] int? categoryId = null,
        [FromQuery] bool? done = null,
        [FromQuery] string? sort = null,
        [FromQuery] bool include = false)
    {
        try
        {
            var todos = await _todoService.GetFilteredTodosAsync(categoryId, done, sort, include);
            return Ok(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting filtered todos");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Sayfalama ile todo'ları getirir</summary>
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<TodoDto>>> GetPaged(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] bool include = false)
    {
        try
        {
            var parameters = new PaginationParameters { PageNumber = pageNumber, PageSize = pageSize };
            var result = await _todoService.GetPagedTodosAsync(parameters, include);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting paged todos");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>ID'ye göre todo getirir</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoDto>> GetById(int id)
    {
        try
        {
            var todo = await _todoService.GetByIdAsync(id);
            if (todo is null) return NotFound($"Todo with ID {id} not found");
            return Ok(todo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting todo {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Kategoriye göre todo'ları getirir</summary>
    [HttpGet("by-category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetByCategory(int categoryId)
    {
        try
        {
            var todos = await _todoService.GetTodosByCategoryAsync(categoryId);
            return Ok(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting todos by category {CategoryId}", categoryId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Tamamlanan todo'ları getirir</summary>
    [HttpGet("completed")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetCompleted()
    {
        try
        {
            var todos = await _todoService.GetAllAsync();
            return Ok(todos.Where(t => t.IsCompleted));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting completed todos");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Bekleyen todo'ları getirir</summary>
    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetPending()
    {
        try
        {
            var todos = await _todoService.GetAllAsync();
            return Ok(todos.Where(t => !t.IsCompleted));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting pending todos");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Önceliğe göre todo'ları getirir</summary>
    [HttpGet("priority/{priority:int}")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetByPriority(int priority)
    {
        try
        {
            if (priority < 1 || priority > 3) return BadRequest("Priority must be between 1 and 3");
            var todos = await _todoService.GetAllAsync();
            return Ok(todos.Where(t => t.Priority == priority));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting todos by priority {Priority}", priority);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Süresi geçmiş todo'ları getirir</summary>
    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetOverdue()
    {
        try
        {
            var now = DateTime.UtcNow;
            var todos = await _todoService.GetAllAsync();
            return Ok(todos.Where(t => t.DueDate.HasValue && t.DueDate.Value < now && !t.IsCompleted));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting overdue todos");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Yeni todo oluşturur</summary>
    [HttpPost]
    public async Task<ActionResult<TodoDto>> Create(CreateTodoDto dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var todo = await _todoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating todo");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Todo günceller</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TodoDto>> Update(int id, UpdateTodoDto dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _todoService.UpdateAsync(id, dto);
            if (updated is null) return NotFound($"Todo with ID {id} not found");
            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating todo {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Todo'yu tamamlandı işaretle</summary>
    [HttpPatch("{id:int}/complete")]
    public async Task<ActionResult<TodoDto>> MarkAsCompleted(int id)
    {
        try
        {
            var dto = await _todoService.GetByIdAsync(id);
            if (dto is null) return NotFound($"Todo with ID {id} not found");

            var update = new UpdateTodoDto
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = true,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                CategoryId = dto.CategoryId
            };

            var result = await _todoService.UpdateAsync(id, update);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while marking todo as completed {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Todo'yu bekleyen işaretle</summary>
    [HttpPatch("{id:int}/pending")]
    public async Task<ActionResult<TodoDto>> MarkAsPending(int id)
    {
        try
        {
            var dto = await _todoService.GetByIdAsync(id);
            if (dto is null) return NotFound($"Todo with ID {id} not found");

            var update = new UpdateTodoDto
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                CategoryId = dto.CategoryId
            };

            var result = await _todoService.UpdateAsync(id, update);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while marking todo as pending {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Todo siler</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var ok = await _todoService.DeleteAsync(id);
            if (!ok) return NotFound($"Todo with ID {id} not found");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting todo {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}