
using GloBus.Data.Models;

namespace GloBus.Infrastructure.Interfaces
{
    public interface ITicketTypesRepository
    {
        Task<List<TicketType>> GetTicketTypes();
    }
}
