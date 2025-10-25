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
        }

    }
}
