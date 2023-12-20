using AutoMapper;
using GloBus.Data;
using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly GloBusContext context;
        private readonly IMapper mapper;

        public UsersRepository(GloBusContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<User> AddUser(UserDTO userDTO)
        {
            User user = mapper.Map<User>(userDTO);

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();

            user = await context.Users
                .Where(u => u.Email == userDTO.Email)
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
