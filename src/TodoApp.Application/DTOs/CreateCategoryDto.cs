namespace TodoApp.Application.DTOs
{
    /// <summary>Yeni kategori oluşturma DTO</summary>
    public sealed class CreateCategoryDto
    {
        /// <summary>Kategori adı (zorunlu)</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Aktiflik (varsayılan: true)</summary>
        public bool IsActive { get; set; } = true;
    }
}
