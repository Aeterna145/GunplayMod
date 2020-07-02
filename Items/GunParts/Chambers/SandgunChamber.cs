using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Gunplay.Items.GunParts.Chambers;

namespace Gunplay.Items.GunParts.Chambers
{
    class SandgunChamber : ChamberPart
    {
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 8;
            item.rare = ItemRarityID.White;
            item.maxStack = 99;
            item.value = Item.sellPrice(silver: 7);
        }
    }
}
