using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.UserModels;
using GameStore.WEB.Settings;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface IUserService
    {
        public Task<ServiceResult<string>> SignInAsync(BasicUserModel basicUserModel, AppSettings appSettings);

        public Task<ServiceResult<(ApplicationUser user, string confirmToken)>> SignUpAsync(BasicUserModel basicUserModel);

        public Task<ServiceResult> ConfirmAsync(string id, string confirmToken);

        public Task<ServiceResult> UpdateUserPasswordAsync(ApplicationUser user, BasicUserModel userModel);

        public Task<ServiceResult<ApplicationUser>> UpdateUserProfileAsync(ClaimsPrincipal contextUser, UpdateUserModel updateUser);

        public Task SendConfirmationMessageAsync(string actionName, string controllerName, (ApplicationUser appUser, string token) data, string scheme);
    }
}