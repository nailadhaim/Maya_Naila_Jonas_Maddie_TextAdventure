using Maya_Naila_Jonas_Maddie_TextAdventure;

namespace TextAdventure.Tests
{
    [TestClass]
    
    
        public class InventoryTests
        {
            private Inventory inventory;
            private Item sword;

            [TestInitialize]
            public void Setup()
            {
                inventory = new Inventory();
                sword = new Item("sword", "Sword", "A sharp weapon");
            }

            [TestMethod]
            public void Add_AddsItemToInventory()
            {
                bool result = inventory.Add(sword);
                Assert.IsTrue(result);
                Assert.IsTrue(inventory.Has("sword"));
            }

            [TestMethod]
            public void Remove_RemovesItemFromInventory_ReturnsTrueIfExists()
            {
                inventory.Add(sword);
                bool result = inventory.Remove("sword");
                Assert.IsTrue(result);
                Assert.IsFalse(inventory.Has("sword"));
            }

            [TestMethod]
            public void Remove_ReturnsFalseIfItemDoesNotExist()
            {
                bool result = inventory.Remove("nonexistent");
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void Has_ReturnsTrueIfItemExists()
            {
                inventory.Add(sword);
                bool result = inventory.Has("sword");
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void Has_ReturnsFalseIfItemDoesNotExist()
            {
                bool result = inventory.Has("nonexistent");
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void All_ReturnsAllItemsInInventory()
            {
                Item shield = new Item("shield", "Shield", "A sturdy shield");
                inventory.Add(sword);
                inventory.Add(shield);
                var allItems = inventory.All().ToList();
                CollectionAssert.Contains(allItems, sword);
                CollectionAssert.Contains(allItems, shield);
                Assert.AreEqual(2, allItems.Count);
            }
        }
    }

