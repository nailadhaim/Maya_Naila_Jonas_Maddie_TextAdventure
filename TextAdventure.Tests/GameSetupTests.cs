using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maya_Naila_Jonas_Maddie_TextAdventure;

namespace TextAdventure.Tests;

[TestClass]
public class GameSetupTests
{
    [TestMethod]
    public void CreateRoomsAndLinks()
    {
        GameSetup setup = new GameSetup();
        Room start = setup.StartRoom;

        Assert.IsNotNull(start.North, "The room should have a room to the north");
        Assert.IsNotNull(start.South, "The room should have a room to the south");
        Assert.IsNotNull(start.East, "The room should have a room to the east");
        Assert.IsNotNull(start.West, "The room should have a room to the west");
    }

    [TestMethod]
    public void SwordExists()
    {
        GameSetup setup = new GameSetup();

        Room down = setup.StartRoom.South;

        Assert.IsTrue(down.GetItems().Exists(i => i.Id == "sword"), "A sword should exist in the lower room");
    }

    [TestMethod]
    public void KeyExists()
    {
        GameSetup setup = new GameSetup();

        Room right = setup.StartRoom.East;

        Assert.IsTrue(right.GetItems().Exists(i => i.Id == "key"), "A key should exist in the right room");
    }
}
