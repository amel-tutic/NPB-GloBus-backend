using AutoMapper;
using GloBus.Data.Models;
using GloBus.Data;
using Microsoft.Extensions.Logging;
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

        //get all active tickets
        public async Task<List<ActiveTickets>> GetAll()
        {
            try
            {
                List<ActiveTickets> activeTickets = await context.ActiveTickets
                                                                 .Include(at => at.Ticket)
                                                                 .ToListAsync();
                return activeTickets;
            }

            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching active tickets: {ex.Message}");
                throw;
            }
        }

        //add an active ticket
        public async Task<ActiveTickets> Add(TicketIdDTO ticketId)
        {
            try
            {
                Ticket t = mapper.Map<Ticket>(ticketId);
                t = await context.Ticket.FindAsync(t.Id);

                if (t == null)
                {
                    string errorMessage = "Ticket doesn't exist!";
                    logger.LogError(errorMessage);
                    throw new ArgumentException(errorMessage);
                }

                ActiveTickets activeTicket = new ActiveTickets
                {
                    Ticket = t
                };

                await context.ActiveTickets.AddAsync(activeTicket);
                await context.SaveChangesAsync();

                return activeTicket;
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"An error occurred while adding active ticket to the database: {ex.Message}");
                throw;
            }
            catch (ArgumentException ex)
            {
                logger.LogError($"An error occurred: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
