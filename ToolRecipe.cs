using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Gunplay.Items;

namespace Gunplay
{
    public class ToolRecipe : ModRecipe
    {
		public string itemType = "";

		public List<string> partTypes = new List<string>();

		public List<string> partKeys = new List<string>();

		public List<Dictionary<string, int>> effectsToAdd = new List<Dictionary<string, int>>();

		public ToolRecipe(Mod mod, string itemType, List<string> partTypes) : base(mod)
		{
			partKeys = new List<string>();
			this.itemType = itemType;
			this.partTypes = partTypes;
		}

		public override bool RecipeAvailable()
		{
			partKeys = new List<string>();
			effectsToAdd = new List<Dictionary<string, int>>();
			bool[] found = new bool[partTypes.Count];
			Player player = Main.LocalPlayer;
			for (int i = 0; i < partTypes.Count; i++)
			{
				for (int j = 0; j < player.inventory.Length; j++)
				{
					Item currentItem = player.inventory[j];
					if (currentItem.type != ModContent.ItemType<ConstructPart>())
					{
						continue;
					}

					ConstructPart part = currentItem.modItem as ConstructPart;
					if (GHelper.GetPartType(part.partKey).type == partTypes[i])
					{
						found[i] = true;
						partKeys.Add(part.partKey);
						effectsToAdd.Add(GHelper.GetPartType(part.partKey).effects);
						break;
					}
				}
			}

			Recipe recipe = Main.recipe[RecipeIndex];
			if (found.All(x => x))
			{
				for (int i = 0; i < recipe.requiredItem.Length; i++)
				{
					if (recipe.requiredItem[i].type != ModContent.ItemType<ConstructPart>())
					{
						continue;
					}
					else
					{
						(recipe.requiredItem[i].modItem as ConstructPart).partKey = partKeys[i];
					}
				}
				(recipe.createItem.modItem as ConstructTool).parts = partKeys;
				(recipe.createItem.modItem as ConstructTool).itemType = itemType;
				(recipe.createItem.modItem as ConstructTool).effects = new Dictionary<string, int>();
				foreach (Dictionary<string, int> effect in effectsToAdd)
				{
					GHelper.AddEffectsToItem(recipe.createItem, effect);
					//(recipe.createItem.modItem as ConstructTool).effects.AddRange(effect);
				}
				(recipe.createItem.modItem as ConstructTool).SetDefaults();
				return base.RecipeAvailable();
			}
			else
			{
				//for (int i = 0; i < recipe.requiredItem.Length; i++)
				//{
				//	if (recipe.requiredItem[i].type != ItemType<ConstructPart>())
				//	{
				//		continue;
				//	}
				//	else
				//	{
				//		(recipe.requiredItem[i].modItem as ConstructPart).partKey = "TerrariaConstruct:UnknownPart";
				//	}
				//}
				return false;
			}
		}

		public override void OnCraft(Item item)
		{
			Player player = Main.LocalPlayer;
			for (int i = 0; i < partKeys.Count; i++)
			{
				for (int j = 0; j < player.inventory.Length; j++)
				{
					Item currentItem = player.inventory[j];
					if (currentItem.type != ModContent.ItemType<ConstructPart>())
					{
						continue;
					}

					ConstructPart partItem = currentItem.modItem as ConstructPart;
					if (partItem.partKey == partKeys[i] && GHelper.GetPartType(partItem.partKey).type == partTypes[i])
					{
						currentItem.TurnToAir();
						break;
					}
				}
			}
			(item.modItem as ConstructTool).parts = partKeys;
			(item.modItem as ConstructTool).itemType = itemType;
			(item.modItem as ConstructTool).effects = new Dictionary<string, int>();
			foreach (Dictionary<string, int> effect in effectsToAdd)
			{
				GHelper.AddEffectsToItem(item, effect);
				//(item.modItem as ConstructTool).effects.AddRange(effect);
			}
			(item.modItem as ConstructTool).SetDefaults();
			FindRecipes();
			RecipeAvailable();
		}

		public override int ConsumeItem(int type, int numRequired)
		{
			return 0; //Consuming items manually in OnCraft.
		}
	}
}
