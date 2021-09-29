using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using TerrariaUI.Base;
using TerrariaUI.Widgets;
using TShockAPI;

namespace TMenu.Controls
{
    public class TButton : TMenuControlBase<Button>
    {
        public TButton(string name, string text, int x, int y, int width, int height, UIConfiguration configuration = null, ButtonStyle style = null, Data.Click clickCommand = null)
        : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            TUIObject = new(x, y, width, height, text, configuration, style, OnClick);
            _text = text;
        }
        public TButton(Data.FileData data) : this(data.Name, data.Text, data.X, data.Y, data.Width, data.Height, data.Configuration, data.Style.StyleEX<ButtonStyle>(), data.ClickCommand) { Data = data; }
        [JsonIgnore]
        private string _text = string.Empty;
        public string Text
        {
            get => _text; 
            set
            {
                _text = value;
                TUIObject.UpdateText(value);
            }
        }
    }
}
