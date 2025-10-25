using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    internal class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool isDeadly { get; set; }
        public bool HasMonster { get; set; }
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
            isDeadly = false;
            HasMonster = false;
            RequiresKey = false;
        }
    }
}
