using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;

namespace TodoApp.Application
{
    /// <summary>Application katmanı DI kayıtları</summary>
    public static class DependencyInjection
    {
        /// <summary>Application servislerini DI container'a ekler</summary>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITodoService, TodoService>();
            return services;
        }
    }
}
