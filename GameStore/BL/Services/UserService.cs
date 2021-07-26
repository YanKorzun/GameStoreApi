using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.Constants;
using GameStore.WEB.DTO;
using GameStore.WEB.Settings;
using GameStore.WEB.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GameStore.BL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ServiceResult<string>> SignIn(UserModel userDTO, AppSettings appSettings)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            var userRoleList = await _userManager.GetRolesAsync(user);
            if (user is null)
            {
                return null;
            }
            if (user.EmailConfirmed)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, userDTO.Password, false);
                if (result.Succeeded)
                {
                    TokenGenerator gtor = new(appSettings);
                    var userRole = userRoleList.FirstOrDefault();
                    var token = gtor.GenerateAccessToken(user.Id, user.UserName, userRole is null ? UserRoleConstants.User : userRole);
                    return new() { Result = ServiceResultType.Success, Data = token };
                }
            }
            return new(ServiceResultType.InternalError);
        }

        public async Task<ServiceResult<(ApplicationUser user, string confirmToken)>> SignUp(string email, string password, string username)
        {
            var user = new ApplicationUser() { Email = email, UserName = username is null ? email : username };
            var result = await _userManager.CreateAsync(user, password);
            user = await _userManager.FindByEmailAsync(email);
            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmTokenEncoded = HttpUtility.UrlEncode(confirmToken);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }
            else if (result.Errors.Any())
            {
                return new(ServiceResultType.InternalError);
            }
            return new() { Result = ServiceResultType.Success, Data = (user, confirmTokenEncoded) };
        }

        //"CfDJ8Msbs77VffBJtuboPM5ql4SYWIopw9IvXJqml%2b0PWGfRCQ2qrGm7Zq154kYb2AietlbEmI1OkEiYFc2K4Z00xEQbhGPdy6cLl1BFu3TusyBUMn4STBDymAv4Fdp0FnHuAtag19SgGq5eoAt3ItSV0iVu2%2bq%2buvEssbdEE%2bLX3rKP5QYoRtgtIUW6UNb4XtPC8DaskWkjUY1t6mMx%2ftBGCSk%3d"
        public async Task<ServiceResult> Confirm(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            var IsEmailConfirmed = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(token));
            if (IsEmailConfirmed.Succeeded)
            {
                return new(ServiceResultType.Success);
            }
            return new(ServiceResultType.InternalError);
        }
    }
}