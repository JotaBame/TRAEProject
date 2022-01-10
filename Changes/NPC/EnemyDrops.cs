using Terraria;
using TRAEProject.Items.Summoner.AbsoluteZero;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.ItemDropRules;
using TRAEProject.Items.Materials;
using TRAEProject.Items.Accesories.ShadowflameCharm;

namespace TRAEProject
{
    public class EnemyDrops: GlobalNPC
    {
        public static readonly int[] MimicDrops = new int[] { ItemID.CrossNecklace, ItemID.PhilosophersStone, ItemID.TitanGlove, ItemID.StarCloak, ItemID.MagicDagger};
       
        public static readonly int[] PirateDrops = new int[] { ItemID.LuckyCoin, ItemID.GoldRing, ItemID.DiscountCard, ItemID.PirateStaff };
      
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.IcyMerman:
                case NPCID.IceElemental:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.FrostStaff; // compare more fields if needed
                    });
                    return;
                case NPCID.GreekSkeleton:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.Javelin; // compare more fields if needed
                    });
                    npcLoot.Add(ItemDropRule.Common(ItemID.Javelin, 25));
                    return;
                case NPCID.IceQueen:
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemType<IceQueenJewel>()));
                        return;
                    }
                case NPCID.Clown:
                    {
                        npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(4, ItemID.WhoopieCushion));
                        npcLoot.Add(ItemDropRule.Common(ItemID.Bananarang, 1));
                        return;
                    }
                case NPCID.DesertLamiaDark:
                case NPCID.DesertLamiaLight:
                    npcLoot.Add(ItemDropRule.Common(ItemID.AncientCloth, 8));
                    return;
                case NPCID.Tim:
                    npcLoot.Add(ItemDropRule.Common(ItemID.BookofSkulls, 2));
                    return;
                case NPCID.JungleCreeper:
                case NPCID.JungleCreeperWall:
                    npcLoot.Add(ItemDropRule.Common(ItemID.PoisonStaff, 50));
                    return;
                case NPCID.BlackRecluse:
                case NPCID.BlackRecluseWall:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.PoisonStaff; // compare more fields if needed
                    });
                    npcLoot.Remove(ItemDropRule.Common(ItemID.PoisonStaff));
                    return;
                case NPCID.ScutlixRider:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.BrainScrambler; // compare more fields if needed
                    });
                    npcLoot.Add(ItemDropRule.Common(ItemID.BrainScrambler, 30));
                    return;
                case NPCID.BrainScrambler:
                    npcLoot.Add(ItemDropRule.Common(ItemID.BrainScrambler, 30));
                    return;
                case NPCID.IceMimic:
                    npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, MimicDrops));
                    return;
                case NPCID.PirateShip:
                    npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, PirateDrops));
                    return;
                case NPCID.IceTortoise:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.FrozenTurtleShell; // compare more fields if needed
                    });
                    npcLoot.Add(ItemDropRule.Common(ItemID.FrozenTurtleShell, 50));
                    return;
                case NPCID.GoblinSummoner:
                    npcLoot.Add(ItemDropRule.Common(ItemType<ShadowflameCharmItem>(), 5));
                    return;
                case NPCID.DesertBeast:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.AncientHorn; // compare more fields if needed
                    });
                    npcLoot.Add(ItemDropRule.Common(ItemID.AncientHorn, 25));
                    return;
                case NPCID.AngryTrapper:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.Uzi; // compare more fields if needed
                    });                  
                    return;
            }
        }
        public override bool PreKill(NPC npc)
        {
            NPCLoader.blockLoot.Add(ItemID.TrifoldMap);
            NPCLoader.blockLoot.Add(ItemID.FastClock);
            switch (npc.type)
            {
                case NPCID.SkeletronHead:
                    NPCLoader.blockLoot.Add(ItemID.BookofSkulls);
                    return true;
                case NPCID.Medusa:
                    if (Main.rand.Next(20) == 0)
                    {
                        Item.NewItem(npc.getRect(), ItemID.PocketMirror, 1);
                    }
                    NPCLoader.blockLoot.Add(ItemID.PocketMirror);
                    return true;
                case NPCID.Plantera:
                    int[] PDrops = new int[] { ItemID.FlowerPow, ItemID.Seedler, ItemID.LeafBlower, ItemID.NettleBurst, ItemID.GrenadeLauncher, ItemID.VenusMagnum};
                    int Drop1 = Main.rand.Next(4);
                    // there is 100% a cleaner way of doing this
                      if (!Main.expertMode && !Main.masterMode)
                    {
					if (Drop1 == 0)
                    {
                        Item.NewItem(npc.getRect(), ItemID.FlowerPow, 1); 
                        Item.NewItem(npc.getRect(), ItemID.Seedler, 1);
                    }
                    if (Drop1 == 1)
                    {
                        Item.NewItem(npc.getRect(), ItemID.LeafBlower, 1);
                        Item.NewItem(npc.getRect(), ItemID.NettleBurst, 1); 
                    }
                    if (Drop1 == 2)
                    {
              Item.NewItem(npc.getRect(), ItemID.GrenadeLauncher, 1);			
			 Item.NewItem(npc.getRect(), ItemID.RocketI, 100); 
                        Item.NewItem(npc.getRect(), ItemID.VenusMagnum, 1);
                    }
                    if (Drop1 == 3)
                    {
                        Item.NewItem(npc.getRect(), ItemID.PygmyStaff, 1);
                        Item.NewItem(npc.getRect(), Main.rand.Next(PDrops), 1);
                    }
					}
                    NPCLoader.blockLoot.Add(ItemID.FlowerPow);
                    NPCLoader.blockLoot.Add(ItemID.Seedler);
                    NPCLoader.blockLoot.Add(ItemID.LeafBlower);             
					NPCLoader.blockLoot.Add(ItemID.WaspGun);
                    NPCLoader.blockLoot.Add(ItemID.NettleBurst); 
                    NPCLoader.blockLoot.Add(ItemID.GrenadeLauncher);                  
					NPCLoader.blockLoot.Add(ItemID.RocketI); 
                    NPCLoader.blockLoot.Add(ItemID.VenusMagnum); 
                    NPCLoader.blockLoot.Add(ItemID.PygmyStaff);
                    return true;
                case NPCID.MoonLordCore:
                    int[] MLDrops = new int[] { ItemID.Meowmere, ItemID.StarWrath, ItemID.Terrarian, ItemID.SDMG, ItemID.Celeb2, ItemID.LunarFlareBook, ItemID.LastPrism, ItemID.RainbowWhip, ItemID.StardustDragonStaff };
                    if (!Main.expertMode && !Main.masterMode)
                    {
                        Item.NewItem(npc.getRect(), Main.rand.Next(MLDrops), 1);
                    }
                    NPCLoader.blockLoot.Add(ItemID.Meowmere);
                    NPCLoader.blockLoot.Add(ItemID.StarWrath);
                    NPCLoader.blockLoot.Add(ItemID.Terrarian);
                    NPCLoader.blockLoot.Add(ItemID.SDMG); 
                    NPCLoader.blockLoot.Add(ItemID.Celeb2); 
                    NPCLoader.blockLoot.Add(ItemID.LunarFlareBook); 
                    NPCLoader.blockLoot.Add(ItemID.LastPrism); 
                    NPCLoader.blockLoot.Add(ItemID.RainbowCrystalStaff); 
                    NPCLoader.blockLoot.Add(ItemID.MoonlordTurretStaff);
                    return true;
                case NPCID.HallowBoss:
                    int[] HallowDrops = new int[] { ItemID.PiercingStarlight, ItemID.FairyQueenMagicItem, ItemID.FairyQueenRangedItem, ItemID.RainbowCrystalStaff};
                    if (!Main.expertMode && !Main.masterMode)
                    {
                        Item.NewItem(npc.getRect(), Main.rand.Next(HallowDrops), 1);
                    }
                    NPCLoader.blockLoot.Add(ItemID.PiercingStarlight);
                    NPCLoader.blockLoot.Add(ItemID.FairyQueenMagicItem);
                    NPCLoader.blockLoot.Add(ItemID.FairyQueenRangedItem);
                    NPCLoader.blockLoot.Add(ItemID.RainbowWhip);
                    return true;
                case NPCID.IceQueen:
                    int[] IceQueenDrops = new int[] { ItemID.SnowmanCannon, ItemID.BlizzardStaff, ItemID.NorthPole, ItemType<AbsoluteZero>() };
                    Item.NewItem(npc.getRect(), Main.rand.Next(IceQueenDrops), 1);
                    NPCLoader.blockLoot.Add(ItemID.SnowmanCannon);
                    NPCLoader.blockLoot.Add(ItemID.NorthPole);
                    NPCLoader.blockLoot.Add(ItemID.BlizzardStaff);
                    return true;
                case 657: // queen slime
                    NPCLoader.blockLoot.Add(ItemID.Smolstar);
                    return true;
				case NPCID.BigMimicHallow:
                    int[] HDrops = new int[] { ItemID.FlyingKnife, ItemID.DaedalusStormbow, ItemID.CrystalVileShard, ItemID.Smolstar };
                    int Ihook = Main.rand.Next(4); 
                    if (Ihook == 0)
                    {
                        Item.NewItem(npc.getRect(), ItemID.IlluminantHook, 1);
                    }
                    Item.NewItem(npc.getRect(), Main.rand.Next(HDrops), 1);
                    NPCLoader.blockLoot.Add(ItemID.FlyingKnife);
                    NPCLoader.blockLoot.Add(ItemID.CrystalVileShard);
                    NPCLoader.blockLoot.Add(ItemID.DaedalusStormbow);
                    NPCLoader.blockLoot.Add(ItemID.IlluminantHook);
                    return true;
					
                case NPCID.BigMimicCrimson:
                    int[] CRDrops = new int[] { ItemID.FetidBaghnakhs, ItemID.SoulDrain, ItemID.DartPistol, ItemID.FleshKnuckles };
                    int Thook = Main.rand.Next(4);
                    if (Thook == 0)
                    {
                        Item.NewItem(npc.getRect(), ItemID.TendonHook, 1);
                    }
                    Item.NewItem(npc.getRect(), Main.rand.Next(CRDrops), 1);
                    NPCLoader.blockLoot.Add(ItemID.FetidBaghnakhs);
                    NPCLoader.blockLoot.Add(ItemID.SoulDrain);
                    NPCLoader.blockLoot.Add(ItemID.DartPistol);
                    NPCLoader.blockLoot.Add(ItemID.FleshKnuckles);
                    NPCLoader.blockLoot.Add(ItemID.TendonHook);
                    return true;
                case NPCID.BigMimicCorruption:
                    int[] CDrops = new int[] { ItemID.ChainGuillotines, ItemID.DartRifle, ItemID.ClingerStaff, ItemID.PutridScent };
                    int Whook = Main.rand.Next(4); 
                    if (Whook == 0)
                    {
                        Item.NewItem(npc.getRect(), ItemID.WormHook, 1);
                    }
                    Item.NewItem(npc.getRect(), Main.rand.Next(CDrops), 1);
                    NPCLoader.blockLoot.Add(ItemID.ChainGuillotines);
                    NPCLoader.blockLoot.Add(ItemID.ClingerStaff);
                    NPCLoader.blockLoot.Add(ItemID.DartRifle);
                    NPCLoader.blockLoot.Add(ItemID.PutridScent);
                    NPCLoader.blockLoot.Add(ItemID.WormHook);
                    return true;
            }


            return true;
        }
    }
}