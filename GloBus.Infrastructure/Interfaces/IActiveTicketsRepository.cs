using GloBus.Data.DTOs;
using GloBus.Data.Models;

namespace GloBus.Infrastructure.Interfaces
{
    public interface IActiveTicketsRepository
    {
        Task<List<ActiveTickets>> GetAll();
        Task<ActiveTickets> Add(TicketIdDTO ticketId);
    }
}
