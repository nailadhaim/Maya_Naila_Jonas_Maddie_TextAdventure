using Maya_Naila_Jonas_Maddie_TextAdventure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure.Tests
{

    public class InventoryTests
    {
        [Fact]
        public void Add_ShouldAddItemToInventory()
        {
            var inventory = new Inventory();
            var item = new Item("sword", "Sword", "A sharp blade");
            bool added = inventory.Add(item);
            Assert.True(added);
            Assert.True(inventory.Has("sword"));
            Assert.Single(inventory.All());
        }

        [Fact]
        public void Remove_ShouldRemoveItemFromInventory()
        {
            var inventory = new Inventory();
            var item = new Item("key", "Key", "Opens a door");
            inventory.Add(item);
            bool removed = inventory.Remove("key");
            Assert.True(removed);
            Assert.False(inventory.Has("key"));
            Assert.Empty(inventory.All());
        }

        [Fact]
        public void Remove_NonExistingItem_ShouldReturnFalse()
        {
            var inventory = new Inventory();
            bool removed = inventory.Remove("nonexistent");
            Assert.False(removed);
        }

        [Fact]
        public void Has_ShouldReturnTrueIfItemExists()
        {
            var inventory = new Inventory();
            inventory.Add(new Item("sword", "Sword", "Sharp weapon"));
            bool exists = inventory.Has("sword");
            Assert.True(exists);
        }

        [Fact]
        public void Has_ShouldReturnFalseIfItemDoesNotExist()
        {
            var inventory = new Inventory();
            bool exists = inventory.Has("key");
            Assert.False(exists);
        }

        [Fact]
        public void All_ShouldReturnAllItemsAsReadOnly()
        {
            var inventory = new Inventory();
            var item1 = new Item("sword", "Sword", "Sharp weapon");
            var item2 = new Item("key", "Key", "Opens a door");
            inventory.Add(item1);
            inventory.Add(item2);
            var allItems = inventory.All().ToList();
            Assert.Equal(2, allItems.Count);
            Assert.Contains(item1, allItems);
            Assert.Contains(item2, allItems);
        }
    }
}
