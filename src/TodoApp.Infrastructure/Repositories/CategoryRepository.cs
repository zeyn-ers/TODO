using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories
{
    /// <summary>Category repository implementasyonu</summary>
    public sealed class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly TodoDbContext _db; // base _context ile çakışmasın diye _db

        public CategoryRepository(TodoDbContext context) : base(context)
        {
            _db = context;
        }

        /// <summary>Sadece aktif kategorileri getirir</summary>
        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _db.Categories
                            .Where(c => c.IsActive)
                            .OrderBy(c => c.Name)
                            .ToListAsync();
        }

        /// <summary>İsme göre tek bir kategori döner (yoksa null)</summary>
        public Task<Category?> GetCategoryByNameAsync(string name)
        {
            return _db.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
