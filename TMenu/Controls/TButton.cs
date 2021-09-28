using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using TerrariaUI.Base;
using TerrariaUI.Widgets;
using TShockAPI;

namespace TMenu.Controls
{
    public class TButton : TMenuControlBase<Button>
    {
        public TButton(string name, string text, int x, int y, int width, int height, UIConfiguration configuration = null, ButtonStyle style = null)
        : base(name, x, y, width, height, configuration, style)
        {
            TUIObject = new(x, y, width, height, text, configuration, style, OnClick);
            _text = text;
        }
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
