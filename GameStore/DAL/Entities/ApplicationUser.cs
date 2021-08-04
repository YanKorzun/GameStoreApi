using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public IList<ApplicationUserRole> UserRoles { get; set; }
        public bool IsDeleted { get; set; }
        public IList<ProductLibraries> ProductLibraries { get; set; } = new List<ProductLibraries>();
    }
}