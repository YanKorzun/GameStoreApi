using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public IList<ApplicationUserRole> UserRoles { get; set; }
        public IList<Game> OwnedGames { get; set; } = new List<Game>();
        public IList<GamesLibrary> GamesLibrary { get; set; } = new List<GamesLibrary>();
    }
}