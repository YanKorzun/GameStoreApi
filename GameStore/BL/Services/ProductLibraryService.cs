using System.Threading.Tasks;
using GameStore.BL.Interfaces;
using GameStore.DAL.Interfaces;

namespace GameStore.BL.Services
{
    public class ProductLibraryService : IProductLibraryService
    {
        private readonly IProductLibraryRepository _productLibraryRepository;

        public ProductLibraryService(IProductLibraryRepository productLibraryRepository)
        {
            _productLibraryRepository = productLibraryRepository;
        }

        public async Task AddItemToLibrary(int userId, int productId)
        {
            await _productLibraryRepository.CreateItemAsync(new(userId, productId));
        }
    }
}