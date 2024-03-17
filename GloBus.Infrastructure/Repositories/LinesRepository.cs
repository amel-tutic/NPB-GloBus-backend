using AutoMapper;
using GloBus.Data.Models;
using GloBus.Data;
using GloBus.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

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
