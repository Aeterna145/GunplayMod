using Gunplay.Items.GunParts;
using Gunplay.Items.Guns;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Gunplay
{
	public static class GunHelper
	{
		public static void AddPart(string key, string materialName, float offsetX, float offsetY, string useTexture, string invTexture, string type, float layer, int power, Dictionary<string, int> effects = null)
		{
			AddPart(key, materialName, new Vector2(offsetX, offsetY), useTexture, invTexture, type, layer, power, effects);
		}

		public static void AddPart(string key, string materialName, Vector2 offset, string useTexture, string invTexture, string type, float layer, int power, Dictionary<string, int> effects = null)
		{
			effects = effects ?? new Dictionary<string, int>();
			Gunplay.parts.Add(key, new PartType(key, materialName, type, layer, power, offset, useTexture, invTexture, effects));
		}

		public static void AddPart(GunPart partType)
		{
			Gunplay.parts.Add(partType.key, partType);
		}

		public static GunPart GetPartType(string key)
		{
			if (Gunplay.parts.ContainsKey(key))
			{
				return Gunplay.parts[key];
			}
			else
			{
				if (Gunplay.doneCrossModContent)
				{
					Gunplay.instance.Logger.Warn($"The key \"{key}\" doesn't exist in the parts dictionary!");
				}
				return Gunplay.parts["TerrariaConstruct:UnknownPart"];
			}
		}

		public static void AddEffect(string key, string name, int maxLevel, bool showLevel, string desc)
		{
			Gunplay.effects.Add(key, new EffectType(key, name, maxLevel, showLevel, desc));
		}

		public static void AddEffect(EffectType effectType)
		{
			Gunplay.effects.Add(effectType.key, effectType);
		}

		public static EffectType GetEffectType(string key)
		{
			if (Gunplay.effects.ContainsKey(key))
			{
				return Gunplay.effects[key];
			}
			else
			{
				Gunplay.instance.Logger.Warn($"The key \"{key}\" doesn't exist in the effects dictionary!");
				return Gunplay.effects["TerrariaConstruct:UnknownEffect"];
			}
		}

		public static void AddEffectsToItem(Item item, Dictionary<string, int> effects)
		{
			if (item.type != ModContent.ItemType<ConstructGun>())
			{
				Gunplay.instance.Logger.Warn("An item that is not a ConstructTool just tried to add effects.");
				return;
			}
			foreach (KeyValuePair<string, int> effect in effects)
			{
				if ((item.modItem as ConstructGun).effects.ContainsKey(effect.Key))
				{
					(item.modItem as ConstructGun).effects[effect.Key] = Math.Min((item.modItem as ConstructGun).effects[effect.Key] + effect.Value, GetEffectType(effect.Key).maxLevel);
				}
				else
				{
					(item.modItem as ConstructGun).effects.Add(effect.Key, effect.Value);
				}
			}
		}

		public static Dictionary<string, int> CapEffectLevels(Dictionary<string, int> effects)
		{
			Dictionary<string, int> effectsCache = effects;
			foreach (KeyValuePair<string, int> effect in effectsCache)
			{
				int maxLevel = GetEffectType(effect.Key).maxLevel;
				if (effect.Value > maxLevel)
				{
					effects[effect.Key] = maxLevel;
				}
			}
			return effects;
		}

		public static int CheckEffect(Item item, string effect)
		{
			if (!Gunplay.effects.ContainsKey(effect))
			{
				Gunplay.instance.Logger.Warn($"The effect \"{effect}\" doesn't exist in the effect dictionary!");
				return -1;
			}
			if (item.type != ModContent.ItemType<ConstructGun>())
			{
				Gunplay.instance.Logger.Warn("An item that wasn't a ConstructTool just tried to check an effect!");
				return -1;
			}
			if ((item.modItem as ConstructGun).effects.ContainsKey(effect))
			{
				return (item.modItem as ConstructGun).effects[effect];
			}
			else
			{
				return -1;
			}
		}

		public static int DamageFromParts(List<string> keys)
		{
			if (keys.Count == 0)
			{
				return 0;
			}

			List<int> powers = new List<int>();
			foreach (string key in keys)
			{
				powers.Add(GetPartType(key).damage);
			}
			//TerrariaConstruct.instance.Logger.Info("Power Total: " + powers.Sum());
			//TerrariaConstruct.instance.Logger.Info("Power Reduced: " + (powers.Sum() / powers.Count));
			return powers.Sum() / powers.Count;
		}

		public static string GetToolName(List<string> keys)
		{
			List<string> names = new List<string>();
			foreach (string key in keys)
			{
				names.Add(GetPartType(key).materialName);
			}
			names.Reverse();
			return string.Join("-", names);
		}

		public static string GetPartName(string key)
		{
			GunPart partType = GetPartType(key);
			return partType.materialName + " " + partType.type;
		}

		public static void SetDefaults(Item item)
		{
			ConstructGun cTool = item.modItem as ConstructGun;
			if (cTool.parts.Count == 0)
			{
				return;
			}
			string type = cTool.itemType;
			cTool.parts = cTool.parts.OrderBy(x => GetPartType(x).layer).ToList();
			int damage = DamageFromParts(cTool.parts);

			item.damage = damage;
			item.knockBack = 1f;
			item.useTime = 30;
			item.useAnimation = 30;

			switch (type)
			{
				case "Gun":
					item.melee = false;
					item.ranged = true;
					item.magic = false;
					item.summon = false;
					item.thrown = false;
					item.useTime = FireRate;
					item.useStyle = ItemUseStyleID.HoldingOut;
					item.useTurn = true;
					item.autoReuse = CanAutoReuse;
					item.UseSound = SoundID.Item1;
					break;
			}
		}

		public static bool CheckForBrokenParts(List<string> keys)
		{
			List<PartType> parts = new List<PartType>();
			keys.ForEach(key => parts.Add(GetPartType(key)));
			return parts.Contains(PartData.UnknownPart);
		}

		public static bool CheckForBrokenEffects(List<string> keys)
		{
			List<EffectType> effects = new List<EffectType>();
			keys.ForEach(key => effects.Add(GetEffectType(key)));
			return effects.Contains(EffectData.UnknownEffect);
		}
	}
}