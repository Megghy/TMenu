using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaUI.Base;
using TerrariaUI.Base.Style;

namespace TMenu.Controls
{
    [NameInJson("closebutton")]
    public class TCloseButton 
    {
    }
    public class CloseButton : VisualObject
    {
        public CloseButton(CloseButton visualObject) : base(visualObject)
        {
        }

        public CloseButton(int x, int y, int width, int height, UIConfiguration configuration = null, UIStyle style = null, Action<VisualObject, Touch> callback = null) : base(x, y, width, height, configuration, style, callback)
        {
        }
    }
}
