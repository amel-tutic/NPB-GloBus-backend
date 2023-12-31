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
using Microsoft.AspNetCore.Http.HttpResults;
using Azure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        //jwt generate
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.FirstName),
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("INEDODJETUPONOVONIKADAVISEKAOZIVCOVEK"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddHours(8);

            var token = new JwtSecurityToken(
                issuer: "https://globus.rs",
                audience: "https://globus.rs",
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //add user
        public async Task<User> AddUser(UserRegisterDTO userRegisterDTO)
        {
            
            User user = mapper.Map<User>(userRegisterDTO);

            bool userExists = await context.Users.AnyAsync(u => u.Email == user.Email);
           
            if (userExists)
            {
                throw new UserExistsException("User already exists");
            }
            else
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.Password);

                user.Password = hashedPassword;

                await context.Users.AddAsync(user);

                await context.SaveChangesAsync();

                user = await context.Users
                    .Where(u => u.Email == userRegisterDTO.Email)
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

        //getAll
        public async Task<List<User>> getAllUsers()
        {
            List<User> users = await context.Users.ToListAsync();
            return users;
        }

        //login
        public async Task<ApiResponse<User>> loginUser(UserLoginDTO userLoginDTO)
        {
            User user = mapper.Map<User>(userLoginDTO);

            user = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if(user == null)
            {
                throw new LoginFailedException("Invalid credentials.");
            }

            bool passwordMatch = BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.Password);

            if(passwordMatch)
            {
                var token = GenerateJwtToken(user);

                var response = new ApiResponse<User>
                {
                    Status = true,
                    Message = "Log in successfull",
                    Data = user,
                    Token = token
                };

                return response;
            }
            else
            {
                throw new LoginFailedException("Invalid credentials.");
            }

           /* logger.LogInformation(user.FirstName + " logged in");*/
        }
    }
}
