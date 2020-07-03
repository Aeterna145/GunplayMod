using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Gunplay
{
    public class GunplayPlayer : Terraria.ModLoader.ModPlayer
    {
        public override void OnEnterWorld(Player player)
        {
            ModContent.GetInstance<Gunplay>().CheatDismantleUserInterface.SetState(null);
            Gunplay.Instance.cheatUIOpen = true;

            base.OnEnterWorld(player);
        }
    }
}
