using AscendedZ.dungeon_crawling.backend.dungeon_items;
using AscendedZ.game_object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.scenes.crawl_ui
{
    public class ShopkeeperUIObject
    {
        private IList<IDungeonItem> _items =
            [
                new TeleportToExitItem(),
                new TeleportToBossRoomItem(),
                new HealPartyMemberFullItem(),
                new HealPartyMemberReviveItem()
            ];

        public IList<IDungeonItem> Items { get => _items; }

        public int Selected { get; set; }

        public IDungeonItem SelectedItem { get => _items[Selected]; }
        public int OrbCount { get => _gameObject.Orbs; }

        private GameObject _gameObject;

        public ShopkeeperUIObject()
        {
            _gameObject = PersistentGameObjects.GameObjectInstance();
        }

        public void Buy()
        {
            var item = Items[Selected];
            if(_gameObject.Orbs - item.Cost >= 0)
            {
                _gameObject.LabrybuceInventoryObject.AddItem(item);
                _gameObject.Orbs -= item.Cost;
                PersistentGameObjects.Save();
            }
        }
    }
}
