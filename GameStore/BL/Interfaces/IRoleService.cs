using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO;
using System.Threading.Tasks;

namespace GameStore.BL.Services
{
    public interface IRoleService
    {
        public Task<ServiceResult> CreateAsync(RoleModel roleModel);

        public Task<ServiceResult> DeleteAsync(string id);

        public Task<ServiceResult> EditAsync(UserWithRoleModel roleModel);
    }
}