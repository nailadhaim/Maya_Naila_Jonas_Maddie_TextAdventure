using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maya_Naila_Jonas_Maddie_TextAdventure;
using System.Linq;

namespace TextAdventure.Tests
{
    [TestClass]
    public sealed class ItemTests
    {
        [TestMethod]
        public void Constructor_AssignsPropertiesCorrectly()
        {
            Item sword = new Item("sword", "Sword", "A sharp weapon");
            Assert.AreEqual("sword", sword.Id);
            Assert.AreEqual("Sword", sword.Name);
            Assert.AreEqual("A sharp weapon", sword.Description);
        }

        [TestMethod]
        public void ToString_ReturnsCorrectFormat()
        {
            Item sword = new Item("sword", "Sword", "A sharp weapon");
            Assert.AreEqual("Sword (sword): A sharp weapon", sword.ToString());
        }
    }
}
