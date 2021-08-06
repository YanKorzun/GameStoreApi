using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public IList<ApplicationUserRole> UserRoles { get; set; }
        public bool IsDeleted { get; set; }
    }
}