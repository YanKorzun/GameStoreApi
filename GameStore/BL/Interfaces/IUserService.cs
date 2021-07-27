using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;
using GameStore.WEB.Settings;
using System.Threading.Tasks;

namespace GameStore.BL.Services
{
    public interface IUserService
    {
        public Task<ServiceResult<string>> SignIn(UserModel userDTO, AppSettings appSettings);

        public Task<ServiceResult<(ApplicationUser user, string confirmToken)>> SignUp(UserModel userModel);

        public Task<ServiceResult> Confirm(string id, string confirmToken);
    }
}