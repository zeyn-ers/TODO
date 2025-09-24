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
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

    // DbSet'ler
    public DbSet<Todo> Todos { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /* ---- TODO ---- */
        modelBuilder.Entity<Todo>(e =>
        {
            e.ToTable("Todos");
            e.HasKey(x => x.Id);

            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(1000);
            e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            e.Property(x => x.Priority).HasDefaultValue(1);
            e.Property(x => x.IsCompleted).HasDefaultValue(false);

            // N:1 ilişki (Todo → Category)
            e.HasOne(x => x.Category)
             .WithMany(c => c.Todos)
             .HasForeignKey(x => x.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        /* ---- CATEGORY ---- */
        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("Categories");
            e.HasKey(x => x.Id);

            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        /* ---- SEED ---- */
        // Category seed (sabit tarihler kullanıyoruz)
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Genel", Description = "Genel görevler", IsActive = true, CreatedAt = new DateTime(2025, 09, 01, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 2, Name = "İş",    Description = "İş ile ilgili görev", IsActive = true, CreatedAt = new DateTime(2025, 09, 01, 0, 0, 0, DateTimeKind.Utc) },
            new Category { Id = 3, Name = "Acil",  Description = "Yüksek öncelik",      IsActive = true, CreatedAt = new DateTime(2025, 09, 01, 0, 0, 0, DateTimeKind.Utc) }
        );

        // Todo seed (CategoryId zorunlu)
        modelBuilder.Entity<Todo>().HasData(
            new Todo
            {
                Id = 1,
                Title = "İlk Todo",
                Description = "Bu bir örnek todo'dur",
                IsCompleted = false,
                CreatedAt = new DateTime(2025, 09, 22, 0, 0, 0, DateTimeKind.Utc),
                Priority = 2,
                CategoryId = 1
            },
            new Todo
            {
                Id = 2,
                Title = "Tamamlanan Todo",
                Description = "Bu todo tamamlanmıştır",
                IsCompleted = true,
                CreatedAt = new DateTime(2025, 09, 21, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 09, 22, 0, 0, 0, DateTimeKind.Utc),
                Priority = 1,
                CategoryId = 1
            }
        );
    }
}
