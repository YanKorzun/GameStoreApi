using GameStore.BL.ResultWrappers;
using GameStore.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BL.Services
{
    public interface IRoleService
    {
        public ServiceResult<ActionResult<IList<ApplicationRole>>> GetRoles();

        public Task<ServiceResult> CreateAsync(string roleName);

        public Task<ServiceResult> DeleteAsync(string id);

        public Task EditAsync(string userId, string roleName);
    }
}