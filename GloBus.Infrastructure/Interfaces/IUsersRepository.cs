using GloBus.Data.DTOs;
using GloBus.Data.Models;
using Microsoft.AspNetCore.Http;
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
        Task<Ticket> AddTicket(TicketDTO request);
        Task<List<User>> getAllUsers();
        
        Task<List<Line>> getAllLines();
        Task<List<TicketType>> getTicketTypes();
        Task<ApiResponse<User>> loginUser(UserLoginDTO request);
        Task<User> GetUserById(String token);
        Task<List<Ticket>> getUserTicket(String token);
        Task<User> GetUserForPenalty(int id);
        Task<Ticket> CheckTicket(AddCreditDTO AddCreditDTO);
        Task<User> AddCredit(String token, AddCreditDTO AddCreditDTO);
        Task<bool> WritePenalty(PenaltyDTO penalty);
        Task<List<Penalty>> getMyWrittenPenalties(HttpContext httpContext);



    }
}
