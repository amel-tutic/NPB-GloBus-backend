using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Authorization;
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

        //approve ticket
        [HttpPut("approveTicket"), Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveTicket(TicketIdDTO ticketId)
        {
            Ticket approvedTicket = await unitOfWork.TicketsRepository.ApproveTicket(ticketId);
            return Ok(approvedTicket);
        }

        //reject ticket
        [HttpPut("rejectTicket"), Authorize(Roles = "admin")]
        public async Task<IActionResult> RejectTicket(TicketIdDTO ticketId)
        {
            Ticket rejectedTicket = await unitOfWork.TicketsRepository.RejectTicket(ticketId);
            return Ok(rejectedTicket);
        }

        [HttpDelete("deleteTicket"), Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            bool isDeleted = await unitOfWork.TicketsRepository.DeleteTicket(id);
            if(isDeleted)
                return Ok(isDeleted);
            throw new Exception("Ticket doesn't exist.");
        }

        [HttpGet("getUnapprovedTickets"), Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUnapprovedTickets()
        {
            List<Ticket> unapprovedTickets = await unitOfWork.TicketsRepository.GetUnapprovedTickets();
            return Ok(unapprovedTickets);
        }

        [HttpPost("checkTicketWithScanner"), Authorize(Roles = "inspector")]
        public async Task<IActionResult> CheckTicketWithScanner([FromQuery] int ticketId)
        {
            Ticket t = await unitOfWork.TicketsRepository.CheckTicketWithScanner(ticketId);
            User u = await unitOfWork.UsersRepository.GetUserForPenalty(t.UserId);
            var result = new { ticket = t, user = u};
            return Ok(result);
        }
    }
}
