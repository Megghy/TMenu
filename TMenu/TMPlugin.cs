using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;

namespace TMenu
{
    [ApiVersion(2, 1)]
    public class TMPlugin : TerrariaPlugin
    {
        public static TMPlugin Instance;
        public TMPlugin(Main game) : base(game) { Instance = this; }
        public override void Initialize()
        {
            
        }
    }
}
