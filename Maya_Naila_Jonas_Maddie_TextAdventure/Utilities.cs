using System;
using System.Security.Cryptography;
using System.Text;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    public static class Utilities
    {
        public static string DeriveRoomKey(string keyshare, string passphrase)
        {
            keyshare ??= "";
            passphrase ??= "";

            var input = Encoding.UTF8.GetBytes(keyshare + ":" + passphrase);

            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(input);

            var sb = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        public static string SanitizeCommand(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                throw new ArgumentException("Lege invoer. Typ 'help' voor commando's.");

            var trimmed = System.Text.RegularExpressions.Regex.Replace(raw, "\\s+", " ").Trim();

            foreach (var c in trimmed)
            {
                if (char.IsControl(c) && c != '\n' && c != '\r')
                    throw new ArgumentException("Invoer bevat verboden tekens.");
            }

            return trimmed;
        }

        public static bool TryParseDirection(string token, out Direction dir)
        {
            dir = Direction.North;
            token = token?.ToLower()?.Trim() ?? "";

            return token switch
            {
                "n" or "north" => (dir = Direction.North) == Direction.North,
                "s" or "south" => (dir = Direction.South) == Direction.South,
                "e" or "east" => (dir = Direction.East) == Direction.East,
                "w" or "west" => (dir = Direction.West) == Direction.West,
                _ => false
            };
        }
    }
}
