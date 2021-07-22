using GameStore.DAL.Entities;
using GameStore.WEB.DTO;
using GameStore.WEB.Settings;
using GameStore.WEB.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

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
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            if (user is null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userDTO.Password, false);
            if (result.Succeeded)
            {
                TokenGenerator gtor = new(appSettings);
                var res = gtor.GenerateAccessToken(_userManager.FindByEmailAsync(userDTO.Email).Result.Id, userDTO.Username, userDTO.UserRole);
                return res;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> SignUp(string email, string password, string username)
        {
            ApplicationUser user = new() { Email = email, UserName = username };
            var result = await _userManager.CreateAsync(user, password);

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
    }
}