using GloBus.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IUsersRepository UsersRepository)
        {
            this.UsersRepository = UsersRepository;
        }

        public IUsersRepository UsersRepository { get; }

        public ITicketsRepository TicketsRepository { get; }

        public ITicketTypesRepository TicketTypesRepository { get; }

        public ILinesRepository LinesRepository { get; }

        public IActiveTicketsRepository ActiveTicketsRepository { get; }

        public IInvalidTicketsRepository InvalidTicketsRepository { get; }

        public IPenaltiesRepository PenaltiesRepository { get; }

        public IRegionsRepository RegionsRepository { get; }
    }
}
