using GloBus.Data.DTOs;
using GloBus.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Infrastructure.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> AddUser(UserRegisterDTO request);
        Task<List<User>> getAllUsers();
        Task<ApiResponse<User>> loginUser(UserLoginDTO request);
        
    }
}
