using System.Security.Claims;
using System.Threading.Tasks;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.BL.Utilities
{
    public class ClaimsUtility : IClaimsUtility
    {
        private readonly IUserRepository _userRepository;

        public ClaimsUtility(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ServiceResult<int> GetUserIdFromClaims(ClaimsPrincipal contextUser)
        {
            var id = contextUser.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = int.TryParse(id, out var number);
            if (!result)
            {
                return new(ServiceResultType.InvalidData);
            }

            return new(ServiceResultType.Success, number);
        }

        public async Task<ServiceResult<ApplicationUser>> GetUserFromClaimsAsync(ClaimsPrincipal contextUser)
        {
            var parseIdResult = GetUserIdFromClaims(contextUser);
            if (parseIdResult.Result is not ServiceResultType.Success)
            {
                return new(parseIdResult.Result);
            }

            var userId = parseIdResult.Data;

            var userSearchResult = await _userRepository.FindUserByIdAsync(userId);
            return userSearchResult;
        }
    }
}