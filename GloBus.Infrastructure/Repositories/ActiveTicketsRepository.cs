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
    }
}
