using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    internal class GameSetup
    {
        public Room StartRoom { get; private set; }

        public GameSetup()
        {
            Room start = new Room("Start room", "You are in the middle of the dungeon");
            Room left = new Room("Left room", "Something feels wrong here.");
            left.IsDeadly = true;

            Room right = new Room("Right room", "You notice a shiny object nearby.");
            Room up = new Room("Upper room", "A large locked door blocks your way.");
            up.RequiresKey = true;

            Room down = new Room("Lower room", "You see a sword on the ground.");
            Room deeper = new Room("Monster room", "You get inside a dark cave, a monster is here");
            deeper.HasMonster = true;
            deeper.MonsterAlive = true;

            start.West = left;
            start.East = right;
            start.North = up;
            start.South = down;

            down.South = deeper;
            deeper.North = down;
        }
    }
}
