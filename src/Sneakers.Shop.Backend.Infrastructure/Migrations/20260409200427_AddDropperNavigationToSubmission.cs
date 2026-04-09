using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sneakers.Shop.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDropperNavigationToSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubmissions_UserProfiles_DropId",
                table: "ProductSubmissions",
                column: "DropId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubmissions_UserProfiles_DropId",
                table: "ProductSubmissions");
        }
    }
}
