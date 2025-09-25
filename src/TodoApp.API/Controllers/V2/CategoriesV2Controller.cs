using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class CategoriesV2Controller : ControllerBase
{
    private readonly ICategoryService _service;
    private readonly ILogger<CategoriesV2Controller> _logger;

    public CategoriesV2Controller(ICategoryService service, ILogger<CategoriesV2Controller> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET: api/v2/categories
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetAll()
    {
        var list = await _service.GetAllCategoriesAsync();
        return Ok(new ApiResponse<IEnumerable<CategoryDto>>(list));
    }

    // GET: api/v2/categories/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetById(int id)
    {
        var item = await _service.GetCategoryByIdAsync(id);
        if (item is null) return NotFound(new ProblemDetails { Title = "Not found", Status = 404 });
        return Ok(new ApiResponse<CategoryDto>(item));
    }

    // POST: api/v2/categories
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> Create([FromBody] CreateCategoryDto dto)
    {
        var created = await _service.CreateCategoryAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id, version = "2.0" }, new ApiResponse<CategoryDto>(created));
    }

    // PUT: api/v2/categories/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        var updated = await _service.UpdateCategoryAsync(id, dto);
        if (updated is null) return NotFound(new ProblemDetails { Title = "Not found", Status = 404 });
        return Ok(new ApiResponse<CategoryDto>(updated, "Updated"));
    }

    // DELETE: api/v2/categories/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        // FK nedeniyle InvalidOperationException fırlarsa global handler ProblemDetails döndürecek
        var ok = await _service.DeleteCategoryAsync(id);
        return Ok(new ApiResponse<bool>(ok, ok ? "Deleted" : "No change"));
    }
}
