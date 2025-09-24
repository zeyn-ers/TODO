using System;

namespace TodoApp.Application.DTOs
{
    /// <summary>Todo DTO (API cevapları için)</summary>
    public class TodoDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; }

        /// <summary>Bağlı kategori Id</summary>
        public int CategoryId { get; set; }

        /// <summary>Bağlı kategori adı (Include ile doldurulmalı)</summary>
        public string? CategoryName { get; set; }
    }

    /// <summary>Yeni todo oluşturma DTO'su</summary>
    public class CreateTodoDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; } = 1;

        /// <summary>Opsiyonel kategori Id (null ise backend default set edebilir)</summary>
        public int? CategoryId { get; set; }
    }

    /// <summary>Todo güncelleme DTO'su (PUT)</summary>
    public class UpdateTodoDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; }

        /// <summary>Güncellemede kategori değişimi (opsiyonel)</summary>
        public int? CategoryId { get; set; }
    }
}
