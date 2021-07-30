using System;
using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Developers { get; set; }
        public string Publishers { get; set; }
        public DateTime PublicationDate { get; set; }
        public IList<GameLibraries> GameLibraries { get; set; } = new List<GameLibraries>();
    }
}