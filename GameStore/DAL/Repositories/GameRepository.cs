using GameStore.DAL.Entities;

namespace GameStore.DAL.Repositories
{
    public class GameRepository : BaseRepository<Game>
    {
        public GameRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}