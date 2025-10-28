using System.Diagnostics;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    internal class Program
    {
        static Inventory inventory;
        static Rooms rooms;
        static void Main(string[] args)
        {
            inventory = new Inventory();
            GameSetup setup = new GameSetup();
            rooms = new Rooms(inventory, setup);

            bool condition = true;
            Console.WriteLine("hello, welcome to the text adventure game!!");
            while (condition == true)
            {
                string input = Console.ReadLine();
                string[] parts = input.Split(" ");
                string commando = parts[0];
                string argument = null;
                if(parts.Length > 1)
                {
                    argument = parts[1].ToLower();
                }
                switch (commando)
                {

                    case "help":
                        SHowCommands();
                        break;
                    case "look":
                        showAll();
                        break;
                    case "inventory":
                        showInventory();
                        break;
                    case "go":
                        GoToRoom(argument);
                        break;
                    case "take":
                        TakeItem(argument);
                        break;
                    case "fight":
                        fight();
                        break;
                    case "quit":
                        condition = false;
                        break;
                    default:
                        Console.WriteLine("type help");
                        break;
                }
            }

        }
        static void SHowCommands()
        {
            Console.WriteLine("Beschikbare commando's:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("help              — toon deze lijst met commando’s");
            Console.WriteLine("look              — toon inventory, huidige kamer, items in de kamer, en uitgangen");
            Console.WriteLine("inventory         — toon enkel inventory");
            Console.WriteLine("go n|e|s|w        — beweeg naar een kamer in de aangegeven richting (noord, oost, zuid, west)");
            Console.WriteLine("take <id>         — pak een item op met de gegeven id");
            Console.WriteLine("fight             — vecht met het monster als je in de juiste kamer bent");
            Console.WriteLine("quit              — stop het spel");
            Console.WriteLine("--------------------------------");
        }
        static void showAll()
        {
            Console.WriteLine(rooms.CurrentRoom.Describe());
        }
        static void showInventory()
        {
            var all = inventory.All().ToList();
            if (all.Count == 0)
            {
                Console.WriteLine("Your inventory is empty.");
                return;
            }

            Console.WriteLine("\nYou have:");
            foreach (var item in all)
            {
                Console.WriteLine($"- {item.Name}: {item.Description}");
            }
        }
        static void GoToRoom(string direction)
        {
            if (string.IsNullOrWhiteSpace(direction))
            {
                Console.WriteLine("Go where? (north, south, east, west)");
                return;
            }

            Direction dir;
            switch (direction)
            {
                case "n": dir = Direction.North; break;
                case "s": dir = Direction.South; break;
                case "e": dir = Direction.East; break;
                case "w": dir = Direction.West; break;
                default:
                    Console.WriteLine("That’s not a valid direction.");
                    return;
            }

            Console.WriteLine(rooms.Move(dir));
        }
        static void TakeItem(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                Console.WriteLine("Take what?");
                return;
            }

            var item = rooms.CurrentRoom.TakeItem(itemId);
            if (item == null)
            {
                Console.WriteLine("There’s no such item here.");
                return;
            }

            inventory.Add(item);
            Console.WriteLine($"You picked up the {item.Name}.");
        }
        static void fight()
        {
            Console.WriteLine(rooms.Fight());
        }
    }
}
