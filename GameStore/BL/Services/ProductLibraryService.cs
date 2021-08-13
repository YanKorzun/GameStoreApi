using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BL.Interfaces;
using GameStore.DAL.Entities;
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

        public async Task AddItemsToLibraryAsync(IEnumerable<ProductLibraries> items)
        {
            await _productLibraryRepository.CreateItemsAsync(items);
        }
    }
}