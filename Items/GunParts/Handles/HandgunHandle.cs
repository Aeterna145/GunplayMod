using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Gunplay.Items.GunParts.Handles;

namespace Gunplay.Items.GunParts.Handles
{
    class HandgunHandle : HandlePart
    {
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.rare = ItemRarityID.White;
            item.maxStack = 99;
            item.value = Item.sellPrice(silver: 58);
        }
    }
}
