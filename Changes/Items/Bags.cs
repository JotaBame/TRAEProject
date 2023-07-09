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
				case ItemID.LockBox:
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AdvFlightSystem>(), 5));
				break;
				case ItemID.IronCrate:
				case ItemID.IronCrateHard:
				itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(20, ItemID.BandofRegeneration, ItemID.CloudinaBottle, ItemID.HermesBoots, ItemID.ShoeSpikes, ItemID.FlareGun));
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
				IItemDropRule bc_scarab = ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemID.ThunderSpear, ItemID.ThunderStaff, ItemID.AncientChisel, ItemID.SandstorminaBottle, ItemID.AnkhCharm, ItemID.MagicConch, ItemID.SandBoots);
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
		
    }
}
