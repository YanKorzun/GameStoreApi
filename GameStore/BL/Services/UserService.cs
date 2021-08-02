using AutoMapper;
using GameStore.BL.Constants;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.Constants;
using GameStore.WEB.DTO;
using GameStore.WEB.Settings;
using GameStore.WEB.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly IUrlHelper _urlHelper;
        private readonly IEmailSender _emailSender;
        private readonly IClaimsUtility _claimsUtility;

        public const string AccountConfirmation = "Account confirmation";

        public UserService(UserManager<ApplicationUser> userManager, IUserRepository userRepository, SignInManager<ApplicationUser> signInManager, IMapper mapper, IRoleService roleService, IUrlHelper urlHelper, IEmailSender emailSender, IClaimsUtility claimsUtility)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _roleService = roleService;
            _urlHelper = urlHelper;
            _claimsUtility = claimsUtility;
        }

        public async Task<ServiceResult<string>> SignInAsync(UserWithPasswordModel userDTO, AppSettings appSettings)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            if (user is null)
            {
                return new(ServiceResultType.InvalidData, ExceptionMessageConstants.MissingUser);
            }

            var userRoleList = await _userManager.GetRolesAsync(user);
            var userRole = userRoleList.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(userRole))
            {
                return new(ServiceResultType.InvalidData);
            }
            else if (!user.EmailConfirmed)
            {
                return new(ServiceResultType.InternalError, "Please confirm your email");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userDTO.Password, false);
            if (!result.Succeeded)
            {
                return new(ServiceResultType.InvalidData);
            }

            var tokenGenerator = new TokenGenerator(appSettings);
            var token = tokenGenerator.GenerateAccessToken(user.Id, user.UserName, userRole);

            return new(ServiceResultType.Success, data: token);
        }

        public async Task<ServiceResult<(ApplicationUser user, string confirmToken)>> SignUpAsync(UserWithPasswordModel userModel)
        {
            var previousUser = await _userManager.FindByEmailAsync(userModel.Email);
            if (previousUser is not null)
            {
                return new(ServiceResultType.InvalidData, "User with same email already exists");
            }

            var user = _mapper.Map<ApplicationUser>(userModel);
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                return new(ServiceResultType.InternalError, "User cannot be created");
            }

            var identityUser = await _userManager.FindByEmailAsync(user.Email);

            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            var confirmTokenEncoded = HttpUtility.UrlEncode(confirmToken);

            await _roleService.EditAsync(new(UserRoleConstants.User, identityUser.Email));
            await _signInManager.SignInAsync(identityUser, false);

            return new(ServiceResultType.Success, (identityUser, confirmTokenEncoded));
        }

        public async Task SendConfirmationMessageAsync(string actionName, string controllerName, (ApplicationUser appUser, string token) data, string scheme)
        {
            var confirmationLink = _urlHelper.Action(actionName, controllerName, new { data.appUser.Id, data.token }, scheme);

            await _emailSender.SendEmailAsync(data.appUser.Email, AccountConfirmation, $"<a href='{confirmationLink}'>confirm</a>");
        }

        public async Task<ServiceResult> ConfirmAsync(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);

            var isEmailConfirmed = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(token));
            if (!isEmailConfirmed.Succeeded)
            {
                return new(ServiceResultType.InternalError);
            }

            return new(ServiceResultType.Success);
        }

        public async Task<ServiceResult> UdpateUserPasswordAsync(ApplicationUser user, UserWithPasswordModel userModel)
        {
            var passwordUpdateResult = await _userRepository.UpdateUserPasswordAsync(user.Id, userModel.Password);

            return passwordUpdateResult;
        }

        public async Task<ServiceResult<ApplicationUser>> UdpateUserProfileAsync(ClaimsPrincipal contextUser, UserModel user)
        {
            var getUserIdResult = _claimsUtility.GetUserIdFromClaims(contextUser);
            if (getUserIdResult.Result is not ServiceResultType.Success)
            {
                return new(getUserIdResult.Result);
            }

            var newApplicationUser = _mapper.Map<ApplicationUser>(user);

            var profileUpdateResult = await _userRepository.UpdateUserAsync(newApplicationUser, getUserIdResult.Data);

            return profileUpdateResult;
        }
    }
}