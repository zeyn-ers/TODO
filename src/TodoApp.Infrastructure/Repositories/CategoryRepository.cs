using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories;

/// <summary>
/// Category repository implementasyonu
/// BaseRepository'den türer ve ICategoryRepository interface'ini implement eder
/// </summary>
public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(TodoDbContext context) : base(context) { }

    /// <summary>
    /// Aktif kategorileri getirir
    /// </summary>
    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
    {
        return await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <summary>
    /// İsme göre kategori arar
    /// </summary>
    public async Task<Category?> GetCategoryByNameAsync(string name)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }
}