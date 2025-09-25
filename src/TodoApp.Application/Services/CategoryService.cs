using AutoMapper;
using Microsoft.Extensions.Logging;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;

namespace TodoApp.Application.Services;

/// <summary>
/// Category service implementasyonu
/// Category ile ilgili iş mantığını yönetir
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category == null ? null : _mapper.Map<CategoryDto>(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync()
    {
        var categories = await _categoryRepository.GetActiveCategoriesAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto)
    {
        try
        {
            _logger.LogInformation("Creating category: {CategoryName}", createDto.Name);
            var category = _mapper.Map<Category>(createDto);
            category.CreatedAt = DateTime.UtcNow;
            
            var createdCategory = await _categoryRepository.AddAsync(category);
            _logger.LogInformation("Category created successfully with ID: {CategoryId}", createdCategory.Id);
            return _mapper.Map<CategoryDto>(createdCategory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create category: {CategoryName}", createDto.Name);
            throw;
        }
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateDto)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null) return null;

        _mapper.Map(updateDto, existingCategory);
        await _categoryRepository.UpdateAsync(existingCategory);
        return _mapper.Map<CategoryDto>(existingCategory);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return false;

        await _categoryRepository.DeleteAsync(category.Id);
        return true;
    }

    public async Task<PagedResult<CategoryDto>> GetPagedCategoriesAsync(PaginationParameters parameters)
    {
        var (items, totalCount) = await _categoryRepository.GetPagedAsync(parameters.PageNumber, parameters.PageSize);
        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(items);
        
        return new PagedResult<CategoryDto>
        {
            Data = categoryDtos,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }
}