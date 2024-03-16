﻿using GloBus.Data.DTOs;
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
        Task<TransactionRequest> sendTransactionRequest(TransactionRequestDTO request);
        Task<bool> DeleteUser(IdDTO IdDTO);
        Task<bool> ApproveUser(int id);
        Task<User> AddInspector(InspectorDTO inspector);
        Task<Ticket> AddTicket(TicketDTO request);
        Task<List<User>> getAllUsers();
        Task<List<TicketType>> getTicketTypes();
        Task<ApiResponse<User>> loginUser(UserLoginDTO request);
        Task<User> GetUserById(String token);
        Task<List<Ticket>> getUserTicket(String token);
        Task<User> AddCredit(TransactionRequest transactionRequest);

        Task<bool> WritePenalty(PenaltyDTO penalty);
        Task<List<Penalty>> getMyWrittenPenalties(HttpContext httpContext);
        Task<Ticket> CheckTicket(TicketIdDTO ticketId);
        Task<User> GetUserForPenalty(int id);
        Task<List<User>> getUnapprovedUsers();
        Task<List<User>> getAllInspectors();
        Task<List<TransactionRequest>> getAllTransactions();
    }
}
