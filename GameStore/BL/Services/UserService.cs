using AutoMapper;
using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.Constants;
using GameStore.WEB.DTO;
using GameStore.WEB.Settings;
using GameStore.WEB.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace GameStore.BL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper, IRoleService roleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _roleService = roleService;
        }

        public async Task<ServiceResult<string>> SignIn(UserWithPasswordModel userDTO, AppSettings appSettings)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            var userRoleList = await _userManager.GetRolesAsync(user);
            var userRole = userRoleList.FirstOrDefault();

            if (user is null || string.IsNullOrWhiteSpace(userRole))
            {
                return new(ServiceResultType.InvalidData);
            }
            if (!user.EmailConfirmed)
            {
                return new(ServiceResultType.InternalError) { ErrorMessage = "Please confirm your email" };
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, userDTO.Password, false);
            if (!result.Succeeded)
            {
                return new(ServiceResultType.InvalidData);
            }
            var tokenGenerator = new TokenGenerator(appSettings);
            var token = tokenGenerator.GenerateAccessToken(user.Id, user.UserName, userRole);

            return new(ServiceResultType.Success, token);
        }

        public async Task<ServiceResult<(ApplicationUser user, string confirmToken)>> SignUp(UserWithPasswordModel userModel)
        {
            var user = _mapper.Map<ApplicationUser>(userModel);
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                return new(ServiceResultType.InternalError);
            }
            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            var confirmTokenEncoded = HttpUtility.UrlEncode(confirmToken);

            await _roleService.EditAsync(new(UserRoleConstants.User, identityUser.Email));
            await _signInManager.SignInAsync(identityUser, false);

            return new(ServiceResultType.Success, (identityUser, confirmTokenEncoded));
        }

        public async Task<ServiceResult> Confirm(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            var isEmailConfirmed = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(token));
            if (!isEmailConfirmed.Succeeded)
            {
                return new(ServiceResultType.InternalError);
            }

            return new(ServiceResultType.Success);
        }

        public ServiceResult<int> GetUserIdFromClaims(ClaimsPrincipal user)
        {
            IEnumerable<Claim> idClaims = user.FindAll(ClaimTypes.NameIdentifier);
            var userId = idClaims.Select(r => r.Value).FirstOrDefault();
            int iUserId;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new(ServiceResultType.NotFound);
            }
            try
            {
                iUserId = int.Parse(userId);
            }
            catch (System.Exception)
            {
                return new(ServiceResultType.InvalidData);
            }
            return new(ServiceResultType.Success, iUserId);
        }
    }
}