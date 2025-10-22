using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    public class Inventory
    {
        private List<Item> items = new();

        public bool Add(Item item)
        {
            items.Add(item);
            return true;
        }

        public bool Remove(string itemId)
        {
            var item = items.FirstOrDefault(i => i.Id == itemId);
            if (item == null) return false;
            items.Remove(item);
            return true;
        }

        public bool Has(string itemId)
        {
            return items.Any(i => i.Id == itemId);
        }

        public IEnumerable<Item> All()
        {
            return items.AsReadOnly();
        }
    }
}

