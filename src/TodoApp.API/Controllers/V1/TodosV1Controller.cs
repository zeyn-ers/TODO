using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Controllers.V1;

/// <summary>
/// Todos Controller V1 - Temel CRUD operasyonları
/// </summary>
[ApiController]
[ApiVersion("1.0")]
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

    /// <summary>Tüm todo'ları getirir</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetAll()
    {
        try
        {
            var todos = await _todoService.GetAllAsync();
            return Ok(todos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting all todos");
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