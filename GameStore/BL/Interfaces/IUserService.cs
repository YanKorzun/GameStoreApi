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
        Task<ServiceResult<string>> SignInAsync(BasicUserModel basicUserModel, AppSettings appSettings);

        Task<ServiceResult<(ApplicationUser user, string confirmToken)>> SignUpAsync(BasicUserModel basicUserModel);

        Task<ServiceResult> ConfirmAsync(string id, string confirmToken);

        Task<ServiceResult> UpdateUserPasswordAsync(ApplicationUser user, BasicUserModel userModel);

        Task<ServiceResult<ApplicationUser>> UpdateUserProfileAsync(ClaimsPrincipal contextUser, UpdateUserModel updateUser);

        Task SendConfirmationMessageAsync(string actionName, string controllerName, (ApplicationUser appUser, string token) data, string scheme);
    }
}