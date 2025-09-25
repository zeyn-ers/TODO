namespace TodoApp.Application.DTOs;

/// <summary>Standart tekil/liste cevap zarfı</summary>
public record ApiResponse<T>(T Data, string? Message = null);
