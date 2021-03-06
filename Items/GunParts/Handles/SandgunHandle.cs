﻿using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Gunplay.Items.GunParts.Handles;

namespace Gunplay.Items.GunParts.Handles
{
    class SandgunHandle : HandlePart
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 14;
            item.rare = ItemRarityID.White;
            item.maxStack = 99;
            item.value = Item.sellPrice(silver: 7);
        }
    }
}
