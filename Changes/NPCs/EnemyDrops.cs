using Terraria;
using TRAEProject.NewContent.Items.Weapons.Summoner.AbsoluteZero;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.ItemDropRules;
using TRAEProject.NewContent.Items.Weapons.Ranged.Jungla;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.Items.Accesories.ShadowflameCharm;
using TRAEProject.NewContent.Items.Accesories.MechanicalEye;
using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;
using TRAEProject.NewContent.Items.Weapons.Melee.HeadHarvester;
using TRAEProject.NewContent.Items.Weapons.Magic.SharpLament;
using TRAEProject.NewContent.Items.Accesories.MobilityJumps;
using TRAEProject.NewContent.NPCs.Underworld;

namespace TRAEProject.Changes.NPCs
{
    public class EnemyDrops: GlobalNPC
    {
        public static readonly int[] MimicDrops = new int[] { ItemID.CrossNecklace, ItemID.PhilosophersStone, ItemID.TitanGlove, ItemID.StarCloak, ItemID.DualHook};
       
        public static readonly int[] PirateDrops = new int[] { ItemID.LuckyCoin, ItemID.GoldRing, ItemID.DiscountCard, ItemID.PirateStaff };
      
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            npcLoot.RemoveWhere(rule =>
            {
                if (rule is not ItemDropWithConditionRule drop) // Type of drop you expect here
                    return false;
                return drop.itemId == ItemID.Yelets; 
            });
            npcLoot.RemoveWhere(rule =>
            {
                if (rule is not ItemDropWithConditionRule drop) // Type of drop you expect here
                    return false;
                return drop.itemId == ItemID.HelFire; 
            });
            
            switch (npc.type)
            {

                case NPCID.DD2DarkMageT1:
                case NPCID.DD2DarkMageT3:
                    npcLoot.Add(ItemDropRule.OneFromOptions(6, ItemID.MonkBelt, ItemID.HuntressBuckler, ItemID.SquireShield, ItemID.ApprenticeScarf));
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.SquireShield; // compare more fields if needed
                    });
                    npcLoot.Remove(ItemDropRule.Common(ItemID.SquireShield)); 
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.ApprenticeScarf; // compare more fields if needed
                    });
                    npcLoot.Remove(ItemDropRule.Common(ItemID.ApprenticeScarf));
                    break;
                case NPCID.DD2OgreT2:
                case NPCID.DD2OgreT3:
                    npcLoot.Add(ItemDropRule.OneFromOptions(6, ItemID.MonkBelt, ItemID.HuntressBuckler, ItemID.SquireShield, ItemID.ApprenticeScarf));
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.MonkBelt; // compare more fields if needed
                    });
                    npcLoot.Remove(ItemDropRule.Common(ItemID.MonkBelt));
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.HuntressBuckler; // compare more fields if needed
                    });
                    npcLoot.Remove(ItemDropRule.Common(ItemID.HuntressBuckler));
                    break;
                case NPCID.RedDevil:
                    npcLoot.Add(ItemDropRule.Common(ItemID.GuideVoodooDoll, 80));
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.FireFeather; // compare more fields if needed
                    });
                    npcLoot.Remove(ItemDropRule.Common(ItemID.FireFeather));
                    break;
                case NPCID.SkeletonSniper:
                     npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.RifleScope; // compare more fields if needed
                    });
                    npcLoot.Remove(ItemDropRule.Common(ItemID.RifleScope));
                    break;

           
                case NPCID.ArmoredViking:
                case NPCID.IceTortoise:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.IceSickle; // compare more fields if needed
                    });
                    break;
                case NPCID.IcyMerman:
                case NPCID.IceElemental:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.FrostStaff; // compare more fields if needed
                    }); npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.IceSickle; // compare more fields if needed
                    });
                    break;
                case NPCID.GreekSkeleton:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.Javelin; // compare more fields if needed
                    });
                    npcLoot.Add(ItemDropRule.Common(ItemID.Javelin, 25));
                    break;
                case NPCID.Clown:
                    npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(4, ItemID.WhoopieCushion));
                    return;
                case NPCID.SkeletonCommando:
                    npcLoot.Add(ItemDropRule.Common(ItemType<MechanicalEyeItem>(), 33));
                    return;
                case NPCID.MartianDrone:
                    npcLoot.Add(ItemDropRule.Common(ItemType<SpaceBalloonItem>(), 25));
                    return;
                case NPCID.DesertDjinn:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.DjinnLamp; // compare more fields if needed
                    });
                    npcLoot.Add(ItemDropRule.Common(ItemID.SpiritFlame, 40));
                    break;
                case NPCID.DesertScorpionWall:
                case NPCID.DesertScorpionWalk:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Gatligator, 30));
                    break;
                case NPCID.DuneSplicerHead:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Gatligator, 20));
                    break;
                case NPCID.DesertLamiaDark:
                case NPCID.DesertLamiaLight:
                    npcLoot.Add(ItemDropRule.Common(ItemID.DjinnLamp, 80));

                    npcLoot.Add(ItemDropRule.Common(ItemID.AncientCloth, 8));
                    break;
                case NPCID.DesertGhoul:
                case NPCID.DesertGhoulHallow:
                case NPCID.DesertGhoulCorruption:
                case NPCID.DesertGhoulCrimson:
                    npcLoot.Add(ItemDropRule.Common(ItemID.DjinnLamp, 80));
                    break;

                case NPCID.Tim:
                    npcLoot.Add(ItemDropRule.Common(ItemID.BookofSkulls, 2));
                    break;
                case NPCID.JungleCreeper:
                case NPCID.JungleCreeperWall:
                    npcLoot.Add(ItemDropRule.Common(ItemID.PoisonStaff, 33));
                    break;
                case NPCID.BlackRecluse:
                case NPCID.BlackRecluseWall:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.PoisonStaff; // compare more fields if needed
                    });
                    npcLoot.Remove(ItemDropRule.Common(ItemID.PoisonStaff));
                    break;
                case NPCID.Scutlix:
                    npcLoot.Add(ItemDropRule.Common(ItemID.BrainScrambler, 30));
                    npcLoot.Add(ItemDropRule.Common(ItemID.BrainScrambler, 30));
                    break;
        
                case NPCID.PirateShip:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.LuckyCoin; // compare more fields if needed
                    }); npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.GoldRing; // compare more fields if needed
                    }); npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.DiscountCard; // compare more fields if needed
                    }); npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.PirateStaff; // compare more fields if needed
                    });
                    npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, PirateDrops));
                    break;

                case NPCID.GoblinSummoner:
                    npcLoot.Add(ItemDropRule.Common(ItemType<ShadowflameCharmItem>(), 5));
                    break;
                case NPCID.DesertBeast:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.AncientHorn; // compare more fields if needed
                    });
                    npcLoot.Add(ItemDropRule.Common(ItemID.AncientHorn, 25));
                    break;
                case NPCID.AngryTrapper:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.Uzi; // compare more fields if needed
                    });                  
                    break;
                case NPCID.CultistBoss:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) 
                        {
                            return false;
                        }
                        return drop.itemId == ItemID.LunarCraftingStation;
                    });
                    LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                    notExpert.OnSuccess(ItemDropRule.Common(ItemID.LunarCraftingStation, 1));
                    npcLoot.Add(notExpert);
                    npcLoot.Add(ItemDropRule.BossBag(ItemID.CultistBossBag));
                    break;
                case NPCID.HeadlessHorseman:
                    npcLoot.Add(ItemDropRule.Common(ItemID.TheHorsemansBlade, 8));
                    break;
                case NPCID.DrManFly:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LevitationJuice>(), 20));
                    break;
                case NPCID.EyeofCthulhu:
                    LeadingConditionRule notExpertRule0 = new LeadingConditionRule(new Conditions.NotExpert());
                    LeadingConditionRule crimson = new LeadingConditionRule(new Conditions.IsCrimson());
                    crimson.OnSuccess(ItemDropRule.Common(ItemType<BloodyArrow>(), 1, 20, 50));
                    notExpertRule0.OnSuccess(crimson);
                    npcLoot.Add(notExpertRule0);
                    break;

                case NPCID.MourningWood:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is LeadingConditionRule lead) // Type of drop you expect here
                        {
                            return true;
                        }
                        return false; // compare more fields if needed
                    });
                    IItemDropRule rule = new LeadingConditionRule(new Conditions.PumpkinMoonDropGatingChance());
                    IItemDropRule itemDropRule = ItemDropRule.Common(ItemID.StakeLauncher);
                    itemDropRule.OnSuccess(ItemDropRule.Common(ItemID.Stake, 1, 30, 60), hideLootReport: true);
                    rule.OnSuccess(new OneFromRulesRule(1, ItemDropRule.Common(ItemID.SpookyHook), ItemDropRule.Common(ItemID.SpookyTwig), itemDropRule, ItemDropRule.Common(ItemID.CursedSapling), ItemDropRule.Common(ItemID.NecromanticScroll), ItemDropRule.Common(ItemType<SharpLament>())));
                    rule.OnSuccess(ItemDropRule.Common(ItemID.MourningWoodTrophy, 4));
                    rule.OnSuccess(ItemDropRule.Common(ItemID.WitchBroom, 10));
                    rule.OnSuccess(ItemDropRule.MasterModeCommonDrop(ItemID.MourningWoodMasterTrophy));
                    rule.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(ItemID.SpookyWoodMountItem, 4));
                    npcLoot.Add(rule);
                break;
                case NPCID.Pumpking:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is LeadingConditionRule lead) // Type of drop you expect here
                        {
                            return true;
                        }
                        return false; // compare more fields if needed
                    });
                    IItemDropRule itemDropRule2 = ItemDropRule.Common(ItemID.CandyCornRifle);
                    itemDropRule2.OnSuccess(ItemDropRule.Common(ItemID.CandyCorn, 1, 50, 100), hideLootReport: true);
                    IItemDropRule itemDropRule3 = ItemDropRule.Common(ItemID.JackOLanternLauncher);
                    itemDropRule3.OnSuccess(ItemDropRule.Common(ItemID.ExplosiveJackOLantern, 1, 25, 50), hideLootReport: true);
                    IItemDropRule rule2 = new LeadingConditionRule(new Conditions.PumpkinMoonDropGatingChance());
                    rule2.OnSuccess(new OneFromRulesRule(1, itemDropRule2, itemDropRule3, ItemDropRule.Common(ItemID.BlackFairyDust), ItemDropRule.Common(ItemID.BatScepter), ItemDropRule.Common(ItemID.RavenStaff), ItemDropRule.Common(ItemID.ScytheWhip), ItemDropRule.Common(ItemID.SpiderEgg), ItemDropRule.Common(ItemType<HeadHarvester>())));
                    rule2.OnSuccess(ItemDropRule.Common(ItemID.PumpkingTrophy, 4));
                    rule2.OnSuccess(ItemDropRule.MasterModeCommonDrop(ItemID.PumpkingMasterTrophy));
                    rule2.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(ItemID.PumpkingPetItem, 4));
                    npcLoot.Add(rule2);
                break;
                case NPCID.IceQueen:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is LeadingConditionRule lead) // Type of drop you expect here
                        {
                            return true;
                        }
                        return false; // compare more fields if needed
                    });
                    IItemDropRule ruleC = new LeadingConditionRule(new Conditions.FrostMoonDropGatingChance());
                    ruleC.OnSuccess(ItemDropRule.Common(ItemID.IceQueenTrophy, 4));
                    ruleC.OnSuccess(ItemDropRule.Common(ItemID.ReindeerBells, 15));
                    ruleC.OnSuccess(ItemDropRule.Common(ItemType<IceQueenJewel>(), 1));
                    ruleC.OnSuccess(ItemDropRule.Common(ItemID.BabyGrinchMischiefWhistle, 15)).OnFailedRoll(ItemDropRule.OneFromOptions(1, ItemID.NorthPole, ItemID.SnowmanCannon, ItemID.BlizzardStaff, ItemType<AbsoluteZero>()));
                    ruleC.OnSuccess(ItemDropRule.MasterModeCommonDrop(ItemID.IceQueenMasterTrophy));
                    ruleC.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(ItemID.IceQueenPetItem, 4));
                    npcLoot.Add(ruleC);
                    break;
                    case NPCID.SantaNK1:
                    IItemDropRule ruleD = new LeadingConditionRule(new Conditions.FrostMoonDropGatingChance());
                    ruleD.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Booster>(), 4));
                    npcLoot.Add(ruleD);
                    break;
                case NPCID.IceMimic:
                    if (!Main.remixWorld)
                        npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, MimicDrops));
                    break;
                case NPCID.Mimic:
                    npcLoot.RemoveWhere(rule => true);
                    if (!Main.remixWorld)

                        npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, MimicDrops));
                    break;
                case NPCID.BigMimicHallow:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if(rule is OneFromOptionsDropRule)
                        {
                            return true;
                        }
                        return false;
                    });
                    npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemID.FlyingKnife, ItemID.DaedalusStormbow, ItemID.CrystalVileShard, ItemID.Smolstar));
                    npcLoot.Add(ItemDropRule.Common(ItemID.IlluminantHook, 4));
                    break;
                case NPCID.BigMimicCrimson:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if(rule is OneFromOptionsDropRule)
                        {
                            return true;
                        }
                        return false;
                    });
                    npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemID.FleshKnuckles, ItemID.FetidBaghnakhs, ItemID.DartPistol, ItemID.SoulDrain));
                    npcLoot.Add(ItemDropRule.Common(ItemID.TendonHook, 4));
                    break;
                case NPCID.BigMimicCorruption:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if(rule is OneFromOptionsDropRule)
                        {
                            return true;
                        }
                        return false;
                    });
                    npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemID.PutridScent, ItemID.DartRifle, ItemID.ChainGuillotines, ItemID.ClingerStaff));
                    npcLoot.Add(ItemDropRule.Common(ItemID.WormHook, 4));
                    break;
                case NPCID.HallowBoss:
                    //this remove both condition rules so we'll have to add terraprisma back too, I don't know how to filter to specific leading condition
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is LeadingConditionRule) 
						{
                            return true;
						}
                        return false;
                    });
                    
                    LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
				    notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemID.RainbowCrystalStaff, ItemID.PiercingStarlight, ItemID.FairyQueenMagicItem, ItemID.FairyQueenRangedItem, ItemID.RainbowWhip));
                    notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<FaeInABottle>(), 5));
                    notExpertRule.OnSuccess(ItemDropRule.Common(4823, 15));
                    notExpertRule.OnSuccess(ItemDropRule.Common(4778, 4));
                    notExpertRule.OnSuccess(ItemDropRule.Common(4715, 50));
                    notExpertRule.OnSuccess(ItemDropRule.Common(4784, 7));
                    notExpertRule.OnSuccess(ItemDropRule.Common(5075, 20));
                    npcLoot.Add(notExpertRule);
                    LeadingConditionRule entry = new LeadingConditionRule(new Conditions.EmpressOfLightIsGenuinelyEnraged());
                    entry.OnSuccess(ItemDropRule.Common(5005));
                    npcLoot.Add(entry);
                    break;
                case NPCID.Plantera:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is LeadingConditionRule) 
						{
                            return true;
						}
                        return false;
                    });
                    LeadingConditionRule notExpertRule2 = new LeadingConditionRule(new Conditions.NotExpert());
					IItemDropRule melee = ItemDropRule.Common(ItemID.Seedler);
					melee.OnSuccess(ItemDropRule.Common(ItemID.FlowerPow));
					IItemDropRule ranged = ItemDropRule.Common(ItemID.VenusMagnum);
					ranged.OnSuccess(ItemDropRule.Common(ItemType<Jungla>()));
					IItemDropRule magic = ItemDropRule.Common(ItemID.NettleBurst);
					magic.OnSuccess(ItemDropRule.Common(ItemID.LeafBlower));
					notExpertRule2.OnSuccess(new OneFromRulesRule(1, melee, ranged, magic));
                    notExpertRule2.OnSuccess(ItemDropRule.Common(2109, 7));
                    notExpertRule2.OnSuccess(ItemDropRule.Common(1141));
                    notExpertRule2.OnSuccess(ItemDropRule.Common(1182, 20));
                    notExpertRule2.OnSuccess(ItemDropRule.Common(1305, 50));
                    notExpertRule2.OnSuccess(ItemDropRule.Common(1157, 4));
                    notExpertRule2.OnSuccess(ItemDropRule.Common(3021, 10));
                    npcLoot.Add(notExpertRule2);
                break;
                case NPCID.MoonLordCore:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is LeadingConditionRule) 
						{
                            return true;
						}
                        return false;
                    });
                    LeadingConditionRule notExpertRule3 = new LeadingConditionRule(new Conditions.NotExpert());
					notExpertRule3.OnSuccess(ItemDropRule.FewFromOptionsNotScalingWithLuck(2, 1, ItemID.Meowmere, ItemID.Terrarian, ItemID.SDMG, ItemID.Celeb2, ItemID.LunarFlareBook, ItemID.LastPrism, /*ItemID.RainbowWhip,*/ ItemID.StardustDragonStaff));
                    notExpertRule3.OnSuccess(ItemDropRule.Common(3373, 7));
                    notExpertRule3.OnSuccess(ItemDropRule.Common(4469, 10));
                    notExpertRule3.OnSuccess(ItemDropRule.Common(3384));
                    notExpertRule3.OnSuccess(ItemDropRule.Common(3460, 1, 70, 90));
                    npcLoot.Add(notExpertRule3);
                break;
            }
        }
        public override bool PreKill(NPC npc)
        {
            NPCLoader.blockLoot.Add(ItemID.TrifoldMap);
            NPCLoader.blockLoot.Add(ItemID.FastClock);
            NPCLoader.blockLoot.Add(ItemID.Yelets);
            NPCLoader.blockLoot.Add(ItemID.HelFire);
            if (Main.remixWorld && npc.type == NPCID.IceMimic)
            {
                if (!Main.hardMode)
                {
                    NPCLoader.blockLoot.Add(ItemID.Frostbrand);
                    //NPCLoader.blockLoot.Add(ItemID.IceBow); ICE MIMIC IS SUPPOSED TO DROP THIS
                    NPCLoader.blockLoot.Add(ItemID.TitanGlove);
                    NPCLoader.blockLoot.Add(ItemID.PhilosophersStone);
                    NPCLoader.blockLoot.Add(ItemID.StarCloak);
                    NPCLoader.blockLoot.Add(ItemID.DualHook);

                    NPCLoader.blockLoot.Add(ItemID.CrossNecklace);


                    NPCLoader.blockLoot.Add(ItemID.FlowerofFrost);
                }
                if (Main.hardMode && Main.rand.NextBool(5))
                {
                    Item.NewItem(npc.GetSource_Loot(), npc.Center, ItemID.SnowballCannon);

                }

            }
            if (Main.remixWorld && Main.hardMode && npc.type == NPCID.Mimic && Main.rand.NextBool(5))
            {
                Item.NewItem(npc.GetSource_Loot(), npc.Center, ItemID.WandofSparking);

            }
            if (!NPC.downedPlantBoss && npc.type == NPCID.Princess)
            {
                NPCLoader.blockLoot.Add(ItemID.PrincessWeapon);

            }
            switch (npc.type)
            {
                case 657: // queen slime
                    NPCLoader.blockLoot.Add(ItemID.Smolstar);
                    return true;
            }


            return true;
        }
    }
}