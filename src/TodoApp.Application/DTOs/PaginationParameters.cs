using System.ComponentModel.DataAnnotations;

namespace TodoApp.Application.DTOs;

/// <summary>
/// Sayfalama parametreleri
/// </summary>
public class PaginationParameters
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    /// <summary>
    /// Sayfa numarası (1'den başlar)
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Sayfa numarası 1'den büyük olmalıdır")]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Sayfa boyutu (maksimum 100)
    /// </summary>
    [Range(1, MaxPageSize, ErrorMessage = "Sayfa boyutu 1-100 arasında olmalıdır")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}