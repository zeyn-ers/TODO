using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixTodoCategoryDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Mevcut default constraint adını bul ve kaldır
            migrationBuilder.Sql(@"
DECLARE @dc sysname, @sql nvarchar(max);
SELECT @dc = d.name
FROM sys.default_constraints d
JOIN sys.columns c
  ON d.parent_object_id = c.object_id AND d.parent_column_id = c.column_id
WHERE d.parent_object_id = OBJECT_ID(N'[Todos]')
  AND c.name = N'CategoryId';

IF @dc IS NOT NULL
BEGIN
    SET @sql = N'ALTER TABLE [Todos] DROP CONSTRAINT [' + @dc + N']';
    EXEC(@sql);
END
");

            // 2) Yeni default: 1
            migrationBuilder.Sql("ALTER TABLE [Todos] ADD DEFAULT (1) FOR [CategoryId];");

            // 3) Güvenlik: 0 olanları 1'e çek
            migrationBuilder.Sql("UPDATE [Todos] SET [CategoryId] = 1 WHERE [CategoryId] = 0;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Default constraint’i kaldır (geri alma)
            migrationBuilder.Sql(@"
DECLARE @dc sysname, @sql nvarchar(max);
SELECT @dc = d.name
FROM sys.default_constraints d
JOIN sys.columns c
  ON d.parent_object_id = c.object_id AND d.parent_column_id = c.column_id
WHERE d.parent_object_id = OBJECT_ID(N'[Todos]')
  AND c.name = N'CategoryId';

IF @dc IS NOT NULL
BEGIN
    SET @sql = N'ALTER TABLE [Todos] DROP CONSTRAINT [' + @dc + N']';
    EXEC(@sql);
END
");
        }
    }
}
