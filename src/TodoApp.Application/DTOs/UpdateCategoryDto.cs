namespace TodoApp.Application.DTOs
{
    /// <summary>Kategori güncelleme DTO</summary>
    public sealed class UpdateCategoryDto
    {
        /// <summary>Kategori adı</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Aktiflik</summary>
        public bool IsActive { get; set; }
    }
}
