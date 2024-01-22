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

namespace GloBus.Infrastructure.Repositories
{
    public class LinesRepository : ILinesRepository
    {
        private readonly GloBusContext context;
        private readonly IMapper mapper;
        private readonly ILogger<Line> logger;

        public LinesRepository(GloBusContext context, IMapper mapper, ILogger<Line> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }
    }
}
