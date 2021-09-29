using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;
using TerrariaUI.Widgets;
using static System.Net.Mime.MediaTypeNames;

namespace TMenu.Controls
{
    public class TArror : TMenuControlBase<Arrow>
    {
        public TArror(Data.FileData data) : base(data)
        {
            Init();
        }

        public TArror(string name, Direction direction, int x, int y, int width, int height, UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null) : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            Init();
        }
        public Direction Direction { get; set; } = Direction.Up;
        public override TMenuControlBase<Arrow> Init()
        {
            var style = TempInitInfo.Style.StyleEX<ArrowStyle>();
            style.Direction = Data.Direction;
            TUIObject = new Arrow(TempInitInfo.X, TempInitInfo.Y, style, OnClick);
            return this;
        }
    }
}
