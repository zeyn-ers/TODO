using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Domain.Interfaces
{
    /// Category repository interface'i
    /// Category verileri için özel sorguları barındırır
    public interface ICategoryRepository : IRepository<Category>
    {
        /// Sadece aktif kategorileri getirir
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();

        /// İsme göre tek bir kategori döner
        Task<Category?> GetCategoryByNameAsync(string name);
    }
}
