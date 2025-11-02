using System;
using Maya_Naila_Jonas_Maddie_TextAdventure;

namespace TextAdventure.Tests
{
    [TestClass]
    public class IntegrationTests
	{
        [TestMethod]
        [TestCategory("Integration")]
        public void Move_CannotEnterLockedRoomWithoutKey()
        {
            var inventory = new Inventory();
            var setup = new GameSetup();
            var rooms = new Rooms(inventory, setup);

            var result = rooms.Move(Direction.North);

            Assert.AreEqual("This door is locked, you need a key!", result);
            Assert.AreEqual(setup.StartRoom, rooms.CurrentRoom);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TakeItem_RemovesItemFromRoom_AndAddsToInventory()
        {
            // Arrange
            var room = new Room("Test room", "Room for testing");
            var item = new Item("key", "Key", "A shiny key");
            room.AddItem(item);

            var inventory = new Inventory();

            // Act
            var takenItem = room.TakeItem("key");
            inventory.Add(takenItem);

            // Assert
            Assert.IsFalse(room.GetItems().Contains(item));
            Assert.IsTrue(inventory.Has("key"));
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Fight_WithAndWithoutSword_Behavior()
        {
            var setup1 = new GameSetup();
            var inventory1 = new Inventory();
            var rooms1 = new Rooms(inventory1, setup1);
            rooms1.Move(Direction.South);
            rooms1.Move(Direction.South);
            var result1 = rooms1.Fight();
            Assert.IsTrue(rooms1.IsGameOver);
            Assert.AreEqual("You tried to fight the monster without a weapon => DEAD", result1);

            var setup2 = new GameSetup();
            var inventory2 = new Inventory();
            var rooms2 = new Rooms(inventory2, setup2);

            rooms2.Move(Direction.South);
            var downRoom = rooms2.CurrentRoom;
            var sword = downRoom.TakeItem("sword");
            inventory2.Add(sword);

            rooms2.Move(Direction.South);
            var result2 = rooms2.Fight();
            Assert.AreEqual("You fought and defeated the monster!!", result2);
            Assert.IsFalse(rooms2.IsGameOver);
        }
    }
}


