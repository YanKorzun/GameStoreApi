using System.Threading.Tasks;

namespace GameStore.BL.Interfaces
{
    public interface IProductLibraryService
    {
        Task AddItemToLibrary(int userId, int productId);
    }
}