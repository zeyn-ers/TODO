using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;
using Xunit;

namespace TodoApp.Tests.Repositories;

/// <summary>
/// CategoryRepository unit testleri
/// </summary>
public class CategoryRepositoryTests : IDisposable
{
    private readonly TodoDbContext _context;
    private readonly CategoryRepository _repository;

    public CategoryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TodoDbContext(options);
        _repository = new CategoryRepository(_context);
    }

    [Fact]
    public async Task GetActiveCategoriesAsync_ShouldReturnOnlyActiveCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Active1", IsActive = true },
            new Category { Id = 2, Name = "Inactive", IsActive = false },
            new Category { Id = 3, Name = "Active2", IsActive = true }
        };

        _context.Categories.AddRange(categories);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetActiveCategoriesAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, c => Assert.True(c.IsActive));
    }

    [Fact]
    public async Task GetCategoryByNameAsync_WithExistingName_ShouldReturnCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "TestCategory", IsActive = true };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetCategoryByNameAsync("TestCategory");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestCategory", result.Name);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_WithNonExistingName_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetCategoryByNameAsync("NonExisting");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnCorrectPageAndCount()
    {
        // Arrange
        var categories = new List<Category>();
        for (int i = 1; i <= 15; i++)
        {
            categories.Add(new Category { Id = i, Name = $"Category{i}", IsActive = true });
        }

        _context.Categories.AddRange(categories);
        await _context.SaveChangesAsync();

        // Act
        var (items, totalCount) = await _repository.GetPagedAsync(2, 5);

        // Assert
        Assert.Equal(5, items.Count());
        Assert.Equal(15, totalCount);
        Assert.Equal("Category6", items.First().Name);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}