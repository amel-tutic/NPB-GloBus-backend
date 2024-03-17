using GloBus.Data.Models;

namespace GloBus.Infrastructure.Interfaces
{
    public interface IInvalidTicketsRepository
    {
        Task<List<InvalidTickets>> GetAll();
    }
}
