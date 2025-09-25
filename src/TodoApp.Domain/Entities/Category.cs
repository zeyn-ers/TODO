using System.ComponentModel.DataAnnotations;

namespace TodoApp.Domain.Entities;

/// <summary>
/// Kategori entity'si - Todo'ları gruplamak için kullanılır
/// </summary>
public class Category
{
    /// <summary>
    /// Kategori ID'si
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Kategori adı (maksimum 100 karakter)
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategori açıklaması (maksimum 500 karakter)
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Kategori aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Bu kategoriye ait todo'lar
    /// </summary>
    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
}