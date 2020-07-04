using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gunplay
{
    public class EffectType
    {
		public string key; //The key that TerrariaConstruct uses to refer back to this EffectType.
		public string name; //The displayed name for this EffectType.
		public string desc; //A description of what this EffectType does.
		public int maxLevel; //The maximum level allowed for this EffectType. TerrariaConstruct will try to cap this effect's level on any tools that use it.
		public bool showLevel; //Toggles whether or not this effect's level should appear in tooltips.

		public EffectType(string key, string name, int maxLevel, bool showLevel, string desc = "No description provided!")
		{
			this.key = key;
			this.name = name;
			this.maxLevel = maxLevel;
			this.showLevel = showLevel;
			this.desc = desc;
		}
	}
}
