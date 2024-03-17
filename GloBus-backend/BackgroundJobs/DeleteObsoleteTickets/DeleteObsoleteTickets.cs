using GloBus.Data;

namespace GloBus_backend.BackgroundJobs.DeleteObsoleteTickets
{
    public class DeleteObsoleteTickets : IDeleteObsoleteTickets
    {
        private readonly GloBusContext context;
        private readonly ILogger<DeleteObsoleteTickets> logger;

        public DeleteObsoleteTickets(GloBusContext context, ILogger<DeleteObsoleteTickets> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void Delete()
        {

        }
    }
}
