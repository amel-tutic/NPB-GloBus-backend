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


        [HttpPut("PromoteToInspector")]
        public async Task<IActionResult> PromoteToInspector(IdDTO userId)
        {
            User promotedUser = await unitOfWork.AdminsRepository.PromoteToInspector(userId);
            return Ok(promotedUser);
        }

        [HttpPut("DemoteFromInspector")]
        public async Task<IActionResult> DemoteFromInspector(IdDTO userId)
        {
            User promotedUser = await unitOfWork.AdminsRepository.DemoteFromInspector(userId);
            return Ok(promotedUser);
        }

        [HttpDelete("deleteLine")]
        public async Task<IActionResult> deleteLine(IdDTO IdDTO)
        {
            bool isDeleted = await unitOfWork.AdminsRepository.deleteLine(IdDTO);
            return Ok(isDeleted);
        }

        [HttpPost("addLine")]
        public async Task<IActionResult> addLine(LineDTO lineDTO)
        {
            Line line = await unitOfWork.AdminsRepository.addLine(lineDTO);
            return Ok(line);
        }

        [HttpPut("editLine")]
        public async Task<IActionResult> editLine(EditLineDTO editLineDTO)
        {
            Line editedLine = await unitOfWork.AdminsRepository.editLine(editLineDTO);
            return Ok(editedLine);
        }

        [HttpDelete("rejectTransaction")]
        public async Task<IActionResult> rejectTransaction(IdDTO IdDTO)
        {
            bool isDeleted = await unitOfWork.AdminsRepository.rejectTransaction(IdDTO);
            return Ok(isDeleted);
        }
    }
}
 