using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maya_Naila_Jonas_Maddie_TextAdventure;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TextAdventure.Tests;

[TestClass]
public class RoomTests
{
    [TestMethod]
    public void AddItem()
    {
        Room room = new Room("Test room", "Testing adding an item to the room");
        Item sword = new Item("sword", "Sword", "A sharp blade");

        room.AddItem(sword);

        Assert.AreEqual(1, room.GetItems().Count);
        Assert.AreEqual("sword", room.GetItems()[0].Id);
    }

    [TestMethod]
    public void RemoveItem()
    {
        Room room = new Room("Test room", "Testing removing an item from the room");
        Item key = new Item("key", "Key", "A small key");
        room.AddItem(key);

        Item taken = room.TakeItem("key");

        Assert.AreEqual(key, taken);
        Assert.AreEqual(0, room.GetItems().Count);
    }

    [TestMethod]
    public void ActiveMonster()
    {
        Room room = new Room("Monster room", "Testing activating a monster in the room");

        room.ActiveMonster();

        Assert.IsTrue(room.HasMonster);
        Assert.IsTrue(room.MonsterAlive);
    }

    [TestMethod]
    public void KillMonster()
    {
        Room room = new Room("Monster room", "Testing killing a monster in the room");
        room.ActiveMonster();

        room.KillMonster();

        Assert.IsFalse(room.MonsterAlive);
    }
}
