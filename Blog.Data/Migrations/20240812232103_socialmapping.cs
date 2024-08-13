using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Blog.Data.Migrations
{
    /// <inheritdoc />
    public partial class socialmapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        
            migrationBuilder.InsertData(
                table: "SocialMedia",
                columns: new[] { "Id", "Link", "SocialMediaType" },
                values: new object[,]
                {
                    { 1, "asdfasdf", 1 },
                    { 2, "asadsfasdf", 2 },
                    { 3, "asadsfasdfawdgasd", 3 },
                    { 4, "asadsfaasdfgasdf", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
