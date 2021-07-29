using AutoMapper;
using GameStore.BL.Enums;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameStore.WEB.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;

        public UserController(IMapper mapper, IUserService userService, IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userService = userService;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(UserModel user)
        {
            var appUser = _mapper.Map<ApplicationUser>(user);
            var userId = _userService.GetUserIdFromClaims(User).Data;

            var updateUser = await _userRepository.UpdateUser(appUser, userId);
            if (updateUser.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return Ok(updateUser);
        }

        [HttpPatch("password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePassword([FromBody] JsonPatchDocument<UserWithPasswordModel> patch)
        {
            var userId = _userService.GetUserIdFromClaims(User).Data;
            var user = (await _userRepository.FindUserByIdAsync(userId)).Data;
            var userModel = _mapper.Map<UserWithPasswordModel>(user);
            patch.ApplyTo(userModel);

            var passwordUpdateResult = await _userRepository.UpdateUserPassword(userId, userModel.Password);
            if (passwordUpdateResult.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInfo()
        {
            var userId = _userService.GetUserIdFromClaims(User).Data;
            var searchResult = await _userRepository.FindUserByIdAsync(userId);
            if (searchResult.Result is not ServiceResultType.Success)
            {
                return BadRequest();
            }

            return Ok(searchResult.Data);
        }
    }
}