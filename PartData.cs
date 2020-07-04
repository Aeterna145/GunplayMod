using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gunplay
{
    public static class PartData
    {
        public static PartType UnknownPart = new PartType("TerrariaConstruct:UnknownPart", "Unknown", "Part", 100f, 0, Vector2.Zero, "Gunplay/Items/GunParts/Barrels/HandgunBarrel", null, null);

        public static PartType CopperPickaxeHead = new PartType("TerrariaConstruct:CopperPickaxeHead", "Copper", "Pickaxe Head", 3f, 35, Vector2.Zero, "Gunplay/Items/GunParts/Barrels/PastKillerBarrel", null, null);
    }
}
