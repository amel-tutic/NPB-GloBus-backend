using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            List<ActiveTickets> activeTickets = await unitOfWork.ActiveTicketsRepository.GetAll();
            return Ok(activeTickets);
        }

        [HttpPost("add")]
        public async Task<IActionResult> add(TicketIdDTO ticketId)
        {
            ActiveTickets activeTicket = await unitOfWork.ActiveTicketsRepository.Add(ticketId);
            return Ok(activeTicket);
        }
    }
}
