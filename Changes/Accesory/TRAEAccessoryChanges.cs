using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using TRAEProject;
using System.Collections.Generic;
using Terraria.Utilities;
using TRAEProject.NewContent.Buffs;
using TRAEProject.Changes;
using TRAEProject.NewContent.Items.Accesories.ShadowflameCharm;
using TRAEProject.Changes.Items;
using TRAEProject.Common.ModPlayers;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Accesories;
using TRAEProject.Common;

namespace TRAEProject.Changes.Accesory
{
    public class ChangesAccessories : GlobalItem
    {
        public static readonly int[] AnkhDebuffList = new int[] { BuffID.Bleeding, BuffID.Poisoned, BuffID.OnFire, BuffID.Venom,
        BuffID.Darkness, BuffID.Blackout, BuffID.Silenced, BuffID.Cursed,
        BuffID.Confused, BuffID.Slow, BuffID.OgreSpit, BuffID.Weak, BuffID.BrokenArmor,
        BuffID.CursedInferno,   BuffID.Frostburn,  BuffID.Chilled,  BuffID.Frozen,
        BuffID.Ichor,   BuffID.Stoned,  BuffID.VortexDebuff,  BuffID.Obstructed,
        BuffID.Electrified, BuffID.ShadowFlame, BuffID.WitheredWeapon, BuffID.WitheredArmor, BuffID.Dazed, BuffID.Burning}; //


        
        static void CelestialStoneStats(Player player)
        {
            player.skyStoneEffects = false;


            
            player.pickSpeed -= 0.1f;
            player.GetDamage<GenericDamageClass>() += 0.08f;
            player.GetCritChance<GenericDamageClass>() += 2;
            player.statDefense += 4;
            player.lifeRegen++;
            player.statManaMax2 += 20;
            player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
            player.GetModPlayer<RangedStats>().chanceNotToConsumeAmmo += 10;
            
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            switch (item.type)
            {
          

                case ItemID.FastClock:
                    Main.time += 4;
                     player.buffImmune[BuffID.Slow] = false;
                    break;
                case ItemID.Bezoar:
                    player.GetModPlayer<Bezoar>().bezoar = true;
                    player.buffImmune[BuffID.Poisoned] = false;
                    break;
                case ItemID.AdhesiveBandage:
                    player.GetModPlayer<BandAid>().Bandaid = true;
                    player.buffImmune[BuffID.Bleeding] = false;
                    break;
                case ItemID.MedicatedBandage:
                    player.GetModPlayer<Bezoar>().bezoar = true;
                    player.GetModPlayer<BandAid>().Bandaid = true;
                    player.buffImmune[BuffID.Poisoned] = false;
                    player.buffImmune[BuffID.Bleeding] = false;
                    break;
                case ItemID.Blindfold:
                    player.AddBuff(BuffID.Obstructed, 1);
                    player.buffImmune[BuffID.Darkness] = false;
                    break;
                case ItemID.Nazar:
                    player.GetModPlayer<NazarDebuffs>().Nazar += 1;
                    player.buffImmune[BuffID.Cursed] = false;
                    break;
                case ItemID.CountercurseMantra:
                    player.GetModPlayer<NazarDebuffs>().Nazar += 1;
                    foreach (int i in AnkhDebuffList)
                    {
                        player.buffImmune[i] = true;
                    }
                    break;
                case ItemID.AnkhShield:
				player.fireWalk = false;
                    foreach (int i in AnkhDebuffList)
                    {
                        player.buffImmune[i] = true;
                    }
                    break;
                case ItemID.AnkhCharm:
                    foreach (int i in AnkhDebuffList)
                    {
                        player.buffImmune[i] = true;
                    }
                    break;
                case ItemID.RoyalGel:
                    player.GetModPlayer<Defense>().RoyalGel = true;
                    break;

                case ItemID.MoltenCharm:
                     player.GetModPlayer<ShadowflameCharmPlayer>().MoltenCharm += 1;
                    player.fireWalk = false;
                    break;

                case ItemID.MechanicalGlove:
                case ItemID.FireGauntlet:

                    player.kbGlove = false;
                    player.meleeScaleGlove = false;
                    break;
                case ItemID.MoonShell:
                    if (player.statLife > player.statLifeMax2 * 0.67)
                    {
                        player.buffImmune[BuffID.Werewolf] = true;
                    }
                    player.GetModPlayer<AccesoryEffects>().wErewolf = true;
                    if (player.statLife < player.statLifeMax2 * 0.5)
                        player.AddBuff(BuffID.IceBarrier, 1, false);
                    player.wolfAcc = false;
                    break;
                case ItemID.MoonCharm:
                    player.GetModPlayer<AccesoryEffects>().wErewolf = true;
                    player.wolfAcc = false;
                    break;
                // CELESTIAL STONE CHANGES
                case ItemID.CelestialStone:
                    CelestialStoneStats(player);

                    
                    break;
                case ItemID.MoonStone:
                    player.skyStoneEffects = false;
                    {
                        if (player.statLife < player.statLifeMax2 * 0.5)
                        {
                            CelestialStoneStats(player);

                        }
                        else if (player.statLife < player.statLifeMax2 * 0.75)
                        {
                            player.pickSpeed -= 1.1f;
                            player.GetDamage<GenericDamageClass>() += 0.04f;
                            player.statDefense += 2;
                            player.lifeRegen++;

                        }
                    }
                    break;
                case ItemID.SunStone:             
                    player.skyStoneEffects = false;
                    {
                        if (player.statLife > player.statLifeMax2 * 0.75)
                        {
                            CelestialStoneStats(player);
                        }
                        else if (player.statLife > player.statLifeMax2 * 0.5)
                        {
                            player.pickSpeed -= 1.1f;
                            player.GetDamage<GenericDamageClass>() += 0.04f;
                            player.statDefense += 2;
                            player.lifeRegen++;

                        }
          
                       
                    }
                   
                    break;
                case ItemID.CelestialShell:
                    CelestialStoneStats(player);
                    player.wolfAcc = false;
                    break;

                case ItemID.DestroyerEmblem:
 
                    player.GetDamage<GenericDamageClass>() -= 0.1f;
                    player.GetCritChance<GenericDamageClass>() += 14;
                    break;
                case ItemID.HerculesBeetle:
                    ++player.maxTurrets;
                    player.GetDamage<SummonDamageClass>() -= 0.15f;
                    break;
                case ItemID.NecromanticScroll:
                    player.maxMinions -= 1;
                    player.GetModPlayer<SummonStats>().minionCritChance += 13;
                    player.GetDamage<SummonDamageClass>() -= 0.1f;
                    break;
                case ItemID.PapyrusScarab:
                     player.GetDamage<SummonDamageClass>() -= 0.15f;
                    player.maxTurrets += 1;

                    player.GetModPlayer<SummonStats>().minionCritChance += 5;


                    break;
                case ItemID.HeroShield:
                    player.hasPaladinShield = false;
                    break;
      
                case ItemID.SquireShield:
                    player.dd2Accessory = false;
                    player.GetDamage<SummonDamageClass>() -= 0.1f;

                    player.GetDamage<MeleeDamageClass>() += 0.07f;
                    if (player.setSquireT2)
                        player.GetDamage<MeleeDamageClass>() += 0.04f;
                    if (player.setSquireT3)
                        player.GetDamage<MeleeDamageClass>() += 0.04f;
                    ++player.maxTurrets;
                    break;
                case ItemID.ApprenticeScarf:
                    ++player.maxTurrets;
                    player.GetDamage<SummonDamageClass>() -= 0.1f;
                    player.GetDamage<MagicDamageClass>() += 0.07f;
                    if (player.setApprenticeT2)
                        player.GetDamage<MagicDamageClass>() += 0.04f;
                    if (player.setApprenticeT3)
                        player.GetDamage<MagicDamageClass>() += 0.04f;
                    player.dd2Accessory = false;
                    break;
                case ItemID.MonkBelt:
                    ++player.maxTurrets;
                    player.GetDamage<SummonDamageClass>() -= 0.03f;
                    if (player.setMonkT2)
                        player.GetDamage<SummonDamageClass>() += 0.04f;
                    if (player.setApprenticeT3)
                        player.GetDamage<SummonDamageClass>() += 0.04f;
                    player.dd2Accessory = false;
                    break;
                case ItemID.HuntressBuckler:
                    ++player.maxTurrets;
                    player.GetDamage<SummonDamageClass>() -= 0.1f;
                    player.GetDamage<RangedDamageClass>() += 0.07f;
                    if (player.setHuntressT2)
                        player.GetDamage<SummonDamageClass>() += 0.04f;
                    if (player.setHuntressT3)
                        player.GetDamage<SummonDamageClass>() += 0.04f;
                    player.dd2Accessory = false;
                    break;

                case ItemID.ReflectiveShades:
                    player.GetCritChance<GenericDamageClass>() += 8;
                    break;
                case ItemID.BoneHelm:
                    ++player.maxTurrets;
                    break;

            }
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.CountsAsClass(DamageClass.Ranged))
            {
                velocity *= player.GetModPlayer<RangedStats>().rangedVelocity; 
                if (item.useAmmo == AmmoID.Bullet || item.useAmmo == AmmoID.CandyCorn)
                {
                    velocity *= player.GetModPlayer<RangedStats>().gunVelocity;
                }
            }
          
        }
         public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string celStone = "8% increased damage\n2% increased critical strike chance\n5% increased melee speed\n10% increased mining speed and reduced ammo usage\nIncreases defense by 4\nIncreases mana by 20\nIncreases life regen by 0.5 per second";
            //string celStone = "4% increased damage, critical strike chance, movement speed, and jump speed\n8% increased melee speed\n10% incresed mining speed and reduced ammo usage\nincreases defense and armor penetration by 4\nincreases max life and mana by 20\nincreases life regen by 0.5hp/s";
             switch (item.type)
            {
                case ItemID.BoneHelm:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {

                        
                                line.Text += "\nIncreases your maximum number of sentries by 1";
 
                        }
                    }
                    break;
                case ItemID.SquireShield:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {

                            line.Text = "7% increased melee damage";
                            if (!TRAEWorld.downedOgre)
                            {
                                line.Text += "\n11% increased melee damage with a full set of Squire Armor";

                            }
                            if (!TRAEWorld.downedBetsy)
                            {
                                line.Text += "\n15% increased melee damage with a full set of Valhalla Knight Armor";

                            }
                        }
                    }
                    break;
                case ItemID.ApprenticeScarf:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "7% increased magic damage";
                            if (!TRAEWorld.downedOgre)
                            {
                                line.Text += "\n11% increased magic damage with a full set of Apprentice Armor";

                            }
                            if (!TRAEWorld.downedBetsy)
                            {
                                line.Text += "\n15% increased magic damage with a full set of Dark Artist Armor";

                            }
                        }
                    }
                    break;
                case ItemID.HuntressBuckler:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "7% increased ranged damage";
                            if (!TRAEWorld.downedOgre)
                            {
                                line.Text += "\n11% increased ranged damage with a full set of Huntress Armor";

                            }
                            if (!TRAEWorld.downedBetsy)
                            {
                                line.Text += "\n15% increased ranged damage with a full set of Red Riding Hood Armor";

                            }
                        }
                    }
                    break;
                case ItemID.MonkBelt:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "7% increased summon damage";
                            if (!TRAEWorld.downedOgre)
                            {
                                line.Text += "\n11% increased summon damage with a full set of Monk Armor";

                            }
                            if (!TRAEWorld.downedBetsy)
                            {
                                line.Text += "\n15% increased summon damage with a full set of Shinobi Infiltrator Armor";

                            }
                        }
                    }
                    break;
                case ItemID.Bezoar:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Significantly increases potency of friendly Poison and Venom";
                        }
                    }
                    break;
                case ItemID.AdhesiveBandage:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases potency of friendly debuffs by 50%";
                        }
                    }
                    break;
                case ItemID.MedicatedBandage:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Significantly increases potency of friendly Poison and Venom\nIncreases potency of friendly debuffs by 50%";
                        }
                    }
                    break;
                case ItemID.Blindfold:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "";
                        }
                    }
                    break;
                case ItemID.FastClock:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Time goes by faster when equipped";
                        }
                    }
                    break;
                case ItemID.Nazar:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Unleashes curses to the wielder and nearby enemies when damaged" +
                                "\nCurses either deal damage over time, reduce contact damage by 20% or defense by 25";
                        }
                    }
                    break;
                case ItemID.CountercurseMantra:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Provides immunity to a large number of debuffs\nUnleashes curses to nearby enemies when damaged";
                        }
                    }
                    break;

          
   
      
         
            
                case ItemID.AnkhShield:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Grants immunity to knockback and most debuffs";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                    break;
               
                case ItemID.MechanicalGlove:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "12% increased melee damage and speed";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                           line.Text = "Enables autoswing for melee weapons";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                        {
                            line.Text = "";
                        }
                    }
                    break;
                case ItemID.FireGauntlet:
                     foreach (TooltipLine line in tooltips)
                    {
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text = "12% increased melee damage and speed";
                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text = "Allows autoswing for all melee weapons";
                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                            {
                                line.Text = "Melee attacks deal fire damage";
                            }
                            if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                            {
                                line.Text = "Melee critical strikes have have a very low chance to incinerate, based on damage dealt";
                            }

                        }
                    }
                    break;

                case ItemID.PutridScent:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "4% increased damage and critical strike chance";
                        }
                    }
                    break;
                 
                case ItemID.DestroyerEmblem:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "22% increased critical strike chance";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                    break;
              
                case ItemID.HeroShield:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Grants immunity to knockback";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Enemies are more likely to target you";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                        {
                            line.Text = "";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                        {
                            line.Text = "";
                        }
                    }
                    break;
        
                case ItemID.MoltenCharm:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Minion damage is stored as Fire energy, up to 3000\nWhip strikes summon a friendly Molten Apparition for every 750 damage stored";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                    break;
                case ItemID.RoyalGel:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nReduces damage of one hit by 25 every 30 seconds";
                        }
                    }
                    break;
                case ItemID.MoonCharm:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Turns the holder into a werewolf when below 67% health";
                        }
                    }
                    break;
                case ItemID.MoonShell:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Turns the holder into a werewolf when below 67% life and into a merfolk when entering water\nPuts a shell around the owner when below 50% life";
                        }
                    }
                    break;
                case ItemID.SunStone:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases all stats as life increases";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Maxes out above 75% life:";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = celStone;
                        }

                    }
                    break;
                case ItemID.MoonStone:

                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases all stats as life goes down";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Maxes out below 50% life:";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = celStone;
                        }



                    }
                    break;
                case ItemID.CelestialStone:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = celStone;
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "";
                        }
                    }
                    break;
                case ItemID.CelestialShell:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Turns into holder into a merfolk when entering water";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = celStone;
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                        {
                            line.Text = "";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip4")
                        {
                            line.Text = "";
                        }
                    }
                    break;
                case ItemID.NecromanticScroll:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "13% increased critical strike chance for minions";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                    break;
                case ItemID.HerculesBeetle:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases your maximum number of sentries by 1";
                        }
                    }
                    break;
                case ItemID.PapyrusScarab:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases your max number of minions and sentries by 1";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "5% increased critical strike chance for minions";

                        }
                    }
                    break;



                case ItemID.ShinyStone:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nIncreases life regeneration by 2hp/s when on the ground";
                        }
                    }
                    return;
            }
        }
    }
 
}






