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

        [HttpGet("getAllLines"), Authorize]

        public async Task<IActionResult> getAllLines()
        {
            try
            {
                List<Line> line = await unitOfWork.UsersRepository.getAllLines();
                return Ok(line);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

        [HttpGet("getUserTicket"), Authorize]
        public async Task<IActionResult> getUserTicket()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            List<Ticket> tickets = await unitOfWork.UsersRepository.getUserTicket(token);
            return Ok(tickets);
        }

        [HttpPost("addCredit"), Authorize]
        public async Task<IActionResult> Add(AddCreditRequest addCreditRequest)
        {
            
            
            /*Console.WriteLine(request.FirstName);*/
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            User user = await unitOfWork.UsersRepository.AddCredit(token, addCreditRequest);

            return Ok(user);
        }
        [HttpPost("CheckTicket"), Authorize]
        public async Task<IActionResult> CheckTicket(AddCreditDTO AddCreditDTO)
        
            {



                Ticket t = await unitOfWork.UsersRepository.CheckTicket(AddCreditDTO);
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
    }
}

