using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maya_Naila_Jonas_Maddie_TextAdventure;


namespace TextAdventure.Tests
{
    [TestClass]
    public sealed class InventoryTests
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
            inventory.Add(sword);
            Assert.IsTrue(inventory.Has("sword"));
        }

        [TestMethod]
        public void Remove_RemovesItemFromInventory()
        {
            inventory.Add(sword);
            inventory.Remove("sword");
            Assert.IsFalse(inventory.Has("sword"));
        }

        [TestMethod]
        public void Item_ToString_ReturnsCorrectFormat()
        {
            Assert.AreEqual("Sword (sword): A sharp weapon", sword.ToString());
        }
    }
}
