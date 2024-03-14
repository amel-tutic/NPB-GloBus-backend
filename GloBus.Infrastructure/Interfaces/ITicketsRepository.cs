using GloBus.Data.DTOs;
using GloBus.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Infrastructure.Interfaces
{
    public interface ITicketsRepository
    {
        Task<Ticket> ApproveTicket(TicketIdDTO ticketId);
        Task<Ticket> RejectTicket(TicketIdDTO ticketId);
        Task<bool> DeleteTicket(int id);
        Task<List<Ticket>> getUnapprovedTickets();
        Task<Ticket> checkTicketWithScanner(int ticketId);
    }
}
