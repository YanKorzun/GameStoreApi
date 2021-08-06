using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using System.Threading.Tasks;

namespace GameStore.DAL.Interfaces
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
        Task<ServiceResult<ApplicationUser>> UpdateUserAsync(ApplicationUser appUser, int userId);

        Task<ServiceResult> UpdateUserPasswordAsync(int userId, string newPasswords);

        Task<ServiceResult<ApplicationUser>> FindUserByIdAsync(int userId);
    }
}