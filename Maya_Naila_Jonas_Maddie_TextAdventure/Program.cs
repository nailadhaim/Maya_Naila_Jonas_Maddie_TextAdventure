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
            Console.WriteLine("Hello, welcome to the text adventure game!!");
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
            Console.WriteLine("Available commands:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("help              - show the list of commands");
            Console.WriteLine("look              - show inventory, current room, items in the room, and exits");
            Console.WriteLine("inventory         - show only your inventory");
            Console.WriteLine("go n|e|s|w        - move to a room in the given direction (north, east, south, west)");
            Console.WriteLine("take <id>         - pick up an item with the given id");
            Console.WriteLine("fight             - fight the monster if you are in the correct room");
            Console.WriteLine("quit              - quit the game");
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
                    Console.WriteLine("\nThat’s not a valid direction.\n");
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
                Console.WriteLine("\nThere’s no such item here.\n");
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
