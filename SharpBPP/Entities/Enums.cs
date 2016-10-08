using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBPP.Entities
{
    [Flags]
    public enum MapDrawingState
    {
        Default = 0,
        RouteChooseA = 1,
        RouteChooseB = 2
    }
}
