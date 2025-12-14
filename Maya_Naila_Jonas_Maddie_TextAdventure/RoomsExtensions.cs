using System;
using System.Reflection;

namespace Maya_Naila_Jonas_Maddie_TextAdventure
{
    public partial class Rooms
    {
        public string AdminNoclipMove(Direction dir)
        {
            try
            {
                var moveMethod = this.GetType().GetMethod("Move", new Type[] { typeof(Direction) });

                if (moveMethod != null)
                {
                    try
                    {
                        var result = moveMethod.Invoke(this, new object[] { dir });
                        return result?.ToString() ?? "Beweging uitgevoerd.";
                    }
                    catch
                    {
                        
                    }
                }

                var fiIndex = this.GetType().GetField("CurrentRoomIndex",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                var fiRooms = this.GetType().GetField("_allRooms",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                if (fiIndex == null || fiRooms == null)
                    return "Noclip niet mogelijk (interne structuur onbekend).";

                int idx = (int)fiIndex.GetValue(this);
                var list = fiRooms.GetValue(this) as System.Collections.IList;

                if (list == null)
                    return "Noclip mislukt (rooms lijst niet gevonden).";

                int newIdx = dir switch
                {
                    Direction.North => Math.Max(0, idx - 1),
                    Direction.South => Math.Min(list.Count - 1, idx + 1),
                    Direction.East => Math.Min(list.Count - 1, idx + 1),
                    Direction.West => Math.Max(0, idx - 1),
                    _ => idx
                };

                fiIndex.SetValue(this, newIdx);

                var curRoomProp = this.GetType().GetProperty("CurrentRoom");
                if (curRoomProp != null)
                {
                    var cur = curRoomProp.GetValue(this);
                    return cur?.ToString() ?? "Noclip uitgevoerd.";
                }

                return "Noclip uitgevoerd (geen extra info).";
            }
            catch (Exception ex)
            {
                return "Fout bij noclip: " + ex.Message;
            }
        }

        public string TryDeriveRoomKeyFromKeyshare(string keyshare, string passphrase)
        {
            return Utilities.DeriveRoomKey(keyshare, passphrase);
        }
    }
}
