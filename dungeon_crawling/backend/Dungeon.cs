using System;
using System.Collections.Generic;


public enum Direction
{
    Left, Up, Right, Down
}

namespace AscendedZ.dungeon_crawling.backend
{
    /// <summary>
    /// Dungeon that exists as a straight line.
    /// Contains 20 encounters divided into groups.
    /// 20 tiles from left to right
    ///     Encounters laid out like so (None, Encounter, None, Event, None, Encounter)
    /// </summary>
    public class Dungeon
    {
        public EventHandler<TileEventId> TileEventTriggered;
        private DungeonGenerator _genenerator;
        private List<WeightedItem<PathType>> _pathTypes;

        private Tile[,] _dungeon;
        private Tile _currentTile;

        public Tile[,] Tiles { get => _dungeon; }
        public Tile Current { get => _currentTile; }
        public Tile Exit { get=> _genenerator.Exit; }
        public Tile Boss { get => _genenerator.Boss; }
        public string RequiredString { get => _genenerator.GetRequiredString(); }
        public bool CanExit { get => _genenerator.CanExit; }
        public FloorTypes FloorType { get => _genenerator.FloorType; }
        public List<SpecialTile> SpecialTiles { get => _genenerator.SpecialTiles; }

        public Dungeon(int tier)
        {
            _genenerator = new DungeonGenerator(tier);
        }

        public void Generate()
        {
            _dungeon = _genenerator.Generate();
            _currentTile = _genenerator.Start;
        }

        public void MoveDirection(int x, int y, bool isTeleport = false)
        {
            _currentTile = _dungeon[x, y];
            if(!isTeleport)
                CheckTileEvent();
        }

        public void Mine(int x, int y)
        {
            _dungeon[x, y].IsPartOfMaze = true;
        }

        public void ProcessAction(TileEventId id)
        {
            _genenerator.ProcessTileEventWithRequired(id);
        }

        private void CheckTileEvent()
        {
            if (!_currentTile.EventTriggered)
            {
                _currentTile.EventTriggered = true;
                TileEventTriggered?.Invoke(this, _currentTile.TileEventId);
            }
        }

    }
}
