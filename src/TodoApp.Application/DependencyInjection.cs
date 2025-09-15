using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Mappings;
using TodoApp.Application.Services;
using TodoApp.Application.Validators;

namespace TodoApp.Application;

/// <summary>
/// Application katmanı için Dependency Injection konfigürasyonu
/// Bu sınıf, Application katmanındaki servisleri DI container'a kaydeder
/// Extension method olarak tanımlanmıştır (AddApplication)
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Application katmanı servislerini DI container'a ekler
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Konfigüre edilmiş service collection</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper - Entity ve DTO arasında dönüşüm yapar
        // MappingProfile sınıfını kullanarak mapping kurallarını yükler
        services.AddAutoMapper(typeof(MappingProfile));

        // FluentValidation - Giriş doğrulama
        // Assembly'deki tüm validator'ları otomatik olarak kaydeder
        services.AddValidatorsFromAssemblyContaining<CreateTodoDtoValidator>();

        // Application Services - İş mantığı servisleri
        // Scoped lifetime: Her HTTP request için yeni instance oluşturur
        services.AddScoped<ITodoService, TodoService>();

        return services;
    }
}
