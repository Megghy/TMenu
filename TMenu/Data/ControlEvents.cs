using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace TMenu.Data
{
    public class ControlEvents
    {
        public struct TButtonClickEventArgs
        {
            public TSPlayer Player { get; private set; }
        }
    }
}
