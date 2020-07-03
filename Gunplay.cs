using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Gunplay
{
    public partial class Gunplay : Mod
    {
        public int SpreadAngle = 0;
        public int FireRate = 0;

        internal bool cheatUIOpen = false;

        internal static Gunplay Instance;
        internal UserInterface DismantleUserInterface;
        internal UserInterface CheatDismantleUserInterface;
        internal Mod herosMod;
        internal Mod cheatSheet;

        public override void Load()
        {
            Instance = this;

            herosMod = ModLoader.GetMod("HEROsMod");
            cheatSheet = ModLoader.GetMod("CheatSheet");

            if (!Main.dedServ)
            {
                DismantleUserInterface = new UserInterface();
                CheatDismantleUserInterface = new UserInterface();
            }
        }

        public override void Unload()
        {
            Instance = null;

            base.Unload();
        }

        public override void PostSetupContent()
        {
            try
            {
                if (herosMod != null)
                {
                    SetupModIntegration(herosMod);
                }
                else if (cheatSheet != null)
                {
                    SetupModIntegration(cheatSheet);
                }
                else
                {

                }
            }
            catch (Exception e)
            {
                Logger.Warn("Gunplay: PostSetupContent Error: " + e.StackTrace + e.Message);
            }

            cheatUIOpen = false;

            base.PostSetupContent();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            DismantleUserInterface?.Update(gameTime);
            CheatDismantleUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (inventoryIndex != -1)
            {
                layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
                    "Gunplay: Dismantle UI",
                    delegate
                    {
                        DismantleUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
                    "Gunplay: Cheat Dismantle UI",
                    delegate
                    {
                        CheatDismantleUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public void SetupModIntegration(Mod mod)
        {
            if (mod == herosMod)
            {
                herosMod.Call("AddPermission", "CheatDismantleUI", "No-Cost Dismantling UI");

                if (!Main.dedServ)
                {
                    herosMod.Call("AddSimpleButton", "CheatDismantleUI", GetTexture("UI/CheatDismantleUIButton"), (Action)CheatDismantleUIButtonActivated, (Action<bool>)PermissionChanged, (Func<string>)CheatUITooltip);
                }
            }
            else if (mod == cheatSheet)
            {

            }
        }

        public string CheatUITooltip()
        {
            return cheatUIOpen ? "Open No-Cost Dismantling UI" : "Close No-Cost Dismantling UI";
        }

        public void CheatDismantleUIButtonActivated()
        {
            cheatUIOpen = !cheatUIOpen;

            if (!cheatUIOpen)
            {
                Main.playerInventory = true;
                Main.npcChatText = ""; // Closes chat.
                Main.PlaySound(SoundID.MenuTick);
                ModContent.GetInstance<Gunplay>().CheatDismantleUserInterface.SetState(new UI.CheatDismantleUI());
            }
            if (cheatUIOpen)
            {
                Main.PlaySound(SoundID.MenuTick);
                ModContent.GetInstance<Gunplay>().CheatDismantleUserInterface.SetState(null);
            }
        }

        public void PermissionChanged(bool hasPermission)
        {
            if (!hasPermission)
                cheatUIOpen = false;
        }
    }
}