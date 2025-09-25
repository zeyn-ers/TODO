namespace TodoApp.Domain.Entities;

/// <summary>
/// Todo'ya bağlı not entity'si
/// </summary>
public class TodoNote
{
    public int Id { get; set; }
    public int TodoId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public Todo Todo { get; set; } = null!;
}