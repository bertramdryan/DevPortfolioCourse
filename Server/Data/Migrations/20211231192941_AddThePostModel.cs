using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Data.Migrations
{
    public partial class AddThePostModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ThumbnailImagePath = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ThumbnailImagePath = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Excerpt = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 65536, nullable: false),
                    PublishDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Published = table.Column<bool>(type: "INTEGER", nullable: false),
                    Author = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name", "ThumbnailImagePath" },
                values: new object[] { 1, "A description of category 1", "Category 1", "uploads/placeholder.jpg" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name", "ThumbnailImagePath" },
                values: new object[] { 2, "A description of category 2", "Category 2", "uploads/placeholder.jpg" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name", "ThumbnailImagePath" },
                values: new object[] { 3, "A description of category 3", "Category 3", "uploads/placeholder.jpg" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "Author", "CategoryId", "Content", "Excerpt", "PublishDate", "Published", "ThumbnailImagePath", "Title" },
                values: new object[] { 1, "Ryan Bertram", 1, "", "This is an except for 1. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.", new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9873), true, "uploads/placeholders.jpg", "First Post" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "Author", "CategoryId", "Content", "Excerpt", "PublishDate", "Published", "ThumbnailImagePath", "Title" },
                values: new object[] { 2, "Ryan Bertram", 2, "", "This is an except for 2. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.", new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9880), true, "uploads/placeholders.jpg", "Second Post" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "Author", "CategoryId", "Content", "Excerpt", "PublishDate", "Published", "ThumbnailImagePath", "Title" },
                values: new object[] { 3, "Ryan Bertram", 3, "", "This is an except for 3. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.", new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9881), true, "uploads/placeholders.jpg", "Third Post" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "Author", "CategoryId", "Content", "Excerpt", "PublishDate", "Published", "ThumbnailImagePath", "Title" },
                values: new object[] { 4, "Ryan Bertram", 1, "", "This is an except for 4. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.", new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9882), true, "uploads/placeholders.jpg", "Fourth Post" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "Author", "CategoryId", "Content", "Excerpt", "PublishDate", "Published", "ThumbnailImagePath", "Title" },
                values: new object[] { 5, "Ryan Bertram", 2, "", "This is an except for 5. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.", new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9884), true, "uploads/placeholders.jpg", "Fifth Post" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "Author", "CategoryId", "Content", "Excerpt", "PublishDate", "Published", "ThumbnailImagePath", "Title" },
                values: new object[] { 6, "Ryan Bertram", 3, "", "This is an except for 6. An excerpt is a little extranction from the larger piece of text. Sort of like a preview.", new DateTime(2021, 12, 31, 19, 29, 41, 631, DateTimeKind.Utc).AddTicks(9885), true, "uploads/placeholders.jpg", "Sixth Post" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
