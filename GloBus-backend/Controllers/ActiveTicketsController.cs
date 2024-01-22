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
    }
}
