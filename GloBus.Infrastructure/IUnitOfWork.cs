using GloBus.Infrastructure.Interfaces;

namespace GloBus.Infrastructure
{
    public interface IUnitOfWork
    {
        IUsersRepository UsersRepository { get; }
        ITicketsRepository TicketsRepository { get; }
        ITicketTypesRepository TicketTypesRepository { get; }
        ILinesRepository LinesRepository { get; }
        IActiveTicketsRepository ActiveTicketsRepository { get; }
        IInvalidTicketsRepository InvalidTicketsRepository { get; }
        IPenaltiesRepository PenaltiesRepository { get; }
        IRegionsRepository RegionsRepository { get; }
        IAdminsRepository AdminsRepository { get; }
    }
}
