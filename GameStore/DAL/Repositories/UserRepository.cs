using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.BL.Enums;
using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(
            IPasswordValidator<ApplicationUser> passwordValidator,
            IPasswordHasher<ApplicationUser> passwordHasher,
            ApplicationDbContext databaseContext,
            UserManager<ApplicationUser> userManager) :
            base(databaseContext)
        {
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
            _userManager = userManager;
        }

        public async Task<ServiceResult<ApplicationUser>> UpdateUserAsync(ApplicationUser appUser, int userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId.ToString());
            existingUser.UserName = appUser.UserName;
            existingUser.PhoneNumber = appUser.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                return new(ServiceResultType.InternalError);
            }

            var fullUser = (await FindUserByIdAsync(userId)).Data;
            return new(ServiceResultType.Success, fullUser);
        }

        public async Task<ServiceResult> UpdateUserPasswordAsync(int userId, string newPassword)
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

        public async Task<ServiceResult<ApplicationUser>> FindUserByIdAsync(int userId)
        {
            var users = await GetUserWithChildrenAsync(o => o.Id == userId);
            return new(ServiceResultType.Success, users);
        }

        private async Task<ApplicationUser> GetUserWithChildrenAsync(Expression<Func<ApplicationUser, bool>> expression)
        {
            return await Entity.AsNoTracking().Include(o => o.ProductLibraries).ThenInclude(o => o.Game)
                .Include(o => o.UserRoles).ThenInclude(o => o.Role).FirstOrDefaultAsync(expression);
        }
    }
}