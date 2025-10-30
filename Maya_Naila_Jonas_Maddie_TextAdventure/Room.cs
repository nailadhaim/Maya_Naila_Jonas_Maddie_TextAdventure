using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    public class Room
    {
        private string name;
        public string Name { get => string.IsNullOrWhiteSpace(name) ? "Uknown room": name ; set => name = value; }

        private string description;
        public string Description { get => string.IsNullOrWhiteSpace(description) ? "No description available." : description; set => description = value; }

        private bool isDeadly;
        public bool IsDeadly { get => isDeadly; private set => isDeadly = value; }

        private bool hasMonster;
        public bool HasMonster { get => hasMonster; private set => hasMonster = value; }

        private bool monsterAlive;
        public bool MonsterAlive { get => monsterAlive; private set => monsterAlive = value; }

        private bool requiresKey;
        public bool RequiresKey { get => requiresKey; private set => requiresKey = value; }

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

        public Room(string name, string description, bool isDeadly,bool requiresKey) : this(name, description)
        {
            this.isDeadly = isDeadly;
            this.requiresKey = requiresKey;
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

        public void ActiveMonster()
        {
            hasMonster = true;
            monsterAlive = true;
        }

        public void KillMonster()
        {
            MonsterAlive = false;
        }

    }
}
