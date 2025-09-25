using TodoApp.Application.DTOs;

namespace TodoApp.Application.Interfaces;

/// <summary>
/// Category service interface'i
/// Category ile ilgili iş mantığı operasyonlarını tanımlar
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Tüm kategorileri getirir
    /// </summary>
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();

    /// <summary>
    /// ID'ye göre kategori getirir
    /// </summary>
    Task<CategoryDto?> GetCategoryByIdAsync(int id);

    /// <summary>
    /// Aktif kategorileri getirir
    /// </summary>
    Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();

    /// <summary>
    /// Yeni kategori oluşturur
    /// </summary>
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto);

    /// <summary>
    /// Kategori günceller
    /// </summary>
    Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateDto);

    /// <summary>
    /// Kategori siler
    /// </summary>
    Task<bool> DeleteCategoryAsync(int id);

    /// <summary>
    /// Sayfalama ile kategorileri getirir
    /// </summary>
    Task<PagedResult<CategoryDto>> GetPagedCategoriesAsync(PaginationParameters parameters);
}