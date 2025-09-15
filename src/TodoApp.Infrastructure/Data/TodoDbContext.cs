using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.Priority).HasDefaultValue(1);
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
        });

        // Seed data
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
