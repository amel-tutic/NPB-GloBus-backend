using AutoMapper;
using GloBus.Data;
using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure.Exceptions;
using GloBus.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace GloBus.Infrastructure.Repositories
{
    public class AdminsRepository: IAdminsRepository
    {
        private readonly GloBusContext context;
        private readonly IMapper mapper;
        private readonly ILogger<Admin> logger;

        public AdminsRepository(GloBusContext context, IMapper mapper, ILogger<Admin> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.context = context;
        }

        //generate jwt
        public string GenerateJwtToken(Admin admin)
        {
            var claims = new List<Claim>
        {

            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new Claim(ClaimTypes.Role, "admin")
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

        //login admin
        public async Task<ApiResponse<Admin>> LoginAdmin(AdminDTO adminDTO)
        {
            try
            {
                Admin admin = mapper.Map<Admin>(adminDTO);

                admin = await context.Admins.FirstOrDefaultAsync(u => u.Email == admin.Email);

                if (admin != null)
                {
                    bool passwordMatch = BCrypt.Net.BCrypt.Verify(adminDTO.Password, admin.Password);

                    if (passwordMatch)
                    {
                        var token = GenerateJwtToken(admin);

                        var response = new ApiResponse<Admin>
                        {
                            Status = true,
                            Message = "Login successful",
                            Data = admin,
                            Token = token
                        };

                        logger.LogInformation("Admin logged in successfully!");
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
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while logging in: {ex.Message}");
                throw;
            }
        }

        //get all lines for admin
        public async Task<List<Line>> GetAllLines()
        {
            try
            {
                List<Line> lines = await context.Line.ToListAsync();
                return lines;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching all lines: {ex.Message}");
                throw;
            }
        }

        //promote to inspector
        public async Task<User> PromoteToInspector(IdDTO id)
        {
            try
            {
                User user = await context.Users.FindAsync(id.Id);

                if (user == null)
                {
                    throw new Exception("User doesn't exist!");
                }

                user.Role = "inspector";

                await context.SaveChangesAsync();
                logger.LogInformation("User successfully promoted to Inspector!");
                return user;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while promoting user to inspector: {ex.Message}");
                throw;
            }
        }

        //demote from inspector
        public async Task<User> DemoteFromInspector(IdDTO id)
        {
            try
            {
                User user = await context.Users.FindAsync(id.Id);

                if (user == null)
                {
                    throw new Exception("User doesn't exist!");
                }

                user.Role = "passenger";

                await context.SaveChangesAsync();
                logger.LogInformation("User successfully demoted from Inspector!");
                return user;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while demoting user from inspector: {ex.Message}");
                throw;
            }
        }

        //delete line
         public async Task<bool> DeleteLine(IdDTO idDTO)
        {
            try
            {
                Line line = await context.Line.FindAsync(idDTO.Id);

                if (line == null)
                {
                    return false;
                }

                context.Line.Remove(line);
                await context.SaveChangesAsync();
                logger.LogInformation("Line successfully deleted!");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while deleting line: {ex.Message}");
                throw;
            }
        }

        //add line
        public async Task<Line> AddLine(LineDTO lineDTO)
        {
            try
            {
                Line line = mapper.Map<Line>(lineDTO);

                bool lineExists = await context.Line.AnyAsync(u => u.Name == line.Name);

                if (lineExists)
                {
                    throw new UserExistsException("Line already exists");
                }

                await context.Line.AddAsync(line);
                await context.SaveChangesAsync();

                line = await context.Line
                    .Where(u => u.Name == lineDTO.Name)
                    .FirstOrDefaultAsync();

                if (line == null)
                {
                    throw new Exception("Line doesn't exist after adding");
                }

                logger.LogInformation("Line added successfully.");
                return line;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while adding line: {ex.Message}");
                throw;
            }
        }

        //edit line
        public async Task<Line> EditLine(EditLineDTO editLine)
        {
            try
            {
                Line line = await context.Line.FindAsync(editLine.Id);

                if (line == null)
                {
                    throw new Exception("Line not found");
                }

                line.Distance = editLine.Distance;
                line.Name = editLine.Name;
                line.Stations = editLine.Stations;

                await context.SaveChangesAsync();
                logger.LogInformation("Line successfully edited!");
                return line;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while editing line: {ex.Message}");
                throw;
            }
        }

        //reject transaction
        public async Task<bool> RejectTransaction(IdDTO idDTO)
        {
            try
            {
                TransactionRequest transactionRequest = await context.TransactionRequests.FindAsync(idDTO.Id);

                if (transactionRequest == null)
                {
                    return false;
                }

                context.TransactionRequests.Remove(transactionRequest);
                await context.SaveChangesAsync();
                logger.LogInformation("Transaction successfully rejected(deleted)!");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while rejecting transaction: {ex.Message}");
                throw;
            }
        }
    }
}
