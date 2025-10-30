using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maya_Naila_Jonas_Maddie_TextAdventure;

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
        Item key = new Item("key", "Key", "A small key"); room.AddItem(key);

        Item taken = room.TakeItem("key");

        Assert.AreEqual(key, taken);
        Assert.AreEqual(0, room.GetItems().Count);
    }

    [TestMethod]
    public void Describe_IncludeItems()
    {
        Room room1 = new Room("Start room", "The start point");
        Room room2 = new Room("Next room", "Room connected to the start");
        room1.East = room2;
        room1.AddItem(new Item("torch", "Torch", "Light source"));

        string description = room1.Describe();

        Assert.IsTrue(description.Contains("Torch"));
        Assert.IsTrue(description.Contains("east"));
    }

    [TestMethod]
    public void Describe_NoItems()
    {
        Room room = new Room("Empty room", "A bare room");

        string description = room.Describe();

        Assert.IsTrue(description.Contains("There are no items here"));
    }
}
