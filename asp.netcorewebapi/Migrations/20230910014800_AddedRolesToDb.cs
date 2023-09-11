using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asp.netcorewebapi.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "05319b2c-7dbc-4665-8584-be1ebaa8701a", "d575d43f-165b-436b-9eef-d7dfd6ec40cd", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f2eb0c69-61b5-4d87-9277-16e6dfc8c4fe", "1ab7e12c-fd50-4008-b397-cd957f79e7c2", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05319b2c-7dbc-4665-8584-be1ebaa8701a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2eb0c69-61b5-4d87-9277-16e6dfc8c4fe");
        }
    }
}
