using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace Gunplay.Items.GunParts
{
    class SandgunChamber : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 8;
            item.rare = ItemRarityID.White;
            item.maxStack = 99;
            item.value = Item.sellPrice(silver: 75);
        }
    }
}
