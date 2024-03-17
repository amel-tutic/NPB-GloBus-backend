using GloBus.Data.DTOs;
using GloBus.Data.Models;

namespace GloBus.Infrastructure.Interfaces
{
    public interface ITicketsRepository
    {
        Task<Ticket> ApproveTicket(TicketIdDTO ticketId);
        Task<Ticket> RejectTicket(TicketIdDTO ticketId);
        Task<bool> DeleteTicket(int id);
        Task<List<Ticket>> GetUnapprovedTickets();
        Task<Ticket> CheckTicketWithScanner(int ticketId);
    }
}
