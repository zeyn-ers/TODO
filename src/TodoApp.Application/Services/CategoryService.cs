using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services
{
    /// <summary>Category alanı uygulama servisi</summary>
    public sealed class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(_mapper.Map<CategoryDto>);
        }

        public async Task<IEnumerable<CategoryDto>> GetActiveAsync()
        {
            var items = await _repo.GetActiveCategoriesAsync();
            return items.Select(_mapper.Map<CategoryDto>);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto?> GetByNameAsync(string name)
        {
            var entity = await _repo.GetCategoryByNameAsync(name);
            return entity is null ? null : _mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            // aynı isimde kategori var mı? (opsiyonel kontrol)
            var exists = await _repo.GetCategoryByNameAsync(dto.Name);
            if (exists is not null)
                throw new System.InvalidOperationException($"Category '{dto.Name}' already exists.");

            var entity = _mapper.Map<Category>(dto);
            await _repo.AddAsync(entity);                 
            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) return null;

            if (!string.Equals(entity.Name, dto.Name, System.StringComparison.OrdinalIgnoreCase))
            {
                var byName = await _repo.GetCategoryByNameAsync(dto.Name);
                if (byName is not null && byName.Id != id)
                    throw new System.InvalidOperationException($"Category '{dto.Name}' already exists.");
            }

            _mapper.Map(dto, entity);
            await _repo.UpdateAsync(entity);             
            return _mapper.Map<CategoryDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity is null) return false;

            await _repo.DeleteAsync(id);                 
            return true;
        }
    }
}
