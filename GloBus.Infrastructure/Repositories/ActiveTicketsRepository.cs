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
    public class ActiveTicketsRepository : IActiveTicketsRepository
    {
        private readonly GloBusContext context;
        private readonly IMapper mapper;
        private readonly ILogger<ActiveTickets> logger;

        public ActiveTicketsRepository(GloBusContext context, IMapper mapper, ILogger<ActiveTickets> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<ActiveTickets> Add(TicketIdDTO ticketId)
        {
            Ticket t = mapper.Map<Ticket>(ticketId);

            t = await context.Ticket.FindAsync(t.Id);

            if(t == null)
            {
                throw new Exception("Ticket doesn't exist!");
            }

            ActiveTickets activeTicket = new ActiveTickets();
            activeTicket.Ticket = t;

            await context.ActiveTickets.AddAsync(activeTicket);

            await context.SaveChangesAsync();

            return activeTicket;
        }

        public async Task<List<ActiveTickets>> GetAll()
        {
            List<ActiveTickets> activeTickets = await context.ActiveTickets
                                                             .Include(at => at.Ticket)
                                                             .ToListAsync();

            return activeTickets;
        }

    }
}
