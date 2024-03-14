using AutoMapper;
using GloBus.Data.Models;
using GloBus.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloBus.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using GloBus.Data.DTOs;

namespace GloBus.Infrastructure.Repositories
{
    public class TicketsRepository : ITicketsRepository
    {
        private readonly GloBusContext context;
        private readonly IMapper mapper;
        private readonly ILogger<Ticket> logger;

        public TicketsRepository(GloBusContext context, IMapper mapper, ILogger<Ticket> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Ticket> ApproveTicket(TicketIdDTO ticketId)
        {
            Ticket ticket = await context.Ticket.FindAsync(ticketId.Id);

            if (ticket == null)
            {
                throw new Exception("Ticket doesn't exist!");
            }
            ticket.isApproved = true;
            ticket.Status = "approved";

            ActiveTickets at = new ActiveTickets();
            at.Ticket = ticket;

            var exists = await context.ActiveTickets
                .Where(at => at.Ticket == ticket)
                .FirstOrDefaultAsync();

            if (exists != null)
                throw new Exception("Ticket is already active!");
            context.ActiveTickets.Add(at);

            await context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket> RejectTicket(TicketIdDTO ticketId)
        {
            Ticket ticket = await context.Ticket.FindAsync(ticketId.Id);

            if (ticket == null)
            {
                throw new Exception("Ticket doesn't exist!");
            }
            ticket.Status = "rejected";

            InvalidTickets it = new InvalidTickets();
            it.Ticket = ticket;

            var exists = await context.InvalidTickets
                .Where(at => at.Ticket == ticket)
                .FirstOrDefaultAsync();

            if (exists != null)
                throw new Exception("Ticket is already invalidated!");
            context.InvalidTickets.Add(it);

            await context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket> checkTicketWithScanner(int ticketId)
        {
            try
            {
                Ticket t = await context.Ticket
                    .Where(t => t.Id == ticketId)
                    .FirstOrDefaultAsync();

                /*DateTime now = DateTime.Now;*/

                if (t != null)
                {
                    return (t);
                }
                else
                {
                    throw new Exception("Ticket not found");
                };

            }
            catch (Exception ex)
            {
                throw new Exception("Internal server error: " + ticketId);
            }
        }

        public async Task<bool> DeleteTicket(int id)
        {
            Ticket ticket = await context.Ticket.FindAsync(id);

            if (ticket == null)
            {
                return false;
            }
            context.Ticket.Remove(ticket);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Ticket>> getUnapprovedTickets()
        {
            List<Ticket> tickets = await context.Ticket
                 .Where(t => t.isApproved == false && t.Status == "pending")
                 .ToListAsync();
            return tickets;
        }
    }
}
