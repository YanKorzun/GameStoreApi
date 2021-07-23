using GameStore.DAL.Entities;
using GameStore.WEB.DTO;
using GameStore.WEB.Settings;
using GameStore.WEB.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GameStore.BL.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string?> SignIn(UserDTO userDTO, AppSettings appSettings)
        {
            var user = _userManager.FindByEmailAsync(userDTO.Email).Result;
            var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
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
                    var res = gtor.GenerateAccessToken(user.Id, user.UserName, userRole is null ? "user" : userRole);
                    return res;
                }
            }
            return null;
        }

        public async Task<bool> SignUp(string email, string password, string username)
        {
            //id =0
            ApplicationUser user = new() { Email = email, UserName = username is null ? email : username };
            var result = await _userManager.CreateAsync(user, password);
            //get user with unique id
            user = await _userManager.FindByEmailAsync(email);
            var confirmToken = HttpUtility.UrlEncode(_userManager.GenerateEmailConfirmationTokenAsync(user).Result);

            EmailSender.SendConfirmMessage(user.Id, confirmToken, "https://localhost:44360/api/auth/email-confirmation", email);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }
            else if (result.Errors.Any())
            {
                return false;
            }
            return true;
        }

        public bool Confirm(int id, string token)
        {
            var user = _userManager.FindByIdAsync(id.ToString());
            var IsEmailConfirmed = _userManager.ConfirmEmailAsync(user.Result, token);
            if (IsEmailConfirmed.Result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}