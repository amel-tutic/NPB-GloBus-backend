using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace GloBus_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;


        public AdminsController(IUnitOfWork UnitOfWork)
        {
            unitOfWork = UnitOfWork;
        }


        [HttpPost("login")]
        public async Task<IActionResult> login(AdminDTO request)
        {

            var user = await unitOfWork.AdminsRepository.loginAdmin(request);

            return Ok(user);

        }

        [HttpGet("getAllLines")]

        public async Task<IActionResult> getAllLines()
        {
            try
            {
                List<Line> lines = await unitOfWork.AdminsRepository.getAllLines();
                return Ok(lines);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

    }
}
