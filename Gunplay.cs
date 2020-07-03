using Gunplay.Items.Guns;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Gunplay
{
    public partial class Gunplay : Mod
    {
        internal static Gunplay instance = ModContent.GetInstance<Gunplay>();
        public static Dictionary<string, PartType> parts = new Dictionary<string, PartType>();
        public static Dictionary<string, EffectType> effects = new Dictionary<string, EffectType>();
        private List<object[]> modPartRecipes;
        private List<object[]> modToolRecipes;

        public static bool doneCrossModContent;

        internal bool cheatUIOpen = false;

        internal static Gunplay Instance;
        internal UserInterface DismantleUserInterface;
        internal UserInterface CheatDismantleUserInterface;
        internal Mod herosMod;
        internal Mod cheatSheet;

        public override void Load()
        {
            Instance = this;

            herosMod = ModLoader.GetMod("HEROsMod");
            cheatSheet = ModLoader.GetMod("CheatSheet");

            doneCrossModContent = false;

            GunHelper.AddPart(PartData.UnknownPart);

            if (!Main.dedServ)
            {
                DismantleUserInterface = new UserInterface();
                CheatDismantleUserInterface = new UserInterface();
            }
        }

        public override void Unload()
        {
            Instance = null;
            instance = null;
            parts = null;
            effects = null;
            modPartRecipes = null;
            modToolRecipes = null;

            base.Unload();
        }

        public override void PostSetupContent()
        {
            try
            {
                if (herosMod != null)
                {
                    SetupModIntegration(herosMod);
                }
                else if (cheatSheet != null)
                {
                    SetupModIntegration(cheatSheet);
                }
                else
                {

                }
            }
            catch (Exception e)
            {
                Logger.Warn("Gunplay: PostSetupContent Error: " + e.StackTrace + e.Message);
            }

            cheatUIOpen = false;

            base.PostSetupContent();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            DismantleUserInterface?.Update(gameTime);
            CheatDismantleUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (inventoryIndex != -1)
            {
                layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
                    "Gunplay: Dismantle UI",
                    delegate
                    {
                        DismantleUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
                    "Gunplay: Cheat Dismantle UI",
                    delegate
                    {
                        CheatDismantleUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public void SetupModIntegration(Mod mod)
        {
            if (mod == herosMod)
            {
                herosMod.Call("AddPermission", "CheatDismantleUI", "No-Cost Dismantling UI");

                if (!Main.dedServ)
                {
                    herosMod.Call("AddSimpleButton", "CheatDismantleUI", GetTexture("UI/CheatDismantleUIButton"), (Action)CheatDismantleUIButtonActivated, (Action<bool>)PermissionChanged, (Func<string>)CheatUITooltip);
                }
            }
            else if (mod == cheatSheet)
            {

            }
        }

        public string CheatUITooltip()
        {
            return cheatUIOpen ? "Open No-Cost Dismantling UI" : "Close No-Cost Dismantling UI";
        }

        public void CheatDismantleUIButtonActivated()
        {
            cheatUIOpen = !cheatUIOpen;

            if (!cheatUIOpen)
            {
                Main.playerInventory = true;
                Main.npcChatText = ""; // Closes chat.
                Main.PlaySound(SoundID.MenuTick);
                ModContent.GetInstance<Gunplay>().CheatDismantleUserInterface.SetState(new UI.CheatDismantleUI());
            }
            if (cheatUIOpen)
            {
                Main.PlaySound(SoundID.MenuTick);
                ModContent.GetInstance<Gunplay>().CheatDismantleUserInterface.SetState(null);
            }
        }

        public void PermissionChanged(bool hasPermission)
        {
            if (!hasPermission)
                cheatUIOpen = false;
        }

        public override object Call(params object[] args)
        {
            try
            {
                string message = args[0] as string;
                if (message == "CheckEffect") //Returns -1 if effect isn't present or item used isn't a ConstructGun. Returns level of effect of effect is present.
                {
                    if ((args[1] as Item).modItem is ConstructGun cTool)
                    {
                        if (cTool.effects.ContainsKey(args[2] as string))
                        {
                            return cTool.effects[args[2] as string];
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (message == "GetItemType") //Returns itemType if item passed in is a ConstructGun. Returns an error otherwise.
                {
                    if ((args[1] as Item).netID == 0)
                    {
                        return "GetItemType called too early!";
                    }
                    else if ((args[1] as Item).modItem is ConstructGun cTool)
                    {
                        return cTool.itemType;
                    }
                    else
                    {
                        return "GetItemType called on an item that is not a ConstructGun!";
                    }
                }
                if (doneCrossModContent) //Done so the following Calls can't be done after mod load. like mid-game.
                {
                    throw new Exception($"Call Error: Gunplay expects the message you sent, \"{message}\", before AddRecipes(). A good place to put this Call() is in PostSetupContent().");
                }
                if (message == "AddPart") //Adds a new PartType to parts.
                {
                    string materialName = args[2] as string;
                    string type = args[5] as string;
                    GunHelper.AddPart(args[1] as string, materialName, Convert.ToSingle(args[3]), Convert.ToSingle(args[4]), args[5] as string, args[6] as string, type, Convert.ToSingle(args[7]), Convert.ToInt32(args[8]), args[9] as Dictionary<string, int>);
                    Logger.Info($"Call Info: Part {materialName} {type} added");
                    return $"Part {materialName} {type} added";
                }
                else if (message == "AddPartRecipe") //Adds the ags to modPartRecipes to add later in AddRecipes.
                {
                    modPartRecipes.Add(args);
                    return "Part Recipe added to list";
                }
                else if (message == "AddToolRecipe") //Adds the args to modToolRecipes to add later in AddRecipes.
                {
                    modToolRecipes.Add(args);
                    return "Tool Recipe added to list";
                }
                else if (message == "AddEffect") //Adds a new EffectType to effects.
                {
                    string name = args[2] as string;
                    GunHelper.AddEffect(args[1] as string, name, Convert.ToInt32(args[3]), Convert.ToBoolean(args[4]), args[5] as string);
                    Logger.Info($"Call Info: Effects {name} added");
                    return $"Effects {name} added";
                }
                else //You dun fucked up.
                {
                    Logger.Error("Call Error: Unknown Message: " + message);
                    return "Call Error: Unknown Message: " + message;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Call Error: " + e.StackTrace + e.Message);
                return "Failure, see logs";
            }
        }

    }
}