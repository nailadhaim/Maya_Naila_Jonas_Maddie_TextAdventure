using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    public class GameSetup
    {
        public Room StartRoom { get; private set; }

        public GameSetup()
        {
            Room start = new Room("Start room", "You are in the middle of the dungeon");
            Room left = new Room("Left room", "Something feels wrong here");
            left.IsDeadly = true;

            Room right = new Room("Right room", "You notice a shiny object nearby");
            Room up = new Room("Upper room", "A large locked door blocks your way");
            up.RequiresKey = true;

            Room down = new Room("Lower room", "You see a sword on the ground");
            Room deeper = new Room("Monster room", "You get inside a dark cave, a monster is here");
            deeper.HasMonster = true;
            deeper.MonsterAlive = true;

            start.West = left;
            left.East = start;

            start.East = right;
            right.West = start;

            start.North = up;
            up.South = start;

            start.South = down;
            down.North = start;

            down.South = deeper;
            deeper.North = down;

            down.South = deeper;
            deeper.North = down;

            right.AddItem(new Item("key", "Key", "An old key"));
            down.AddItem(new Item("sword", "Sword", "A sharp weapon to defend yourself"));

            StartRoom = start;
        }
    }
}
