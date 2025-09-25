using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/todos/{todoId:int}/[controller]")]
[Produces("application/json")]
public class TodoNotesV2Controller : ControllerBase
{
    private readonly ITodoNoteService _service;
    private readonly ILogger<TodoNotesV2Controller> _logger;

    public TodoNotesV2Controller(ITodoNoteService service, ILogger<TodoNotesV2Controller> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: api/v2/todos/5/todonotes
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TodoNoteDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<TodoNoteDto>>>> GetAll(int todoId)
    {
        var list = await _service.GetByTodoIdAsync(todoId);
        return Ok(new ApiResponse<IEnumerable<TodoNoteDto>>(list));
    }

    // GET: api/v2/todos/5/todonotes/12
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<TodoNoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TodoNoteDto>>> GetById(int todoId, int id)
    {
        // GetByIdAsync method yok, sadece GetByTodoIdAsync var
        var allNotes = await _service.GetByTodoIdAsync(todoId);
        var item = allNotes.FirstOrDefault(n => n.Id == id);
        if (item is null) return NotFound(new ProblemDetails { Title = "Not found", Status = 404 });
        return Ok(new ApiResponse<TodoNoteDto>(item));
    }

    // POST: api/v2/todos/5/todonotes
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TodoNoteDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<TodoNoteDto>>> Create(int todoId, [FromBody] CreateTodoNoteDto dto)
    {
        var created = await _service.CreateAsync(todoId, dto);
        return CreatedAtAction(nameof(GetById), new { todoId, id = created.Id, version = "2.0" }, new ApiResponse<TodoNoteDto>(created));
    }

    // PUT: api/v2/todos/5/todonotes/12
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<TodoNoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TodoNoteDto>>> Update(int todoId, int id, [FromBody] UpdateTodoNoteDto dto)
    {
        var updated = await _service.UpdateAsync(todoId, id, dto);
        if (updated is null) return NotFound(new ProblemDetails { Title = "Not found", Status = 404 });
        return Ok(new ApiResponse<TodoNoteDto>(updated, "Updated"));
    }

    // DELETE: api/v2/todos/5/todonotes/12
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int todoId, int id)
    {
        var ok = await _service.DeleteAsync(todoId, id);
        return Ok(new ApiResponse<bool>(ok, ok ? "Deleted" : "No change"));
    }
}
