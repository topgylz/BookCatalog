using Microsoft.EntityFrameworkCore.Migrations;

namespace BookCatalog.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ff1e5209-4c3c-4a4b-ba13-8c6c60972bf8", "648bb02f-42b8-457b-ac80-d3b779eb69b1", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8e041617-dcc9-4139-bec0-a61a3b752872", "67b26e88-572e-49ee-b19d-e2117e3e4ec9", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e041617-dcc9-4139-bec0-a61a3b752872");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff1e5209-4c3c-4a4b-ba13-8c6c60972bf8");
        }
    }
}
