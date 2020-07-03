using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Gunplay.Items.GunParts.Attachments
{
    // Universal attachment part code goes here.
    public abstract class AttachmentPart : GunPart
    {
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(mod, "Gunplay:Attachment", "Attachment"));

            foreach (TooltipLine line in tooltips)
            {
                if (line.mod == "Gunplay" && line.Name == "Gunplay:Attachment")
                {
                    line.overrideColor = Color.Gray;
                }
            }

            base.ModifyTooltips(tooltips);
        }
    }
}
