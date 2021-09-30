using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TerrariaUI.Widgets;
using TerrariaUI.Widgets.Data;

namespace TMenu.Controls
{
    [NameInJson("itemrack")]
    internal class TItemRack : TMenuControlBase<ItemRack>
    {
        public TItemRack(Data.FileData data) : base(data)
        {
            Init();
        }

        public TItemRack(string name, string x, string y, ItemData item, string text, UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null) : base(name, x, y, "-1", "-1", configuration, style, clickCommand)
        {
            Data.Text = text;
            Data.Item = item;
            Init();
        }
        public ItemData Item {
            get => Data.Item;
            set
            {
                Data.Item = value;
                if(TUIObject is not null)
                {
                    ((ItemRackStyle)TUIObject.Style).Type = (short)value.NetID;
                    TUIObject.UpdateSelf();
                }
            }
        }
        public string Text
        {
            get => Data.Text;
            set
            {
                Data.Text = value;
                TUIObject?.SetText(value);
            }
        }
        public override TMenuControlBase<ItemRack> Init()
        {
            var s = Data.Style.StyleEX<ItemRackStyle>();
            s.Type = (short)Item.NetID;
            TUIObject = new(Data.X, Data.X, s, OnClick);
            TUIObject.DrawWithSection = true;
            TUIObject.FrameSection = true;
            if (!string.IsNullOrEmpty(Text))
                TUIObject.SetText(Text);
            return this;
        }
    }
}
