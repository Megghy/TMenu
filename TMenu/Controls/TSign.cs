using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TerrariaUI.Widgets;

namespace TMenu.Controls
{
    [NameInJson("sign")]
    public class TSign : TMenuControlBase<VisualSign>
    {
        public TSign(Data.FileData data) : base(data)
        {
            Init();
        }

        public TSign(string name, string x, string y, string width, string height, string text = "", UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null) : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            Data.Text = text;
            Init();
        }
        public string Text
        {
            get => Data.Text;
            set
            {
                Data.Text = value;
                TUIObject?.Set(value);
            }
        }
        public override TMenuControlBase<VisualSign> Init()
        {
            TUIObject = new VisualSign(Data.X, Data.Y, Data.Width, Data.Height, Text, Data.Config, Data.Style, OnClick);
            return this;
        }
    }
}
