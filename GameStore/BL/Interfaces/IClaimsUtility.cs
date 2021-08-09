using System.Security.Claims;
using System.Threading.Tasks;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;

namespace GameStore.BL.Interfaces
{
    public interface IClaimsUtility
    {
        ServiceResult<int> GetUserIdFromClaims(ClaimsPrincipal user);

        Task<ServiceResult<ApplicationUser>> GetUserFromClaimsAsync(ClaimsPrincipal user);
    }
}