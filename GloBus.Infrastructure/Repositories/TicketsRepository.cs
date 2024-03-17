using AutoMapper;
using GloBus.Data.Models;
using GloBus.Data;
using Microsoft.Extensions.Logging;
using GloBus.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using GloBus.Data.DTOs;

namespace GloBus.Infrastructure.Repositories
{
    public class TicketsRepository : ITicketsRepository
    {
        private readonly GloBusContext context;
        private readonly IMapper mapper;
        private readonly ILogger<Ticket> logger;

        public TicketsRepository(GloBusContext context, IMapper mapper, ILogger<Ticket> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        //approve ticket
        public async Task<Ticket> ApproveTicket(TicketIdDTO ticketId)
        {
            try
            {
                Ticket ticket = await context.Ticket.FindAsync(ticketId.Id);

                if (ticket == null)
                {
                    throw new Exception("Ticket doesn't exist!");
                }

                ticket.isApproved = true;
                ticket.Status = "approved";
                logger.LogInformation($"Approved ticket with id: {ticket.Id}");

                ActiveTickets at = new ActiveTickets
                {
                    Ticket = ticket
                };

                var exists = await context.ActiveTickets
                    .Where(at => at.Ticket == ticket)
                    .FirstOrDefaultAsync();

                if (exists != null)
                {
                    throw new Exception("Ticket is already active!");
                }

                context.ActiveTickets.Add(at);
                await context.SaveChangesAsync();
                logger.LogInformation($"Ticket with id {ticket.Id} added to active tickets.");
                return ticket;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while approving ticket: {ex.Message}");
                throw;
            }
        }

        //reject ticket
        public async Task<Ticket> RejectTicket(TicketIdDTO ticketId)
        {
            try
            {
                Ticket ticket = await context.Ticket.FindAsync(ticketId.Id);

                if (ticket == null)
                {
                    throw new Exception("Ticket doesn't exist!");
                }

                ticket.Status = "rejected";
                logger.LogInformation($"Rejected ticket with id: {ticket.Id}");


                InvalidTickets it = new InvalidTickets
                {
                    Ticket = ticket
                };

                var exists = await context.InvalidTickets
                    .Where(at => at.Ticket == ticket)
                    .FirstOrDefaultAsync();

                if (exists != null)
                {
                    throw new Exception("Ticket is already invalidated!");
                }

                context.InvalidTickets.Add(it);
                await context.SaveChangesAsync();
                logger.LogInformation($"Ticket with id {ticket.Id} added to invalid tickets.");
                return ticket;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while rejecting ticket: {ex.Message}");
                throw;
            }
        }

        //check ticket with scanner
        public async Task<Ticket> CheckTicketWithScanner(int ticketId)
        {
            try
            {
                Ticket t = await context.Ticket
                    .Where(t => t.Id == ticketId)
                    .FirstOrDefaultAsync();

                /*DateTime now = DateTime.Now;*/

                if (t != null)
                {
                    return (t);
                }
                else
                {
                    throw new Exception("Ticket not found");
                };

            }
            catch (Exception ex)
            {
                throw new Exception("Internal server error, ticket id: " + ticketId);
            }
        }

        //delete ticket
        public async Task<bool> DeleteTicket(int id)
        {
            try
            {
                Ticket ticket = await context.Ticket.FindAsync(id);

                if (ticket == null)
                {
                    return false;
                }

                context.Ticket.Remove(ticket);
                await context.SaveChangesAsync();
                logger.LogInformation($"Ticket with id {id} successfully deleted!");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while deleting ticket: {ex.Message}");
                return false;
            }
        }

        //get unapproved tickets
        public async Task<List<Ticket>> GetUnapprovedTickets()
        {
            try
            {
                List<Ticket> tickets = await context.Ticket
                    .Where(t => !t.isApproved && t.Status == "pending")
                    .ToListAsync();

                return tickets;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching unapproved tickets: {ex.Message}");
                throw new Exception("Error fetching unapproved tickets", ex);
            }
        }
    }
}
