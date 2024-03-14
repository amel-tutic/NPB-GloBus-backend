using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloBus.Data.Migrations
{
    /// <inheritdoc />
    public partial class remove_region : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Regions_RegionId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Line_Regions_RegionId",
                table: "Line");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Line_RegionId",
                table: "Line");

            migrationBuilder.DropIndex(
                name: "IX_Admins_RegionId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Line");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Admins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

