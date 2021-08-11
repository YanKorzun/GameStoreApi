using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.DAL.Entities;

namespace GameStore.BL.Interfaces
{
    public interface IProductLibraryService
    {
        Task AddItemsToLibrary(IEnumerable<ProductLibraries> items);
    }
}