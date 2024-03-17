using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GloBus_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvalidTicketsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public InvalidTicketsController(IUnitOfWork UnitOfWork)
        {
            unitOfWork = UnitOfWork;
        }

        //get all invalid tickets
        [HttpGet("getAll"), Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            List<InvalidTickets> invalidTickets = await unitOfWork.InvalidTicketsRepository.GetAll();
            return Ok(invalidTickets);
        }

    }
}
