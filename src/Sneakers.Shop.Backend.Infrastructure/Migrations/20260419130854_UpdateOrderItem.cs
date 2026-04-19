using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sneakers.Shop.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "WarehouseItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseItemId1",
                table: "OrderItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseItems_ProductId1",
                table: "WarehouseItems",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_WarehouseItemId1",
                table: "OrderItems",
                column: "WarehouseItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_WarehouseItems_WarehouseItemId1",
                table: "OrderItems",
                column: "WarehouseItemId1",
                principalTable: "WarehouseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_WarehouseItems_WarehouseItemId",
                table: "Reservations",
                column: "WarehouseItemId",
                principalTable: "WarehouseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseItems_Products_ProductId1",
                table: "WarehouseItems",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_WarehouseItems_WarehouseItemId1",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_WarehouseItems_WarehouseItemId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseItems_Products_ProductId1",
                table: "WarehouseItems");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseItems_ProductId1",
                table: "WarehouseItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_WarehouseItemId1",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "WarehouseItems");

            migrationBuilder.DropColumn(
                name: "WarehouseItemId1",
                table: "OrderItems");
        }
    }
}
