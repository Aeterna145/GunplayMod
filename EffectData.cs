using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gunplay
{
    public static class EffectData
    {
        public static EffectType UnknownEffect = new EffectType("TerrariaConstruct:UnknownEffect", "Unknown", int.MaxValue, false, "This effect is broken!");

        public static EffectType Beastmaster = new EffectType("TerrariaConstruct:Beastmaster", "Beastmaster", 5, true, "Increases max minions by 1 and summon damage by 5% per level when held.");
    }
}
