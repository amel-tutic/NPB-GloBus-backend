using AutoMapper;
using GloBus.Data.Models;
using GloBus.Data;
using GloBus.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
namespace GloBus.Infrastructure.Repositories
{
    public class RegionsRepository : IRegionsRepository
    {
        private readonly GloBusContext context;
        private readonly IMapper mapper;
  /*      private readonly ILogger<Region> logger;*/

        public RegionsRepository(GloBusContext context, IMapper mapper /*, ILogger<Region> logger*/)
        {
            this.context = context;
            this.mapper = mapper;
           /* this.logger = logger;*/
        }
    }
}
