using AutoMapper;
using GloBus.Data.Models;
using GloBus.Data;
using GloBus.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
