using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;
using GameStore.WEB.Settings;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameStore.BL.Services
{
    public interface IUserService
    {
        public Task<ServiceResult<string>> SignIn(UserWithPasswordModel userDTO, AppSettings appSettings);

        public Task<ServiceResult<(ApplicationUser user, string confirmToken)>> SignUp(UserWithPasswordModel userModel);

        public Task<ServiceResult> Confirm(string id, string confirmToken);

        public ServiceResult<int> GetUserIdFromClaims(ClaimsPrincipal user);
    }
}