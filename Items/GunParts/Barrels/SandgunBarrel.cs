using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Gunplay.Items.GunParts.Barrels;

namespace Gunplay.Items.GunParts.Barrels
{
    class SandgunBarrel : BarrelPart
    {
        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 14;
            item.rare = ItemRarityID.White;
            item.maxStack = 99;
            item.value = Item.sellPrice(silver: 7);
        }
    }
}
