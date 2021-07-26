using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class ApplicationUser : IdentityUser<int>, IBaseUser
    {
        public IList<ApplicationRole> UserRoles { get; set; }
    }
}