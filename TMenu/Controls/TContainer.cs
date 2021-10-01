using TerrariaUI.Base;
using TerrariaUI.Base.Style;

namespace TMenu.Controls
{
    [NameInJson("container")]
    internal class TContainer : TMenuControlBase<VisualContainer>
    {
        public TContainer(string name, string x, string y, string width, string height, UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null)
        : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            Init();
        }
        public TContainer(Data.MenuOriginData data) : base(data)
        {
            Init();
        }
        public override TMenuControlBase<VisualContainer> Init()
        {
            TUIObject = new VisualContainer(Data.X, Data.Y, Data.Width, Data.Height, Data.Config, Data.Style.StyleEX<ContainerStyle>(), OnClick);
            return this;
        }
    }
}
