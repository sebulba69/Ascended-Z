

namespace AscendedZ.dungeon_crawling.backend
{
    public class Tile
    {
        private int _x, _y;

        public bool EventTriggered { get; set; }
        public bool Visited { get; set; }
        public bool IsPartOfMaze { get; set; }
        public int X { get => _x; }
        public int Y { get => _y; }
        public TileEventId TileEventId { get; set; }
        public string Graphic { get; set; }
        public string Entity { get; set; }
        public string EntityImage { get; set; }
        public int[] TPLocation { get; set; }

        public Tile(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}
