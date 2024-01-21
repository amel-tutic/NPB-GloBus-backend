using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloBus.Data.Migrations
{
    /// <inheritdoc />
    public partial class change_active_and_invalid_ticket_type_to_ticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ticket",
                table: "InvalidTickets",
                newName: "TicketId");

            migrationBuilder.RenameColumn(
                name: "Ticket",
                table: "ActiveTickets",
                newName: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_InvalidTickets_TicketId",
                table: "InvalidTickets",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveTickets_TicketId",
                table: "ActiveTickets",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveTickets_Ticket_TicketId",
                table: "ActiveTickets",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvalidTickets_Ticket_TicketId",
                table: "InvalidTickets",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveTickets_Ticket_TicketId",
                table: "ActiveTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_InvalidTickets_Ticket_TicketId",
                table: "InvalidTickets");

            migrationBuilder.DropIndex(
                name: "IX_InvalidTickets_TicketId",
                table: "InvalidTickets");

            migrationBuilder.DropIndex(
                name: "IX_ActiveTickets_TicketId",
                table: "ActiveTickets");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "InvalidTickets",
                newName: "Ticket");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "ActiveTickets",
                newName: "Ticket");
        }
    }
}
