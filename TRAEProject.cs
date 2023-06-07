using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using TRAEProject;
using TRAEProject.Changes.Weapon;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TRAEProject.NewContent.Items.Accesories.ShadowflameCharm;
using static Terraria.ModLoader.ModContent;
using ReLogic.Content;
using TRAEProject.NewContent.Items.Armor.IceArmor;
using TRAEProject.Changes.Weapon.Ranged;
using TRAEProject.Changes.Recipes;
using Terraria.DataStructures;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.IO;
using System;
using TRAEProject.Changes.Prefixes;

namespace TRAEProject
{
    public class TRAEProj : Mod
    {
        public static TRAEProj Instance;

        public static Texture2D debugCross;

        public const string DreadHead1 = "TRAEProject/Changes/NPCs/Boss/Dreadnautilus/MapIcon";
        public const string DreadHead2 = "TRAEProject/Changes/NPCs/Boss/Dreadnautilus/MapIcon2";
        public static int IceMajestyCape;
        public override void AddRecipeGroups()/* tModPorter Note: Removed. Use ModSystem.AddRecipeGroups */
        {
            RecipeGroup group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Emblem", new int[]
            {
                ItemID.WarriorEmblem,
                ItemID.RangerEmblem,
                ItemID.SorcererEmblem,
                ItemID.SummonerEmblem
            });
            RecipeGroup.RegisterGroup("Emblem", group);
            RecipeGroup CloudBalloon1 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Cloud in a Balloon", new int[]
            {
                ItemID.CloudinaBalloon,
                ItemID.BlueHorseshoeBalloon
            });
            RecipeGroup.RegisterGroup("CloudBalloon", CloudBalloon1);
            RecipeGroup Jumpgroup1 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Cloud in a Bottle", new int[]
            {
                ItemID.CloudinaBottle,
                ItemID.CloudinaBalloon,
                ItemID.BlueHorseshoeBalloon
            });
            RecipeGroup.RegisterGroup("CloudJump", Jumpgroup1);
            RecipeGroup BlizzardBalloon1 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Blizzard in a Balloon", new int[]
            {
                ItemID.BlizzardinaBalloon,
                ItemID.WhiteHorseshoeBalloon
            });
            RecipeGroup.RegisterGroup("BlizzardBalloon", BlizzardBalloon1);
            RecipeGroup Jumpgroup2 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Blizzard in a Bottle", new int[]
            {
                ItemID.BlizzardinaBottle,
                ItemID.BlizzardinaBalloon,
                ItemID.WhiteHorseshoeBalloon
            });
            RecipeGroup.RegisterGroup("BlizzardJump", Jumpgroup2);
            RecipeGroup SandstormBalloon1 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Sandstorm in a Balloon", new int[]
            {
                ItemID.SandstorminaBalloon,
                ItemID.YellowHorseshoeBalloon
            });
            RecipeGroup.RegisterGroup("SandstormBalloon", SandstormBalloon1);
            RecipeGroup Jumpgroup3 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Sandstorm in a Bottle", new int[]
            {
                ItemID.SandstorminaBottle,
                ItemID.SandstorminaBalloon,
                ItemID.YellowHorseshoeBalloon
            });
            RecipeGroup.RegisterGroup("SandstormJump", Jumpgroup3);
            RecipeGroup Adamantite = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Adamantite Bar", new int[]
            {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar
            });
            RecipeGroup.RegisterGroup("Adamantite", Adamantite); // RENAME THIS TO ADAMANTITE
            RecipeGroup Shadowscales = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Evil Tissue", new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample
            });
            RecipeGroup.RegisterGroup("Shadowscales", Shadowscales); RecipeGroup group1 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Tsunami in a Bottle", new int[]
            {
                ItemID.TsunamiInABottle,
                ItemID.SharkronBalloon
            });
            RecipeGroup.RegisterGroup("TsunamiJump", group1);
            RecipeGroup group2 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Fart in a jar", new int[]
             {
                ItemID.FartinaJar,
                ItemID.FartInABalloon
             });
            RecipeGroup.RegisterGroup("FartJump", group2);
            RecipeGroup group3 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Honey Balloon", new int[]
            {
                ItemID.HoneyComb,
                ItemID.HoneyBalloon
            });
            RecipeGroup.RegisterGroup("HoneyBalloon", group3);
        }
    
        public override void Load()
        {
            Terraria.IL_Projectile.StatusNPC += (il) => {
                var c = new ILCursor(il);
                if (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(359)))
                {
                    c.Emit(OpCodes.Pop);
                    c.Emit(OpCodes.Ldc_I4, int.MinValue);
                }
                if (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(379))) {
                    c.Emit(OpCodes.Pop);
                    c.Emit(OpCodes.Ldc_I4, int.MinValue);
                }

                if (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(390))) {
                    c.Emit(OpCodes.Pop);
                    c.Emit(OpCodes.Ldc_I4, int.MaxValue);
                }
                if (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(391)))
                {
                    c.Emit(OpCodes.Pop);
                    c.Emit(OpCodes.Ldc_I4, int.MinValue);
                }
                if (c.TryGotoNext(MoveType.After, x => x.MatchLdcI4(392))) {
                    c.Emit(OpCodes.Pop);
                    c.Emit(OpCodes.Ldc_I4, int.MinValue);
                }
   
            };
            Instance = this;
            AddBossHeadTexture(DreadHead1);
            AddBossHeadTexture(DreadHead2);
            if (!Main.dedServ)
            {
                IceMajestyCape = EquipLoader.AddEquipTexture(this, "TRAEProject/NewContent/Items/Armor/IceArmor/IceMajestyBreastplate_Back", EquipType.Back, GetModItem(ItemType<IceMajestyBreastplate>()));
            }
            Main.QueueMainThreadAction(() =>
            {
                QwertyFlexOnBame.CreateDolphin();
                if (!Main.dedServ)
                {
                    TRAEProject.Changes.Resprites.Resprites.LoadSprites();
                    debugCross = Request<Texture2D>("TRAEProject/DebugCross", AssetRequestMode.ImmediateLoad).Value;
                }

            });
            // WingStats[] array = new WingStats[47];
            //array[46] = new WingStats(25, 4f); // fledgling, speed up from 3f
            //array[1] = new WingStats(100, 6.25f); // Demon
            //array[2] = new WingStats(100, 6.25f); // Angel 
            //array[13] = new WingStats(100, 6.25f); // leaf
            //array[25] = new WingStats(130, 6.75f); // fin
            //array[7] = new WingStats(130, 6.75f);// fairy
            //array[6] = new WingStats(130, 6.75f);// harpy
            //array[10] = new WingStats(130, 6.75f); // frozen
            //array[4] = new WingStats(150, 6.5f); // jetpack
            //array[15] = new WingStats(160, 7.5f); // bee
            //array[5] = new WingStats(160, 7.5f); // butterfly
            //array[14] = new WingStats(160, 8f, 2f); // bat, horizontal speed 7.5f > 8f and acceleration boost
            //array[9] = new WingStats(160, 7.5f); // flame
            //array[11] = new WingStats(170, 7.5f); // spectre
            //array[8] = new WingStats(170, 7.5f); // steampunk
            //array[27] = new WingStats(170, 7.5f); // Mothron, 20% faster ascent
            //array[24] = new WingStats(170, 7.5f); // Beetle
            //array[22] = new WingStats(170, 6.5f, 1f, hasHoldDownHoverFeatures: true, 8f, 8f); // hoverboard
            //array[21] = new WingStats(180, 7.5f); // spooky
            //array[20] = new WingStats(180, 9f); // tattered fairy, buffed horizontal speed
            //array[12] = new WingStats(180, 7.5f); // steampunk
            //array[23] = new WingStats(180, 9f, 2f); // festive, increased horizontal speed and acceleration
            //array[26] = new WingStats(180, 8f, 2f); // fishron
            //array[45] = new WingStats(180, 8f, 4.5f, hasHoldDownHoverFeatures: true, 16f, 16f); // starboard
            //array[37] = new WingStats(150, 7f, 2.5f, hasHoldDownHoverFeatures: true, 9f, 8f); // Betsy's, nerfed hover speed
            //array[44] = new WingStats(150, 7.2f, 2f); // empress, nerfed horizontal speed to 7.2
            //new WingStats(150, 6f, 2.5f, hasHoldDownHoverFeatures: true, 12f, 12f);
            //array[29] = new WingStats(180, 9f, 2.5f); // solar
            //array[32] = new WingStats(180, 11f, 3f); // stardust
            //array[30] = new WingStats(180, 7.5f, 1.5f, hasHoldDownHoverFeatures: false, -1, 1f); // booster, buffed horizontal speed from 6.5 to 7.5f, removed hover
            //array[31] = new WingStats(180, 7.5f, 1.5f, hasHoldDownHoverFeatures: true, 10f, 8f); // mantle, buffed horizontal velocity from 6.5f to 7.5
            //array[43] = (array[41] = (array[42] = (array[40] = (array[39] = (array[38] = (array[36] = (array[35] = (array[34] = (array[33] = (array[28] = (array[19] = (array[18] = (array[17] = (array[16] = (array[3] = new WingStats(150, 7f)))))))))))))))); // devs
            //ArmorIDs.Wing.Sets.Stats = array;

        }
        public override void Unload()
        {
            Main.QueueMainThreadAction(() =>
            {
                if (!Main.dedServ)
                {
                    TRAEProject.Changes.Resprites.Resprites.UnloadSprites();
                }

            });
        }
        public override void AddRecipes()/* tModPorter Note: Removed. Use ModSystem.AddRecipes */
        {
            WeaponRecipes.Load(this);
            AccesoryRecipes.Load(this);
            MiscRecipes.Load(this);
            ArmorRecipes.Load(this);
        }
        public override void PostAddRecipes()/* tModPorter Note: Removed. Use ModSystem.PostAddRecipes */
        {
            foreach (Recipe recipe in Main.recipe)
            {
                WeaponRecipes.Modify(recipe);
                AccesoryRecipes.Modify(recipe);
                MiscRecipes.Modify(recipe);
                ArmorRecipes.Modify(recipe);
            }
        }
        public override object Call(params object[] args) 
        {
			try 
            {
				// Where should other mods call? They could call at end of Load?
				string message = args[0] as string;
				if (message == "BoomerangPrefix") 
                {
					int type = Convert.ToInt32(args[1]);
                    GiveWeaponsPrefixes.otherModBoomeragLikes.Add(type);
					return "Success";
				}
				else 
                {
					Logger.Error("TRAE Call Error: Unknown Message: " + message);
				}
			}
			catch (Exception e) 
            {
				Logger.Error("TRAE Call Error: " + e.StackTrace + e.Message);
			}
			return "Failure";
		}
    }
}
