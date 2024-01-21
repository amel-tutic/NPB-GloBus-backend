using GloBus.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
