using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class ApplicationRole : IdentityRole<int>, IBaseUser
    {
        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        public ApplicationRole() : base()
        {
        }

        public IList<ApplicationUserRole> UserRoles { get; set; }
    }
}