using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Data.Migrations
{
    /// <inheritdoc />
    public partial class abouts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "About",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[] { 1, "Yenilikçi bir Endüstriyel Tasarımcı olarak, tasarım düşüncesini kullanarak yaratıcı çözümler üretiyorum. Teknoloji tutkumu tasarımlarıma yansıtırken, boş zamanlarımda amatör ressamlık yaparak sanatsal yönümü geliştiriyorum.", "Ben Gül çiçek zengin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "About",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
