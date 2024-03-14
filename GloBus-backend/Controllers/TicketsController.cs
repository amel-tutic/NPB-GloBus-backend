using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloBus_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public TicketsController(IUnitOfWork UnitOfWork)
        {
            unitOfWork = UnitOfWork;
        }

        [HttpPut("approveTicket")]
        public async Task<IActionResult> approveTicket(TicketIdDTO ticketId)
        {
            Ticket approvedTicket = await unitOfWork.TicketsRepository.ApproveTicket(ticketId);
            return Ok(approvedTicket);
        }

        [HttpPut("rejectTicket")]
        public async Task<IActionResult> rejectTicket(TicketIdDTO ticketId)
        {
            Ticket rejectedTicket = await unitOfWork.TicketsRepository.RejectTicket(ticketId);
            return Ok(rejectedTicket);
        }

        [HttpDelete("deleteTicket")]
        public async Task<IActionResult> deleteTicket(int id)
        {
            bool isDeleted = await unitOfWork.TicketsRepository.DeleteTicket(id);
            return Ok(isDeleted);
        }

        [HttpGet("getUnapprovedTickets")]
        public async Task<IActionResult> GetUnapprovedTickets()
        {
            List<Ticket> unapprovedTickets = await unitOfWork.TicketsRepository.getUnapprovedTickets();
            return Ok(unapprovedTickets);
        }

        [HttpPost("checkTicketWithScanner")]
        public async Task<IActionResult> CheckTicketWithScanner([FromQuery] int ticketId)
        {
            Ticket t = await unitOfWork.TicketsRepository.checkTicketWithScanner(ticketId);
            User u = await unitOfWork.UsersRepository.GetUserForPenalty(t.UserId);
            var result = new { ticket = t, user = u};
            return Ok(result);
        }
    }
}
