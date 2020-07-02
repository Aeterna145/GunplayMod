using Gunplay.Items.GunParts.Barrels;
using Gunplay.Items.GunParts.Chambers;
using Gunplay.Items.GunParts.Handles;
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
        /// </summary>
        public static void GunRecipe(Mod mod, int tileID, int result, int part1 = 0, int part2 = 0, int part3 = 0, int part4 = 0)
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

            recipe.AddTile(tileID);
            recipe.SetResult(result);
            recipe.AddRecipe();
        }

        public override void AddRecipes()
        {
            VanillaGuns();

            base.AddRecipes();
        }

        /// <summary>
        /// Where we put recipes for vanilla guns using our modded parts.
        /// </summary>
        public void VanillaGuns()
        {
            GunRecipe(this, TileID.Anvils, ItemID.Sandgun, ModContent.ItemType<SandgunBarrel>(), ModContent.ItemType<SandgunChamber>(), ModContent.ItemType<SandgunHandle>());
        }
    }
}
