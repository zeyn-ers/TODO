using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Interfaces
{
    /// <summary>Category kullanım senaryoları için servis kontratı</summary>
    public interface ICategoryService
    {
        /// <summary>Tüm kategorileri listeler</summary>
        Task<IEnumerable<CategoryDto>> GetAllAsync();

        /// <summary>Aktif kategorileri listeler</summary>
        Task<IEnumerable<CategoryDto>> GetActiveAsync();

        /// <summary>Id'ye göre tek kategori</summary>
        Task<CategoryDto?> GetByIdAsync(int id);

        /// <summary>İsme göre tek kategori</summary>
        Task<CategoryDto?> GetByNameAsync(string name);

        /// <summary>Yeni kategori oluşturur</summary>
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);

        /// <summary>Kategoriyi günceller</summary>
        Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto);

        /// <summary>Kategoriyi siler</summary>
        Task<bool> DeleteAsync(int id);
    }
}
