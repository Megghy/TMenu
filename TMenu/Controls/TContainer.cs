using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TerrariaUI.Widgets;

namespace TMenu.Controls
{
    internal class TContainer : TMenuControlBase<VisualContainer>
    {
        public TContainer(string name, int x, int y, int width, int height, UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null)
        : base(name, x, y, width, height, configuration, style, clickCommand) {
            var s = new ContainerStyle();
            s.Stratify(style);
            TUIObject = new VisualContainer(x, y, width, height, configuration, s, OnClick);
        }
        public TContainer(Data.FileData data) : this(data.Name, data.X, data.Y, data.Width, data.Height, data.Config, data.Style.StyleEX<ContainerStyle>(), data.ClickCommand) { Data = data; }

    }
}
