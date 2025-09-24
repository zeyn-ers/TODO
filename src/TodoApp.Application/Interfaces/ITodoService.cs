using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Interfaces
{
    /// <summary>Todo kullanım senaryoları için servis kontratı</summary>
    public interface ITodoService
    {
        Task<IEnumerable<TodoDto>> GetAllAsync();
        Task<TodoDto?> GetByIdAsync(int id);
        Task<TodoDto> CreateAsync(CreateTodoDto dto);
        Task<TodoDto?> UpdateAsync(int id, UpdateTodoDto dto);
        Task<bool> DeleteAsync(int id);

        /// <summary>Belirli kategoriye ait todo’ları listeler</summary>
        Task<IEnumerable<TodoDto>> GetTodosByCategoryAsync(int categoryId); // ✅ eklendi
    }
}
