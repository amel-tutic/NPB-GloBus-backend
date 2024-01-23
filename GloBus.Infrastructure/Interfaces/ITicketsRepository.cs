﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Infrastructure.Interfaces
{
    public interface ITicketsRepository
    {
        Task<bool> ApproveTicket(int id);
        Task<bool> DeleteTicket(int id);
    }
}
