using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GloBus_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public TicketTypesController(IUnitOfWork UnitOfWork)
        {
            unitOfWork = UnitOfWork;
        }

        //get ticket types
        [HttpGet("getTicketTypes"), Authorize]
        public async Task<IActionResult> getTicketTypes()
        {
            List<TicketType> ticketTypes = await unitOfWork.TicketTypesRepository.GetTicketTypes();
            return Ok(ticketTypes);
        }
    }
}
