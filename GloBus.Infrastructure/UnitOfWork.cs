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
        public UnitOfWork(IUsersRepository UsersRepository,
                          ITicketsRepository TicketsRepository,
                          ITicketTypesRepository TicketTypesRepository,
                          ILinesRepository LinesRepository,
                          IActiveTicketsRepository ActiveTicketsRepository,
                          IInvalidTicketsRepository InvalidTicketsRepository,
                          IPenaltiesRepository PenaltiesRepository,
                          IRegionsRepository RegionsRepository
                         )
        {
            this.UsersRepository = UsersRepository;
            this.TicketsRepository = TicketsRepository;
            this.TicketTypesRepository = TicketTypesRepository;
            this.LinesRepository = LinesRepository;
            this.ActiveTicketsRepository = ActiveTicketsRepository;
            this.InvalidTicketsRepository = InvalidTicketsRepository;
            this.PenaltiesRepository = PenaltiesRepository;
            this.RegionsRepository = RegionsRepository;
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
