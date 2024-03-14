using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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
        public async Task<IActionResult> Add(UserRegisterDTO request)
        {
            /*Console.WriteLine(request.FirstName);*/
            

            User user = await unitOfWork.UsersRepository.AddUser(request);

            return Ok(user);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(IdDTO IdDTO)
        {
            bool isDeleted = await unitOfWork.UsersRepository.DeleteUser(IdDTO);
            return Ok(isDeleted);
        }

        [HttpPut("approveUser")]
        public async Task<IActionResult> approveUser(int id)
        {
            bool isApproved = await unitOfWork.UsersRepository.ApproveUser(id);
            return Ok(isApproved);
        }
        

        [HttpPost("addInspector")]
        public async Task<IActionResult> addInspector(InspectorDTO inspector)
        {
            User user = await unitOfWork.UsersRepository.AddInspector(inspector);
            return Ok(user);
        }

        [HttpGet("getAll"),Authorize]

        public async Task<IActionResult> getAll()
        {
            try
            {
                List<User> users = await unitOfWork.UsersRepository.getAllUsers();
                return Ok(users); 
            }
            catch (Exception ex)
            {
                return StatusCode(500,  ex.Message);
            }
           
        }

       

        [HttpGet("getTicketTypes"), Authorize]

        public async Task<IActionResult> getTicketTypes()
        {
            try
            {
                List<TicketType> ticketTypes = await unitOfWork.UsersRepository.getTicketTypes();
                return Ok(ticketTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> login(UserLoginDTO request)
        {
           
                var user = await unitOfWork.UsersRepository.loginUser(request);

                return Ok(user);
            
        }

        [HttpGet("getUserById"), Authorize]
        public async Task<IActionResult> GetUserById()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var user = await unitOfWork.UsersRepository.GetUserById(token);
            return Ok(user);
        }

        [HttpPost("addTicket"), Authorize]
        public async Task<IActionResult> Add(TicketDTO request)
        {
            /*Console.WriteLine(request.FirstName);*/


            Ticket ticket = await unitOfWork.UsersRepository.AddTicket(request);

            return Ok("Ticket purchased successfully");
        }

        [HttpGet("getUserTickets"), Authorize]
        public async Task<IActionResult> getUserTickets()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            List<Ticket> tickets = await unitOfWork.UsersRepository.getUserTicket(token);
            return Ok(tickets);
        }

        [HttpPost("addCredit"), Authorize]
        public async Task<IActionResult> Add(CreditDTO addCreditRequest)
        {
            
            
            /*Console.WriteLine(request.FirstName);*/
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            User user = await unitOfWork.UsersRepository.AddCredit(token, addCreditRequest);

            return Ok(user);
        }
        [HttpPost("CheckTicket"), Authorize(Roles = "inspector")]
        public async Task<IActionResult> CheckTicket(TicketIdDTO ticketId)
        
            {
                Ticket t = await unitOfWork.UsersRepository.CheckTicket(ticketId);
                User u = await unitOfWork.UsersRepository.GetUserForPenalty(t.UserId);
                var result = new { ticket = t, user = u };
                return Ok(result);

            }
        [HttpPost("WritePenalty"), Authorize]

        public async Task<IActionResult> WritePenalty(PenaltyDTO penalty)
        {
            bool isWrited = await unitOfWork.UsersRepository.WritePenalty(penalty);
            return Ok(isWrited);

        }

        [HttpGet("getMyWrittenPenalties"), Authorize]
        public async Task<IActionResult> getMyWrittenPenalties()
        {
            List<Penalty> penalties = await unitOfWork.UsersRepository.getMyWrittenPenalties(HttpContext);

            return Ok(penalties);

        }

        [HttpGet("getUnapprovedUsers")]
        public async Task<IActionResult> GetUnapprovedUsers()
        {
            List<User> unapprovedUsers = await unitOfWork.UsersRepository.getUnapprovedUsers();
            return Ok(unapprovedUsers);
        }

        [HttpGet("getAllInspectors")]
        public async Task<IActionResult> GetAllInspectors()
        {
            List<User> inspectors = await unitOfWork.UsersRepository.getAllInspectors();
            return Ok(inspectors);
        }

    }
}

