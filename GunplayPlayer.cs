using Gunplay.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
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

		public static readonly PlayerLayer ConstructTool = new PlayerLayer("TerrariaConstruct", "ConstructTool", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.HeldItem.type == ModContent.ItemType<ConstructTool>() && drawPlayer.itemAnimation != 0)
			{
				List<Texture2D> textures = new List<Texture2D>();
				List<Vector2> offsets = new List<Vector2>();
				List<string> partKeys = (drawPlayer.HeldItem.modItem as ConstructTool).parts;
				foreach (string key in partKeys)
				{
					try
					{
						PartType partType = GHelper.GetPartType(key);
						Texture2D texture = ModContent.GetTexture(partType.useTexture);
						textures.Add(texture);
						Vector2 offset = partType.offset;
						offsets.Add(offset);
					}
					catch (Exception e)
					{
						Gunplay.Instance.Logger.Error("Something failed when getting useTexture or offset in the PlayerLayer! " + e.Message);
					}
				}
				Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16);
				for (int index = 0; index < textures.Count; index++)
				{
					Texture2D texture = textures[index];
					Vector2 offset = offsets[index];
					DrawData data = new DrawData(
						texture,
						drawInfo.itemLocation - (offset.RotatedBy(drawPlayer.itemRotation) / 2f) - Main.screenPosition,
						texture.Frame(),
						drawPlayer.HeldItem.GetAlpha(color),
						drawPlayer.itemRotation,
						new Vector2(drawPlayer.direction == 1 ? 0 : texture.Width, texture.Height),
						drawPlayer.HeldItem.scale,
						drawInfo.spriteEffects,
						0);
					Main.playerDrawData.Add(data);
				}
			}
		});

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			ConstructTool.visible = true;
			layers.Add(ConstructTool);
		}
	}
}
