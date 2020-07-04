using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace Gunplay.Items.GunParts
{
    // Universal part code goes here.
    public abstract class GunPart : ModItem
    {
		public string key; //The key that TerrariaConstruct uses to refer back to this PartType.
		public string materialName; //The displayed name of this PartType's material. Used for setting item names.
		public string type; //The type of part this is. Used for setting ConstructPart names and determining if a part can be used in a recipe.
		public float layer; //On what layer this PartType should draw on when in a tool. For general reference, Tool Rods have a layer of 1f and Pickaxe Heads has a layer of 3f.
		public string useTexture; //The path to the texture this PartType uses when on a tool and in use.
		public string invTexture; //The path to the texture this PartType uses when in a player's inventory. Defaults to useTexture if not set.
		public int damage; //The effective "power" of a tool. Either pick/axe/hammer power for tool parts or a component of the total damage.
		public Vector2 offset; //The offset of this PartType's texture. For thing like Axes.
        public Dictionary<string, int> effects; //A Dictionary of which effects this PartType (via keys) has and what level each of them are.
        public bool CanAutoReuse;
		public int SpreadAngle;
		public int FireRate;

		public void PartType(string key, string materialName, string type, float layer, int damage, Vector2 offset, string useTexture, bool CanAutoReuse, int SpreadAngle, int FireRate, string invTexture = null, Dictionary<string, int> effects = null)
		{
			this.key = key;
			this.materialName = materialName;
			this.type = type;
			this.layer = layer;
			this.damage = damage;
			this.offset = offset;
			this.useTexture = useTexture;
			this.invTexture = invTexture ?? useTexture;
            this.effects = effects ?? new Dictionary<string, int>();
            this.CanAutoReuse = CanAutoReuse;
            this.FireRate = FireRate;
			this.SpreadAngle = SpreadAngle;
		}
	}
}
