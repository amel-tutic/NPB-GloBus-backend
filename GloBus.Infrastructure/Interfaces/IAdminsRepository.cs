using GloBus.Data.DTOs;
using GloBus.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Infrastructure.Interfaces
{
    public interface IAdminsRepository
    {
        Task<ApiResponse<Admin>> loginAdmin(AdminDTO request);
        Task<List<Line>> getAllLines();
    }
}
