namespace TodoApp.Domain.Entities;

/// <summary>
/// Todo entity'si - Domain katmanında bulunan ana veri modelimiz
/// Bu sınıf, veritabanındaki Todos tablosunu temsil eder
/// Domain katmanı, iş kurallarını ve veri yapısını içerir
/// </summary>
public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public int Priority { get; set; } = 1;

    // 🔗 Category ile ilişki
    /// <summary>
    /// Todo'nun bağlı olduğu kategori Id'si (Foreign Key)
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Todo'nun kategorisi (Navigation property)
    /// </summary>
    public Category? Category { get; set; }
}
