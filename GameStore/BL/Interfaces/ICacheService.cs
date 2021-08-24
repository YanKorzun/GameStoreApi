using GameStore.BL.ResultWrappers;

namespace GameStore.BL.Interfaces
{
    public interface ICacheService<T>
    {
        ServiceResult<T> GetEntity(int id);
        void SetEntity(int id, T entity);
        ServiceResult RemoveEntity(int id);
    }
}