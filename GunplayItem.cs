using Gunplay.Items.GunParts;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Gunplay
{
    class GunplayItem : GlobalItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SandgunBarrel>());
            recipe.AddIngredient(ItemType<SandgunChamber>());
            recipe.AddIngredient(ItemType<SandgunHandle>());
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(ItemID.Sandgun);
            recipe.AddRecipe();
        }
    }
}
