using System;
using System.Collections.Generic;
using System.Linq;
using FakeProvider;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using TerrariaUI;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TerrariaUI.Widgets;
using TShockAPI;

namespace TMenu.Controls
{
    public class TPanel : TMenuControlBase<Panel>
    {
        public TPanel(string name, int x, int y, int width, int height, UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null)
            : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            Init();
        }
        public TPanel(Data.FileData data) : base(data)
        {
            Init();
        }
        public override TMenuControlBase<Panel> Init()
        {
            FakePanel = FakeProviderAPI.CreatePersonalTileProvider(ID.ToString(), new() { }, TempInitInfo.X, TempInitInfo.Y, TempInitInfo.Width, TempInitInfo.Height);
            //FakePanel = FakeProviderAPI.CreateTileProvider(ID.ToString(), TempInitInfo.X, TempInitInfo.Y, TempInitInfo.Width, TempInitInfo.Height);
            TUIObject = new Panel(ID.ToString(), TempInitInfo.X, TempInitInfo.Y, TempInitInfo.Width, TempInitInfo.Height, TempInitInfo.Configuration, TempInitInfo.Style.StyleEX<PanelStyle>(), FakePanel, new());
            return this;
        }
        public TileProvider FakePanel { get; set; }
        public Guid ID { get; set; } = Guid.NewGuid();
        public void Show(TSPlayer plr)
        {
            AddUser(plr);
            TUI.Create(TUIObject);
        }
        public void AddUser(TSPlayer plr)
        {
            FakePanel.Observers.Add(plr.Index);
            TUIObject.Observers.Add(plr.Index);
        }
        public void Dispose()
        {
            if (TUI.Roots.Any(r => r.Name == ID.ToString()))
                TUI.Destroy(TUIObject);
            FakePanel?.Dispose();
        }
    }
}
