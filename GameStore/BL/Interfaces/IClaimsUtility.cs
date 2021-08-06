using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface IClaimsUtility
    {
        ServiceResult<int> GetUserIdFromClaims(ClaimsPrincipal user);

        Task<ServiceResult<ApplicationUser>> GetUserFromClaimsAsync(ClaimsPrincipal user);
    }
}