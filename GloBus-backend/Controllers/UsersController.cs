using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure;
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
        public async Task<IActionResult> Add(UserDTO request)
        {
            Console.WriteLine(request.FirstName);
            

            User user = await unitOfWork.UsersRepository.AddUser(request);

            return Ok(user);
        }

        [HttpGet("getAll")]

        public async Task<IActionResult> getAll()
        {
            try
            {
                List<User> users = await unitOfWork.UsersRepository.getAllUsers();
                return Ok(users); // Vraća 200 OK s listom korisnika
            }
            catch (Exception ex)
            {
                // Ovdje možete dodati logiku za rukovanje greškama
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
           
        }


    }
}
