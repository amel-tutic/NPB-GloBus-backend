using GloBus.Data.DTOs;
using GloBus.Data.Models;
using Microsoft.AspNetCore.Http;

namespace GloBus.Infrastructure.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> AddUser(UserRegisterDTO request);
        Task<TransactionRequest> SendTransactionRequest(TransactionRequestDTO request);
        Task<bool> DeleteUser(IdDTO idDTO);
        Task<bool> ApproveUser(int id);
        Task<User> AddInspector(InspectorDTO inspector);
        Task<Ticket> AddTicket(TicketDTO request);
        Task<List<User>> GetAllUsers();
        Task<ApiResponse<User>> LoginUser(UserLoginDTO request);
        Task<User> GetUserById(string token);
        Task<List<Ticket>> GetUserTickets(String token);
        Task<User> AddCredit(TransactionRequest transactionRequest);

        Task<bool> WritePenalty(PenaltyDTO penalty);
        Task<List<Penalty>> GetMyWrittenPenalties(HttpContext httpContext);
        Task<Ticket> CheckTicket(TicketIdDTO ticketId);
        Task<User> GetUserForPenalty(int id);
        Task<List<User>> GetUnapprovedUsers();
        Task<List<User>> GetAllInspectors();
        Task<List<TransactionRequest>> GetAllTransactions();
    }
}
