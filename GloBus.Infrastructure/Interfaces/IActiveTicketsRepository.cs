using GloBus.Data.DTOs;
using GloBus.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Infrastructure.Interfaces
{
    public interface IActiveTicketsRepository
    {
        Task<List<ActiveTickets>> GetAll();
        Task<ActiveTickets> Add(TicketIdDTO ticketId);
    }
}
