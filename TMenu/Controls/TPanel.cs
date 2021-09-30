using System;
using FakeProvider;
using TerrariaUI;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TerrariaUI.Widgets;
using TShockAPI;

namespace TMenu.Controls
{
    [Serializable]
    [NameInJson("panel")]
    public class TPanel : TMenuControlBase<Panel>, ICloneable
    {
        public TPanel(string name, string x, string y, string width, string height, UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null)
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
            FakePanel = Personal ? FakeProviderAPI.CreatePersonalTileProvider(ID.ToString(), new() { }, Data.X, Data.Y, Data.Width, Data.Height) : FakeProviderAPI.CreateTileProvider(ID.ToString(), Data.X, Data.Y, Data.Width, Data.Height);
            TUIObject = new Panel(ID.ToString(), Data.X, Data.Y, Data.Width, Data.Height, Data.Config, Data.Style.StyleEX<PanelStyle>(), FakePanel, Personal ? new() : null);
            return this;
        }
        public TileProvider FakePanel { get; set; }
        public Guid ID { get; set; } = Guid.NewGuid();
        public bool Moveable => Data.Moveable;
        public bool Personal => Data.Personal;
        public bool IsFromFile => !string.IsNullOrEmpty(Data.Path);
        public TSPlayer User { get; set; }

        public void AddUser(TSPlayer plr)
        {
            if (!Personal)
                throw new("This is a public menu, no users can be added to it.");
            FakePanel.Observers.Add(plr.Index);
            TUIObject.Observers.Add(plr.Index);
            User = plr;
        }
        public void Dispose()
        {
            TUI.Destroy(TUIObject);
            FakePanel.Dispose();
        }

        public object Clone(TSPlayer plr)
        {
            return Core.Parser.Deserilize(Data.Path, plr);
        }
        public object Clone()
        {
            return IsFromFile ? Core.Parser.Deserilize(Data.Path) : Utils.CreateClone(this);
        }
    }
}
