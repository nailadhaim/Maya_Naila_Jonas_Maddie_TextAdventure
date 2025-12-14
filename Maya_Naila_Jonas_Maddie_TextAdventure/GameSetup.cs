using System;
using System.Collections.Generic;
using System.Text;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    public class GameSetup
    {
        public Room StartRoom { get; private set; }

        public GameSetup()
        {
            Room start = new Room(
                "Start room",
                Convert.ToBase64String(Encoding.UTF8.GetBytes("You are in the middle of the dungeon"))
            );

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

            right.AddItem(new Item("key", "Key", "An old key"));
            down.AddItem(new Item("sword", "Sword", "A sharp weapon to defend yourself"));

            left.IsEncrypted = true;
            left.RoomKeyId = "secretRoom1";
            left.EncryptedFilePath = "secretRoom1.enc";

            deeper.IsEncrypted = true;
            deeper.RoomKeyId = "secretRoom2";
            deeper.EncryptedFilePath = "secretRoom2.enc";

            StartRoom = start;
        }
    }
}
