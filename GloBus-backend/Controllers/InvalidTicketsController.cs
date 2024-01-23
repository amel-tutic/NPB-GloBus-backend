using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            List<InvalidTickets> invalidTickets = await unitOfWork.InvalidTicketsRepository.GetAll();
            return Ok(invalidTickets);
        }

    }
}
