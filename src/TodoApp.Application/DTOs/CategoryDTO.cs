namespace TodoApp.Application.DTOs
{
    /// <summary>Category DTO (API cevapları)</summary>
    public sealed class CategoryDto
    {
        /// <summary>Kategori Id</summary>
        public int Id { get; set; }

        /// <summary>Kategori adı</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Aktiflik durumu</summary>
        public bool IsActive { get; set; } = true;
    }
}
