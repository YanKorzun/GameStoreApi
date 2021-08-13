using System.Threading.Tasks;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Users;
using GameStore.WEB.Settings;

namespace GameStore.BL.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<string>> SignInAsync(BasicUserModel basicUserModel, AppSettings appSettings);

        Task<ServiceResult<(ApplicationUser user, string confirmToken)>> SignUpAsync(BasicUserModel basicUserModel);

        Task<ServiceResult> ConfirmAsync(string id, string confirmToken);

        Task<ServiceResult> UpdateUserPasswordAsync(ApplicationUser user, BasicUserModel userModel);

        Task<ServiceResult<ApplicationUser>> UpdateUserProfileAsync(int userId,
            UpdateUserModel updateUser);

        Task SendConfirmationMessageAsync(string actionName, string controllerName,
            (ApplicationUser appUser, string token) data, string scheme);

        Task<ServiceResult<ApplicationUser>> GetUserAsync(int id);
    }
}