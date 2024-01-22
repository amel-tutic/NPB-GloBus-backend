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

        public async Task<bool> ApproveTicket(int id)
        {
            Ticket ticket = await context.Ticket.FindAsync(id);

            if (ticket == null)
            {
                return false;
            }
            ticket.isApproved = true;
            await context.SaveChangesAsync();
            return true;
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
    }
}
