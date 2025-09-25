using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.API.Controllers.V1;

/// <summary>
/// Categories Controller V1 - Temel kategori operasyonları
/// </summary>
[ApiController]
[ApiVersion("1.0")]
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

    /// <summary>Tüm kategorileri getirir</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all categories");
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