using GameStore.DAL.Entities;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Identity;

namespace GameStore.DAL.Repositories
{
    public class GameLibraryRepository : BaseRepository<ProductLibraries>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;

        public GameLibraryRepository
              (
              ApplicationDbContext databaseContext,
              UserManager<ApplicationUser> userManager,
              AppSettings appSettings
              ) : base(databaseContext)
        {
            _userManager = userManager;
            _appSettings = appSettings;
        }
    }
}