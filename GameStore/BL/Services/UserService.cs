using AutoMapper;
using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.Constants;
using GameStore.WEB.DTO;
using GameStore.WEB.Settings;
using GameStore.WEB.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using GameStore.BL.Constants;
using GameStore.BL.Interfaces;

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

        public const string AccountConfirmation = "Account confirmation";

        public UserService(UserManager<ApplicationUser> userManager, IUserRepository userRepository, SignInManager<ApplicationUser> signInManager, IMapper mapper, IRoleService roleService, IUrlHelper urlHelper, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _roleService = roleService;
            _urlHelper = urlHelper;
        }

        public async Task<ServiceResult<string>> SignInAsync(UserWithPasswordModel userDTO, AppSettings appSettings)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);

            var userRoleList = await _userManager.GetRolesAsync(user);
            var userRole = userRoleList.FirstOrDefault();

            if (user is null || string.IsNullOrWhiteSpace(userRole))
            {
                return new(ServiceResultType.InvalidData);
            }
            else if (!user.EmailConfirmed)
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

            return new(ServiceResultType.Success) { Data = token };
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

        public ServiceResult<int> GetUserIdFromClaims(ClaimsPrincipal httpUserContext)
        {
            IEnumerable<Claim> idClaims = httpUserContext.FindAll(ClaimTypes.NameIdentifier);
            var userId = idClaims.Select(r => r.Value).FirstOrDefault();

            return new(ServiceResultType.Success, int.Parse(userId));
        }

        public async Task<ServiceResult<ApplicationUser>> GetUserFromClaimsAsync(ClaimsPrincipal httpUserContext)
        {
            var parseIdResult = GetUserIdFromClaims(httpUserContext);
            if (parseIdResult.Result is not ServiceResultType.Success)
            {
                return new(parseIdResult.Result);
            }

            var userId = parseIdResult.Data;

            var userSearchResult = await _userRepository.FindUserByIdAsync(userId);
            if (userSearchResult.Result is not ServiceResultType.Success)
            {
                return userSearchResult;
            }

            return new(ServiceResultType.Success, userSearchResult.Data);
        }

        public async Task<ServiceResult> UdpateUserPasswordAsync(ClaimsPrincipal httpUserContext, JsonPatchDocument<UserWithPasswordModel> patch)
        {
            var user = await GetUserFromClaimsAsync(httpUserContext);
            var userModel = _mapper.Map<UserWithPasswordModel>(user.Data);
            patch.ApplyTo(userModel);

            var passwordUpdateResult = await _userRepository.UpdateUserPasswordAsync(user.Data.Id, userModel.Password);

            return passwordUpdateResult;
        }

        public async Task<ServiceResult<ApplicationUser>> UdpateUserProfileAsync(ClaimsPrincipal httpUserContext, UserModel user)
        {
            var getUserIdResult = GetUserIdFromClaims(httpUserContext);
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