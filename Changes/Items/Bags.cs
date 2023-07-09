using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.Items.Weapons.Jungla;
using TRAEProject.NewContent.Items.Weapons.Ammo;
using TRAEProject.NewContent.Items.Armor.Joter;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Accesories.ExtraJumps;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;

namespace TRAEProject.Changes.Items
{
    public class Bags : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
			
			if(item.expert)
			{
				LeadingConditionRule condition = new LeadingConditionRule(new Conditions.IsHardmode());
				IItemDropRule tridentDrop = ItemDropRule.Common(ItemType<JoterTrident>(), 100);
				tridentDrop.OnSuccess(ItemDropRule.Common(ItemType<JoterMask>(), 1));
				condition.OnSuccess(tridentDrop);
				itemLoot.Add(condition);
			}
			
			switch(item.type)
			{
				case ItemID.EyeOfCthulhuBossBag:
					itemLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
						{
                            return false;
						}
                        return drop.itemId == ItemID.UnholyArrow; // compare more fields if needed
                    });
					LeadingConditionRule corruption = new LeadingConditionRule(new Conditions.IsCorruption());
					corruption.OnSuccess(ItemDropRule.Common(ItemID.UnholyArrow, 1, 100, 200));
					itemLoot.Add(corruption);
					LeadingConditionRule crimson = new LeadingConditionRule(new Conditions.IsCrimson());
					crimson.OnSuccess(ItemDropRule.Common(ItemType<BloodyArrow>(), 1, 100, 200));
					itemLoot.Add(crimson);
				break;
				case ItemID.SkeletronBossBag:
					itemLoot.RemoveWhere(rule =>
                    {
                        if (rule is not OneFromOptionsNotScaledWithLuckDropRule drop) // Type of drop you expect here
						{
                            return false;
						}
						for(int i = 0; i < drop.dropIds.Length; i++)
						{
							if(drop.dropIds[i] == ItemID.BookofSkulls)
							{
								return true;
							}
							
						}
                        return false;
                    });
					itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemID.SkeletronHand, ItemID.SkeletronMask));
				break;	
				case ItemID.PlanteraBossBag:
					itemLoot.RemoveWhere(rule =>
                    {
                        if (rule is OneFromRulesRule) // Type of drop you expect here
						{
                            return true;
						}
                        return false;
                    });
					IItemDropRule melee = ItemDropRule.Common(ItemID.Seedler);
					melee.OnSuccess(ItemDropRule.Common(ItemID.FlowerPow));
					IItemDropRule ranged = ItemDropRule.Common(ItemID.VenusMagnum);
					ranged.OnSuccess(ItemDropRule.Common(ItemType<Jungla>()));
					IItemDropRule magic = ItemDropRule.Common(ItemID.NettleBurst);
					magic.OnSuccess(ItemDropRule.Common(ItemID.LeafBlower));
					itemLoot.Add(new OneFromRulesRule(1, melee, ranged, magic));
				break;
				case ItemID.CultistBossBag:
				itemLoot.Add(ItemDropRule.Common(ItemID.LunarCraftingStation, 1));
				itemLoot.Add(ItemDropRule.Common(ItemType<LuminiteFeather>(), 1));
				break;
				case ItemID.FairyQueenBossBag:
				itemLoot.RemoveWhere(rule =>
				{
					if (rule is not OneFromOptionsNotScaledWithLuckDropRule drop) // Type of drop you expect here
					{
						return false;
					}
					for(int i = 0; i < drop.dropIds.Length; i++)
					{
						if(drop.dropIds[i] == ItemID.FairyQueenMagicItem)
						{
							return true;
						}
						
					}
					return false;
				});
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FaeInABottle>(), 5));
				itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemID.RainbowCrystalStaff, ItemID.PiercingStarlight, ItemID.FairyQueenMagicItem, ItemID.FairyQueenRangedItem));
				break;
				case ItemID.MoonLordBossBag:
					itemLoot.RemoveWhere(rule =>
                    {
                        if (rule is not OneFromOptionsNotScaledWithLuckDropRule drop) // Type of drop you expect here
						{
                            return false;
						}
						for(int i = 0; i < drop.dropIds.Length; i++)
						{
                 
                            if (drop.dropIds[i] == ItemID.StarWrath)
							{
								return true;
							}
							
						}
                        return false;
                    });
                    itemLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                        {
                            return false;
                        }


                        if (drop.itemId == ItemID.GravityGlobe)
                        {
                            return true;
                        }



                        return false;
                    }); 
                    itemLoot.Add(ItemDropRule.FewFromOptionsNotScalingWithLuck(2, 1, ItemID.Meowmere, ItemID.Terrarian, ItemID.SDMG, ItemID.Celeb2, ItemID.LunarFlareBook, ItemID.LastPrism, ItemID.RainbowWhip, ItemID.StardustDragonStaff));
                        break;
				case ItemID.ObsidianLockbox:
				itemLoot.RemoveWhere(rule =>
				{
					if (rule is OneFromOptionsNotScaledWithLuckDropRule)
					{
						return true;
					}
					return false;
				});
				itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ChestLoot.ShadowItems));
				itemLoot.Add(ItemDropRule.Common(ItemID.TreasureMagnet, 4));
				break;
				case ItemID.OasisCrate:
				case ItemID.OasisCrateHard:
				itemLoot.RemoveWhere(rule =>
				{
					//most of the crate's loot is tied to this one drop rule which has sub rules within it, so we'll have to rewrite most of it
					if (rule is AlwaysAtleastOneSuccessDropRule)
					{
						return true;
					}
					return false;
				});
				IItemDropRule bc_scarab = ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemID.ThunderSpear, ItemID.ThunderStaff, ItemID.AncientChisel, ItemID.SandstorminaBottle, ItemID.AnkhCharm, ItemID.MagicConch);
				IItemDropRule bc_bomb = ItemDropRule.NotScalingWithLuck(ItemID.ScarabBomb, 4, 4, 6);
				IItemDropRule[] potions = new IItemDropRule[]
				{
					ItemDropRule.NotScalingWithLuck(ItemID.ObsidianSkinPotion, 1, 2, 4),
					ItemDropRule.NotScalingWithLuck(ItemID.SpelunkerPotion, 1, 2, 4),
					ItemDropRule.NotScalingWithLuck(ItemID.HunterPotion, 1, 2, 4),
					ItemDropRule.NotScalingWithLuck(ItemID.GravitationPotion, 1, 2, 4),
					ItemDropRule.NotScalingWithLuck(ItemID.MiningPotion, 1, 2, 4),
					ItemDropRule.NotScalingWithLuck(ItemID.HeartreachPotion, 1, 2, 4)
				};
				IItemDropRule bc_goldCoin = ItemDropRule.NotScalingWithLuck(ItemID.GoldCoin, 4, 5, 13);
				IItemDropRule bc_fossil = ItemDropRule.NotScalingWithLuck(3380, 4, 10, 16); // sturdy fossil
				IItemDropRule[] oresTier1 = new IItemDropRule[]
				{
					ItemDropRule.NotScalingWithLuck(ItemID.CopperOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.TinOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.IronOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.LeadOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.SilverOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.TungstenOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.GoldOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.PlatinumOre, 1, 30, 49)
				};
				IItemDropRule[] hardmodeOresTier1 = new IItemDropRule[]
				{
					ItemDropRule.NotScalingWithLuck(ItemID.CobaltOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.PalladiumOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.MythrilOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.OrichalcumOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.AdamantiteOre, 1, 30, 49),
					ItemDropRule.NotScalingWithLuck(ItemID.TitaniumOre, 1, 30, 49)
				};
				IItemDropRule[] barsTier1 = new IItemDropRule[]
				{
					ItemDropRule.NotScalingWithLuck(ItemID.IronBar, 1, 10, 20),
					ItemDropRule.NotScalingWithLuck(ItemID.LeadBar, 1, 10, 20),
					ItemDropRule.NotScalingWithLuck(ItemID.SilverBar, 1, 10, 20),
					ItemDropRule.NotScalingWithLuck(ItemID.TungstenBar, 1, 10, 20),
					ItemDropRule.NotScalingWithLuck(ItemID.GoldBar, 1, 10, 20),
					ItemDropRule.NotScalingWithLuck(ItemID.PlatinumBar, 1, 10, 20)
				};
				IItemDropRule[] hardmodeBarsTier1 = new IItemDropRule[]
				{
					ItemDropRule.NotScalingWithLuck(ItemID.CobaltBar, 1, 8, 20),
					ItemDropRule.NotScalingWithLuck(ItemID.PalladiumBar, 1, 8, 20),
					ItemDropRule.NotScalingWithLuck(ItemID.MythrilBar, 1, 8, 20),
					ItemDropRule.NotScalingWithLuck(ItemID.OrichalcumBar, 1, 8, 20),
					ItemDropRule.NotScalingWithLuck(ItemID.AdamantiteBar, 1, 8, 20),
					ItemDropRule.NotScalingWithLuck(ItemID.TitaniumBar, 1, 8, 20)
				};
				List<IItemDropRule> oresList = new List<IItemDropRule>();
				List<IItemDropRule> barsList = new List<IItemDropRule>();
				oresList.AddRange(oresTier1);
				oresList.AddRange(hardmodeOresTier1);
				barsList.AddRange(barsTier1);
				barsList.AddRange(hardmodeBarsTier1);
				IItemDropRule[] oasis = new IItemDropRule[] 
				{
					bc_scarab,
					bc_bomb,
					bc_goldCoin,
					bc_fossil,
					ItemDropRule.SequentialRulesNotScalingWithLuck(1, new OneFromRulesRule(5, oresTier1), new OneFromRulesRule(3, 2, barsTier1)),
					new OneFromRulesRule(3, potions),
				};
				IItemDropRule[] mirage = new IItemDropRule[] 
				{
					bc_scarab,
					bc_bomb,
					bc_goldCoin,
					bc_fossil,
					ItemDropRule.SequentialRulesNotScalingWithLuck(1, new OneFromRulesRule(5, oresList.ToArray()), new OneFromRulesRule(3, 2, barsList.ToArray())),
					new OneFromRulesRule(3, potions),
				};
				if(item.type == ItemID.OasisCrate)
				{
					itemLoot.Add(ItemDropRule.AlwaysAtleastOneSuccess(oasis));
				}
				else
				{
					itemLoot.Add(ItemDropRule.AlwaysAtleastOneSuccess(mirage));
				}
				//itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemID.ThunderSpear, ItemID.ThunderStaff, ItemID.AncientChisel, ItemID.SandstorminaBottle, ItemID.AnkhCharm, ItemID.MagicConch));
				break;
                case ItemID.FloatingIslandFishingCrate:
                case ItemID.FloatingIslandFishingCrateHard:

                    break;
            }
            base.ModifyItemLoot(item, itemLoot);
        }
		public static readonly int[] ShadowChestLoot = new int[] { ItemID.HellwingBow, ItemID.Flamelash, ItemID.FlowerofFire, ItemID.Sunfury };
		/*
		public override bool PreOpenVanillaBag(string context, Player player, int arg)
		{
			if (context == "bossBag" && arg != ItemID.EyeOfCthulhuBossBag && arg != ItemID.EyeOfCthulhuBossBag && arg != ItemID.SkeletronBossBag && arg != ItemID.EaterOfWorldsBossBag && arg != ItemID.BrainOfCthulhuBossBag && arg != ItemID.KingSlimeBossBag && arg != ItemID.QueenBeeBossBag && arg != ItemID.DeerclopsBossBag && arg != ItemID.WallOfFleshBossBag)
			{
				if (Main.rand.NextBool(100))
				{
					player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<JoterTrident>());
					player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<JoterMask>());

				}
			}
			if (context == "bossBag" && arg == ItemID.EyeOfCthulhuBossBag)
			{
				if (Main.tenthAnniversaryWorld && !Main.getGoodWorld)
				{
					player.TryGettingDevArmor(player.GetSource_OpenItem(arg));
				}
				if (Main.rand.Next(7) == 0)
				{
					player.QuickSpawnItem(player.GetSource_OpenItem(arg), 2112);
				}
				if (Main.rand.Next(30) == 0)
				{
					player.QuickSpawnItem(player.GetSource_OpenItem(arg), 1299);
				}
				if (WorldGen.crimson)
				{
					int num9 = Main.rand.Next(20) + 10;
					num9 += Main.rand.Next(20) + 10;
					num9 += Main.rand.Next(20) + 10;
					player.QuickSpawnItem(player.GetSource_OpenItem(arg), 880, num9);
					num9 = Main.rand.Next(3) + 1;
					player.QuickSpawnItem(player.GetSource_OpenItem(arg), 2171, num9);
					num9 = Main.rand.Next(30) + 20;
					player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<BloodyArrow>(), num9);
				}
				else
				{
					int num10 = Main.rand.Next(20) + 10;
					num10 += Main.rand.Next(20) + 10;
					num10 += Main.rand.Next(20) + 10;
					player.QuickSpawnItem(player.GetSource_OpenItem(arg), 56, num10);
					num10 = Main.rand.Next(3) + 1;
					player.QuickSpawnItem(player.GetSource_OpenItem(arg), 59, num10);
					num10 = Main.rand.Next(30) + 20;
					player.QuickSpawnItem(player.GetSource_OpenItem(arg), 47, num10);
				}
				player.QuickSpawnItem(player.GetSource_OpenItem(arg), 3097);
				return false;
			}
			if (context == "bossBag" && arg == ItemID.SkeletronBossBag)
			{
			player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemID.BoneGlove, 1);
				if (Main.rand.Next(3) == 0)
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.SkeletronHand, 1);
				if (Main.rand.Next(3) == 0)
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.SkeletronMask, 1);
				return false;
			}
			if (context == "bossBag" && arg == ItemID.PlanteraBossBag)
			{
				player.TryGettingDevArmor(player.GetSource_OpenItem(arg));
				if (Main.rand.Next(7) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.PlanteraMask);
				}
			player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.TempleKey);
			player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.SporeSac);
				if (Main.rand.Next(15) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.Seedling);
				}
				if (Main.rand.Next(20) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.TheAxe);
				}
				if (Main.rand.Next(2) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.PygmyStaff);
				}
				if (Main.rand.Next(10) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.ThornHook);
				}
				switch (Main.rand.Next(3))
				{
					case 0:
					player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemType<Jungla>());
					player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.VenusMagnum);
						break;
					case 1:
					player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.Seedler);
					player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.FlowerPow);
						break;
					case 2:
					player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.LeafBlower);
					player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.NettleBurst);
						break;
				}
				return false;
			}
			if (context == "bossBag" && arg == ItemID.CultistBossBag) // TO DO
			{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemID.LunarCraftingStation);
				player.QuickSpawnItem(player.GetSource_OpenItem(arg), ItemType<LuminiteFeather>());
			}
				if (context == "bossBag" && arg == ItemID.FairyQueenBossBag) 
				{
					if (Main.rand.Next(5) == 0)
					{
						player.QuickSpawnItem(player.GetSource_OpenItem(arg), ModContent.ItemType<FaeInABalloon>());
					}
				
				player.TryGettingDevArmor(player.GetSource_OpenItem(arg));
			player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.EmpressFlightBooster);
				if (Main.rand.Next(7) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.FairyQueenMask);
				}
				if (Main.rand.Next(10) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.RainbowWings);
				}
				if (Main.rand.Next(20) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.SparkleGuitar);
				}
				if (Main.rand.Next(4) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.HallowBossDye);
				}
				if (Main.rand.Next(20) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.RainbowCursor);
				}
				switch (Main.rand.Next(4))
				{
					case 0:
					player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.PiercingStarlight);
						break;
					case 1:
					player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.FairyQueenMagicItem);
						break;
					case 2:
					player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.FairyQueenRangedItem);
						break;
					case 3:
					player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.RainbowCrystalStaff);
						break;
				}
				return false;
			}
			if (context == "bossBag" && arg == ItemID.MoonLordBossBag) // TO DO
			{
				player.TryGettingDevArmor(player.GetSource_OpenItem(arg));
				if (Main.rand.Next(7) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.BossMaskMoonlord);
				}
				if (Main.rand.Next(10) == 0)
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.MeowmereMinecart);
				}
				if (!player.HasItem(ItemID.PortalGun))
				{
				player.QuickSpawnItem(player.GetSource_OpenItem(arg),3384);
				}
			player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.LunarOre, Main.rand.Next(90, 111));
			player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.GravityGlobe);
			player.QuickSpawnItem(player.GetSource_OpenItem(arg),ItemID.SuspiciousLookingTentacle);
			player.QuickSpawnItem(player.GetSource_OpenItem(arg),4954); // celestial starboard
				int item = Utils.SelectRandom<int>(Main.rand, 3063, 3389, 1553, 3930, 3541, 3570, ItemID.RainbowWhip, ItemID.StardustDragonStaff);
			player.QuickSpawnItem(player.GetSource_OpenItem(arg),item); 
				return false;
			}

			if (context == "crate" && (arg == ItemID.OasisCrate || arg == ItemID.OasisCrateHard))
			{
				int index = Main.rand.Next(ChestLoot.PyramidItems.Length);
				int itemWhoAmI = Item.NewItem(player.GetSource_OpenItem(arg), (int)player.position.X, (int)player.position.Y, player.width, player.height, ChestLoot.PyramidItems[index], 1, noBroadcast: false, -1);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(21, -1, -1, null, itemWhoAmI, 1f);
				}

				if (Main.rand.Next(4) == 0)
				{
					int bombAmount = Main.rand.Next(4, 7);
					int itemWhoAmI2 = Item.NewItem(player.GetSource_OpenItem(arg),(int)player.position.X, (int)player.position.Y, player.width, player.height, ItemID.ScarabBomb, bombAmount);
					if (Main.netMode == 1)
					{
						NetMessage.SendData(21, -1, -1, null, itemWhoAmI2, 1f);
					}
				}
				if (Main.rand.Next(2) == 0)
				{
					int fossilAmount = Main.rand.Next(10, 17);
					int itemWhoAmI3 = Item.NewItem(player.GetSource_OpenItem(arg),(int)player.position.X, (int)player.position.Y, player.width, player.height, ItemID.FossilOre, fossilAmount);
					if (Main.netMode == 1)
					{
						NetMessage.SendData(21, -1, -1, null, itemWhoAmI3, 1f);
					}
				}
				FishingCrateLoot(player, arg);
				return false;
			}
			if (context == "obsidianLockBox")
			{
				int index = Main.rand.Next(ChestLoot.ShadowItems.Length);
				int number = Item.NewItem(player.GetSource_OpenItem(arg),(int)player.position.X, (int)player.position.Y, player.width, player.height, ChestLoot.ShadowItems[index], 1, noBroadcast: false, -1);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(21, -1, -1, null, number, 1f);
				}
				return false;
			}
			return true;
		}
		void FishingCrateLoot(Player player, int crateItemID)
        {
			if (Main.rand.Next(4) == 0)
			{
				int num28 = Main.rand.Next(6);
				switch (num28)
				{
					case 0:
						num28 = 288;
						break;
					case 1:
						num28 = 296;
						break;
					case 2:
						num28 = 304;
						break;
					case 3:
						num28 = 305;
						break;
					case 4:
						num28 = 2322;
						break;
					case 5:
						num28 = 2323;
						break;
				}
				int stack35 = Main.rand.Next(2, 5);
				int number55 = Item.NewItem(player.GetSource_OpenItem(num28),(int)player.position.X, (int)player.position.Y, player.width, player.height, num28, stack35);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(21, -1, -1, null, number55, 1f);
				}
			}
			if (Main.rand.Next(2) == 0)
			{
				int type28 = Main.rand.Next(188, 190);
				int stack36 = Main.rand.Next(5, 18);
				int number56 = Item.NewItem(player.GetSource_OpenItem(type28),(int)player.position.X, (int)player.position.Y, player.width, player.height, type28, stack36);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(21, -1, -1, null, number56, 1f);
				}
			}
			if (Main.rand.Next(2) == 0)
			{
				int type29 = ((Main.rand.Next(2) != 0) ? 2675 : 2676);
				int stack37 = Main.rand.Next(2, 7);
				int number57 = Item.NewItem(player.GetSource_OpenItem(type29),(int)player.position.X, (int)player.position.Y, player.width, player.height, type29, stack37);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(21, -1, -1, null, number57, 1f);
				}
			}
			bool hardmode = ItemID.Sets.IsFishingCrateHardmode[crateItemID];
			if (!hardmode)
			{
				return;
			}
			if (Main.rand.Next(2) == 0)
			{
				int type37 = 521;
				if (crateItemID == 3986)
				{
					type37 = 520;
				}
				int stack40 = Main.rand.Next(2, 6);
				int number65 = Item.NewItem(player.GetSource_OpenItem(type37),(int)player.position.X, (int)player.position.Y, player.width, player.height, type37, stack40);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(21, -1, -1, null, number65, 1f);
				}
			}
			if (Main.rand.Next(2) == 0)
			{
				int type38 = 522;
				int stack41 = Main.rand.Next(2, 6);
				switch (crateItemID)
				{
					case 3983:
						type38 = 1332;
						break;
					case 3986:
						type38 = 502;
						stack41 = Main.rand.Next(4, 11);
						break;
				}
				int number66 = Item.NewItem(player.GetSource_OpenItem(type38),(int)player.position.X, (int)player.position.Y, player.width, player.height, type38, stack41);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(21, -1, -1, null, number66, 1f);
				}
			}
		}
		*/
    }
}
