using Gunplay.Items;
using Gunplay.Items.GunParts.Barrels;
using Gunplay.Items.GunParts.Chambers;
using Gunplay.Items.GunParts.Handles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Gunplay
{
    public partial class Gunplay : Mod
    {
        /// <summary>
        /// Used to make vanilla gun recipes out of modded items.
        /// <para><paramref name="mod"/> - Our mod class.</para>
        /// <para><paramref name="tileID"/> - Our wanted <c>TileID</c></para>
        /// <para><paramref name="result"/> - Our wanted result (<c>ItemID</c> or <c>ModContent.ItemType</c>)</para>
        /// <para><paramref name="part1"/> - Gun part</para>
        /// <para><paramref name="part2"/> - Gun part</para>
        /// <para><paramref name="part3"/> - Gun part</para>
        /// <para><paramref name="part4"/> - Gun part</para>
        /// <para><paramref name="part5"/> - Gun part</para>
        /// <para><paramref name="part6"/> - Gun part</para>
        /// </summary>
        public static void GunRecipe(Mod mod, int tileID, int result, int part1 = 0, int part2 = 0, int part3 = 0, int part4 = 0, int part5 = 0, int part6 = 0)
        {
            ModRecipe recipe = new ModRecipe(mod);

            if (part1 != 0)
                recipe.AddIngredient(part1);
            if (part2 != 0)
                recipe.AddIngredient(part2);
            if (part3 != 0)
                recipe.AddIngredient(part3);
            if (part4 != 0)
                recipe.AddIngredient(part4);
            if (part5 != 0)
                recipe.AddIngredient(part5);
            if (part6 != 0)
                recipe.AddIngredient(part6);

            recipe.AddTile(tileID);
            recipe.SetResult(result);
            recipe.AddRecipe();
        }

        public static void CreatePartRecipe(Mod mod, int material, int amount, int tile, string partKey)
        {
            PartRecipe recipe = new PartRecipe(mod, partKey);
            recipe.AddIngredient(material, amount);
            recipe.AddTile(tile);
            recipe.SetResult(ModContent.ItemType<ConstructPart>());
            recipe.AddRecipe();
        }

        public static void CreateToolRecipe(Mod mod, int tile, string itemType, List<string> partTypes)
        {
            ToolRecipe recipe = new ToolRecipe(mod, itemType, partTypes);
            for (int i = 0; i < partTypes.Count; i++)
            {
                recipe.AddIngredient(ModContent.ItemType<ConstructPart>(), 1);
            }
            recipe.AddTile(tile);
            recipe.SetResult(ModContent.ItemType<ConstructTool>());
            recipe.AddRecipe();
        }

        public override void AddRecipes()
        {
            VanillaGuns();

            CreatePartRecipe(Instance, ItemID.CopperBar, 8, TileID.Anvils, PartData.CopperPickaxeHead.key);

            foreach (object[] args in modPartRecipes)
            {
                try
                {
                    CreatePartRecipe(Instance, Convert.ToInt32(args[1]), Convert.ToInt32(args[2]), Convert.ToInt32(args[3]), args[4] as string);
                    Logger.Info("Part Recipe added for part with key " + args[4]);
                }
                catch (Exception e)
                {
                    Logger.Error("Error adding part recipe: " + e.StackTrace + e.Message);
                }
            }

            CreateToolRecipe(Instance, TileID.Anvils, "Pickaxe", new List<string> { "Pickaxe Head", "Tool Rod" });
            CreateToolRecipe(Instance, TileID.Anvils, "Axe", new List<string> { "Axe Head", "Tool Rod" });
            CreateToolRecipe(Instance, TileID.Anvils, "Hammer", new List<string> { "Hammer Head", "Tool Rod" });

            foreach (object[] args in modToolRecipes)
            {
                try
                {
                    CreateToolRecipe(Instance, Convert.ToInt32(args[1]), args[2] as string, args[3] as List<string>);
                    Logger.Info("Tool Recipe added for tool " + args[2]);
                }
                catch (Exception e)
                {
                    Logger.Error("Error adding tool recipe: " + e.StackTrace + e.Message);
                }
            }

            doneCrossModContent = true;
            modPartRecipes = null;
            modToolRecipes = null;

            base.AddRecipes();
        }

        public override void PostAddRecipes()
        {
            //https://github.com/gardenappl/DyeEasy/blob/master/DyeEasy.cs
            for (int i = 0; i < Main.recipe.Length; i++)
            {
                Recipe recipe = Main.recipe[i];
                if (recipe is PartRecipe pRecipe)
                {
                    (recipe.createItem.modItem as ConstructPart).partKey = pRecipe.partKey;
                }
            }

            base.PostAddRecipes();
        }

        /// <summary>
        /// Where we put recipes for vanilla guns using our modded parts.
        /// </summary>
        public void VanillaGuns()
        {
            GunRecipe(this, TileID.Anvils, ItemID.Sandgun, ModContent.ItemType<SandgunBarrel>(), ModContent.ItemType<SandgunChamber>(), ModContent.ItemType<SandgunHandle>());
            GunRecipe(this, TileID.Anvils, ItemID.Handgun, ModContent.ItemType<HandgunBarrel>(), ModContent.ItemType<HandgunChamber>(), ModContent.ItemType<HandgunHandle>());
        }
    }
}
