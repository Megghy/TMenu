using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using TerrariaApi.Server;

namespace TMenu
{
    [ApiVersion(2, 1)]
    public class TMPlugin : TerrariaPlugin
    {
        public static TMPlugin Instance;
        public TMPlugin(Main game) : base(game) { Instance = this; }
        public override void Initialize()
        {
            ServerApi.Hooks.GamePostInitialize.Register(this, OnPostInit);
#if DEBUG
            ServerApi.Hooks.NetGreetPlayer.Register(this, JOIN);
            ServerApi.Hooks.ServerLeave.Register(this, leave);
#endif
        }
        private void OnPostInit(EventArgs args)
        {
            Core.Files.Init();
        }
#if DEBUG
        Controls.TPanel panel;
        void leave(LeaveEventArgs args)
        {
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
            panel = Data.Menus.First();
            panel.Show(plr);
        }
#endif
    }
}
