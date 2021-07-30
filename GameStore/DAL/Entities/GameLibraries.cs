namespace GameStore.DAL.Entities
{
    public class GameLibraries
    {
        public GameLibraries()
        {
        }

        public GameLibraries(int userId, int gameId)
        {
            UserId = userId;
            GameId = gameId;
        }

        public int UserId { get; set; }
        public int GameId { get; set; }

        public ApplicationUser AppUser { get; set; }
        public Game Game { get; set; }
    }
}