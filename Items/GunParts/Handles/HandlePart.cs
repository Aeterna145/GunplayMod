using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Gunplay.Items.GunParts.Handles
{
    // Universal handle part code goes here.
    public abstract class HandlePart : GunPart
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(mod, "Gunplay:Handle", "Handle"));

            foreach (TooltipLine line in tooltips)
            {
                if (line.mod == "Gunplay" && line.Name == "Gunplay:Handle")
                {
                    line.overrideColor = Color.Gray;
                }
            }

            base.ModifyTooltips(tooltips);
        }
    }
}
