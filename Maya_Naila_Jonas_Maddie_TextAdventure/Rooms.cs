using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    }
    internal class Rooms
    {
        public Room CurrentRoom { get; private set; }
        private Inventory playerInventory;

        public bool IsGameOver { get; private set; }
        public bool IsWin { get; private set; }

        public Rooms(Inventory inventory, GameSetup setup)
        {
            playerInventory = inventory;
            CurrentRoom = setup.StartRoom;
        }

        public string Move(Direction direction)
        {
            Room nextRoom = null;

            switch (direction)
            {
                case Direction.North:
                    nextRoom = CurrentRoom.North;
                    break;
                case Direction.South:
                    nextRoom = CurrentRoom.South;
                    break;

                case Direction.East:
                    nextRoom = CurrentRoom.East;
                    break;

                case Direction.West:
                    nextRoom = CurrentRoom.West;
                    break;

                default:
                    return "This is not a correct direction.";
            }


            if (nextRoom == null)
            {
                return "You can't go that way.";
            }

            if (nextRoom.RequiresKey && !playerInventory.Has("key"))
            {
                return "This door is locked, you need a key.";
            }

            if (nextRoom.IsDeadly)
            {
                IsGameOver = true;
                return "You stepped into a deadly trap. GAME OVER";
            }

            if (nextRoom.RequiresKey && playerInventory.Has("key"))
            {
                IsWin = true;
                return "You unlocked the door and escaped. WIN";
            }

            if (CurrentRoom.HasMonster && CurrentRoom.MonsterAlive)
            {
                IsGameOver = true;
                return "You tried to run away while the monster is alive. DEAD";
            }

            CurrentRoom = nextRoom;
            return CurrentRoom.Describe();
        }

        public string Fight()
        {   
            if (!CurrentRoom.HasMonster)
            {
                return "There is nothing to fight here.";
            }

            if (!playerInventory.Has("sword"))
            {
                IsGameOver = true;
                return "You tried to fight the monster without a weapon. DEAD";
            }

            if (CurrentRoom.MonsterAlive == false)
            {
                return "The monster is already defeated.";
            }

            CurrentRoom.MonsterAlive = false;
            return "You fought and defeated the monster!";
        }
    }
}
