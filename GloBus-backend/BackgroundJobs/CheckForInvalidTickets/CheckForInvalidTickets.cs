using GloBus.Data;

namespace GloBus_backend.BackgroundJobs.CheckForInvalidTickets
{
    public class CheckForInvalidTickets : ICheckForInvalidTickets
    {
        private readonly GloBusContext context;
        private readonly ILogger<CheckForInvalidTickets> logger;

        public CheckForInvalidTickets(GloBusContext context, ILogger<CheckForInvalidTickets> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void Check()
        {
            /*Console.WriteLine("The reccuring job fired off! Hooray!");*/
            logger.LogInformation("The reccuring job fired off! Hooray!");
        }
    }
}
