using System.Collections.Generic;
using Terraria.ID;

namespace Gunplay.UI
{
    class ValidGunList
    {
        public static List<int> ValidGuns()
        {
            List<int> guns = new List<int>();
            guns.Add(ItemID.Sandgun);
            guns.Add(ItemID.Shotgun);
            guns.Add(ItemID.Handgun);
            guns.Add(ItemID.SniperRifle);
            guns.Add(ItemID.TacticalShotgun);
            guns.Add(ItemID.Uzi);
            return guns;
        }
    }
}
