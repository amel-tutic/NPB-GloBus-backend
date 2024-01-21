using GloBus.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloBus_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public LinesController(IUnitOfWork UnitOfWork)
        {
            unitOfWork = UnitOfWork;
        }
    }
}
