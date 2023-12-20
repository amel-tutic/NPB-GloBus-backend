using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloBus_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UsersController(IUnitOfWork UnitOfWork)
        {
            unitOfWork = UnitOfWork;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(UserDTO request)
        {
            User user = await unitOfWork.UsersRepository.AddUser(request);

            return Ok(user);
        }
    }
}
