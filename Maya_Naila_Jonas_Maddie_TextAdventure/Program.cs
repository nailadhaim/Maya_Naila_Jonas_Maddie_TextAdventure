using System.Diagnostics;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool condition = true;
            while(condition == true)
            {
                string input = Console.ReadLine();
                string[] parts = input.Split(" ");
                string commando = parts[0];
                string argument = parts[1].ToLower();
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
                        GoToRoom();
                        break;
                    case "take":
                        TakeItem();
                        break;
                    case "fight":
                        fight();
                        break;
                    case "quit":
                        condition = false;
                        break;
                    default:
                        break;
                }
            }
        }
        static void SHowCommands()
        {
        }
        static void showAll()
        {
        }
        static void showInventory()
        {
        }
        static void GoToRoom()
        {
        }
        static void TakeItem()
        {
        }
        static void fight()
        {
        }
    }
}
