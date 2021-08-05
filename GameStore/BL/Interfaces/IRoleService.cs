using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO;
using GameStore.WEB.DTO.UserModels;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface IRoleService
    {
        public Task<ServiceResult> CreateAsync(RoleModel roleModel);

        public Task<ServiceResult> DeleteAsync(string id);

        public Task<ServiceResult> EditAsync(BasicUserRoleModel basicUserRoleModel);
    }
}