﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FakeItEasy;
using GameStore.BL.Enums;
using GameStore.BL.Interfaces;
using GameStore.BL.Mappers;
using GameStore.BL.ResultWrappers;
using GameStore.BL.Services;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.DTO.Users;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace GameStore.UnitTests.BL.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task ShouldReturnSuccessFromSignInAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var settings = fixture.Create<AppSettings>();

            var roleList = new List<string> { fixture.Create<string>() };

            var appuser = fixture.Build<ApplicationUser>().With(o => o.EmailConfirmed, true).Create();

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).Returns(roleList);
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns(appuser);
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._))
                .Returns(SignInResult.Success);

            //Act
            var result = await userService.SignInAsync(basicUserDto, settings);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnInvalidDataOnCheckFromSignInAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var settings = fixture.Create<AppSettings>();

            var roleList = new List<string> { fixture.Create<string>() };

            var appuser = fixture.Build<ApplicationUser>().With(o => o.EmailConfirmed, true).Create();

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).Returns(roleList);
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns(appuser);
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._))
                .Returns(SignInResult.Failed);

            //Act
            var result = await userService.SignInAsync(basicUserDto, settings);

            //Assert
            Assert.Equal(ServiceResultType.InvalidData, result.Result);

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnInternalErrorOnNotConfirmedMailFromSignInAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var settings = fixture.Create<AppSettings>();

            var roleList = new List<string> { fixture.Create<string>() };

            var appuser = fixture.Build<ApplicationUser>().With(o => o.EmailConfirmed, false).Create();

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).Returns(roleList);
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns(appuser);
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._))
                .Returns(SignInResult.Failed);

            //Act
            var result = await userService.SignInAsync(basicUserDto, settings);

            //Assert
            Assert.Equal(ServiceResultType.InternalError, result.Result);

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task ShouldReturnInvalidDataOnEmptyRolesFromSignInAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var settings = fixture.Create<AppSettings>();

            //Act
            var result = await userService.SignInAsync(basicUserDto, settings);

            //Assert
            Assert.Equal(ServiceResultType.InvalidData, result.Result);

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task ShouldReturnInvalidDataOnEmptyUserFromSignInAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var settings = fixture.Create<AppSettings>();

            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns<ApplicationUser>(null);

            //Act
            var result = await userService.SignInAsync(basicUserDto, settings);

            //Assert
            Assert.Equal(ServiceResultType.InvalidData, result.Result);

            A.CallTo(() => userManager.GetRolesAsync(A<ApplicationUser>._)).MustNotHaveHappened();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => signInManager.CheckPasswordSignInAsync(A<ApplicationUser>._, A<string>._, A<bool>._))
                .MustNotHaveHappened();
        }


        [Fact]
        public async Task ShouldReturnSuccessFromSignUpAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();

            A.CallTo(() => userManager.CreateAsync(A<ApplicationUser>._, A<string>._)).Returns(IdentityResult.Success);
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns<ApplicationUser>(null);

            //Act
            var result = await userService.SignUpAsync(basicUserDto);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => userManager.CreateAsync(A<ApplicationUser>._, A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedTwiceExactly();
        }

        [Fact]
        public async Task ShouldReturnInternalErrorFromSignUpAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();

            A.CallTo(() => userManager.CreateAsync(A<ApplicationUser>._, A<string>._)).Returns(IdentityResult.Failed());
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).Returns<ApplicationUser>(null);

            //Act
            var result = await userService.SignUpAsync(basicUserDto);

            //Assert
            Assert.Equal(ServiceResultType.InternalError, result.Result);

            A.CallTo(() => userManager.CreateAsync(A<ApplicationUser>._, A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.FindByEmailAsync(A<string>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnInvalidDataFromSignUpAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var fixture = new Fixture
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
        public async Task ShouldSendConfirmationMessageAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var actionName = "send";
            var controllerName = "UserController";
            var data = fixture.Create<(ApplicationUser, string)>();
            var scheme = "scheme";

            //Act
            await userService.SendConfirmationMessageAsync(actionName, controllerName, data, scheme);

            //Assert
            A.CallTo(() => emailSender.SendEmailAsync(A<string>._, A<string>._, A<string>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnSuccessFromConfirmAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var id = "1";
            var token = "UserController";

            A.CallTo(() => userManager.ConfirmEmailAsync(A<ApplicationUser>._, A<string>._))
                .Returns(IdentityResult.Success);

            //Act
            var result = await userService.ConfirmAsync(id, token);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => userManager.FindByIdAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.ConfirmEmailAsync(A<ApplicationUser>._, A<string>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnInternalErrorFromConfirmAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var id = "1";
            var token = "UserController";

            A.CallTo(() => userManager.ConfirmEmailAsync(A<ApplicationUser>._, A<string>._))
                .Returns(IdentityResult.Failed());

            //Act
            var result = await userService.ConfirmAsync(id, token);

            //Assert
            Assert.Equal(ServiceResultType.InternalError, result.Result);

            A.CallTo(() => userManager.FindByIdAsync(A<string>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => userManager.ConfirmEmailAsync(A<ApplicationUser>._, A<string>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldUpdateUserPasswordAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();
            var user = fixture.Create<ApplicationUser>();

            A.CallTo(() => userRepository.UpdateUserPasswordAsync(A<int>._, A<string>._))
                .Returns(new ServiceResult(ServiceResultType.Success));

            //Act
            var result = await userService.UpdateUserPasswordAsync(user, basicUserDto);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => userRepository.UpdateUserPasswordAsync(A<int>._, A<string>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldUpdateUserProfileAsync()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var userId = 1;
            var user = fixture.Create<ApplicationUser>();
            var basicUserDto = fixture.Build<BasicUserDto>().With(o => o.Email, "ajsdhaijusd@gmail.com").Create();

            A.CallTo(() => userRepository.UpdateUserAsync(A<ApplicationUser>._, A<int>._))
                .Returns(new ServiceResult<ApplicationUser>(ServiceResultType.Success, user));

            //Act
            var result = await userService.UpdateUserProfileAsync(userId, basicUserDto);

            //Assert
            Assert.Equal(ServiceResultType.Success, result.Result);

            A.CallTo(() => userRepository.UpdateUserAsync(A<ApplicationUser>._, A<int>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ShouldReturnUserFromCache()
        {
            //Arrange
            var userManager = A.Fake<UserManager<ApplicationUser>>();
            var userRepository = A.Fake<IUserRepository>();
            var signInManager = A.Fake<SignInManager<ApplicationUser>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var userId = 1;
            var user = fixture.Create<ApplicationUser>();

            A.CallTo(() => cacheService.GetEntity(A<int>._)).Returns(new ServiceResult<ApplicationUser>());
            A.CallTo(() => userRepository.FindUserByIdAsync(A<int>._))
                .Returns(new ServiceResult<ApplicationUser>(ServiceResultType.Success, user));

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
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);
            var fixture = new Fixture
            {
                Behaviors = { new NullRecursionBehavior() }
            };

            var userId = 1;
            var user = fixture.Create<ApplicationUser>();


            A.CallTo(() => cacheService.GetEntity(A<int>._))
                .Returns(new ServiceResult<ApplicationUser>(ServiceResultType.Success, user));

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
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
            {
                new RoleModelProfile(), new UserModelProfile(), new OrderModelProfile(), new ProductModelProfile()
            })));
            var roleService = A.Fake<IRoleService>();
            var urlHelper = A.Fake<IUrlHelper>();
            var emailSender = A.Fake<IEmailSender>();
            var cacheService = A.Fake<ICacheService<ApplicationUser>>();

            var userService = new UserService(userManager, userRepository, signInManager, mapper, roleService,
                urlHelper, emailSender, cacheService);

            var userId = 1;

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