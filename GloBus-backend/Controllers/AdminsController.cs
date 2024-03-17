using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Authorization;
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

        //login admin
        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminDTO request)
        {
            var user = await unitOfWork.AdminsRepository.LoginAdmin(request);

            return Ok(user);
        }

        //get all lines for admin
        [HttpGet("getAllLines"), Authorize]
        public async Task<IActionResult> GetAllLines()
        {
                List<Line> lines = await unitOfWork.AdminsRepository.GetAllLines();
                return Ok(lines);
        }

        //promote user to inspector
        [HttpPut("PromoteToInspector"), Authorize(Roles = "admin")]
        public async Task<IActionResult> PromoteToInspector(IdDTO userId)
        {
            User promotedUser = await unitOfWork.AdminsRepository.PromoteToInspector(userId);
            return Ok(promotedUser);
        }

        //demote user from inspector
        [HttpPut("DemoteFromInspector"), Authorize(Roles = "admin")]
        public async Task<IActionResult> DemoteFromInspector(IdDTO userId)
        {
            User promotedUser = await unitOfWork.AdminsRepository.DemoteFromInspector(userId);
            return Ok(promotedUser);
        }

        //delete line
        [HttpDelete("deleteLine"), Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteLine(IdDTO idDTO)
        {
            bool isDeleted = await unitOfWork.AdminsRepository.DeleteLine(idDTO);
            if(isDeleted)
                return Ok(isDeleted);
            throw new Exception("Line doesn't exist.");
        }

        //add line
        [HttpPost("addLine"), Authorize(Roles = "admin")]
        public async Task<IActionResult> AddLine(LineDTO lineDTO)
        {
            Line line = await unitOfWork.AdminsRepository.AddLine(lineDTO);
            return Ok(line);
        }

        //edit line
        [HttpPut("editLine"), Authorize(Roles = "admin")]
        public async Task<IActionResult> EditLine(EditLineDTO editLineDTO)
        {
            Line editedLine = await unitOfWork.AdminsRepository.EditLine(editLineDTO);
            return Ok(editedLine);
        }

        //reject(delete) transaction
        [HttpDelete("rejectTransaction"), Authorize(Roles = "admin")]
        public async Task<IActionResult> RejectTransaction(IdDTO idDTO)
        {
            bool isDeleted = await unitOfWork.AdminsRepository.RejectTransaction(idDTO);
            return Ok(isDeleted);
        }
    }
}
 