using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GloBus_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveTicketsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ActiveTicketsController(IUnitOfWork UnitOfWork)
        {
            unitOfWork = UnitOfWork;
        }

        //get all active tickets
        [HttpGet("getAll"), Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
                List<ActiveTickets> activeTickets = await unitOfWork.ActiveTicketsRepository.GetAll();
                return Ok(activeTickets);
        }

        //add an active ticket
        [HttpPost("add"), Authorize(Roles = "admin")]
        public async Task<IActionResult> Add(TicketIdDTO ticketId)
        {
                ActiveTickets activeTicket = await unitOfWork.ActiveTicketsRepository.Add(ticketId);
                return Ok(activeTicket);
        }
    }
}
