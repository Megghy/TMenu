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
    internal class TChest : TMenuControlBase<VisualChest>
    {
        public TChest(Data.FileData data) : base(data)
        {
            Init();
        }

        public TChest(string name, int x, int y, int width, int height, UIConfiguration configuration = null, UIStyle style = null, Data.Click clickCommand = null) : base(name, x, y, width, height, configuration, style, clickCommand)
        {
            Init();
        }

        public override TMenuControlBase<VisualChest> Init()
        {
            throw new NotImplementedException();
        }
    }
}
