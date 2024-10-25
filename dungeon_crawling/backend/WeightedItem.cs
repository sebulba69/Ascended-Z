using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend
{
    public class WeightedItem<E>
    {
        private E _item;
        private int _weight;

        public E Item { get => _item; }
        public int Weight { get => _weight; }

        public WeightedItem(E item, int weight)
        {
            _item = item;
            _weight = weight;
        }
    }
}
