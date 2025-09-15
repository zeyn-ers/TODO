namespace TodoApp.Domain.Entities;

/// <summary>
/// Todo entity'si - Domain katmanında bulunan ana veri modelimiz
/// Bu sınıf, veritabanındaki Todos tablosunu temsil eder
/// Domain katmanı, iş kurallarını ve veri yapısını içerir
/// </summary>
public class Todo
{
    /// <summary>
    /// Todo'nun benzersiz kimliği (Primary Key)
    /// Veritabanında otomatik artan ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Todo'nun başlığı - Zorunlu alan
    /// Maksimum 200 karakter olabilir
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Todo'nun detaylı açıklaması - Opsiyonel alan
    /// Maksimum 1000 karakter olabilir
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Todo'nun tamamlanma durumu
    /// false: Bekliyor, true: Tamamlandı
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Todo'nun oluşturulma tarihi
    /// Sistem tarafından otomatik set edilir
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Todo'nun son güncellenme tarihi
    /// İlk oluşturulduğunda null, güncellemelerde set edilir
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Todo'nun bitiş tarihi - Opsiyonel
    /// null olabilir, gelecek tarih olmalı
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Todo'nun öncelik seviyesi
    /// 1: Düşük, 2: Orta, 3: Yüksek
    /// Varsayılan değer: 1 (Düşük)
    /// </summary>
    public int Priority { get; set; } = 1;
}
