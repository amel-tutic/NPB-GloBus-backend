using GloBus.Data;
using GloBus.Data.Models;
using Microsoft.EntityFrameworkCore;

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
            var currentDate = DateTime.Now;

            var invalidTickets = context.ActiveTickets
                .Include(at => at.Ticket)
                .Where(at => at.Ticket.ToDate < currentDate)
                .ToList();

            foreach (var invalidTicket in invalidTickets)
            {
                if (invalidTicket.Ticket != null)
                {
                    invalidTicket.Ticket.Status = "expired";
                    context.InvalidTickets.Add(new InvalidTickets
                    {
                        Ticket = invalidTicket.Ticket              
                    });

                    context.ActiveTickets.Remove(invalidTicket);
                }
            }

            context.SaveChanges();

            logger.LogInformation("The recurring job successfully completed!");  
        }

    }
}
