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
#if DEBUG
            ServerApi.Hooks.NetGreetPlayer.Register(this, JOIN);
#endif
        }
#if DEBUG
        void JOIN(GreetPlayerEventArgs args)
        {
            var plr = TShockAPI.TShock.Players[args.Who];
            /*
            var p = new Controls.TPanel("aa", plr.TileX, plr.TileY, 50, 50, null, new() { Wall = WallID.AmberGemspark });
            var c = p.AddChild(new Controls.TContainer("ee", 0, 0, 10, 10, null, new() { Wall = WallID.WhiteDynasty }));
            var b = c.AddChild(new Controls.TButton("bb", "woc", 0, 0, 10, 10));
            //p.TUIObject.UpdateSelf();
            p.Show(plr);*/
            var panel = Core.FileManager.DeserilizeInner("{\"menu\":{\"type\":\"panel\",\"x\":\"2100\",\"y\":250,\"width\":50,\"height\":50,\"name\":\"mainmenu\",\"child\":[{\"type\":\"button\",\"text\":\"text\",\"x\":\"0\",\"y\":0,\"width\":20,\"height\":20,\"name\":\"button\",\"style\":{\"wall\":150},\"click\":{\"message\":\"test\"}}]}}");
            panel.Show(plr);
        }
#endif
    }
}
