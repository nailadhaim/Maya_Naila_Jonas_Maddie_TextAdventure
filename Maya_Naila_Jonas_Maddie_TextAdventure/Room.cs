using System;
using System.Collections.Generic;
using System.Text;

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
        public bool IsEncrypted { get; set; } = false;
        public string RoomKeyId { get; set; }
        public string EncryptedFilePath { get; set; }


        public Room(string name, string description, bool encrypted = false)
        {
            Name = name;

       
            Description = encrypted ? EncryptText(description) : description;

            IsDeadly = false;
            HasMonster = false;
            RequiresKey = false;
            MonsterAlive = false;
        }
        
        public static string EncryptText(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static string DecryptText(string encrypted)
        {
            try
            {
                var bytes = Convert.FromBase64String(encrypted);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
             
                return encrypted;
            }
        }
     

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public Item TakeItem(string id)
        {
            var foundItem = items.Find(i => i.Id == id);

            if (foundItem != null)
                items.Remove(foundItem);

            return foundItem;
        }

        public List<Item> GetItems()
        {
            return items;
        }

        public string Describe()
        {
            string decryptedDescription = DecryptText(Description);

            string text = $"\nYou are in {Name}. {decryptedDescription}\n";

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
                text += "\nThere are no items here\n";
            }

            List<string> exits = new List<string>();

            if (North != null) exits.Add("north");
            if (South != null) exits.Add("south");
            if (East != null) exits.Add("east");
            if (West != null) exits.Add("west");

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
