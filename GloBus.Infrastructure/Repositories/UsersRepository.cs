using AutoMapper;
using GloBus.Data;
using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure.Exceptions;
using GloBus.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace GloBus.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly GloBusContext context;
        private readonly IMapper mapper;
        private readonly ILogger<User> logger;

        public UsersRepository(GloBusContext context, IMapper mapper, ILogger<User> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<User> AddUser(UserDTO userDTO)
        {
            
            User user = mapper.Map<User>(userDTO);

            bool userExists = await context.Users.AnyAsync(u => u.Email == user.Email);
           
            if (userExists)
            {
                throw new UserExistsException("User already exists");
            }
            else
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

                user.Password = hashedPassword;

                await context.Users.AddAsync(user);

                await context.SaveChangesAsync();

                user = await context.Users
                    .Where(u => u.Email == userDTO.Email)
                    .FirstOrDefaultAsync();
                if(user == null)
                {
                   
                    throw new UserExistsException("User doesn't exists");
                }
                else
                {
                    logger.LogInformation("User added.");
                     return user; 
                }
            
            } 

        }


        public async Task<List<User>> getAllUsers()
        {
            List<User> users = await context.Users.ToListAsync();
            return users;
        }  
    }
}
