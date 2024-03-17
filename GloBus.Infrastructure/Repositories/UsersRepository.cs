using AutoMapper;
using GloBus.Data;
using GloBus.Data.DTOs;
using GloBus.Data.Models;
using GloBus.Infrastructure.Exceptions;
using GloBus.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
/*using static System.Net.WebRequestMethods;*/
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

        //generate jwt
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

        //add (register) user
        public async Task<User> AddUser(UserRegisterDTO userRegisterDTO)
        {
            try
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

                    if (user == null)
                    {
                        throw new Exception("User not found after registration");
                    }
                    else
                    {
                        logger.LogInformation("User successfully added.");
                        return user;
                    }
                }
            }
            catch (UserExistsException ex)
            {
                logger.LogError($"User already exists: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred during user registration: {ex.Message}");
                throw;
            }
        }

        //delete user
        public async Task<bool> DeleteUser(IdDTO idDTO)
        {
            try
            {
                User user = await context.Users.FindAsync(idDTO.Id);

                if (user == null)
                {
                    return false;
                }

                context.Users.Remove(user);
                await context.SaveChangesAsync();
                logger.LogInformation($"User with id {idDTO.Id} successfully deleted.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while deleting user: {ex.Message}");
                throw new Exception("Error deleting user", ex);
            }
        }

        //get all users
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                List<User> users = await context.Users.ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching all users: {ex.Message}");
                throw new Exception("Error fetching all users", ex);
            }
        }

        //approve user
        public async Task<bool> ApproveUser(int id)
        {
            try
            {
                User user = await context.Users.FindAsync(id);

                if (user == null)
                {
                    return false;
                }

                user.IsApproved = true;
                await context.SaveChangesAsync();
                logger.LogInformation($"User with id {id} successfully approved.");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while approving user: {ex.Message}");
                throw new Exception("Error approving user", ex);
            }
        }

        //add inspector
        public async Task<User> AddInspector(InspectorDTO inspector)
        {
            try
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
                        throw new Exception("User not found after addition");
                    }
                    else
                    {
                        logger.LogInformation("User added.");
                        return user;
                    }
                }
            }
            catch (UserExistsException ex)
            {
                logger.LogError($"User already exists: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while adding user: {ex.Message}");
                throw;
            }
        }

        //GetUserById
        public async Task<User> GetUserById(string token)
        {
            try
            {
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

                // parse userid to int
                if (!int.TryParse(userId, out int id))
                {
                    throw new TokenNotFound("Invalid format user id");
                }
                User u =  await context.Users.FindAsync(id);
                if (u == null)
                {
                    throw new Exception("User not found.");
                }

                return u;
            }
            catch (TokenNotFound ex)
            {
                logger.LogError($"Token not found: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while getting user by ID: {ex.Message}");
                throw;
            }
        }

        //login user
        public async Task<ApiResponse<User>> LoginUser(UserLoginDTO userLoginDTO)
        {
            try
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
                            Message = "Log in successful",
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
                    throw new LoginFailedException("User not found");
                }
            }
            catch (LoginFailedException ex)
            {
                logger.LogError($"Login failed: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred during login: {ex.Message}");
                throw new Exception("Internal server error");
            }
        }

        //add ticket
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

        //get user tickets
        public async Task<List<Ticket>> GetUserTickets(string token)
        {
            try
            {
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

                // parse userId to int
                if (!int.TryParse(userId, out int id))
                {
                    throw new TokenNotFound("Invalid user ID format.");
                }

                List<Ticket> tickets = await context.Ticket
                                                    .Where(ticket => ticket.UserId == id)
                                                    .ToListAsync();
                return tickets;
            }
            catch (TokenNotFound ex)
            {
                logger.LogError($"Token not found: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching user tickets: {ex.Message}");
                throw new Exception("Internal server error");
            }
        }

        //add credit
        public async Task<User> AddCredit(TransactionRequest transactionRequest)
        {
            try
            {
                TransactionRequest transactionReq = await context.TransactionRequests.FindAsync(transactionRequest.Id);

               /* if (transactionReq == null)
                {
                    throw new NotFoundException("Transaction request not found");
                }*/

                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == transactionRequest.UserId);

                if (user == null)
                {
                    throw new UserExistsException("User not found");
                }

                user.Credit += transactionRequest.Credit;
                context.TransactionRequests.Remove(transactionReq);
                await context.SaveChangesAsync();

                logger.LogInformation($"Credit added successfully for User ID: {user.Id}");
                return user;
            }
            /*catch (NotFoundException ex)
            {
                logger.LogError($"Error adding credit: {ex.Message}");
                throw;
            }*/
            catch (UserExistsException ex)
            {
                logger.LogError($"Error adding credit: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while adding credit: {ex.Message}");
                throw new Exception("Internal Server Error");
            }
        }

        //check ticket
        public async Task<Ticket> CheckTicket(TicketIdDTO ticketId)
        {
            try
            {
                Ticket ticket = await context.Ticket
                    .Where(t => t.Id == ticketId.Id)
                    .FirstOrDefaultAsync();

                if (ticket != null)
                {
                    return ticket;
                }
                else
                {
                    /*throw new TicketNotFoundException("Ticket not found");*/
                    throw new Exception("Ticket not found");
                }
            }
            /*catch (TicketNotFoundException ex)
            {
                logger.LogError($"Ticket not found: {ex.Message}");
                throw;
            }*/
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while checking ticket: {ex.Message}");
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        //get user for penalty
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
                logger.LogError($"An error occurred while getting user for penalty: {ex.Message}");
                throw new Exception("Internal server error"); throw new Exception("Internal server error");
            }
        }

        //write penalty
        public async Task<bool> WritePenalty(PenaltyDTO penalty)
        {
            try
            {
                Penalty p = mapper.Map<Penalty>(penalty);

                await context.Penalty.AddAsync(p);

                await context.SaveChangesAsync();
                logger.LogInformation($"Penalty is written. Id: {p.Id}");
                return true;
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"Error while writing penalty to the database: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occured while writing penalty. " + ex.Message);
                return false;
            }
        }

        //get written penalties
        public async Task<List<Penalty>> GetMyWrittenPenalties(HttpContext httpContext)
        {
            try
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
                    throw new TokenNotFound("Invalid user ID format.");
                }

                List<Penalty> penalties = await context.Penalty
                    .Where(penalty => penalty.InspectorId == id)
                    .ToListAsync();

                return penalties;
            }
            catch (TokenNotFound ex)
            {
                logger.LogError($"Token not found: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while getting written penalties: {ex.Message}");
                throw new Exception("Internal server error");
            }
        }

        //get unapproved users
        public async Task<List<User>> GetUnapprovedUsers()
        {
            try
            {
                List<User> users = await context.Users
                    .Where(u => u.IsApproved == false && u.Role == "passenger")
                    .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching unapproved users: {ex.Message}");
                throw new Exception("Internal server error");
            }
        }

        //get all inspectors
        public async Task<List<User>> GetAllInspectors()
        {
            try
            {
                List<User> inspectors = await context.Users
                    .Where(u => u.Role == "inspector")
                    .ToListAsync();

                return inspectors;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching all inspectors: {ex.Message}");
                throw new Exception("Internal server error");
            }
        }

        //get all transactions
        public async Task<List<TransactionRequest>> GetAllTransactions()
        {
            try
            {
                List<TransactionRequest> transactionRequests = await context.TransactionRequests
                    .ToListAsync();

                return transactionRequests;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while fetching all transactions: {ex.Message}");
                throw new Exception("Internal server error");
            }
        }

        //send transaction request
        public async Task<TransactionRequest> SendTransactionRequest(TransactionRequestDTO transactionRequestDTO)
        {
            try
            {
                TransactionRequest transactionRequest = mapper.Map<TransactionRequest>(transactionRequestDTO);

                await context.TransactionRequests.AddAsync(transactionRequest);

                await context.SaveChangesAsync();

                transactionRequest = await context.TransactionRequests
                    .Where(u => u.UserId == transactionRequestDTO.UserId)
                    .FirstOrDefaultAsync();

                if (transactionRequest == null)
                {
                    logger.LogError($"Request doesn't exist after adding it to the table.");
                    throw new Exception("Request doesn't exist after adding it to the table.");
                }
                else
                {
                    logger.LogInformation("Request sent.");
                    return transactionRequest;
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred while sending transaction request: {ex.Message}");
                throw new Exception("Internal server error");
            }

        }

    }
}
