using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PollOptions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PollOptions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PollOptions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PollOptions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PollOptions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Polls",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Polls",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<string[]>(
                name: "Roles",
                table: "Users",
                type: "text[]",
                nullable: false,
                defaultValue: new[] { "User" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[,]
                {
                    { 1, "admin@example.com", "Admin", "admin123" },
                    { 2, "ne_admin@example.com", "NeAdmin", "tochno_ne_admin123" }
                });

            migrationBuilder.InsertData(
                table: "Polls",
                columns: new[] { "Id", "AuthorID", "CreatedAt", "Description", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 11, 15, 16, 38, 19, 802, DateTimeKind.Utc).AddTicks(9720), "Проголосуй за лучший язык программирования.", "Любимый язык программирования", new DateTime(2024, 11, 15, 16, 38, 19, 802, DateTimeKind.Utc).AddTicks(9721) },
                    { 2, 2, new DateTime(2024, 11, 15, 16, 38, 19, 802, DateTimeKind.Utc).AddTicks(9722), "Кто станет президентом США в 2028.", "Президент США", new DateTime(2024, 11, 15, 16, 38, 19, 802, DateTimeKind.Utc).AddTicks(9723) }
                });

            migrationBuilder.InsertData(
                table: "PollOptions",
                columns: new[] { "Id", "Content", "PollId" },
                values: new object[,]
                {
                    { 1, "C#", 1 },
                    { 2, "Java", 1 },
                    { 3, "Python", 1 },
                    { 4, "Трамп(наш слон)", 2 },
                    { 5, "Харрис(не наш слон)", 2 }
                });
        }
    }
}
