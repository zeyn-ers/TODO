using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Controllers.V2;

/// <summary>
/// Categories Controller V2 - Gelişmiş kategori operasyonları
/// </summary>
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>Sayfalama ile kategorileri getirir (V2 özelliği)</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<CategoryDto>>> GetPagedCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var parameters = new PaginationParameters { PageNumber = pageNumber, PageSize = pageSize };
            var result = await _categoryService.GetPagedCategoriesAsync(parameters);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting paged categories");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>ID'ye göre kategori getirir</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found");

            return Ok(category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Aktif kategorileri getirir</summary>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetActiveCategories()
    {
        try
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Kategori istatistikleri getirir (V2 yeni özellik)</summary>
    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetCategoryStats()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var stats = new
            {
                TotalCategories = categories.Count(),
                ActiveCategories = categories.Count(c => c.IsActive),
                InactiveCategories = categories.Count(c => !c.IsActive)
            };
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting category stats");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Yeni kategori oluşturur</summary>
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createDto)
    {
        try
        {
            var category = await _categoryService.CreateCategoryAsync(createDto);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating category");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Kategori günceller</summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, UpdateCategoryDto updateDto)
    {
        try
        {
            var category = await _categoryService.UpdateCategoryAsync(id, updateDto);
            if (category == null)
                return NotFound($"Category with ID {id} not found");

            return Ok(category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Kategoriyi aktif/pasif yapar (V2 yeni özellik)</summary>
    [HttpPatch("{id}/toggle-status")]
    public async Task<ActionResult<CategoryDto>> ToggleCategoryStatus(int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found");

            var updateDto = new UpdateCategoryDto
            {
                Name = category.Name,
                Description = category.Description,
                IsActive = !category.IsActive
            };

            var updated = await _categoryService.UpdateCategoryAsync(id, updateDto);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while toggling category status");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>Kategori siler</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
                return NotFound($"Category with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}