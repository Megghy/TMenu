using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TerrariaUI.Base;
using TerrariaUI.Widgets;
using TMenu.Controls;
using TShockAPI;

namespace TMenu.Core
{
    internal class Cmd
    {
        public static void OnCommand(CommandArgs args)
        {
            var plr = args.Player;
            var cmd = args.Parameters;
            if (args.Parameters.Any())
                switch (args.Parameters[0].ToLower())
                {
                    case "create":
                    case "open":
                        if (cmd.Count > 1)
                        {
                            Create(plr, cmd[1]);
                        }
                        break;
                    case "close":
                        if (cmd.Count > 1)
                        {
                            Close(plr, cmd[1]);
                        }
                        break;
                    case "info":
                        if (cmd.Count > 1)
                        {
                            Info(plr, cmd[1]);
                        }
                        break;
                    default:
                        HelpText();
                        break;
                }
            else
                HelpText();
            void HelpText()
            {

            }
        }
        private static void Create(TSPlayer plr, string name)
        {
            if (IO.FindMenu(name) is { } menu)
            {
                if (!plr.OpenMenu(menu))
                    plr.SendErrorMessage($"你已开启过了菜单: \"{menu.Name}\"");
            }
            else
                plr.SendErrorMessage($"未找到名为: \"{name}\" 的菜单");
        }
        private static void Close(TSPlayer plr, string name)
        {
            if (!plr.CloseMenu(name))
                plr.SendErrorMessage($"未找到名为: \"{name}\" 的已开启的菜单");
        }
        internal static void Info(TSPlayer plr, string name)
        {
            if (IO.FindMenuLike(name) is { Count: > 0 } list)
            {
                if (list.Count > 1)
                    plr.SendMultipleMatchError(list.Select(m => m.Name));
                else
                    plr.SendMessage(GetInfoString(list.First()), Color.White);
            }
            else
                plr.SendErrorMessage($"未找到名称中包含 {name} 的菜单.");
            string GetInfoString(Data.MenuOriginData mData)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"> {mData.Name.Color("9ECEDB")} <Personal: {mData.Personal}> <");
                sb.AppendLine($"-----------------------------------------");
                AddChild(sb, "-- ", mData);
                return sb.ToString();
            }
            void AddChild(StringBuilder sb, string prefix, Data.MenuOriginData mData)
            {
                sb.AppendLine($"{prefix}| {(Data.ControlName.TryGetValue(mData.Type, out var t) ? t.Name : "Unknown").Color("9EA4DB")} <X: {mData.OriginX}, Y: {mData.OriginY}, Width: {mData.OriginWidth}, Height: {mData.OriginHeight}> {mData.Text}");
                mData.Childs.ForEach(c => AddChild(sb, prefix + "-- ", c));
            }
        }
    }
}
