using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApp.Application.DTOs;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Interfaces;
using Xunit;

namespace TodoApp.Tests.Services;

/// <summary>
/// CategoryService unit testleri
/// </summary>
public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<CategoryService>> _mockLogger;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _mockRepository = new Mock<ICategoryRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<CategoryService>>();
        _service = new CategoryService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ShouldReturnMappedCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Test1" },
            new Category { Id = 2, Name = "Test2" }
        };
        var categoryDtos = new List<CategoryDto>
        {
            new CategoryDto { Id = 1, Name = "Test1" },
            new CategoryDto { Id = 2, Name = "Test2" }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);
        _mockMapper.Setup(m => m.Map<IEnumerable<CategoryDto>>(categories)).Returns(categoryDtos);

        // Act
        var result = await _service.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        _mockMapper.Verify(m => m.Map<IEnumerable<CategoryDto>>(categories), Times.Once);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WithValidId_ShouldReturnCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Test" };
        var categoryDto = new CategoryDto { Id = 1, Name = "Test" };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
        _mockMapper.Setup(m => m.Map<CategoryDto>(category)).Returns(categoryDto);

        // Act
        var result = await _service.GetCategoryByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task CreateCategoryAsync_ShouldCreateAndReturnCategory()
    {
        // Arrange
        var createDto = new CreateCategoryDto { Name = "New Category" };
        var category = new Category { Id = 1, Name = "New Category" };
        var categoryDto = new CategoryDto { Id = 1, Name = "New Category" };

        _mockMapper.Setup(m => m.Map<Category>(createDto)).Returns(category);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Category>())).ReturnsAsync(category);
        _mockMapper.Setup(m => m.Map<CategoryDto>(category)).Returns(categoryDto);

        // Act
        var result = await _service.CreateCategoryAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Category", result.Name);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Test" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
        _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteCategoryAsync(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}