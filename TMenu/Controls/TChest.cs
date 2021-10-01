using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TerrariaUI.Widgets;
using TerrariaUI.Widgets.Data;
using static System.Net.Mime.MediaTypeNames;

namespace TMenu.Controls
{
    [NameInJson("chest")]
    internal class TChest : TMenuControlBase<VisualChest>
    {
        public TChest(Data.MenuOriginData data) : base(data)
        {
            Init();
        }

        public TChest(string name, string x, string y, string width, string height, UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null) : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            Init();
        }
        public ItemData[] Items
        {
            get => Data.Items;
            set
            {
                Data.Items = value;
                if (TUIObject is not null)
                {
                    TUIObject.Set(value);
                    TUIObject.UpdateSelf();
                }
            }
        }
        public override TMenuControlBase<VisualChest> Init()
        {
            if (Data.Items?.Count() < 40)
            {
                var list = new ItemData[40];
                Data.Items.CopyTo(list, 0);
                Data.Items = list;
            }
            TUIObject = new(Data.X, Data.Y, Data.Items, Data.Config, Data.Style, OnClick);
            TUIObject.DrawWithSection = true;
            TUIObject.FrameSection = true;
            return this;
        }
    }
}
