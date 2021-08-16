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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace GameStore.UnitTests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task ShouldReturnApplicationUser()
        {
            //Arrange
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var autoMapper = A.Fake<IMapper>();
            var userService = A.Fake<IUserService>();

            var appUser = fixture.Create<ApplicationUser>();

            var contextUser =
                new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, appUser.Id.ToString())
                }));


            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = contextUser
                }
            };

            var userController = new UserController(userService, autoMapper)
            {
                ControllerContext = new()
                {
                    HttpContext = httpContextAccessor.HttpContext
                }
            };

            var searchResult = new ServiceResult<ApplicationUser>(ServiceResultType.Success, appUser);

            A.CallTo(() => userService.GetUserAsync(A<int>._)).Returns(searchResult);


            //Act
            var result = await userController.GetInfo();


            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True(okResult.StatusCode.HasValue);
            Assert.Equal(appUser.Id, ((ApplicationUser)okResult.Value).Id);
            Assert.Equal(appUser.UserName, ((ApplicationUser)okResult.Value).UserName);
            Assert.Equal(appUser.Email, ((ApplicationUser)okResult.Value).Email);
            Assert.Equal(appUser.PhoneNumber, ((ApplicationUser)okResult.Value).PhoneNumber);
        }


        [Fact]
        public async Task ShouldUpdateProfile()
        {
            //Arrange
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var autoMapper = A.Fake<IMapper>();
            var userService = A.Fake<IUserService>();

            var userId = fixture.Create<int>();

            var contextUser =
                new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, userId.ToString())
                }));


            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = contextUser
                }
            };

            var userController = new UserController(userService, autoMapper)
            {
                ControllerContext = new()
                {
                    HttpContext = httpContextAccessor.HttpContext
                }
            };


            var curUserDto = fixture.Create<UpdateUserModel>();
            var curUser = fixture.Build<ApplicationUser>().With(o => o.Id, userId).Create();

            curUser.UserName = curUserDto.UserName;
            curUser.PhoneNumber = curUserDto.PhoneNumber;


            var serviceResult = new ServiceResult<ApplicationUser>(ServiceResultType.Success, curUser);

            A.CallTo(() => userService.UpdateUserProfileAsync(A<int>._, A<UpdateUserModel>._)).Returns(serviceResult);


            //Act
            var result = await userController.UpdateUser(curUserDto);


            //Assert
            Assert.IsType<NoContentResult>(result);
        }


        [Fact]
        public async Task ShouldUpdatePassword()
        {
            //Arrange
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var autoMapper = A.Fake<IMapper>();
            var userService = A.Fake<IUserService>();

            var patch = new JsonPatchDocument<BasicUserModel>
            {
                Operations = { new Operation<BasicUserModel>("replace", "/Password", null, "myNewPassword43@") }
            };

            var userId = fixture.Create<int>();

            var contextUser =
                new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, userId.ToString())
                }));


            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = contextUser
                }
            };

            var userController = new UserController(userService, autoMapper)
            {
                ControllerContext = new()
                {
                    HttpContext = httpContextAccessor.HttpContext
                }
            };

            var curUser = fixture.Build<ApplicationUser>().With(o => o.Id, userId).Create();
            var patchedUser = JsonConvert.DeserializeObject<ApplicationUser>(JsonConvert.SerializeObject(curUser));

            patchedUser.PasswordHash = fixture.Create<string>();

            var getUserResult = new ServiceResult<ApplicationUser>(ServiceResultType.Success, curUser);
            var getPatchedUserResult = new ServiceResult<ApplicationUser>(ServiceResultType.Success, patchedUser);

            A.CallTo(() => userService.GetUserAsync(A<int>._))
                .Returns(getUserResult);

            A.CallTo(() => userService.UpdateUserPasswordAsync(A<ApplicationUser>._, A<BasicUserModel>._))
                .Returns(getPatchedUserResult);


            //Act
            var result = await userController.UpdatePassword(patch);


            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}