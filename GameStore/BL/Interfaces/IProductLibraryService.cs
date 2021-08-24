using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.DAL.Entities;

namespace GameStore.BL.Interfaces
{
    public interface IProductLibraryService
    {
        Task AddItemsToLibraryAsync(IEnumerable<ProductLibraries> items);
    }
}