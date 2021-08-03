using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.WEB.Settings;
using Microsoft.AspNetCore.Identity;

namespace GameStore.DAL.Repositories
{
    public class ProductLibraryRepository : BaseRepository<ProductLibraries>, IProductLibraryRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;

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