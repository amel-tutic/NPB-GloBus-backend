using GloBus.Data.DTOs;
using GloBus.Data.Models;

namespace GloBus.Infrastructure.Interfaces
{
    public interface IAdminsRepository
    {
        Task<ApiResponse<Admin>> LoginAdmin(AdminDTO request);
        Task<List<Line>> GetAllLines();

        Task<User> PromoteToInspector(IdDTO id);

        Task<User> DemoteFromInspector(IdDTO id);

        Task<bool> DeleteLine(IdDTO id);
        Task<Line> AddLine(LineDTO lineDTO);

        Task<Line> EditLine(EditLineDTO editLineDTO);

        Task<bool> RejectTransaction(IdDTO idDTO);

    }
}
