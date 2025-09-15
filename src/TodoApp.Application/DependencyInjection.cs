using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Mappings;
using TodoApp.Application.Services;
using TodoApp.Application.Validators;

namespace TodoApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<CreateTodoDtoValidator>();

        // Services
        services.AddScoped<ITodoService, TodoService>();

        return services;
    }
}
