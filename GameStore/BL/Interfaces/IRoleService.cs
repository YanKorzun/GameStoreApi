using GameStore.BL.ResultWrappers;
using GameStore.WEB.DTO;
using GameStore.WEB.DTO.UserModels;
using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface IRoleService
    {
        Task<ServiceResult> CreateAsync(RoleModel roleModel);

        Task<ServiceResult> DeleteAsync(string id);

        Task<ServiceResult> EditAsync(BasicUserRoleModel basicUserRoleModel);
    }
}