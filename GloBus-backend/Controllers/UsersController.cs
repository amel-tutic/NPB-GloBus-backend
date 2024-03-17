using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        //add (register) user
        [HttpPost("add")]
        public async Task<IActionResult> Add(UserRegisterDTO request)
        {
            User user = await unitOfWork.UsersRepository.AddUser(request);
            return Ok(user);
        }

        //delete user
        [HttpDelete("delete"), Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(IdDTO idDTO)
        {
            bool isDeleted = await unitOfWork.UsersRepository.DeleteUser(idDTO);
            if(isDeleted)
                return Ok(isDeleted);
            throw new Exception("User doesn't exist");
        }

        //approve user
        [HttpPut("approveUser"), Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveUser(int id)
        {
            bool isApproved = await unitOfWork.UsersRepository.ApproveUser(id);
            if(isApproved)
                return Ok(isApproved);
            throw new Exception("User doesn't exist");
        }

        //add inspector
        [HttpPost("addInspector"), Authorize(Roles = "admin")]
        public async Task<IActionResult> AddInspector(InspectorDTO inspector)
        {
            User user = await unitOfWork.UsersRepository.AddInspector(inspector);
            return Ok(user);
        }

        //get all users
        [HttpGet("getAll"), Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
                List<User> users = await unitOfWork.UsersRepository.GetAllUsers();
                return Ok(users);   
        }

        //login user
        [HttpPost("login")]
        public async Task<IActionResult> login(UserLoginDTO request)
        {
                var user = await unitOfWork.UsersRepository.LoginUser(request);
                return Ok(user); 
        }

        //get user by id
        [HttpGet("getUserById"), Authorize]
        public async Task<IActionResult> GetUserById()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var user = await unitOfWork.UsersRepository.GetUserById(token);
            return Ok(user);
        }

        //add ticket
        [HttpPost("addTicket"), Authorize]
        public async Task<IActionResult> Add(TicketDTO request)
        {
            Ticket ticket = await unitOfWork.UsersRepository.AddTicket(request);
            return Ok("Ticket purchased successfully");
        }

        //get user tickets
        [HttpGet("getUserTickets"), Authorize]
        public async Task<IActionResult> GetUserTickets()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            List<Ticket> tickets = await unitOfWork.UsersRepository.GetUserTickets(token);
            return Ok(tickets);
        }

        //add credit
        [HttpPost("addCredit"), Authorize]
        public async Task<IActionResult> Add(TransactionRequest transactionRequest)
        {
            User user = await unitOfWork.UsersRepository.AddCredit(transactionRequest);
            return Ok(user);
        }

        //send transaction request
        [HttpPost("sendTransactionRequest"), Authorize(Roles = "passenger")]
        public async Task<IActionResult> SendTransactionRequest(TransactionRequestDTO request)
        {
            TransactionRequest transactionRequest = await unitOfWork.UsersRepository.SendTransactionRequest(request);
            return Ok(transactionRequest);
        }

        //check ticket
        [HttpPost("CheckTicket"), Authorize(Roles = "inspector")]
        public async Task<IActionResult> CheckTicket(TicketIdDTO ticketId)
        {
                Ticket t = await unitOfWork.UsersRepository.CheckTicket(ticketId);
                User u = await unitOfWork.UsersRepository.GetUserForPenalty(t.UserId);
                var result = new { ticket = t, user = u };
                return Ok(result);
        }

        //write penalty
        [HttpPost("WritePenalty"), Authorize(Roles = "inspector")]
        public async Task<IActionResult> WritePenalty(PenaltyDTO penalty)
        {
            bool isWritten = await unitOfWork.UsersRepository.WritePenalty(penalty);
            if(isWritten)
                return Ok(isWritten);
            throw new Exception("Error while writing penalty.");
        }

        //get written penalties
        [HttpGet("getMyWrittenPenalties"), Authorize(Roles = "inspector")]
        public async Task<IActionResult> GetMyWrittenPenalties()
        {
            List<Penalty> penalties = await unitOfWork.UsersRepository.GetMyWrittenPenalties(HttpContext);

            return Ok(penalties);
        }

        //get unapproved users
        [HttpGet("getUnapprovedUsers"), Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUnapprovedUsers()
        {
            List<User> unapprovedUsers = await unitOfWork.UsersRepository.GetUnapprovedUsers();
            return Ok(unapprovedUsers);
        }

        //get all inspectors
        [HttpGet("getAllInspectors"), Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllInspectors()
        {
            List<User> inspectors = await unitOfWork.UsersRepository.GetAllInspectors();
            return Ok(inspectors);
        }

        //get all transactions
        [HttpGet("getAllTransactions"), Authorize(Roles = "admin")]
        public async Task<IActionResult> getAllTransactions()
        {
            List<TransactionRequest> inspectors = await unitOfWork.UsersRepository.GetAllTransactions();
            return Ok(inspectors);
        }
    }
}

