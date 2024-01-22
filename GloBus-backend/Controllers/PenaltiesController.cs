using GloBus.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloBus_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PenaltiesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PenaltiesController(IUnitOfWork UnitOfWork)
        {
            unitOfWork = UnitOfWork;
        }
    }
}
