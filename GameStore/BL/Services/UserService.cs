using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using GameStore.BL.Constants;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.Constants;
using GameStore.WEB.DTO.Users;
using GameStore.WEB.Settings;
using GameStore.WEB.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.BL.Services
{
    public class UserService : IUserService
    {
        public const string AccountConfirmation = "Account confirmation";
        private readonly ICacheService<ApplicationUser> _cacheService;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUrlHelper _urlHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<ApplicationUser> userManager, IUserRepository userRepository,
            SignInManager<ApplicationUser> signInManager, IMapper mapper, IRoleService roleService,
            IUrlHelper urlHelper, IEmailSender emailSender, ICacheService<ApplicationUser> cacheService)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            _cacheService = cacheService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _roleService = roleService;
            _urlHelper = urlHelper;
        }

        public async Task<ServiceResult<string>> SignInAsync(BasicUserModel basicUserModel, AppSettings appSettings)
        {
            var user = await _userManager.FindByEmailAsync(basicUserModel.Email);
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

            if (!user.EmailConfirmed)
            {
                return new(ServiceResultType.InternalError, "Please confirm your email");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, basicUserModel.Password, false);
            if (!result.Succeeded)
            {
                return new(ServiceResultType.InvalidData);
            }

            var tokenGenerator = new TokenGenerator(appSettings);
            var token = tokenGenerator.GenerateAccessToken(user.Id, user.UserName, userRole);

            return new(ServiceResultType.Success, data: token);
        }

        public async Task<ServiceResult<(ApplicationUser user, string confirmToken)>> SignUpAsync(
            BasicUserModel basicUserModel)
        {
            var previousUser = await _userManager.FindByEmailAsync(basicUserModel.Email);
            if (previousUser is not null)
            {
                return new(ServiceResultType.InvalidData,
                    "User with same email already exists");
            }

            var user = _mapper.Map<ApplicationUser>(basicUserModel);
            var result = await _userManager.CreateAsync(user, basicUserModel.Password);
            if (!result.Succeeded)
            {
                return new(ServiceResultType.InternalError,
                    "User cannot be created");
            }

            var identityUser = await _userManager.FindByEmailAsync(user.Email);

            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            var confirmTokenEncoded = HttpUtility.UrlEncode(confirmToken);

            await _roleService.EditAsync(new(UserRoleConstants.User, identityUser.Email));
            await _signInManager.SignInAsync(identityUser, false);

            return new(ServiceResultType.Success,
                (identityUser, confirmTokenEncoded));
        }

        public async Task SendConfirmationMessageAsync(string actionName, string controllerName,
            (ApplicationUser appUser, string token) data, string scheme)
        {
            var confirmationLink =
                _urlHelper.Action(actionName, controllerName, new { data.appUser.Id, data.token }, scheme);

            await _emailSender.SendEmailAsync(data.appUser.Email, AccountConfirmation,
                $"<a href='{confirmationLink}'>confirm</a>");
        }

        public async Task<ServiceResult> ConfirmAsync(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);

            var isEmailConfirmed = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(token));

            return !isEmailConfirmed.Succeeded
                ? new(ServiceResultType.InternalError)
                : new ServiceResult(ServiceResultType.Success);
        }

        public async Task<ServiceResult> UpdateUserPasswordAsync(ApplicationUser user, BasicUserModel updateUserModel)
        {
            var passwordUpdateResult = await _userRepository.UpdateUserPasswordAsync(user.Id, updateUserModel.Password);

            _cacheService.Remove(user.Id);

            return passwordUpdateResult;
        }

        public async Task<ServiceResult<ApplicationUser>> UpdateUserProfileAsync(int userId,
            UpdateUserModel updateUser)
        {
            var newApplicationUser = _mapper.Map<ApplicationUser>(updateUser);

            var profileUpdateResult = await _userRepository.UpdateUserAsync(newApplicationUser, userId);

            _cacheService.Remove(userId);

            return profileUpdateResult;
        }

        public async Task<ServiceResult<ApplicationUser>> GetUserAsync(int id)
        {
            var userSearchResult = _cacheService.GetEntity(id);
            if (userSearchResult.Result is ServiceResultType.Success)
            {
                return userSearchResult;
            }

            userSearchResult = await _userRepository.FindUserByIdAsync(id);
            if (userSearchResult.Result is not ServiceResultType.Success)
            {
                return new(ServiceResultType.NotFound);
            }

            _cacheService.Set(id, userSearchResult.Data);

            return userSearchResult;
        }
    }
}