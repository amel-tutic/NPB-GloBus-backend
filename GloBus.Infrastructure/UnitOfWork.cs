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
    }
}
