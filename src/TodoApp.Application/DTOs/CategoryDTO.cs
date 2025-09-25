namespace TodoApp.Application.DTOs;

/// <summary>
/// Category Data Transfer Object
/// API'den dönen kategori bilgilerini taşır
/// </summary>
public class CategoryDto
{
    /// <summary>
    /// Kategori ID'si
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Kategori adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategori açıklaması
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Kategori aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}