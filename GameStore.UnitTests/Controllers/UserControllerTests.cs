using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FakeItEasy;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.Controllers;
using GameStore.WEB.DTO.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GameStore.UnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly ApplicationUser _appUser;
        private readonly IMapper _autoMapper = A.Fake<IMapper>();

        private readonly Fixture _fixture = new()
        {
            Behaviors = { new NullRecursionBehavior() }
        };

        private readonly UserController _userController;

        private readonly IUserService _userService = A.Fake<IUserService>();

        public UserControllerTests()
        {
            _appUser = _fixture.Create<ApplicationUser>();

            var contextUser =
                new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, _appUser.Id.ToString())
                }));


            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = contextUser
                }
            };

            _userController = new(_userService, _autoMapper)
            {
                ControllerContext = new()
                {
                    HttpContext = httpContextAccessor.HttpContext
                }
            };
        }

        [Fact]
        public async Task ShouldReturnApplicationUser()
        {
            //Arrange
            var searchResult = new ServiceResult<ApplicationUser>(ServiceResultType.Success, _appUser);

            A.CallTo(() => _userService.GetUserAsync(A<int>._)).Returns(searchResult);

            //Act
            var result = await _userController.GetInfo();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True(okResult.StatusCode.HasValue);
            Assert.Equal(_appUser.Id, ((ApplicationUser)okResult.Value).Id);
            Assert.Equal(_appUser.UserName, ((ApplicationUser)okResult.Value).UserName);
            Assert.Equal(_appUser.Email, ((ApplicationUser)okResult.Value).Email);
            Assert.Equal(_appUser.PhoneNumber, ((ApplicationUser)okResult.Value).PhoneNumber);
        }


        [Fact]
        public async Task ShouldReturnUpdatedUser()
        {
            //Arrange
            _appUser.Id = _fixture.Create<int>();

            var curUserDto = _fixture.Create<UpdateUserModel>();
            var curUser = _fixture.Build<ApplicationUser>().With(o => o.Id, _appUser.Id).Create();

            curUser.UserName = curUserDto.UserName;
            curUser.PhoneNumber = curUserDto.PhoneNumber;


            var serviceResult = new ServiceResult<ApplicationUser>(ServiceResultType.Success, curUser);

            A.CallTo(() => _userService.UpdateUserProfileAsync(A<int>._, A<UpdateUserModel>._)).Returns(serviceResult);


            //Act
            var result = await _userController.UpdateUser(curUserDto);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}