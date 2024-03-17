using AutoMapper;
using GloBus.Data.Models;
using GloBus.Data;
using GloBus.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace GloBus.Infrastructure.Repositories
{
    public class InvalidTicketsRepository : IInvalidTicketsRepository
    {
        private readonly GloBusContext context;
        private readonly IMapper mapper;
        private readonly ILogger<InvalidTickets> logger;

        public InvalidTicketsRepository(GloBusContext context, IMapper mapper, ILogger<InvalidTickets> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        //get all invalid tickets
        public async Task<List<InvalidTickets>> GetAll()
        {
            try
            {
                List<InvalidTickets> invalidTickets = await context.InvalidTickets
                    .Include(it => it.Ticket)
                    .ToListAsync();

                return invalidTickets;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching invalid tickets: {ex.Message}");
                throw new Exception("Error fetching invalid tickets", ex);
            }
        }

    }
}
