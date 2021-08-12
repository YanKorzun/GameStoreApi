using System.Threading.Tasks;
using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO.Roles;
using GameStore.WEB.DTO.Users;

namespace GameStore.BL.Interfaces
{
    public interface IRoleService
    {
        Task<ServiceResult> CreateAsync(RoleDto roleDto);

        Task<ServiceResult> DeleteAsync(string id);

        Task<ServiceResult> EditAsync(BasicUserRoleModel basicUserRoleModel);
    }
}