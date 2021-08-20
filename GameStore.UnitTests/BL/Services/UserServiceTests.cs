using AutoFixture;
using AutoMapper;
using FakeItEasy;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.ResultWrappers;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO.Users;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GameStore.UnitTests.BL.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task ShoudReturnSuccessFromSignInAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var settings = fixture.Create<AppSettings>();

            var roleList = new List<string>() { fixture.Create<string>() };

            var appuser = fixture.Build<ApplicationUser>().With(o => o.EmailConfirmed, true).Create();

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).Returns(roleList);
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns(appuser);
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._)).Returns(Microsoft.AspNetCore.Identity.SignInResult.Success);

            //Act
            var result = await userService.SignInAsync(basicUserDto, settings);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShoudReturnInvalidDataOnCheckFromSignInAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var settings = fixture.Create<AppSettings>();

            var roleList = new List<string>() { fixture.Create<string>() };

            var appuser = fixture.Build<ApplicationUser>().With(o => o.EmailConfirmed, true).Create();

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).Returns(roleList);
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns(appuser);
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._)).Returns(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            //Act
            var result = await userService.SignInAsync(basicUserDto, settings);

            //Assert
            Assert.Equal(ServiceResultType.InvalidData, result.Result);

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShoudReturnInternalErrorOnNotConfirmedMailFromSignInAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var settings = fixture.Create<AppSettings>();

            var roleList = new List<string>() { fixture.Create<string>() };

            var appuser = fixture.Build<ApplicationUser>().With(o => o.EmailConfirmed, false).Create();

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).Returns(roleList);
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns(appuser);
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._)).Returns(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            //Act
            var result = await userService.SignInAsync(basicUserDto, settings);

            //Assert
            Assert.Equal(ServiceResultType.InternalError, result.Result);

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ShoudReturnInvalidDataOnEmptyRolesFromSignInAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var settings = fixture.Create<AppSettings>();


            var appuser = fixture.Build<ApplicationUser>().With(o => o.EmailConfirmed, false).Create();


            //Act
            var result = await userService.SignInAsync(basicUserDto, settings);

            //Assert
            Assert.Equal(ServiceResultType.InvalidData, result.Result);

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ShoudReturnInvalidDataOnEmptyUserFromSignInAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var settings = fixture.Create<AppSettings>();

            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns<ApplicationUser>(null);

            var appuser = fixture.Build<ApplicationUser>().With(o => o.EmailConfirmed, false).Create();


            //Act
            var result = await userService.SignInAsync(basicUserDto, settings);

            //Assert
            Assert.Equal(ServiceResultType.InvalidData, result.Result);

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustNotHaveHappened();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._)).MustNotHaveHappened();
        }


        [Fact]
        public async Task ShoudReturnSuccessFromSignUpAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();

            A.CallTo(() => userManager.CreateAsync(A<ApplicationUser>._, A<string>._)).Returns(IdentityResult.Success);
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns<ApplicationUser>(null);

            var appuser = fixture.Build<ApplicationUser>().With(o => o.EmailConfirmed, false).Create();


            //Act
            var result = await userService.SignUpAsync(basicUserDto);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => userManager.CreateAsync(A<ApplicationUser>._, A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedTwiceExactly();
        }

        [Fact]
        public async Task ShoudReturnInternalErrorFromSignUpAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();

            A.CallTo(() => userManager.CreateAsync(A<ApplicationUser>._, A<string>._)).Returns(IdentityResult.Failed());
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns<ApplicationUser>(null);

            var appuser = fixture.Build<ApplicationUser>().With(o => o.EmailConfirmed, false).Create();


            //Act
            var result = await userService.SignUpAsync(basicUserDto);

            //Assert
            Assert.Equal(ServiceResultType.InternalError, result.Result);

            A.CallTo(() => userManager.CreateAsync(A<ApplicationUser>._, A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShoudReturnInvalidDataFromSignUpAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();

            //Act
            var result = await userService.SignUpAsync(basicUserDto);

            //Assert
            Assert.Equal(ServiceResultType.InvalidData, result.Result);

            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly(); 
            A.CallTo(() => userManager.CreateAsync(A<ApplicationUser>._, A<string>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ShoudBeCalledOnceInSendConfirmationMessageAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);

            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var actionName = "send";
            var controllerName = "UserController";
            var data = fixture.Create<(ApplicationUser, string)>();
            var scheme = "scheme";

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();

            //Act
            await userService.SendConfirmationMessageAsync(actionName, controllerName, data, scheme);

            //Assert
            A.CallTo(() => emailSender.SendEmailAsync(A<string>._, A<string>._, A<string>._)).MustHaveHappenedOnceExactly(); 
        }

        [Fact]
        public async Task ShoudReturnSuccessFromConfirmAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);
                        
            var id = "1";
            var token = "UserController";

            A.CallTo(() => userManager.ConfirmEmailAsync(A<ApplicationUser>._, A<string>._)).Returns(IdentityResult.Success);
            
            //Act
            var result = await userService.ConfirmAsync(id, token);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => userManager.FindByIdAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.ConfirmEmailAsync(A<ApplicationUser>._, A<string>._)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ShoudReturnInternalErrorFromConfirmAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);

            var id = "1";
            var token = "UserController";

            A.CallTo(() => userManager.ConfirmEmailAsync(A<ApplicationUser>._, A<string>._)).Returns(IdentityResult.Failed());

            //Act
            var result = await userService.ConfirmAsync(id, token);

            //Assert
            Assert.Equal(ServiceResultType.InternalError, result.Result);

            A.CallTo(() => userManager.FindByIdAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.ConfirmEmailAsync(A<ApplicationUser>._, A<string>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShoudBeCalledOnceInUpdateUserPasswordAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var user = fixture.Create<ApplicationUser>();

            A.CallTo(() => userRepository.UpdateUserPasswordAsync(A<int>._, A<string>._)).Returns(new ServiceResult(ServiceResultType.Success));
            
            //Act
            var result = await userService.UpdateUserPasswordAsync(user, basicUserDto);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => userRepository.UpdateUserPasswordAsync(A<int>._, A<string>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShoudBeCalledOnceInUpdateUserProfileAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var userId = 1;
            var user = fixture.Create<ApplicationUser>();
            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();


            A.CallTo(() => userRepository.UpdateUserAsync(A<ApplicationUser>._, A<int>._)).Returns(new ServiceResult<ApplicationUser>(ServiceResultType.Success, user));

            //Act
            var result = await userService.UpdateUserProfileAsync(userId, basicUserDto);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => userRepository.UpdateUserAsync(A<ApplicationUser>._, A<int>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnUserFromCache()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var userId = 1;
            var user = fixture.Create<ApplicationUser>();
            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();


            A.CallTo(() => cacheService.GetEntity(A<int>._)).Returns(new ServiceResult<ApplicationUser>());
            A.CallTo(() => userRepository.FindUserByIdAsync( A<int>._)).Returns(new ServiceResult<ApplicationUser>(ServiceResultType.Success, user));

            //Act
            var result = await userService.GetUserAsync(userId);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => cacheService.GetEntity(A<int>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userRepository.FindUserByIdAsync(A<int>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnUserFromDatabase()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var userId = 1;
            var user = fixture.Create<ApplicationUser>();
            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();


            A.CallTo(() => cacheService.GetEntity(A<int>._)).Returns(new ServiceResult<ApplicationUser>(ServiceResultType.Success, user));

            //Act
            var result = await userService.GetUserAsync(userId);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => cacheService.GetEntity(A<int>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userRepository.FindUserByIdAsync(A<int>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ShouldReturnNotFoundFromGetUserAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = A.Fake<IMapper>();
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService, urlHelper, emailSender, cacheService);
            var fixture = new Fixture()
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var userId = 1;
            var user = fixture.Create<ApplicationUser>();
            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();


            A.CallTo(() => cacheService.GetEntity(A<int>._)).Returns(new ServiceResult<ApplicationUser>());
            A.CallTo(() => userRepository.FindUserByIdAsync(A<int>._)).Returns(new ServiceResult<ApplicationUser>());

            //Act
            var result = await userService.GetUserAsync(userId);

            //Assert
            Assert.Equal(ServiceResultType.NotFound, result.Result);

            A.CallTo(() => cacheService.GetEntity(A<int>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userRepository.FindUserByIdAsync(A<int>._)).MustHaveHappenedOnceExactly();
        }
    }
}
