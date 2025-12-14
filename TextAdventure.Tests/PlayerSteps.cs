using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maya_Naila_Jonas_Maddie_TextAdventure;
using TechTalk.SpecFlow;
using System;

namespace TextAdventure.Tests
{
    [Binding]
    public class PlayerSteps
    {
        private Inventory inventory;
        private Rooms rooms;
        private string lastMoveResult;  

        [Given(@"the player is in the start room")]
        public void GivenThePlayerIsInTheStartRoom()
        {
            inventory = new Inventory();
            GameSetup setup = new GameSetup();
            rooms = new Rooms(inventory, setup);
        }

        [When(@"the player goes (.*)")]
        public void WhenThePlayerGoes(string direction)
        {
            Direction dir = direction.ToLower() switch
            {
                "north" => Direction.North,
                "south" => Direction.South,
                "east" => Direction.East,
                "west" => Direction.West,
                _ => throw new Exception("Invalid direction")
            };

            lastMoveResult = rooms.Move(dir);
        }

        [When(@"the player takes the (.*)")]
        public void WhenThePlayerTakesTheItem(string itemId)
        {
            var item = rooms.CurrentRoom.TakeItem(itemId);
            inventory.Add(item);
        }

        [When(@"the player fights the monster")]
        public void WhenThePlayerFightsTheMonster()
        {
            lastMoveResult = rooms.Fight();
        }

        [Then(@"the monster should be defeated")]
        public void ThenTheMonsterShouldBeDefeated()
        {
            Assert.IsFalse(rooms.CurrentRoom.MonsterAlive);
        }

        [Then(@"the player should not be dead")]
        public void ThenThePlayerShouldNotBeDead()
        {
            Assert.IsFalse(rooms.IsGameOver);
        }

        [Then(@"the player should be dead")]
        public void ThenThePlayerShouldBeDead()
        {
            Assert.IsTrue(rooms.IsGameOver);
        }

        [Then(@"the player should not be able to enter")]
        public void ThenThePlayerShouldNotBeAbleToEnter()
        {
            Assert.AreEqual("This door is locked, you need a key!", lastMoveResult);
        }

        [Then(@"the player should see ""(.*)""")]
        public void ThenThePlayerShouldSee(string expectedMessage)
        {
            Assert.AreEqual(expectedMessage, lastMoveResult);
        }
    }
}
