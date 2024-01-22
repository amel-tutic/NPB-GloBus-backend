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
        Task<User> AddCredit(String token, CreditDTO AddCreditValue);

        Task<bool> WritePenalty(PenaltyDTO penalty);
        Task<List<Penalty>> getMyWrittenPenalties(HttpContext httpContext);

        Task<Ticket> CheckTicket(CreditDTO AddCreditDTO);
        Task<User> GetUserForPenalty(int id);
    }
}
