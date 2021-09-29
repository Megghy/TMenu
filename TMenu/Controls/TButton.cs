using Newtonsoft.Json;
using TerrariaUI.Base;
using TerrariaUI.Widgets;

namespace TMenu.Controls
{
    public class TButton : TMenuControlBase<Button>
    {
        public TButton(string name, int x, int y, int width, int height, string text = "", UIConfiguration configuration = null, ButtonStyle style = null, Data.Click clickCommand = null)
        : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            Text = text;
            Init();
        }
        public TButton(Data.FileData data) : base(data)
        {
            Text = data.Text;
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
                TUIObject?.UpdateText(value);
            }
        }
        public override TMenuControlBase<Button> Init()
        {
            TUIObject = new Button(TempInitInfo.X, TempInitInfo.Y, TempInitInfo.Width, TempInitInfo.Height, Text, TempInitInfo.Configuration, TempInitInfo.Style.StyleEX<ButtonStyle>(), OnClick);
            return this;
        }
    }
}
