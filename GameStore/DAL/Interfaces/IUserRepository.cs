using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using System.Threading.Tasks;

namespace GameStore.DAL.Interfaces
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
        public Task<ServiceResult<ApplicationUser>> UpdateUserAsync(ApplicationUser appUser, int userId);

        public Task<ServiceResult> UpdateUserPasswordAsync(int userId, string newPasswords);

        public Task<ServiceResult<ApplicationUser>> FindUserByIdAsync(int userId);
    }
}