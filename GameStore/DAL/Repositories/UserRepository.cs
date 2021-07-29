using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GameStore.DAL.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public UserRepository(
            IPasswordValidator<ApplicationUser> passwordValidator,
            IPasswordHasher<ApplicationUser> passwordHasher,
            ApplicationDbContext databaseContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppSettings appSettings) :
            base(databaseContext)
        {
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _userManager = userManager;
        }

        public async Task<ServiceResult<ApplicationUser>> AddLibraryGame(string email, GamesLibrary game)
        {
            var existingUser = await GetUserWithChildren(usr => usr.Email == email);

            existingUser.GamesLibrary.Add(game);

            var updateResult = await _userManager.UpdateAsync(existingUser);
            return new(ServiceResultType.Success, existingUser);
        }

        public async Task<ServiceResult<ApplicationUser>> UpdateUser(ApplicationUser appUser, int userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId.ToString());
            existingUser.UserName = appUser.UserName;
            existingUser.Email = appUser.Email;
            existingUser.PhoneNumber = appUser.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(existingUser);

            return new(ServiceResultType.Success, existingUser);
        }

        public async Task<ServiceResult> UpdateUserPassword(int userId, string newPassword)
        {
            var existingUser = await _userManager.FindByIdAsync(userId.ToString());

            var validateResult = await _passwordValidator.ValidateAsync(_userManager, existingUser, newPassword);
            if (!validateResult.Succeeded)
            {
                return new(ServiceResultType.InternalError);
            }
            existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, newPassword);
            await _userManager.UpdateAsync(existingUser);

            return new(ServiceResultType.Success);
        }

        public async Task<ServiceResult<ApplicationUser>> FindUserByIdAsync(int userId) => new(ServiceResultType.Success, await GetUserWithChildren(o => o.Id == userId));

        private async Task<ApplicationUser> GetUserWithChildren(Expression<Func<ApplicationUser, bool>> expression)
            => await _entity.Include(o => o.OwnedGames).Include(o => o.GamesLibrary).Include(o => o.UserRoles).ThenInclude(o => o.Role).FirstOrDefaultAsync(expression);
    }
}