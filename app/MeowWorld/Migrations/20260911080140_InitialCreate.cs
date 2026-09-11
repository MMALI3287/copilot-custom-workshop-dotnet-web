using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MeowWorld.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Breed = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsFavorite = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cats", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cats",
                columns: new[] { "Id", "Age", "Breed", "CreatedAt", "Description", "IsFavorite", "Name" },
                values: new object[,]
                {
                    { 1, 3, "三毛猫", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "おとなしい性格", false, "みけ" },
                    { 2, 5, "黒猫", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "甘えん坊", false, "くろ" },
                    { 3, 2, "白猫", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "しろ" },
                    { 4, 1, "茶トラ", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "元気いっぱい", false, "チャチャ" },
                    { 5, 4, "ロシアンブルー", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "静かな環境が好き", false, "ソラ" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cats");
        }
    }
}
