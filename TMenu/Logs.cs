using System;
using Microsoft.Xna.Framework;

namespace TMenu
{
    public class Logs
    {
        public static void Info(object text)
        {
            TShockAPI.TShock.Log.Info($"<{TMPlugin.Instance.Name}> " + text.ToString());
            TShockAPI.TSPlayer.Server.SendInfoMessage($"<[C/E4C6EF:{TMPlugin.Instance.Name}]> " + text.ToString());
        }
        public static void Error(object text)
        {
            TShockAPI.TShock.Log.Error($"<{TMPlugin.Instance.Name}> " + text.ToString());
            TShockAPI.TSPlayer.Server.SendErrorMessage($"<[C/E4C6EF:{TMPlugin.Instance.Name}]> " + text.ToString());
        }
        public static void Warn(object text)
        {
            TShockAPI.TSPlayer.Server.SendWarningMessage($"<{TMPlugin.Instance.Name}> " + text.ToString());
            TShockAPI.TShock.Log.Warn($"<[C/E4C6EF:{TMPlugin.Instance.Name}]> " + text.ToString());
        }
        public static void Success(object text)
        {
            TShockAPI.TSPlayer.Server.SendSuccessMessage($"<{TMPlugin.Instance.Name}> " + text.ToString());
            TShockAPI.TShock.Log.Warn($"<[C/E4C6EF:{TMPlugin.Instance.Name}]> " + text.ToString());
        }
    }
}
