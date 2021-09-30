using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TMenu.Controls;
using TShockAPI;

namespace TMenu
{
    [ApiVersion(2, 1)]
    public class TMPlugin : TerrariaPlugin
    {
        public static TMPlugin Instance;
        public TMPlugin(Main game) : base(game) { Instance = this; }
        public override string Name => "TMenu";
        public override string Description => "一个基于TUI的高度可定制化的菜单插件";
        public override string Author => "Megghy";
        public override Version Version => Environment.Version;
        public override void Initialize()
        {
            Data.Init();

            ServerApi.Hooks.GamePostInitialize.Register(this, OnPostInit);
            ServerApi.Hooks.ServerJoin.Register(this, OnPlayerJoin);
            TShockAPI.Hooks.GeneralHooks.ReloadEvent += reload => Core.Files.Load();

            Commands.ChatCommands.Add(new("tmenu.use", Core.Cmd.OnCommand, new[] { "tmenu", "tm", "菜单" }));
#if DEBUG
            ServerApi.Hooks.NetGreetPlayer.Register(this, JOIN);
            ServerApi.Hooks.ServerLeave.Register(this, leave);
#endif
        }
        private void OnPostInit(EventArgs args)
        {
            Core.Files.Load();
        }
        private void OnPlayerJoin(JoinEventArgs args)
        {
            if (TShock.Players[args.Who] is { } plr)
            {
                plr.SetData("TMenu.Using", new List<TPanel>());
            }
        }
#if DEBUG
        TPanel panel;
        void leave(LeaveEventArgs args)
        {
            var plr = TShock.Players[args.Who];
            plr.CloseAllMenu();
            //panel.Dispose();
        }
        void JOIN(GreetPlayerEventArgs args)
        {
            var plr = TShockAPI.TShock.Players[args.Who];
            /*
            var p = new Controls.TPanel("aa", plr.TileX, plr.TileY, 50, 50, null, new() { Wall = WallID.AmberGemspark });
            var c = p.AddChild(new Controls.TContainer("ee", 0, 0, 10, 10, null, new() { Wall = WallID.WhiteDynasty }));
            var b = c.AddChild(new Controls.TButton("bb", "woc", 0, 0, 10, 10));
            //p.TUIObject.UpdateSelf();
            p.Show(plr);*/
            Task.Run(() =>
            {
                plr.OpenMenu(Data.Menus.First());
                Core.Cmd.Info(plr, Data.Menus.First().Name);
            });
        }
#endif
    }
}
