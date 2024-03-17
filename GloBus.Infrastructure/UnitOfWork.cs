using GloBus.Infrastructure.Interfaces;

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
                          IRegionsRepository RegionsRepository,
                          IAdminsRepository AdminsRepository
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
            this.AdminsRepository = AdminsRepository;
        }

        public IUsersRepository UsersRepository { get; }
        public ITicketsRepository TicketsRepository { get; }
        public ITicketTypesRepository TicketTypesRepository { get; }
        public ILinesRepository LinesRepository { get; }
        public IActiveTicketsRepository ActiveTicketsRepository { get; }
        public IInvalidTicketsRepository InvalidTicketsRepository { get; }
        public IPenaltiesRepository PenaltiesRepository { get; }
        public IRegionsRepository RegionsRepository { get; }
        public IAdminsRepository AdminsRepository { get; }
    }
}
