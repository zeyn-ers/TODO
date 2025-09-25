using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;

namespace TodoApp.Infrastructure;

/// <summary>
/// Infrastructure katmanı için Dependency Injection konfigürasyonu
/// Bu sınıf, Infrastructure katmanındaki servisleri DI container'a kaydeder
/// Extension method olarak tanımlanmıştır (AddInfrastructure)
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Infrastructure katmanı servislerini DI container'a ekler
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Uygulama konfigürasyonu (appsettings.json)</param>
    /// <returns>Konfigüre edilmiş service collection</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Entity Framework DbContext'i kaydet
        // SQL Server connection string'i appsettings.json'dan alır
        services.AddDbContext<TodoDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repository'leri kaydet
        // Scoped lifetime: Her HTTP request için yeni instance oluşturur
        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITodoNoteRepository, TodoNoteRepository>();

        return services;
    }
}
