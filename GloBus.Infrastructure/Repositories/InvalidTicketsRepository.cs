﻿using AutoMapper;
using GloBus.Data.Models;
using GloBus.Data;
using GloBus.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<List<InvalidTickets>> GetAll()
        {
            List<InvalidTickets> invalidTickets = await context.InvalidTickets
                .Include(it => it.Ticket)
                .ToListAsync();

            return invalidTickets;
        }

    }
}
