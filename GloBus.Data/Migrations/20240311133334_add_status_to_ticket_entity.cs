using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloBus.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_status_to_ticket_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Ticket");
        }
    }
}
