using GloBus.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GloBus_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public RegionsController(IUnitOfWork UnitOfWork)
        {
            unitOfWork = UnitOfWork;
        }
    }
}
