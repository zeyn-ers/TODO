using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces;

/// <summary>
/// Category repository interface'i
/// Repository Pattern - veri erişim katmanını soyutlar
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    /// <summary>
    /// Aktif kategorileri getirir
    /// </summary>
    /// <returns>Aktif kategoriler listesi</returns>
    Task<IEnumerable<Category>> GetActiveCategoriesAsync();

    /// <summary>
    /// İsme göre kategori arar
    /// </summary>
    /// <param name="name">Kategori adı</param>
    /// <returns>Bulunan kategori veya null</returns>
    Task<Category?> GetCategoryByNameAsync(string name);
}