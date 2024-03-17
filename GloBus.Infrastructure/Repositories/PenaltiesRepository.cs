using AutoMapper;
using GloBus.Data.Models;
using GloBus.Data;
using GloBus.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace GloBus.Infrastructure.Repositories
{
    public class PenaltiesRepository : IPenaltiesRepository
    {
        private readonly GloBusContext context;
        private readonly IMapper mapper;
        private readonly ILogger<Penalty> logger;

        public PenaltiesRepository(GloBusContext context, IMapper mapper, ILogger<Penalty> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }
    }
}
