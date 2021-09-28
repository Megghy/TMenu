using TerrariaUI.Base;
using TerrariaUI.Widgets;

namespace TMenu.Controls
{
    public class TButton : TMenuControlBase<Button>
    {
        public TButton(string name, string text, int x, int y, int width, int height, UIConfiguration configuration = null, ButtonStyle style = null)
        : base(name, x, y, width, height, configuration, style)
        {
            Text = text;
        }
        public override void Init()
        {
            TUIObject.UpdateText(Text);
        }
        public string Text { get; set; }
    }
}
