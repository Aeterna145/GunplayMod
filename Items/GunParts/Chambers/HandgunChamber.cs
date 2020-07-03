using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Gunplay.Items.GunParts.Chambers;

namespace Gunplay.Items.GunParts.Chambers
{
    class HandgunChamber : ChamberPart
    {
        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 16;
            item.rare = ItemRarityID.White;
            item.maxStack = 99;
            item.value = Item.sellPrice(silver: 58);
        }
    }
}
