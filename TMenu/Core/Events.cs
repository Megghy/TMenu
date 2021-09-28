using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMenu.Core
{
    public class Events
    {
        public struct Click
        {
            public string[] Command { get; set; }
            public string Goto { get; set; }
            public string Message { get; set; }
        }
    }
}
