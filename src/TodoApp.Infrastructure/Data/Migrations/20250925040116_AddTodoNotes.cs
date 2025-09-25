using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TodoApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTodoNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TodoId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoNotes_Todos_TodoId",
                        column: x => x.TodoId,
                        principalTable: "Todos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TodoNotes",
                columns: new[] { "Id", "Content", "CreatedAt", "TodoId" },
                values: new object[,]
                {
                    { 1, "Bu ilk todo için bir not", new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, "İkinci not örneği", new DateTime(2025, 9, 22, 1, 0, 0, 0, DateTimeKind.Utc), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoNotes_TodoId",
                table: "TodoNotes",
                column: "TodoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoNotes");
        }
    }
}
