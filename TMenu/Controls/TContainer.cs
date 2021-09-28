using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUI.Base;
using TerrariaUI.Widgets;

namespace TMenu.Controls
{
    internal class TContainer : TMenuControlBase<VisualContainer>
    {
        public TContainer(string name, int x, int y, int width, int height, UIConfiguration configuration = null, ButtonStyle style = null)
        : base(name, x, y, width, height, configuration, style) { }

        public List<VisualObject> Childs => TUIObject.Child;
    }
}
