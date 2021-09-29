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
    public class TSign : TMenuControlBase<VisualSign>
    {
        public TSign(Data.FileData data) : base(data)
        {
            Text = data.Text;
            Init();
        }

        public TSign(string name, int x, int y, int width, int height, string text = "", UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null) : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            Text = text;
            Init();
        }
        [JsonIgnore]
        private string _text = string.Empty;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                TUIObject?.Set(value);
            }
        }
        public override TMenuControlBase<VisualSign> Init()
        {
            TUIObject = new VisualSign(TempInitInfo.X, TempInitInfo.Y, TempInitInfo.Width, TempInitInfo.Height, Text, TempInitInfo.Configuration, TempInitInfo.Style, OnClick);
            return this;
        }
    }
}
