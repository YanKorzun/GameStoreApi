using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace GameStore.DAL.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public IList<ApplicationUserRole> UserRoles { get; set; }
        public bool IsDeleted { get; set; }
    }
}