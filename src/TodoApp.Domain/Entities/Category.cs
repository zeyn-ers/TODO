namespace TodoApp.Domain.Entities;

/// <summary>
/// Category entity'si - Todo'ların ait olduğu kategorileri temsil eder
/// </summary>
public class Category
{
    /// <summary>
    /// Kategorinin benzersiz kimliği (Primary Key)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Kategorinin adı (max 100 karakter)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategorinin açıklaması (max 500 karakter)
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Kategorinin oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Kategori aktif mi?
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Bu kategoriye bağlı todo'lar
    /// </summary>
    public ICollection<Todo> Todos { get; set; } = new List<Todo>();
}
