using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Mono.Cecil.Cil.OpCodes;
using static Terraria.ModLoader.ModContent;

namespace Gunplay
{
    class GunplayNPC : GlobalNPC
    {
        public override bool Autoload(ref string name)
        {
            IL.Terraria.Main.GUIChatDrawInner += HookAdjustButton;
            return base.Autoload(ref name);
        }

        private void HookAdjustButton(ILContext il)
        {
            var c = new ILCursor(il).Goto(0);
            if (!c.TryGotoNext(i => i.MatchLdcI4(NPCID.ArmsDealer))) throw new Exception("Can't patch Arms Dealer shop button");
            if (!c.TryGotoNext(i => i.MatchLdcI4(NPCID.ArmsDealer))) throw new Exception("Can't patch Arms Dealer shop button");
            c.Index += 2;
            c.EmitDelegate<Func<string>>(() => "Gunplay Mod Dismantle Gun Function");
            c.Emit(Stloc_S, (byte)10);
        }

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            switch (npc.type)
            {
                case NPCID.ArmsDealer:
                    if (!firstButton)
                    {
                        Main.playerInventory = true;
                        Main.npcChatText = ""; // Closes chat.
                        Main.PlaySound(SoundID.MenuTick);
                        GetInstance<Gunplay>().DismantleUserInterface.SetState(new UI.DismantleUI());
                    }
                    break;
            }
        }
    }
}
