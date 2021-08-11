using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Identity;

namespace GameStore.DAL.Repositories
{
    public class ProductLibraryRepository : BaseRepository<ProductLibraries>, IProductLibraryRepository
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductLibraryRepository
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