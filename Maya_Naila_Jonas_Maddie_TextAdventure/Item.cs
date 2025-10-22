﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    public class Item
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }

        public Item(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public override string ToString()
        {
            return $"{Name} ({Id}): {Description}";
        }
    }
}
