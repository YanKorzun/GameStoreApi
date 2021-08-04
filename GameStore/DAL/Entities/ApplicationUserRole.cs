using Microsoft.AspNetCore.Identity;

namespace GameStore.DAL.Entities
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public bool IsDeleted { get; set; }
        public ApplicationUser AppUser { get; set; }

        public ApplicationRole Role { get; set; }
    }
}