using System.Security.Claims;
using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;

namespace GameStore.BL.Utilities
{
    public static class ClaimsUtility
    {
        public static ServiceResult<int> GetUserIdFromClaims(ClaimsPrincipal contextUser)
        {
            var id = contextUser.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = int.TryParse(id, out var number);
            if (!result)
            {
                return new(ServiceResultType.InvalidData);
            }

            return new(ServiceResultType.Success, number);
        }
    }
}