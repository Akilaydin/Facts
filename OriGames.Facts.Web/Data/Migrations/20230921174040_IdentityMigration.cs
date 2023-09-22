using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OriGames.Facts.Web.Migrations
{
    /// <inheritdoc />
    public partial class IdentityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6f2bf65d-1918-4de6-956d-4fc44d764032", null, "Administrator", "ADMINISTRATOR" },
                    { "ad08d828-7ead-49a4-87db-69548f1d7a00", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d19aaf3f-2af3-4dba-8d33-d33209af8715", 0, "99736276-abd2-4a62-a1e0-3ac9b48b1a62", "test@t.com", true, false, null, "TEST@T.COM", "TEST@T.COM", "AQAAAAIAAYagAAAAEGPxN8KxdgYrZ/xNUho0tjW75sWvhqiK+4e4qflw00RVL3B59TlBSmhn4OonMRxqBQ==", "+79999622215", true, "105307c3-b85a-410b-ba74-4d25e86aa3ef", false, "test@t.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f2bf65d-1918-4de6-956d-4fc44d764032");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad08d828-7ead-49a4-87db-69548f1d7a00");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d19aaf3f-2af3-4dba-8d33-d33209af8715");
        }
    }
}
