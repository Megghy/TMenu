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
    [NameInJson("arrow")]
    public class TArrow : TMenuControlBase<Arrow>
    {
        public TArrow(Data.MenuOriginData data) : base(data)
        {
            Init();
        }

        public TArrow(string name, Direction direction, string x, string y, string width, string height, UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null) : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            Init();
        }
        public Direction Direction
        {
            get => Data.Direction;
            set
            {
                Data.Direction = value;
                ((ArrowStyle)TUIObject.Style).Direction = value;
                TUIObject.UpdateSelf();
            }
        }
        public override TMenuControlBase<Arrow> Init()
        {
            var style = Data.Style.StyleEX<ArrowStyle>();
            style.Direction = Data.Direction;
            TUIObject = new Arrow(Data.X, Data.Y, style, OnClick);
            return this;
        }
    }
}
