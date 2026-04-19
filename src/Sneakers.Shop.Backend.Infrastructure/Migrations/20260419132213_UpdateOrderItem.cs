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
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseItems_Products_ProductId",
                table: "WarehouseItems");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_WarehouseItems_WarehouseItemId",
                table: "Reservations",
                column: "WarehouseItemId",
                principalTable: "WarehouseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseItems_Products_ProductId",
                table: "WarehouseItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_WarehouseItems_WarehouseItemId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseItems_Products_ProductId",
                table: "WarehouseItems");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseItems_Products_ProductId",
                table: "WarehouseItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
