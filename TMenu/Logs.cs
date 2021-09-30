using System;
using Microsoft.Xna.Framework;

namespace TMenu
{
    public class Logs
    {
        public static readonly string LogPrefix = "[TMenu] ";
        public static void Info(object text)
        {
            TShockAPI.TShock.Log.ConsoleInfo(LogPrefix + text.ToString());
        }
        public static void Error(object text)
        {
            TShockAPI.TShock.Log.ConsoleError(LogPrefix + text.ToString());
        }
        public static void Warn(object text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"LogPrefix + text.ToString()");
            Console.ForegroundColor = ConsoleColor.White;
            TShockAPI.TShock.Log.Warn(LogPrefix + text.ToString());
        }
        public static void Success(object text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(LogPrefix + text.ToString());
            Console.ForegroundColor = ConsoleColor.White;
            TShockAPI.TShock.Log.Warn(LogPrefix + text.ToString());
        }
    }
}
