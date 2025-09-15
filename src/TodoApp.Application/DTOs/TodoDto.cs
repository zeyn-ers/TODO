namespace TodoApp.Application.DTOs;

/// <summary>
/// Todo Data Transfer Object (DTO)
/// API'den dönen todo verilerini temsil eder
/// Entity'den DTO'ya dönüşüm AutoMapper ile yapılır
/// </summary>
public class TodoDto
{
    /// <summary>
    /// Todo'nun benzersiz kimliği
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Todo'nun başlığı
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Todo'nun detaylı açıklaması
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Todo'nun tamamlanma durumu
    /// </summary>
    public bool IsCompleted { get; set; }
    
    /// <summary>
    /// Todo'nun oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Todo'nun son güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Todo'nun bitiş tarihi
    /// </summary>
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// Todo'nun öncelik seviyesi
    /// </summary>
    public int Priority { get; set; }
}

/// <summary>
/// Todo oluşturma için DTO
/// API'ye yeni todo oluşturmak için gönderilen veri modeli
/// Id, CreatedAt, UpdatedAt alanları otomatik set edilir
/// </summary>
public class CreateTodoDto
{
    /// <summary>
    /// Todo'nun başlığı - Zorunlu alan
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Todo'nun detaylı açıklaması - Opsiyonel
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Todo'nun bitiş tarihi - Opsiyonel
    /// </summary>
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// Todo'nun öncelik seviyesi - Varsayılan: 1 (Düşük)
    /// </summary>
    public int Priority { get; set; } = 1;
}

/// <summary>
/// Todo güncelleme için DTO
/// API'ye mevcut todo'yu güncellemek için gönderilen veri modeli
/// Tüm alanlar güncellenebilir
/// </summary>
public class UpdateTodoDto
{
    /// <summary>
    /// Todo'nun başlığı - Zorunlu alan
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Todo'nun detaylı açıklaması - Opsiyonel
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Todo'nun tamamlanma durumu
    /// </summary>
    public bool IsCompleted { get; set; }
    
    /// <summary>
    /// Todo'nun bitiş tarihi - Opsiyonel
    /// </summary>
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// Todo'nun öncelik seviyesi
    /// </summary>
    public int Priority { get; set; }
}
