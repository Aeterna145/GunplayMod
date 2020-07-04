using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Gunplay.Items
{
    public class ConstructPart : ModItem
    {
        public override string Texture => "Terraria/Item_1";

		public string partKey;

		public ConstructPart()
		{
			partKey = "";
		}

		public override void SetDefaults()
		{
			item.height = 32;
			item.width = 32;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.Name == "ItemName" && line.mod == "Terraria")
				{
					line.text = GHelper.GetPartName(partKey);
				}
			}
			string effectsText = "";
			Dictionary<string, int> effects = GHelper.GetPartType(partKey).effects;
			foreach (KeyValuePair<string, int> pair in effects)
			{
				EffectType effectType = GHelper.GetEffectType(pair.Key);
				if (effectsText != "")
				{
					effectsText += "\n";
				}
				effectsText += $"{effectType.name}";
				if (effectType.showLevel)
				{
					effectsText += $" Level {pair.Value}";
				}
				if (Main.keyState.PressingShift())
				{
					effectsText += $": {effectType.desc}";
				}
			}
			tooltips.Add(new TooltipLine(mod, "TerrariaConstruct:EffectsLine", effectsText));

			if (!Main.keyState.PressingShift() && effects.Count > 0)
			{
				tooltips.Add(new TooltipLine(mod, "TerrariaConstruct:ShiftMessageLine", "Press Shift to see descriptions!"));
			}

			if (GHelper.GetPartType(partKey) == PartData.UnknownPart)
			{
				TooltipLine line = new TooltipLine(mod, "TerrariaConstruct:BrokenPartLine", "This part is broken! Did you disable a mod since you last played?");
				tooltips.Add(line);
			}
		}

		public override ModItem Clone(Item item)
		{
			ConstructPart myClone = (ConstructPart)base.Clone(item);
			myClone.partKey = partKey;
			return myClone;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			PartType partType = GHelper.GetPartType(partKey);
			item.height = ModContent.GetTexture(partType.invTexture).Height;
			item.width = ModContent.GetTexture(partType.invTexture).Width;
			item.SetNameOverride(GHelper.GetPartName(partKey));
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			PartType pType = GHelper.GetPartType(partKey);
			item.height = ModContent.GetTexture(pType.invTexture).Height;
			item.width = ModContent.GetTexture(pType.invTexture).Width;
			item.SetNameOverride(GHelper.GetPartName(partKey));
			return false;
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = ModContent.GetTexture("Gunplay/Items/GunParts/Barrels/HandgunBarrel");
			try
			{
				texture = ModContent.GetTexture(GHelper.GetPartType(partKey).invTexture);
			}
			catch (Exception e)
			{
				Gunplay.Instance.Logger.Error("Something failed when getting invTexture in PostDrawInInventory! " + e.Message);
			}
			//spriteBatch.Draw(texture, new Vector2(0, 0), null, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			//Vector2 modifiedPosition = position /*- Main.itemTexture[item.type].Size()*//* + texture.Size()*/;

			//Texture2D originalTexture = Main.itemTexture[item.type];
			//Rectangle originalFrame = originalTexture.Frame(1, 1, 0, 0);
			float inventoryScale = Main.inventoryScale;

			//Vector2 modifiedPosition = position - (Main.itemTexture[item.type].Size() * Main.inventoryScale) / 2f + (Main.itemTexture[item.type].Size() * Main.inventoryScale) / 2f;
			//modifiedPosition = modifiedPosition + (texture.Size() * Main.inventoryScale) / 2f - (texture.Size() * Main.inventoryScale) / 2f;
			Vector2 modifiedPosition = position + new Vector2(texture.Width / 2f, texture.Height / 2f);

			Vector2 modifiedOrigin = texture.Size() * ((scale / inventoryScale) / 2f - 0.5f);

			// position2 = position + (orig. tex size * invScale)/2f - (orig. tex size) * 1f / 2f
			//position = position2 - ((orig. tex size * invScale)/2f - (orig. tex size) * 1f / 2f) => position = position2 - (orig. tex size * invScale)/2f + (orig. tex size) * inv. scale / 2f)
			//spriteBatch.Draw(texture, modifiedPosition, null, drawColor, 0, modifiedOrigin, scale, SpriteEffects.None, 0f);
			//spriteBatch.Draw(texture, modifiedPosition, null, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, position, null, drawColor, 0, modifiedOrigin, scale, SpriteEffects.None, 0f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.GetTexture("Gunplay/Items/GunParts/Barrels/HandgunBarrel");
			Vector2 position = item.position - Main.screenPosition + new Vector2(item.width / 2, item.height / 2);
			try
			{
				texture = ModContent.GetTexture(GHelper.GetPartType(partKey).invTexture);
			}
			catch (Exception e)
			{
				Gunplay.Instance.Logger.Error("Something failed when getting invTexture in PostDrawInWorld! " + e.Message);
			}
			spriteBatch.Draw(texture, position, null, lightColor, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
		}

		public override void Load(TagCompound tag)
		{
			partKey = tag.GetString("partKey");
			SetDefaults();
		}

		public override TagCompound Save()
		{
			return new TagCompound
			{
				{ "partKey", partKey }
			};
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(partKey);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			partKey = reader.ReadString();
			SetDefaults();
		}
	}
}
