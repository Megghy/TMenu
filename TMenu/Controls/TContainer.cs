using TerrariaUI.Base;
using TerrariaUI.Base.Style;

namespace TMenu.Controls
{
    internal class TContainer : TMenuControlBase<VisualContainer>
    {
        public TContainer(string name, int x, int y, int width, int height, UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null)
        : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            Init();
        }
        public TContainer(Data.FileData data) : base(data)
        {
            Init();
        }

        public override TMenuControlBase<VisualContainer> Init()
        {
            TUIObject = new VisualContainer(TempInitInfo.X, TempInitInfo.Y, TempInitInfo.Width, TempInitInfo.Height, TempInitInfo.Configuration, TempInitInfo.Style.StyleEX<ContainerStyle>(), OnClick);
            return this;
        }
    }
}
