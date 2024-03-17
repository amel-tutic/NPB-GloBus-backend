using AutoMapper;
using GloBus.Data.Models;
using GloBus.Data;
using GloBus.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace GloBus.Infrastructure.Repositories
{
    public class TicketTypesRepository : ITicketTypesRepository
    {
        private readonly GloBusContext context;
        private readonly IMapper mapper;
        private readonly ILogger<TicketType> logger;

        public TicketTypesRepository(GloBusContext context, IMapper mapper, ILogger<TicketType> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        //get ticket types
        public async Task<List<TicketType>> GetTicketTypes()
        {
            try
            {
                List<TicketType> types = await context.TicketType.ToListAsync();
                return types;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching ticket types: {ex.Message}");
                throw new Exception("Error fetching ticket types", ex);
            }
        }
    }
}
