using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sneakers.Shop.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTargetAudienceEntitie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AudienceId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TargetAudienceId",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TargetAudience",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetAudience", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_TargetAudienceId",
                table: "Products",
                column: "TargetAudienceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TargetAudience_TargetAudienceId",
                table: "Products",
                column: "TargetAudienceId",
                principalTable: "TargetAudience",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_TargetAudience_TargetAudienceId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "TargetAudience");

            migrationBuilder.DropIndex(
                name: "IX_Products_TargetAudienceId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AudienceId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TargetAudienceId",
                table: "Products");
        }
    }
}
