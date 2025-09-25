using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Controllers.V2;

/// <summary>
/// TodoNotes Controller V2 - Todo notları yönetimi
/// </summary>
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/todos/{todoId:int}/notes")]
public class TodoNotesController : ControllerBase
{
    private readonly ITodoNoteService _todoNoteService;
    private readonly ILogger<TodoNotesController> _logger;

    public TodoNotesController(ITodoNoteService todoNoteService, ILogger<TodoNotesController> logger)
    {
        _todoNoteService = todoNoteService;
        _logger = logger;
    }

    /// <summary>Todo'ya ait notları getirir</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoNoteDto>>> GetNotes(int todoId)
    {
        try
        {
            var notes = await _todoNoteService.GetByTodoIdAsync(todoId);
            return Ok(notes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting notes for todo {TodoId}", todoId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Todo'ya yeni not ekler</summary>
    [HttpPost]
    public async Task<ActionResult<TodoNoteDto>> CreateNote(int todoId, CreateTodoNoteDto dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var note = await _todoNoteService.CreateAsync(todoId, dto);
            return CreatedAtAction(nameof(GetNotes), new { todoId }, note);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating note for todo {TodoId}", todoId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Todo notunu günceller</summary>
    [HttpPut("{noteId:int}")]
    public async Task<ActionResult<TodoNoteDto>> UpdateNote(int todoId, int noteId, UpdateTodoNoteDto dto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _todoNoteService.UpdateAsync(todoId, noteId, dto);
            if (updated == null) return NotFound($"Note with ID {noteId} not found for todo {todoId}");
            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating note {NoteId} for todo {TodoId}", noteId, todoId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Todo notunu siler</summary>
    [HttpDelete("{noteId:int}")]
    public async Task<IActionResult> DeleteNote(int todoId, int noteId)
    {
        try
        {
            var deleted = await _todoNoteService.DeleteAsync(todoId, noteId);
            if (!deleted) return NotFound($"Note with ID {noteId} not found for todo {todoId}");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting note {NoteId} for todo {TodoId}", noteId, todoId);
            return StatusCode(500, "Internal server error");
        }
    }
}