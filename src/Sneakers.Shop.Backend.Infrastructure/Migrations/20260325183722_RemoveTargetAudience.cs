using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sneakers.Shop.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTargetAudience : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_TargetAudience_AudienceId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "TargetAudience");

            migrationBuilder.DropIndex(
                name: "IX_Products_AudienceId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AudienceId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Products",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "TargetAudience",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetAudience",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Products",
                newName: "CreateAt");

            migrationBuilder.AddColumn<Guid>(
                name: "AudienceId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TargetAudience",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetAudience", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_AudienceId",
                table: "Products",
                column: "AudienceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TargetAudience_AudienceId",
                table: "Products",
                column: "AudienceId",
                principalTable: "TargetAudience",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
