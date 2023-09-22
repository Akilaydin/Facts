using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OriGames.Facts.Web.Migrations
{
    /// <inheritdoc />
    public partial class NotificationConfigurationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Subject = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Content = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    IsSent = table.Column<bool>(type: "boolean", nullable: false),
                    From = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    To = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Content",
                table: "Notifications",
                column: "Content");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_From",
                table: "Notifications",
                column: "From");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Subject",
                table: "Notifications",
                column: "Subject");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_To",
                table: "Notifications",
                column: "To");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
