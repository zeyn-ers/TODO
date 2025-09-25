namespace TodoApp.Application.DTOs;

/// <summary>Sayfalanmış sonuçlar için cevap zarfı</summary>
public record ApiPaged<T>(PagedResult<T> Data, string? Message = null);
