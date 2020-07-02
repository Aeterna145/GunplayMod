using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Gunplay
{
    public class Gunplay : Mod
    {
        public int SpreadAngle = 0;
        public int FireRate = 0;
        internal UserInterface DismantleUserInterface;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                DismantleUserInterface = new UserInterface();
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            DismantleUserInterface?.Update(gameTime);
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
            }
        }
    }
}