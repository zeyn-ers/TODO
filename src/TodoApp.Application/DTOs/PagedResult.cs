namespace TodoApp.Application.DTOs;

/// <summary>
/// Sayfalama sonucu için generic sınıf
/// </summary>
/// <typeparam name="T">Veri tipi</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Sayfa verileri
    /// </summary>
    public IEnumerable<T> Data { get; set; } = new List<T>();

    /// <summary>
    /// Toplam kayıt sayısı
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Sayfa numarası (1'den başlar)
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Sayfa boyutu
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Toplam sayfa sayısı
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Önceki sayfa var mı?
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Sonraki sayfa var mı?
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
}