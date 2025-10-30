using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maya_Naila_Jonas_Maddie_TextAdventure;

namespace TextAdventure.Tests;

[TestClass]
public class RoomsTests
{
    [TestMethod]
    public void Move_ChangeCurrentRoom()
    {
        Inventory inventory = new Inventory();
        GameSetup setup = new GameSetup();
        Rooms rooms = new Rooms(inventory, setup);

        string result = rooms.Move(Direction.South);

        Assert.IsTrue(result.Contains("Sword"));
    }

    [TestMethod]
    public void LockedDoorWithoutKey()
    {
        Inventory inventory = new Inventory();
        GameSetup setup = new GameSetup();
        Rooms rooms = new Rooms(inventory, setup);

        string result = rooms.Move(Direction.North);

        Assert.AreEqual("The door is locked, you need a key", result);
    }

    [TestMethod]
    public void DeadlyRoomGameOver()
    {
        Inventory inventory = new Inventory();
        GameSetup setup = new GameSetup();
        Rooms rooms = new Rooms(inventory, setup);

        string result = rooms.Move(Direction.West);

        Assert.IsTrue(rooms.IsGameOver);
        Assert.AreEqual("You stepped into a deadly trap => GAME OVER", result);
    }

    [TestMethod]
    public void FightWithSword_DefeatMonster()
    {
        Inventory inventory = new Inventory();
        inventory.Add(new Item("sword", "Sword", "Weapon"));
        GameSetup setup = new GameSetup();
        Rooms rooms = new Rooms(inventory, setup);
        rooms.Move(Direction.South);
        rooms.Move(Direction.South);

        string result = rooms.Fight();

        Assert.AreEqual("You fought and defeated the monster", result);
        Assert.IsFalse(rooms.CurrentRoom.MonsterAlive);
    }

    [TestMethod]
    public void FightWithoutSword_Death()
    {
        Inventory inventory = new Inventory();
        GameSetup setup = new GameSetup();
        Rooms rooms = new Rooms(inventory, setup);
        rooms.Move(Direction.South);
        rooms.Move(Direction.South);

        string result = rooms.Fight();

        Assert.IsTrue(rooms.IsGameOver);
        Assert.AreEqual("You fought the monster without a weapon => DEAD", result);
    }
}
