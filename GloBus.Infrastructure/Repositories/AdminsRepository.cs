using AutoMapper;
using GloBus.Data;
using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure.Exceptions;
using GloBus.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<ApiResponse<Admin>> loginAdmin(AdminDTO adminDTO)
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
                        Message = "Log in successfull",
                        Data = admin,
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
        }

        public async Task<List<Line>> getAllLines()
        {
            List<Line> lines = await context.Line.ToListAsync();
            return lines;
        }


        //token
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
    }
}
