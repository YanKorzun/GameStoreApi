using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using GameStore.WEB.DTO;
using System.Threading.Tasks;

namespace GameStore.DAL.Interfaces
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
        public Task<ServiceResult<ApplicationUser>> UpdateUser(ApplicationUser appUser, int userId);

        public Task<ServiceResult<ApplicationUser>> AddLibraryGame(string email, GamesLibrary game);

        public Task<ServiceResult> UpdateUserPassword(int userId, string newPasswords);

        public Task<ServiceResult<ApplicationUser>> FindUserByIdAsync(int userId);
    }
}