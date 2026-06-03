using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.Items.Weapons.Ranged.Jungla;

using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;
using TRAEProject.NewContent.Items.Armor.Joter;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Accesories.MobilityJumps;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;
using TRAEProject.NewContent.Items.Accesories.AdvFlight;
using System.Linq;
using System;

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

			switch (item.type)
            {
				case ItemID.KingSlimeBossBag:
					itemLoot.Add(ItemDropRule.Common(ItemID.Katana, 4));
                    break;
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
						for (int i = 0; i < drop.dropIds.Length; i++)
						{
							if (drop.dropIds[i] == ItemID.FairyQueenMagicItem)
							{
								return true;
							}

						}
						return false;
					});
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FaeInABottle>(), 5));
					itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemID.RainbowCrystalStaff, ItemID.PiercingStarlight, ItemID.FairyQueenMagicItem, ItemID.FairyQueenRangedItem, ItemID.RainbowWhip));
					break;
				case ItemID.MoonLordBossBag:
					itemLoot.RemoveWhere(rule =>
					{
						if (rule is FromOptionsWithoutRepeatsDropRule) // Type of drop you expect here
						{
							return true;
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
					itemLoot.Add(ItemDropRule.FewFromOptionsNotScalingWithLuck(2, 1, ItemID.Meowmere, ItemID.Terrarian, ItemID.SDMG, ItemID.Celeb2, ItemID.LunarFlareBook, ItemID.LastPrism, /*ItemID.RainbowWhip,*/ ItemID.StardustDragonStaff));
					break;
				case ItemID.ObsidianLockbox:
        

                    itemLoot.RemoveWhere(rule =>
                    {
                        return true;
                    });
                    int[] newArray = ChestLoot.ShadowItems.Concat(new int[] { ItemID.MoonStone, ItemType<AdvFlightSystem>(), ItemID.GravityGlobe }).ToArray();
                    itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, newArray));
                    itemLoot.Add(ItemDropRule.Common(ItemID.TreasureMagnet, 4));
                    break;
				case ItemID.LockBox:
				itemLoot.RemoveWhere(rule =>
				{
					return true;
				});
				itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ChestLoot.DungeonItems));
  				break;
	 
				case ItemID.OasisCrate:
				case ItemID.OasisCrateHard:

                    List<IItemDropRule> dropTable = itemLoot.Get();
                    List<IItemDropRule>.Enumerator layerOne = dropTable.GetEnumerator();
                    for (int i = 0; i < dropTable.Count; i++)
                    {
                        layerOne.MoveNext();
                        if (layerOne.Current is OneFromOptionsNotScaledWithLuckDropRule)
                        {
                            OneFromOptionsNotScaledWithLuckDropRule layerTwo = layerOne.Current as OneFromOptionsNotScaledWithLuckDropRule;
                            if (Array.Exists<int>(layerTwo.dropIds, id => id == 4262))
                            {
                                dropTable.RemoveAt(i);
								dropTable.Insert(i, ItemDropRule.OneFromOptionsNotScalingWithLuckWithX(layerTwo.chanceDenominator, layerTwo.chanceNumerator, Array.FindAll<int>(layerTwo.dropIds, id => id != 4262)));
                            };
                              
                        }
                    }
                    break;
                case ItemID.FloatingIslandFishingCrate:
                case ItemID.FloatingIslandFishingCrateHard:
                    //itemLoot.Add(ItemDropRule.AlwaysAtleastOneSuccess(oasis));

                    break;
				case ItemID.HallowedFishingCrateHard:
                    {
  
                    }
					break;
            }
 
            base.ModifyItemLoot(item, itemLoot);
        }
		public static readonly int[] ShadowChestLoot = new int[] { ItemID.HellwingBow, ItemID.Flamelash, ItemID.FlowerofFire, ItemID.Sunfury };
		
    }
}
