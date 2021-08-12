using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Repositories
{
    public class ProductLibraryRepository : BaseRepository<ProductLibraries>, IProductLibraryRepository
    {
        public ProductLibraryRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {
        }
    }
}