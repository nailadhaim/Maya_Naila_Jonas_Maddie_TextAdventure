using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    internal class Rooms
    {
        public Room CurrentRoom { get; private set; }

        private Room start;
        private Room left;
        private Room right;
        private Room up;
        private Room down;
        private Room deeper;

        private Inventory playerInventory;

        public bool IsGameOver { get; private set; }
        public bool IsWin { get; private set; }

        public Rooms(Inventory inventory)
        {
            playerInventory = inventory;
            CurrentRoom = start;
            SetUpWorld();
        }

        private void SetUpWorld()
        {
            start = new Room("Start room", "You are in the middle of the dungeon");
            left = new Room("Left room", "Something feels wrong here.");
            left.isDeadly = true;

            right = new Room("Right room", "You notice a shiny object nearby.");
            up = new Room("Upper room", "A large locked door blocks your way.");
            up.RequiresKey = true;

            down = new Room("Lower room", "You see a sword on the ground.");
            deeper = new Room("Monster room", "A dark cave, a monster is here");
            deeper.HasMonster = true;

            start.West = left;
            start.East = right;
            start.North = up;
            start.South = down;

            down.South = deeper;
            deeper.North = down;
        }

    }
}
