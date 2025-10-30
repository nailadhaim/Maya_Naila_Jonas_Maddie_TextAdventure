using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    public class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeadly { get; set; }
        public bool HasMonster { get; set; }
        public bool MonsterAlive { get; set; }
        public bool RequiresKey { get; set; }

        public Room North { get; set; }
        public Room South { get; set; }
        public Room East { get; set; }
        public Room West { get; set; }

        private List<Item> items = new List<Item>();

        public Room(string name, string description)
        {
            Name = name;
            Description = description;
            IsDeadly = false;
            HasMonster = false;
            RequiresKey = false;
            MonsterAlive = false;
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public Item TakeItem(string id)
        {
            Item foundItem = null;
            foreach (Item item in items)
            {
                if (item.Id == id)
                {
                    foundItem = item;
                    break;
                }
            }

            if (foundItem != null)
            {
                items.Remove(foundItem);
            }
            return foundItem;
        }

        public List<Item> GetItems()
        {
            return items;
        }

        public string Describe()
        {
            string text = $"\nYou are in {Name}. {Description}\n";

            if (items.Count > 0)
            {
                text += "\nYou see here:\n";
                foreach (Item i in items)
                {
                    text += "- " + i.Name + ": " + i.Description + "\n";
                }
            }
            else
            {
                text += "\nThere are no items here.\n";
            }

            List<string> exits = new List<string>();

            if (North != null)
            {
                exits.Add("north");
            }
            if (South != null)
            {
                exits.Add("south");
            }
            if (East != null)
            {
                exits.Add("east");
            }
            if (West != null)
            {
                exits.Add("west");
            }

            if (exits.Count > 0)
            {
                text += $"\nFrom here, you can go {string.Join(", ", exits)}\n";
            }
            else
            {
                text += "\nYou can't go anywhere from here\n";
            }

            return text;
        }
    }
}
