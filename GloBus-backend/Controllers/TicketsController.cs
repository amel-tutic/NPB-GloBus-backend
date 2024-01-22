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
        public async Task<IActionResult> approveTicket(int id)
        {
            bool isApproved = await unitOfWork.TicketsRepository.ApproveTicket(id);
            return Ok(isApproved);
        }

        [HttpDelete("deleteTicket")]
        public async Task<IActionResult> deleteTicket(int id)
        {
            bool isDeleted = await unitOfWork.TicketsRepository.DeleteTicket(id);
            return Ok(isDeleted);
        }
    }
}
