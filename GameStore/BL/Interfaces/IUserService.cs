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
        public Task<ServiceResult<string>> SignInAsync(UserWithPasswordModel userDTO, AppSettings appSettings);

        public Task<ServiceResult<(ApplicationUser user, string confirmToken)>> SignUpAsync(UserWithPasswordModel userModel);

        public Task<ServiceResult> ConfirmAsync(string id, string confirmToken);

        public Task<ServiceResult> UdpateUserPasswordAsync(ApplicationUser user, UserWithPasswordModel userModel);

        public Task<ServiceResult<ApplicationUser>> UdpateUserProfileAsync(ClaimsPrincipal contextUser, UserModel user);

        public Task SendConfirmationMessageAsync(string actionName, string controllerName, (ApplicationUser appUser, string token) data, string scheme);
    }
}