using System;
using System.Collections.Generic;
using FakeProvider;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TerrariaUI.Widgets;
using TShockAPI;

namespace TMenu.Controls
{
    internal class TPanel : TMenuControlBase<Panel>
    {
        public TPanel(string name, int x, int y, int width, int height, UIConfiguration configuration = null, UIStyle style = null)
            : base(name, x, y, width, height, configuration, style)
        {
            var s = new PanelStyle();
            s.Stratify(style);
            FakePanel = FakeProviderAPI.CreatePersonalTileProvider(ID.ToString(), new(), TempInitInfo.X, TempInitInfo.Y, TempInitInfo.Width, TempInitInfo.Height);
            //FakePanel = FakeProviderAPI.CreateTileProvider(ID.ToString(), TempInitInfo.X, TempInitInfo.Y, TempInitInfo.Width, TempInitInfo.Height);
            TUIObject = new Panel(ID.ToString(), TempInitInfo.X, TempInitInfo.Y, TempInitInfo.Width, TempInitInfo.Height, TempInitInfo.Configuration, s, FakePanel, new());
        }
        public TileProvider FakePanel { get; set; }
        public Guid ID { get; set; } = Guid.NewGuid();
        public void Show(TSPlayer plr)
        {
            AddUser(plr);
            TerrariaUI.TUI.Create(TUIObject);
        }
        public void AddUser(TSPlayer plr)
        {
            FakePanel.Observers.Add(plr.Index);
            TUIObject.Observers.Add(plr.Index);
        }
    }
}
