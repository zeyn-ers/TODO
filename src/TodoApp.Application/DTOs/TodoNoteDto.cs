namespace TodoApp.Application.DTOs;

/// <summary>
/// TodoNote DTO
/// </summary>
public class TodoNoteDto
{
    public int Id { get; set; }
    public int TodoId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// TodoNote oluşturma DTO
/// </summary>
public class CreateTodoNoteDto
{
    public string Content { get; set; } = string.Empty;
}

/// <summary>
/// TodoNote güncelleme DTO
/// </summary>
public class UpdateTodoNoteDto
{
    public string Content { get; set; } = string.Empty;
}