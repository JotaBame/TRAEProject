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
using TRAEProject.NewContent.Items.Accesories.EvilEye;
using TRAEProject.NewContent.Items.Accesories.ShadowflameCharm;
using TRAEProject.Changes.Items;
using TRAEProject.Common.ModPlayers;
using static Terraria.ModLoader.ModContent;


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

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            switch (item.type)
            {
                case ItemID.FeralClaws:
                case ItemID.PowerGlove:
                case ItemID.MechanicalGlove:
                case ItemID.BerserkerGlove:
                    player.GetModPlayer<MeleeStats>().TRAEAutoswing = true;
                    break;
            }
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
                    player.lavaImmune = true;
                    player.GetModPlayer<ShadowflameCharmPlayer>().MoltenCharm += 1;
                    player.fireWalk = false;
                    break;
                case ItemID.BeeCloak:
                    player.honeyCombItem = null;
                    player.starCloakItem = null;
                    player.GetModPlayer<OnHitItems>().NewbeesOnHit = true;
                    break;
                case ItemID.CrossNecklace:
                case ItemID.StarVeil:
                    player.starCloakItem = null;
                    player.GetModPlayer<OnHitItems>().NewstarsOnHit = true;
                    break;
                case ItemID.MechanicalGlove:
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
                    player.skyStoneEffects = false;
                    player.GetDamage<GenericDamageClass>() += 0.08f;
                    player.GetCritChance<GenericDamageClass>() += 2;
                    player.statDefense += 4;
                    player.lifeRegen++;
                    player.statManaMax2 += 20;
                    player.meleeSpeed += 0.05f;
                    player.moveSpeed += 0.1f;
                    player.GetModPlayer<RangedStats>().chanceNotToConsumeAmmo += 10;// new bonus
                    // total stats: +8% damage, +2% crit, +0.5 hp/s, +4 defense. +5% melee speed, +20 max mana, +10% movement speed
                    break;
                case ItemID.MoonStone:
                    if (!Main.dayTime)
                    {
                        player.skyStoneEffects = false;
                        player.GetDamage<GenericDamageClass>() += 0.08f;
                        player.GetCritChance<GenericDamageClass>() += 2;
                        player.statDefense += 4;
                        player.lifeRegen++;
                        player.statManaMax2 += 20;
                        player.meleeSpeed += 0.05f;
                        player.moveSpeed += 0.1f;
                        player.GetModPlayer<RangedStats>().chanceNotToConsumeAmmo += 10;
                    }
                    break;
                case ItemID.SunStone:
                    if (Main.dayTime)
                    {
                        player.skyStoneEffects = false;
                        player.GetDamage<GenericDamageClass>() += 0.08f;
                        player.GetCritChance<GenericDamageClass>() += 2;
                        player.statDefense += 4;
                        player.lifeRegen++;
                        player.statManaMax2 += 20;
                        player.meleeSpeed += 0.05f;
                        player.moveSpeed += 0.1f;
                        player.GetModPlayer<RangedStats>().chanceNotToConsumeAmmo += 10;
                    }
                    break;
                case ItemID.CelestialShell:
                    player.skyStoneEffects = false;
                    player.GetDamage<GenericDamageClass>() += 0.08f;
                    player.GetCritChance<GenericDamageClass>() += 2;
                    player.statDefense += 4;
                    player.lifeRegen++;
                    player.statManaMax2 += 20;
                    player.meleeSpeed += 0.05f;
                    player.moveSpeed += 0.1f;
                    player.GetModPlayer<RangedStats>().chanceNotToConsumeAmmo += 10;
                    player.wolfAcc = false;
                    break;
                case ItemID.BandofStarpower:
                    player.GetModPlayer<Mana>().manaRegenBoost += 0.1f;
                    player.statManaMax2 -= 20;
                    break;
                case ItemID.ManaRegenerationBand:
                    player.statManaMax2 -= 20;
                    player.GetModPlayer<Mana>().manaRegenBoost += 0.1f;
                    player.lifeRegen += 2;
                    break;
                case ItemID.MagicCuffs:
                    player.GetModPlayer<OnHitItems>().magicCuffsCount += 1; // the duration of the buff is the Hit's damage multiplied by this number, then multiplied by 3. This also decides how much mana is recovered.
                    player.statManaMax2 -= 20;
                    player.magicCuffs = false;
                    break;
                case ItemID.CelestialCuffs:
                    player.statManaMax2 -= 20;
                    player.GetModPlayer<OnHitItems>().magicCuffsCount += 1;
                    player.magicCuffs = false;
                    player.GetModPlayer<Mana>().celestialCuffsOverload = true;
                    break;
                case ItemID.StarCloak:
                    player.starCloakItem = null;
                    player.GetModPlayer<OnHitItems>().NewstarsOnHit = true;
                    break;
                case ItemID.ManaCloak:
                    player.starCloakItem_manaCloakOverrideItem = item;
                    player.GetModPlayer<Mana>().manaCloak = true;
                    player.manaCost += 0.08f;
                    break;
                case ItemID.ManaFlower:
                case ItemID.MagnetFlower:
                    player.GetModPlayer<Mana>().newManaFlower = true;
                    player.manaCost += 0.08f;
                    break;
                case ItemID.ArcaneFlower:
                    player.manaCost += 0.08f;
                    player.GetModPlayer<Mana>().newManaFlower = true;
                    player.GetDamage<MagicDamageClass>() += 0.05f;
                    player.GetCritChance<MagicDamageClass>() += 5;
                    break;
                case ItemID.CelestialEmblem:
                    player.GetDamage<MagicDamageClass>() -= 0.03f;
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
                    player.GetModPlayer<SummonStats>().minionCritChance += 5;
                    player.GetDamage<SummonDamageClass>() -= 0.1f;
                    break;
                case ItemID.PapyrusScarab:
                    ++player.maxTurrets;
                    player.GetModPlayer<SummonStats>().minionCritChance += 5;
                    player.GetDamage<SummonDamageClass>() -= 0.15f;
                    break;
                case ItemID.HeroShield:
                    player.hasPaladinShield = false;
                    break;
                case ItemID.BerserkerGlove:
                    player.kbGlove = false;
                    player.meleeScaleGlove = false;
                    break;
                case ItemID.SquireShield:
                    player.dd2Accessory = false;
                    player.GetDamage<MeleeDamageClass>() += 0.1f;
                    ++player.maxTurrets;
                    break;
                case ItemID.ApprenticeScarf:
                    ++player.maxTurrets;
                    player.GetDamage<SummonDamageClass>() -= 0.1f;
                    player.GetDamage<MagicDamageClass>() += 0.1f;
                    player.dd2Accessory = false;
                    break;
                case ItemID.MonkBelt:
                    ++player.maxTurrets;
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    player.dd2Accessory = false;
                    break;
                case ItemID.HuntressBuckler:
                    ++player.maxTurrets;
                    player.GetDamage<RangedDamageClass>() += 0.1f;
                    player.dd2Accessory = false;
                    break;
                case ItemID.RifleScope:
                    player.GetModPlayer<RangedStats>().gunVelocity += 0.8f; 
                    break;
                case ItemID.SniperScope:
                    player.GetModPlayer<RangedStats>().gunVelocity += 0.8f;
                    player.GetDamage<RangedDamageClass>() -= 0.1f;
                    break;
                case ItemID.ReconScope:
                    player.GetModPlayer<RangedStats>().rangedVelocity += 0.8f;
                    player.GetModPlayer<RangedStats>().Magicandgunquiver += 1; 
                    player.GetDamage<RangedDamageClass>() -= 0.1f; 
                    player.GetCritChance<RangedDamageClass>() -= 10;
                    break;
            }
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.CountsAsClass(DamageClass.Ranged))
            {
                velocity *= player.GetModPlayer<RangedStats>().rangedVelocity;
            }
            if (item.CountsAsClass(DamageClass.Ranged) && (item.useAmmo == AmmoID.Bullet || item.useAmmo == AmmoID.CandyCorn))
            {
                velocity *= player.GetModPlayer<RangedStats>().gunVelocity;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.SquireShield:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "10% increased melee damage";
                        }
                    }
                    break;
                case ItemID.ApprenticeScarf:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "10% increased magic damage";
                        }
                    }
                    break;
                case ItemID.HuntressBuckler:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "10% increased ranged damage";
                        }
                    }
                    break;   
				case ItemID.Bezoar:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Significantly increases potency of friendly Poison";
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
                            line.Text = "Significantly increases potency of friendly Poison\nIncreases potency of friendly debuffs by 50%";
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
                case ItemID.HoneyComb:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {

                            line.Text = "Releases bees and increases life regeneration when damaged";
                        }
                    }
                    break;

                case ItemID.BeeCloak:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Causes buzzy stars to fall when damaged";
                        }
                    }
                    break;
                case ItemID.Shackle:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\nTemporarily increases defense when damaged";
                        }
                    }
                    break;
                case ItemID.BandofStarpower:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases mana regeneration rate by 10%";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                    break;
                case ItemID.ManaRegenerationBand:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases mana and health regeneration rate";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
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
                case ItemID.MagicCuffs:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Restores mana when damaged";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Can go over maximum mana";
                        }
                    }
                    break;
                case ItemID.CelestialCuffs:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Restores mana when damaged, can go over maximum mana";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Increases pickup range for mana stars";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
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
                            line.Text = "";
                        }
                    }
                    break;
                case ItemID.MoltenSkullRose:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Magic attacks lower enemy contact damage by 15%";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "Enemies near the player take 10% more damage";
                        }
                    }
                    break;
                case ItemID.ObsidianSkullRose:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Magic attacks lower enemy contact damage by 15%";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Enemies near the player take 10% more damage";
                        }
                    }
                    break;
                case 3999: // Magma Skull
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Enemies near the player take 10% more damage";
                        }
                    }
                    break;
                case ItemID.ObsidianSkull:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Enemies near the player take 10% more damage";
                        }
                    }
                    break;
                case ItemID.ObsidianShield:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Enemies near the player take 10% more damage";
                        }
                    }
                    break;
                case ItemID.ObsidianRose:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Magic attacks lower enemy contact damage by 15%";
                        }
                    }
                    break;
                case ItemID.CrossNecklace:
                case ItemID.StarVeil:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nMore effective on strong hits";
                        }
                    }
                    break;
                case ItemID.MagnetFlower:
                case ItemID.ManaFlower:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Magic critical hits have a chance to spawn a mana star";
                        }
                    }
                    break;
                case ItemID.ArcaneFlower:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Magic critical hits have a chance to spawn a mana star\n5% increased magic damage and critical strike chance";
                        }
                    }
                    break;
                case ItemID.ManaCloak:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Magic critical hits have a chance to cause a damaging star to fall";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Stars restore mana when collected";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "Automatically uses mana potions when needed";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                        {
                            line.Text = "";
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
                case ItemID.CelestialEmblem:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "12% increased magic damage";
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
                case ItemID.BerserkerGlove:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "12% increased melee speed";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Enables auto swing for melee weapons\nEnemies are more likely to target you";
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
                            line.Text = "Minion damage is stored as Fire energy, up to 2250\nWhip strikes summon a friendly Molten Apparition for every 600 damage stored";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "The wearer is immune to lava";
                        }
                    }
                    break;
                case ItemID.RoyalGel:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nReduces damage by 25 every 30 seconds";
                        }
                    }
                    break;
                case ItemID.MoonCharm:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Turns the holder into a werewolf when missing health";
                        }
                    }
                    break;
                case ItemID.MoonShell:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Turns the holder into a werewolf when missing health and into a merfolk when entering water\nPuts a shell around the owner when below 50% life";
                        }
                    }
                    break;
                case ItemID.MoonStone:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Minor increases to all stats during the night";
                        }
                    }
                    break;
                case ItemID.SunStone:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Minor increases to all stats during the day";
                        }
                    }
                    break;
                case ItemID.CelestialStone:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Minor increases to all stats";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
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
                    }
                    break;
                case ItemID.NecromanticScroll:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Gives minions a 5% chance to crit";
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
                            line.Text = "Gives minions a 5% chance to crit";
                        }
                    }
                    break;
                case ItemID.MagicQuiver:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Your arrows will bounce towards nearby enemies after a certain time or hitting a tile, losing damage in the process";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                    break;
                case ItemID.StalkersQuiver:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Your arrows will bounce towards nearby enemies after a certain time or hitting a tile, losing damage in the process";

                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "5% increased ranged damage and critical strike chance";
                        }
                    }
                    break;
                case ItemID.MoltenQuiver:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Your arrows will bounce towards nearby enemies after a certain time or hitting a tile, losing damage in the process";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Lights wooden arrows ablaze";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "Quiver in fear!";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                        {
                            line.Text = "";
                        }
                    }
                    break;
                case ItemID.RifleScope:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases view range for guns (right click to zoom out!)";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "80% increased velocity for guns";
                        }
                    }
                    break;
                case ItemID.SniperScope:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "80% increased velocity for guns\n10% increased ranged critical strike chance";
                        }
                    }
                    break;
                case ItemID.ReconScope:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases view range for guns (right click to zoom out!)";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "80% increased velocity for ranged weapons";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "Your ranged attacks will bounce towards their nearby enemy";
                        }
                    }
                    break;
            }
        }
    }
}






