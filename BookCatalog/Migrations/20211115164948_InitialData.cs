using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCatalog.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Name", "Year" },
                values: new object[] { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Мёртвые души", 1842 });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Name", "Year" },
                values: new object[] { new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "Капитанская дочка", 1836 });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "BookId", "Surname" },
                values: new object[] { new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Гоголь" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "BookId", "Surname" },
                values: new object[] { new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "Пушкин" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: new Guid("80abbca8-664d-4b20-b5de-024705497d4a"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"));
        }
    }
}
