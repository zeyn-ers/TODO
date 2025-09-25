using System.ComponentModel.DataAnnotations;

namespace TodoApp.Application.DTOs;

/// <summary>
/// Kategori güncelleme DTO'su
/// API'ye gönderilen kategori güncelleme verilerini taşır
/// </summary>
public class UpdateCategoryDto
{
    /// <summary>
    /// Kategori adı (zorunlu, maksimum 100 karakter)
    /// </summary>
    [Required(ErrorMessage = "Kategori adı zorunludur")]
    [MaxLength(100, ErrorMessage = "Kategori adı maksimum 100 karakter olabilir")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategori açıklaması (opsiyonel, maksimum 500 karakter)
    /// </summary>
    [MaxLength(500, ErrorMessage = "Açıklama maksimum 500 karakter olabilir")]
    public string? Description { get; set; }

    /// <summary>
    /// Kategori aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;
}