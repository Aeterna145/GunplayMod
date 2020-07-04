using Gunplay.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Gunplay
{
    public class PartRecipe : ModRecipe
    {
		public string partKey = "";

		public PartRecipe(Mod mod, string partKey) : base(mod)
		{
			this.partKey = partKey;
		}

		public override void OnCraft(Item item)
		{
			if (item.modItem is ConstructPart cPart)
			{
				cPart.partKey = partKey;
				cPart.SetDefaults();
			}
			else
			{
				Gunplay.Instance.Logger.Error("A PartRecipe just tried to craft an item that wasn't a ConstructPart!");
			}
		}
	}
}
