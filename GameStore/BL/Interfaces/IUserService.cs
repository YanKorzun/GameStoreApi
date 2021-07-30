using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.JsonPatch;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameStore.BL.Services
{
    public interface IUserService
    {
        public Task<ServiceResult<string>> SignInAsync(UserWithPasswordModel userDTO, AppSettings appSettings);

        public Task<ServiceResult<(ApplicationUser user, string confirmToken)>> SignUpAsync(UserWithPasswordModel userModel);

        public Task<ServiceResult> ConfirmAsync(string id, string confirmToken);

        public ServiceResult<int> GetUserIdFromClaims(ClaimsPrincipal user);

        public Task<ServiceResult<ApplicationUser>> GetUserFromClaimsAsync(ClaimsPrincipal user);

        public Task<ServiceResult> UdpateUserPasswordAsync(ClaimsPrincipal userInfo, JsonPatchDocument<UserWithPasswordModel> patch);

        public Task<ServiceResult<ApplicationUser>> UdpateUserProfileAsync(ClaimsPrincipal httpUserContext, UserModel user);

        public Task SendConfirmationMessageAsync(string actionName, string controllerName, (ApplicationUser appUser, string token) data, string scheme);
    }
}