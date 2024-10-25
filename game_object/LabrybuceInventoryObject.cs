using AscendedZ.dungeon_crawling.backend.dungeon_items;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object
{
    public class LabrybuceInventoryObject
    {
        /// <summary>
        /// Name, Item
        /// </summary>
        public Dictionary<string, IDungeonItem> Inventory { get; set; }

        public LabrybuceInventoryObject()
        {
            Inventory = new Dictionary<string, IDungeonItem>();
        }

        public void AddItem(IDungeonItem item) 
        {
            if (Inventory.ContainsKey(item.Name)) 
            {
                Inventory[item.Name].Amount++;
            }
            else
            {
                item.Amount = 1;
                Inventory.Add(item.Name, item);
            }
        }

        public void UseItem(IDungeonItem item)
        {
            if (Inventory.ContainsKey(item.Name)) 
            {
                Inventory[item.Name].Amount--;
                if (Inventory[item.Name].Amount <= 0)
                    Inventory.Remove(item.Name);
            }
        }

        public List<IDungeonItem> GetItems() 
        {
            List<IDungeonItem> items = new List<IDungeonItem>();
            foreach (var key in Inventory.Keys) 
            {
                if (Inventory[key].Amount > 0)
                    items.Add(Inventory[key]);
            }
            return items;
        }
    }
}
