using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sneakers.Shop.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionSizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubmissionSize",
                columns: table => new
                {
                    ProductSubmissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    SizeInCm = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionSize", x => new { x.ProductSubmissionId, x.Id });
                    table.ForeignKey(
                        name: "FK_SubmissionSize_ProductSubmissions_ProductSubmissionId",
                        column: x => x.ProductSubmissionId,
                        principalTable: "ProductSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubmissionSize");
        }
    }
}
