using Gunplay.Items.GunParts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Gunplay
{
    class GunplayItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.Sandgun:
                    tooltips.Add(new TooltipLine(mod, "Gunplay:Dismantle", "Can be dismantled"));
                    break;
                case ItemID.Shotgun:
                    tooltips.Add(new TooltipLine(mod, "Gunplay:Dismantle", "Can be dismantled"));
                    break;
                case ItemID.Handgun:
                    tooltips.Add(new TooltipLine(mod, "Gunplay:Dismantle", "Can be dismantled"));
                    break;
                case ItemID.SniperRifle:
                    tooltips.Add(new TooltipLine(mod, "Gunplay:Dismantle", "Can be dismantled"));
                    break;
                case ItemID.TacticalShotgun:
                    tooltips.Add(new TooltipLine(mod, "Gunplay:Dismantle", "Can be dismantled"));
                    break;
                case ItemID.Uzi:
                    tooltips.Add(new TooltipLine(mod, "Gunplay:Dismantle", "Can be dismantled"));
                    break;
            }
            
            foreach (TooltipLine line in tooltips)
            {
                if (line.mod == "Gunplay" && line.Name == "Gunplay:Dismantle")
                {
                    line.overrideColor = Color.Gray;
                }
            }

            base.ModifyTooltips(item, tooltips);
        }
    }
}
