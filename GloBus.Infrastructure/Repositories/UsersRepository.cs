﻿using AutoMapper;
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
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Http;

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
       
        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
        {
              
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Role, user.Role)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("INEDODJETUPONOVONIKADAVISEKAOZIVCOVEK"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddMinutes(10);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7269", 
                audience: "https://localhost:7269",
                claims: claims, 
                expires: expires, 
                signingCredentials: credentials 
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
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
                    throw new Exception("User doesn't exists");
                    
                }
                else
                {
                    logger.LogInformation("User added.");
                     return user; 
                }
            
            } 

        }

        //delete user
        public async Task<bool> DeleteUser(IdDTO IdDTO)
        {
            User user = await context.Users.FindAsync(IdDTO.Id);
            if (user == null)
            {
                return false;
            }
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }
            //getAll
            public async Task<List<User>> getAllUsers()
        {
            List<User> users = await context.Users.ToListAsync();
            return users;
        }

        //approve user
        public async Task<bool> ApproveUser(int id)
        {
            User user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            user.IsApproved = true; 
            await context.SaveChangesAsync();
            return true;
        }

        //add inspector
        public async Task<User> AddInspector(InspectorDTO inspector)
        {
            User user = mapper.Map<User>(inspector);

            bool userExists = await context.Users.AnyAsync(u => u.Email == user.Email);

            if (userExists)
            {
                throw new UserExistsException("User already exists");

            }
            else
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(inspector.Password);

                user.Password = hashedPassword;

                await context.Users.AddAsync(user);

                await context.SaveChangesAsync();

                user = await context.Users
                    .Where(u => u.Email == inspector.Email)
                    .FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new Exception("User doesn't exist");

                }
                else
                {
                    logger.LogInformation("User added.");
                    return user;
                }

            }
        }

        public async Task<List<TicketType>> getTicketTypes()
        {
            List<TicketType> types = await context.TicketType.ToListAsync();
            return types;
        }


        //GetUserById
        public async Task<User> GetUserById(String token)
        {
            try
            {
                // Dobijte JWT token iz zaglavlja zahteva

                if (string.IsNullOrEmpty(token))
                {
                    throw new TokenNotFound("JWT is not found.");
                }

                // Dešifrujte JWT token
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken == null)
                {
                    throw new TokenNotFound("Failed decoding of the JWT token.");
                }

                // Dobijte ID korisnika iz Claim-a
                var userId = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    throw new TokenNotFound("ID was not found.");
                   
                }

                // Pretvorite ID korisnika u integer (ili drugi tip koji koristite za ID)
                if (!int.TryParse(userId, out int id))
                {
                    throw new TokenNotFound("Nevalidan format ID korisnika.");
                }
                User u =  await context.Users.FindAsync(id);

                return u;

            }
            catch (Exception ex)
            {
                throw new Exception("Internal server error");

                
            }
        }

        //login
        public async Task<ApiResponse<User>> loginUser(UserLoginDTO userLoginDTO)
        {
            
                User user = mapper.Map<User>(userLoginDTO);

                user = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (user != null)
                {
                    bool passwordMatch = BCrypt.Net.BCrypt.Verify(userLoginDTO.Password, user.Password);

                    if (passwordMatch)
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
                    throw new LoginFailedException("Invalid credentials");
                    
                  
                    
                }

                }
                else
                {
                throw new LoginFailedException("Invalid credentials");


            }



            /* logger.LogInformation(user.FirstName + " logged in");*/


        }
        public async Task<Ticket> AddTicket(TicketDTO TicketDto)
        {
             
            Ticket ticket = mapper.Map<Ticket>(TicketDto);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == ticket.UserId);
            var ticketType = await context.TicketType.FirstOrDefaultAsync(u => u.id == ticket.TicketType);
            user.Credit -= ticketType.price;

            await context.Ticket.AddAsync(ticket);

                await context.SaveChangesAsync();

            ticket = await context.Ticket
                .Where(t => t.UserId == TicketDto.UserId)
                .FirstOrDefaultAsync();
            if (ticket == null)
            {
                throw new Exception("Ticket doesn't exists");

            }
            else
            {
                logger.LogInformation("Ticket added.");
                return ticket;
            }



        }
        public async Task<List<Ticket>> getUserTicket(String token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new TokenNotFound("JWT is not found.");
            }

            // Dešifrujte JWT token
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new TokenNotFound("Failed decoding of the JWT token.");
            }

            // Dobijte ID korisnika iz Claim-a
            var userId = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new TokenNotFound("ID was not found.");

            }

            // Pretvorite ID korisnika u integer (ili drugi tip koji koristite za ID)
            if (!int.TryParse(userId, out int id))
            {
                throw new TokenNotFound("Nevalidan format ID korisnika.");
            }
            List<Ticket> tickets = await context.Ticket
    .Where(ticket => ticket.UserId == id)
    .ToListAsync();
            return tickets;
        }

        public async Task<User> AddCredit(string token, CreditDTO addCreditRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    throw new TokenNotFound("JWT is not found.");
                }

                // Dešifrujte JWT token
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken == null)
                {
                    throw new TokenNotFound("Failed decoding of the JWT token.");
                }

                // Dobijte ID korisnika iz Claim-a
                var userId = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    throw new TokenNotFound("ID was not found.");

                }

                // Pretvorite ID korisnika u integer (ili drugi tip koji koristite za ID)
                if (!int.TryParse(userId, out int id))
                {
                    throw new TokenNotFound("Nevalidan format ID korisnika.");
                }
                // Dobijanje ID-a korisnika iz tokena
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    throw new UserExistsException("User not found");

                  
                }
                else
                {
                    user.Credit += addCreditRequest.Credit;
                    await context.SaveChangesAsync();
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Internal Server Error");
            }
        }
        public async Task<Ticket> CheckTicket(TicketIdDTO ticketId)
        {
            /*Console.WriteLine(ticketId.Id);*/
            try
            {
                Ticket t = await context.Ticket
                    .Where(t => t.Id == ticketId.Id)
                    .FirstOrDefaultAsync();

                DateTime now = DateTime.Now;

                if (t != null)
                {
                    return (t);
                }
                else
                {
                    throw new Exception("Ticket not found");
                };

            }
            catch (Exception ex)
            {
                throw new Exception("Internal server error: " + ticketId.Id);
            }
        }

        public async Task<User> GetUserForPenalty(int id)
        {
            try
            {


                User u = await context.Users.FindAsync(id);
                if (u != null)
                {
                    return u;
                }
                else
                {
                    throw new Exception("User not found");
                }




            }
            catch (Exception ex)
            {
                throw new Exception("Internal server error"); throw new Exception("Internal server error");




            }
        }

        public async Task<bool> WritePenalty(PenaltyDTO penalty)
        {
            try
            {
                Penalty p = mapper.Map<Penalty>(penalty);

                await context.Penalty.AddAsync(p);

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        public async Task<List<Penalty>> getMyWrittenPenalties(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                throw new TokenNotFound("JWT is not found.");
            }

            
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new TokenNotFound("Failed decoding of the JWT token.");
            }

            
            var userId = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new TokenNotFound("ID was not found.");
            }

            
            if (!int.TryParse(userId, out int id))
            {
                throw new TokenNotFound("Nevalidan format ID korisnika.");
            }

            List<Penalty> penalties = await context.Penalty
                .Where(penalty => penalty.InspectorId == id)
                .ToListAsync();
            return penalties;
        }

        public async Task<List<User>> getUnapprovedUsers()
        {
            List<User> users = await context.Users
                .Where(u => u.IsApproved == false && u.Role == "passenger")
                .ToListAsync();
            return users;
        }

        public async Task<List<User>> getAllInspectors()
        {
            List<User> inspectors = await context.Users
                .Where(u => u.Role == "inspector")
                .ToListAsync();
            return inspectors;
        }
    }
}
