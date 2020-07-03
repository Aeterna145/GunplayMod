using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Gunplay.Items.GunParts.Barrels;

namespace Gunplay.Items.GunParts.Barrels
{
    class HandgunBarrel : BarrelPart
    {
        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 14;
            item.rare = ItemRarityID.White;
            item.maxStack = 99;
            item.value = Item.sellPrice(silver: 58);
        }
    }
}
