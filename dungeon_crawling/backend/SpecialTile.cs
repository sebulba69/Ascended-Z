using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend
{
    public class SpecialTile
    {
        public string Graphic{ get; set; }
        public int[] Coordinates { get; set; }
        public string Entity { get; set; }
        public TileEventId Id { get; set; }
    }
}
