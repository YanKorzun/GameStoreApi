using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using GameStore.BL.Interfaces;
using GameStore.WEB.Controllers;
using Xunit;

namespace GameStore.UnitTests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task ShouldReturnApplicationUser()
        {
            //Arrange
            var autoMapper = A.Fake<IMapper>();
            var userService = A.Fake<IUserService>();

            var contextUser =
                new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new(ClaimTypes.NameIdentifier, "2") }));

            var userController = new UserController(userService, autoMapper);
            var result = await userController.GetInfo();

            Assert.NotNull(result.Value);
        }
    }
}