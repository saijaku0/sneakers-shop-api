using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sneakers.Shop.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DropperPayouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DropperId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PaidAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropperPayouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropperPayouts_AspNetUsers_DropperId",
                        column: x => x.DropperId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DropperPayouts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSubmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DropId = table.Column<Guid>(type: "uuid", nullable: false),
                    BrandId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetAudience = table.Column<string>(type: "text", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ModeratorId = table.Column<Guid>(type: "uuid", nullable: true),
                    CheckedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSubmissions_AspNetUsers_DropId",
                        column: x => x.DropId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSubmissions_AspNetUsers_ModeratorId",
                        column: x => x.ModeratorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSubmissions_Brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalOrders = table.Column<int>(type: "integer", nullable: false),
                    TotalPayouts = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AverageOrderValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesSnapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DropperPayouts_DropperId",
                table: "DropperPayouts",
                column: "DropperId");

            migrationBuilder.CreateIndex(
                name: "IX_DropperPayouts_ProductId",
                table: "DropperPayouts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DropperPayouts_Status",
                table: "DropperPayouts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubmissions_BrandId",
                table: "ProductSubmissions",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubmissions_DropId",
                table: "ProductSubmissions",
                column: "DropId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubmissions_ModeratorId",
                table: "ProductSubmissions",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubmissions_Status",
                table: "ProductSubmissions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SalesSnapshots_Date",
                table: "SalesSnapshots",
                column: "Date",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DropperPayouts");

            migrationBuilder.DropTable(
                name: "ProductSubmissions");

            migrationBuilder.DropTable(
                name: "SalesSnapshots");
        }
    }
}
