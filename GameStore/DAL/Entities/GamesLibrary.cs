namespace GameStore.DAL.Entities
{
    public class GamesLibrary
    {
        public GamesLibrary()
        {
        }

        public GamesLibrary(int userId, int gameId)
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