using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FakeItEasy;
using GameStore.BL.Enums;
using GameStore.BL.Mappers;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO.Roles;
using GameStore.WEB.DTO.Users;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace GameStore.UnitTests.BL.Services
{
    public class RoleServiceTests
    {
        [Fact]
        public async Task ShouldReturnSuccessOnCreateRoleAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var roleManager = A.Fake<RoleManager<ApplicationRole>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));

            var roleService = new RoleService(userManager, roleManager, mapper);
            var role = new Fixture().Create<RoleDto>();

            A.CallTo(() => roleManager.CreateAsync(A<ApplicationRole>._))
                .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = await roleService.CreateAsync(role);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => roleManager.CreateAsync(A<ApplicationRole>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnInternalErrorOnCreateRoleAsync()
        {
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var roleManager = A.Fake<RoleManager<ApplicationRole>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));

            var roleService = new RoleService(userManager, roleManager, mapper);
            var role = new Fixture().Create<RoleDto>();

            A.CallTo(() => roleManager.CreateAsync(A<ApplicationRole>._))
                .Returns(Task.FromResult(IdentityResult.Failed()));

            //Act
            var result = await roleService.CreateAsync(role);

            //Assert
            Assert.Equal(ServiceResultType.InternalError, result.Result);

            A.CallTo(() => roleManager.CreateAsync(A<ApplicationRole>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnSuccessOnDeleteAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var roleManager = A.Fake<RoleManager<ApplicationRole>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));

            var roleService = new RoleService(userManager, roleManager, mapper);
            var roleId = new Fixture().Create<string>();

            A.CallTo(() => roleManager.DeleteAsync(A<ApplicationRole>._))
                .Returns(Task.FromResult(IdentityResult.Success));

            //Act
            var result = await roleService.DeleteAsync(roleId);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => roleManager.DeleteAsync(A<ApplicationRole>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnInternalErrorOnDeleteAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var roleManager = A.Fake<RoleManager<ApplicationRole>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));

            var roleService = new RoleService(userManager, roleManager, mapper);
            var roleId = new Fixture().Create<string>();


            A.CallTo(() => roleManager.DeleteAsync(A<ApplicationRole>._))
                .Returns(Task.FromResult(IdentityResult.Failed()));

            //Act
            var result = await roleService.DeleteAsync(roleId);

            //Assert
            Assert.Equal(ServiceResultType.InternalError, result.Result);

            A.CallTo(() => roleManager.DeleteAsync(A<ApplicationRole>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => roleManager.FindByIdAsync(A<string>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteAsync()
        {
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var roleManager = A.Fake<RoleManager<ApplicationRole>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));

            var roleService = new RoleService(userManager, roleManager, mapper);
            var roleId = new Fixture().Create<string>();


            A.CallTo(() => roleManager.FindByIdAsync(A<string>._)).Returns(Task.FromResult<ApplicationRole>(null));

            //Act
            var result = await roleService.DeleteAsync(roleId);

            //Assert
            Assert.Equal(ServiceResultType.NotFound, result.Result);

            A.CallTo(() => roleManager.FindByIdAsync(A<string>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnSuccessOnEditAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var roleManager = A.Fake<RoleManager<ApplicationRole>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));

            var roleService = new RoleService(userManager, roleManager, mapper);
            var role = new Fixture().Build<BasicUserRoleDto>().With(o => o.Email, "mynewEmail@gmail.com").Create();
            A.CallTo(() => roleManager.FindByNameAsync(A<string>._)).Returns(Task.FromResult<ApplicationRole>(null));

            //Act
            var result = await roleService.EditAsync(role);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.RemoveFromRolesAsync(A<ApplicationUser>._, A<IList<string>>._))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => roleManager.FindByNameAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.AddToRoleAsync(A<ApplicationUser>._, A<string>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnEditAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var roleManager = A.Fake<RoleManager<ApplicationRole>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));

            var roleService = new RoleService(userManager, roleManager, mapper);
            var role = new Fixture().Build<BasicUserRoleDto>().With(o => o.Email, "mynewEmail@gmail.com").Create();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns(Task.FromResult<ApplicationUser>(null));

            //Act
            var result = await roleService.EditAsync(role);

            //Assert
            Assert.Equal(ServiceResultType.NotFound, result.Result);

            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustNotHaveHappened();
            A.CallTo(() => userManager.RemoveFromRolesAsync(A<ApplicationUser>._, A<List<string>>._))
                .MustNotHaveHappened();
            A.CallTo(() => roleManager.FindByNameAsync(A<string>._)).MustNotHaveHappened();
            A.CallTo(() => userManager.AddToRoleAsync(A<ApplicationUser>._, A<string>._)).MustNotHaveHappened();
        }
    }
}