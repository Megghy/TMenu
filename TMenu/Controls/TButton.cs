using TerrariaUI.Base;
using TerrariaUI.Widgets;

namespace TMenu.Controls
{
    [NameInJson("button")]
    public class TButton : TMenuControlBase<Button>
    {
        public TButton(string name, string x, string y, string width, string height, string text = "", UIConfiguration configuration = null, ButtonStyle style = null, Data.Click clickCommand = null)
        : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            Data.Text = text;
            Init();
        }
        public TButton(Data.FileData data) : base(data)
        {
            Init();
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
        public override TMenuControlBase<Button> Init()
        {
            TUIObject = new Button(Data.X, Data.Y, Data.Width, Data.Height, Text, Data.Config, Data.Style.StyleEX<ButtonStyle>(), OnClick);
            return this;
        }
    }
}
