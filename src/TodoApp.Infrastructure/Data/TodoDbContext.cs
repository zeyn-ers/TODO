using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Data;

/// <summary>
/// Entity Framework DbContext sınıfı
/// Bu sınıf, veritabanı ile uygulama arasındaki köprü görevi görür
/// Infrastructure katmanında bulunur ve veri erişim mantığını yönetir
/// </summary>
public class TodoDbContext : DbContext
{
    /// <summary>
    /// Constructor - Dependency Injection ile DbContextOptions alır
    /// Bu sayede connection string ve diğer veritabanı ayarları dışarıdan verilir
    /// </summary>
    /// <param name="options">Veritabanı konfigürasyon seçenekleri</param>
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Todos tablosunu temsil eden DbSet
    /// Bu property üzerinden CRUD operasyonları yapılır
    /// </summary>
    public DbSet<Todo> Todos { get; set; }

    /// <summary>
    /// Model oluşturma konfigürasyonu
    /// Bu metod, entity'lerin veritabanı tablolarına nasıl dönüştürüleceğini belirler
    /// </summary>
    /// <param name="modelBuilder">Model yapılandırıcı</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Todo entity'si için özel konfigürasyonlar
        modelBuilder.Entity<Todo>(entity =>
        {
            // Primary Key tanımı
            entity.HasKey(e => e.Id);
            
            // Title alanı: Zorunlu ve maksimum 200 karakter
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            
            // Description alanı: Opsiyonel ve maksimum 1000 karakter
            entity.Property(e => e.Description).HasMaxLength(1000);
            
            // CreatedAt alanı: Zorunlu
            entity.Property(e => e.CreatedAt).IsRequired();
            
            // Priority alanı: Varsayılan değer 1 (Düşük öncelik)
            entity.Property(e => e.Priority).HasDefaultValue(1);
            
            // IsCompleted alanı: Varsayılan değer false (Bekliyor)
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
        });

        // Seed Data - Veritabanı oluşturulduğunda örnek veriler eklenir
        // Bu veriler, uygulamanın ilk çalıştırılmasında otomatik olarak eklenir
        modelBuilder.Entity<Todo>().HasData(
            new Todo
            {
                Id = 1,
                Title = "İlk Todo",
                Description = "Bu bir örnek todo'dur",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                Priority = 2
            },
            new Todo
            {
                Id = 2,
                Title = "Tamamlanan Todo",
                Description = "Bu todo tamamlanmıştır",
                IsCompleted = true,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow,
                Priority = 1
            }
        );
    }
}
