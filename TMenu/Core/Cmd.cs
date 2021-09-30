using System.Linq;
using System.Text;
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
            if (Files.FindMenu(name) is { } menu)
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
            if (Files.FindMenuLike(name) is { Count: > 0 } list)
            {
                if (list.Count > 1)
                    plr.SendMultipleMatchError(list.Select(m => m.Name));
                else
                    plr.SendInfoMessage(GetInfoString(list.First()));
            }
            else
                plr.SendErrorMessage($"未找到名称中包含 {name} 的菜单.");
            string GetInfoString(TPanel menu)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"> {menu.Name.Color("9ECEDB")} {{{menu.ID}}} <");
                sb.AppendLine($"-----------------------------------------");
                AddChild(sb, "-- ", menu.TUIObject);
                return sb.ToString();
            }
            void AddChild(StringBuilder sb, string prefix, VisualObject obj)
            {
                sb.AppendLine($"{prefix}| {obj.GetType().Name.Color("9EA4DB")} <{obj.X}, {obj.Y}> {(obj.GetType().IsSubclassOf(typeof(Label)) ? obj.GetType().GetProperty("RawText").GetValue(obj) : "")}");
                obj.Child.ForEach(c => AddChild(sb, prefix + "-- ", c));
            }
        }
    }
}
