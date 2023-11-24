using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject;
using System.Collections.Generic;
using TRAEProject.Changes.Accesory;
using TRAEProject.Changes.Armor;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace ChangesArmor
{
    public class ChangesArmor : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void UpdateEquip(Item item, Player player)
    {
            switch (item.type)
            {
                case ItemID.SpiderBreastplate:
                    player.moveSpeed += 0.1f;
                    break;
                case ItemID.PearlwoodHelmet:
                    player.jumpSpeedBoost += 1f;
                    break;
                case ItemID.PearlwoodBreastplate:
                    Player.jumpHeight += 2;
                    break;
                case ItemID.PearlwoodGreaves:
                    player.moveSpeed += 0.15f;
                    break;
                case ItemID.AncientArmorHat:
                    player.GetDamage<SummonDamageClass>() += 0.17f;
                    player.maxTurrets += 1;
                    break;
                case ItemID.AncientArmorShirt:
                    player.GetDamage<SummonDamageClass>() += 0.05f; 
                    player.maxTurrets += 1;
                    break;
                case ItemID.AncientArmorPants:
                    player.moveSpeed += 0.1f;
                    player.GetDamage<SummonDamageClass>() += 0.03f;
                    break;
                case ItemID.GladiatorHelmet:
                case ItemID.GladiatorBreastplate:
                case ItemID.GladiatorLeggings:
                    player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
                    break;
                case ItemID.PharaohsMask:
                    player.moveSpeed += 0.05f;
                    break;
                case ItemID.PharaohsRobe:
                    player.moveSpeed += 0.05f;
                    break;
                case ItemID.MeteorHelmet:
                case ItemID.MeteorSuit:
                case ItemID.MeteorLeggings:
                    player.GetDamage<MagicDamageClass>() += 0.02f;
                    break;
                case ItemID.RuneRobe:
                    player.statManaMax2 += 100;
                    player.manaCost -= 0.21f;
                    break;
                case ItemID.RuneHat:
                    player.GetDamage<MagicDamageClass>() += 0.15f;
                    player.GetCritChance<MagicDamageClass>()  += 15;
                    break;
                case ItemID.OrichalcumMask:
                    player.GetDamage<MeleeDamageClass>()  -= 0.11f;
                    player.GetCritChance<MeleeDamageClass>()  += 13;
                    break;
                case ItemID.PirateHat:
                    player.whipRangeMultiplier += 0.3f;
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    break;
                case ItemID.PirateShirt:
                    player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.12f;
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    break;
                case ItemID.PiratePants:
                    player.moveSpeed += 0.1f;

                    player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.08f;
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    break;
                case ItemID.DjinnsCurse:
                    player.jumpSpeedBoost += 1f;
                    break;
                case ItemID.ChlorophytePlateMail:
                    player.GetDamage<GenericDamageClass>() += 0.05f;
                    break;
                case ItemID.ChlorophyteGreaves:
                    player.GetCritChance<GenericDamageClass>() += 2;
                    player.moveSpeed += 0.05f;
                    break;
//////////////////////// OOA
/// T1: SQUIRE, MONK, HUNTRESS, APPRENTICE
                case ItemID.SquireGreatHelm:
                    player.lifeRegen -= 2;
                    break;
                case ItemID.SquirePlating:
                    player.GetDamage<MeleeDamageClass>() -= 0.05f; 
                    player.GetDamage<SummonDamageClass>() -= 0.05f;
                    player.lifeRegen += 2;
                    break;
                case ItemID.SquireGreaves:
                    player.GetCritChance<MeleeDamageClass>() -= 10;
                    player.GetDamage<SummonDamageClass>() += 0.05f;
                    player.moveSpeed -= 0.15f;
                    break;
                case ItemID.MonkPants:
                case ItemID.HuntressPants:
                case ItemID.ApprenticeTrousers:
                    player.moveSpeed -= 0.1f;
                    break;
////////// T2: VK, SHINOBI, RED RIDING, DARK ARTIST
                case ItemID.SquireAltHead:
                    player.lifeRegen += 4;
                    ++player.maxMinions;
                    break;
                case ItemID.SquireAltShirt:
                    player.lifeRegen -= 4;
                    break;
                case ItemID.SquireAltPants:
                    player.moveSpeed -= 0.2f;
                    break;
                case ItemID.MonkAltShirt:
                    player.GetAttackSpeed(DamageClass.Melee) -= 0.2f;
                    player.GetDamage<MeleeDamageClass>()  += 0.2f;
                    break;
                case ItemID.MonkAltHead:
                    player.GetAttackSpeed(DamageClass.Melee) += 0.2f;
                    player.GetDamage<MeleeDamageClass>()  -= 0.2f;
                    break;
                case ItemID.MonkAltPants:
                case ItemID.HuntressAltPants:
                case ItemID.ApprenticeAltPants:
                    player.moveSpeed -= 0.15f;
                    break;
                case ItemID.ApprenticeHat:
                    player.statManaMax2 += 40;
                    break;
                case ItemID.ApprenticeAltHead:
                    player.statManaMax2 += 60;
                    break;
///////////////// end of OOA
                case ItemID.NinjaHood:
                case ItemID.NinjaShirt:
                case ItemID.NinjaPants:
                    player.GetCritChance<GenericDamageClass>() += 3;
                    break;
                case ItemID.CrystalNinjaLeggings:
                    player.moveSpeed -= 0.05f;
                    player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
                    break;
                case ItemID.CrystalNinjaHelmet:
                    player.manaCost -= 0.10f;
                    break;
                case ItemID.SpectreMask:
                    player.manaCost += 0.04f; // now -9% instead of -13%
                    break;
                case ItemID.SpectreHood:
                    player.statManaMax2 += 100;
                    player.manaCost -= 0.20f;
                    player.GetDamage<MagicDamageClass>() += 0.05f;
                    player.GetCritChance<MagicDamageClass>() += 5;
                    break;
                case ItemID.Goggles:
                    player.GetCritChance<GenericDamageClass>() += 8;
                    break;
            }
        }
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.WoodHelmet && body.type == ItemID.WoodBreastplate && legs.type == ItemID.WoodGreaves)
                return "WoodSet";
            if (head.type == ItemID.BorealWoodHelmet && body.type == ItemID.BorealWoodBreastplate && legs.type == ItemID.BorealWoodGreaves)
                return "WoodSet";
            if (head.type == ItemID.PalmWoodHelmet && body.type == ItemID.PalmWoodBreastplate && legs.type == ItemID.PalmWoodGreaves)
                return "WoodSet";
            if (head.type == ItemID.ShadewoodHelmet && body.type == ItemID.ShadewoodBreastplate && legs.type == ItemID.ShadewoodGreaves)
                return "WoodSet";
            if (head.type == ItemID.EbonwoodHelmet && body.type == ItemID.EbonwoodBreastplate && legs.type == ItemID.EbonwoodGreaves)
                return "WoodSet";
            if (head.type == ItemID.RichMahoganyHelmet && body.type == ItemID.RichMahoganyBreastplate && legs.type == ItemID.RichMahoganyGreaves)
                return "WoodSet";  
            if (head.type == ItemID.CopperHelmet && body.type == ItemID.CopperChainmail && legs.type == ItemID.CopperGreaves)
                return "CopperSet";
            if (head.type == ItemID.TinHelmet && body.type == ItemID.TinChainmail && legs.type == ItemID.TinGreaves)
                return "TinSet";
            if ((head.type == ItemID.IronHelmet || head.type == ItemID.AncientIronHelmet) && body.type == ItemID.IronChainmail && legs.type == ItemID.IronGreaves)
                return "IronSet";
            if (head.type == ItemID.LeadHelmet && body.type == ItemID.LeadChainmail && legs.type == ItemID.LeadGreaves)
                return "LeadSet";
            if (head.type == ItemID.SilverHelmet && body.type == ItemID.SilverChainmail && legs.type == ItemID.SilverGreaves)
                return "SilverSet";
            if (head.type == ItemID.TungstenHelmet && body.type == ItemID.TungstenChainmail && legs.type == ItemID.TungstenGreaves)
                return "TungstenSet";
            if ((head.type == ItemID.GoldHelmet || head.type == ItemID.AncientGoldHelmet) && body.type == ItemID.GoldChainmail && legs.type == ItemID.GoldGreaves)
                return "GoldSet";
            if (head.type == ItemID.PlatinumHelmet && body.type == ItemID.PlatinumChainmail && legs.type == ItemID.PlatinumGreaves)
                return "PlatinumSet";
            if (head.type == ItemID.PharaohsMask && body.type == ItemID.PharaohsRobe)
                return "PharaohSet";
            if (head.type == ItemID.RuneHat && body.type == ItemID.RuneRobe)
                return "WizardSetHM";
            if (head.type == ItemID.AncientArmorHat && body.type == ItemID.AncientArmorShirt && legs.type == ItemID.AncientArmorPants)
                return "AncientSet";
            if (head.type == ItemID.TurtleHelmet && body.type == ItemID.TurtleScaleMail && legs.type == ItemID.TurtleLeggings)
                return "TurtleSet";
         
            if (head.type == ItemID.AdamantiteHelmet && body.type == ItemID.AdamantiteBreastplate && legs.type == ItemID.AdamantiteLeggings)
                return "AdamantiteSet";

            if (head.type == ItemID.CobaltMask && body.type == ItemID.CobaltBreastplate && legs.type == ItemID.CobaltLeggings)
                return "CobaltSet";
            if ((head.type == ItemID.AncientHallowedHeadgear || head.type == ItemID.AncientHallowedHelmet || head.type == ItemID.AncientHallowedMask || head.type == ItemID.HallowedHeadgear || head.type == ItemID.HallowedHelmet || head.type == ItemID.HallowedMask) && (body.type == ItemID.HallowedPlateMail || body.type == ItemID.AncientHallowedPlateMail) && (legs.type == ItemID.AncientHallowedGreaves || legs.type == ItemID.HallowedGreaves))
                return "HallowedSet"; 
            if ((head.type == ItemID.AncientHallowedHood || head.type == ItemID.HallowedHood) && (body.type == ItemID.HallowedPlateMail || body.type == ItemID.AncientHallowedPlateMail) && (legs.type == ItemID.AncientHallowedGreaves || legs.type == ItemID.HallowedGreaves))
                return "HallowedSetSummon"; 
            if (head.type == ItemID.ChlorophyteMask && body.type == ItemID.ChlorophytePlateMail && legs.type == ItemID.ChlorophyteGreaves)
                return "ChloroMeleeSet";

            if (head.type == ItemID.SpectreHood && body.type == ItemID.SpectreRobe && legs.type == ItemID.SpectrePants)
                return "SpectreHoodSet";
            if (head.type == ItemID.SpectreMask && body.type == ItemID.SpectreRobe && legs.type == ItemID.SpectrePants)
                return "SpectreMaskSet";
            if (head.type == ItemID.PirateHat && body.type == ItemID.PirateShirt && legs.type == ItemID.PiratePants)
                return "PirateSet"; 
            if (head.type == ItemID.GladiatorHelmet && body.type == ItemID.GladiatorBreastplate && legs.type == ItemID.GladiatorLeggings)
                return "GladiatorSet"; 
            if (head.type == ItemID.FossilHelm && body.type == ItemID.FossilShirt && legs.type == ItemID.FossilPants)
                return "FossilSet";
            if (head.type == ItemID.CrimsonHelmet && body.type == ItemID.CrimsonScalemail && legs.type == ItemID.CrimsonGreaves)
                return "CrimsonSet";
                if ((head.type == ItemID.ShadowHelmet || head.type == ItemID.AncientShadowHelmet) && (body.type == ItemID.ShadowScalemail || body.type == ItemID.AncientShadowScalemail) && (legs.type == ItemID.ShadowGreaves || legs.type == ItemID.AncientShadowGreaves))
                return "ShadowSet";
            if (head.type == ItemID.FrostHelmet && body.type == ItemID.FrostBreastplate && legs.type == ItemID.FrostLeggings)
                return "FrostSet";
            if (head.type == ItemID.CrystalNinjaHelmet && body.type == ItemID.CrystalNinjaChestplate && legs.type == ItemID.CrystalNinjaLeggings)
                return "CrystalAssassinSet";
            if (head.type == ItemID.PearlwoodHelmet && body.type == ItemID.PearlwoodBreastplate && legs.type == ItemID.PearlwoodGreaves)
                return "PearlwoodSet";
            return base.IsArmorSet(head, body, legs);
        }
        public override void UpdateArmorSet(Player player, string armorSet)
        {
            if (armorSet == "WoodSet")
            {
                player.setBonus = "Reduces damage taken by 5%";
                player.statDefense -= 1;
            }
            if (armorSet == "WoodSetPlus")
            {
                player.setBonus = "Reduces damage taken by 5%*";
                player.GetModPlayer<SetBonuses>().secretPearlwoodSetBonus = true;
            }
            if (armorSet == "CopperSet") 
            {
                player.setBonus = "Reduces damage taken by 8%";
                player.endurance += 0.08f;
                player.statDefense -= 2;
            }
            if (armorSet == "TinSet")
            {
                player.setBonus = "Reduces damage taken by 9%";
                player.endurance += 0.09f;
                player.statDefense -= 2;
            }
            if (armorSet == "IronSet") // Revisit
            {
                player.setBonus = "Reduces damage taken by 10%";
                player.endurance += 0.1f;
                player.statDefense -= 2;
            }
            if (armorSet == "LeadSet")
            {
                player.setBonus = "Reduces damage taken by 11%";
                player.endurance += 0.11f;
                player.statDefense -= 3;
            }
            if (armorSet == "SilverSet") // Revisit
            {
                player.setBonus = "Reduces damage taken by 12%";
                player.endurance += 0.12f;
                player.statDefense -= 3;
            }
            if (armorSet == "TungstenSet")
            {
                player.setBonus = "Reduces damage taken by 13%";
                player.endurance += 0.13f;
                player.statDefense -= 3;
            }
            if (armorSet == "GoldSet") // Revisit
            { 
                player.setBonus = "Reduces damage taken by 14%";
            player.endurance += 0.14f;
            player.statDefense -= 3;
        }
            if (armorSet == "PlatinumSet")
            {
                player.setBonus = "Reduces damage taken by 15%";
                player.endurance += 0.15f;
                player.statDefense -= 4;
            }
            if (armorSet == "PharaohSet")
            {
                player.setBonus = "Grants an improved double jump and the ability to float for a few seconds";
                player.GetJumpState(ExtraJump.SandstormInABottle).Enable();
                player.carpet = true;
            }
            if (armorSet == "WizardSetHM")
            {
                player.setBonus = "Return quintuple damage taken to near enemies";
                player.GetModPlayer<OnHitEffects>().runethorns += 5f;
            }
            if (armorSet == "ShadowSet")
            {
                player.setBonus = "Increases Movement speed by 20%\nGreatly increases acceleration";
            }
            if (armorSet == "CrimsonSet")
            {
                player.lifeRegenTime += 2; 
                player.setBonus = "Greatly increases natural healing rate";
            }
            if (armorSet == "AncientSet")
            {
                player.setBonus = "Converts all minion slots into sentry slots";
                player.maxTurrets += player.maxMinions;
                player.maxMinions -= player.maxMinions;
            }
            if (armorSet == "TurtleSet")
            {
                player.setBonus = "Damage taken is reflected to nearby enemies with thrice the strength\nReduces damage taken by 15%";
                player.GetModPlayer<OnHitEffects>().newthorns += 3f;
                player.thorns -= 2f;
                player.turtleThorns = false;
            }
            if (armorSet == "ChloroMeleeSet")
            {
                player.setBonus = "Summons a powerful leaf crystal to shoot at nearby enemies";
                player.endurance -= 0.05f;
            }
            if (armorSet == "AdamantiteSet")
            {
                player.setBonus = "20% increased melee speed";
                player.moveSpeed -= 0.2f;
            }


            if (armorSet == "PirateSet")
            {
                player.setBonus = "Increases your maximum number of minions by 1\nAll whips gain a stackable 4% minion crit tag";
				player.GetModPlayer<SetBonuses>().PirateSet = true;
				player.maxMinions++;
            }
            if (armorSet == "SpectreHoodSet")
            {
                
                player.setBonus = "Magic attacks heal the player and allies";
                player.GetDamage<MagicDamageClass>() += 0.4f; // +0.4 to negate the reduction
            }
            if (armorSet == "FrostSet")
            {
                player.setBonus = "Melee and ranged attacks inflict frostburn\nDouble tap down to freeze all enemies around you, 10 second cooldown";
                player.GetModPlayer<FrostArmor>().frostArmor = true;
                player.GetDamage<RangedDamageClass>() -= 0.1f;
                player.GetDamage<MeleeDamageClass>() -= 0.1f;
            }
            if (armorSet == "FossilSet")
            {
                player.setBonus += "\nRanged weapons have 4 armor penetration";
                if (player.HeldItem.CountsAsClass(DamageClass.Ranged))
                {
                    player.GetArmorPenetration(DamageClass.Generic) += 4;
                }
            }
            if (armorSet == "CrystalAssassinSet")
            {
                player.setBonus = "10% increased damage, critical strike chance and movement speed";
                player.moveSpeed += 0.1f;
            }
            if (armorSet == "PearlwoodSet")
            {
                player.setBonus = "Allows the ability to dash";
                player.dashType = 5;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.Goggles:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\n8% increased critical strike chance";
                        }
                    }
                    break;
                case ItemID.GladiatorHelmet:
                case ItemID.GladiatorBreastplate:
                case ItemID.GladiatorLeggings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\n5% increased melee speed";
                        }
                    }
                    return;
                case ItemID.SpiderBreastplate:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text += "\n10% increased movement speed";
                        }
                    }
                    break;
                case ItemID.PearlwoodHelmet:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\n20% increased jump speed";
                        }
                    }
                    return;
                case ItemID.PearlwoodBreastplate:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\nSlightly increased jump duration";
                        }
                    }
                    return;
                case ItemID.PearlwoodGreaves:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\n15% increased movement speed";
                        }
                    }
                    return;
                case ItemID.SpectreHood:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "Increases maximum mana by 100 and 20% reduced mana cost\n5% increased magic damage and critical strike chance";
                        }
                    }
                    return;
                case ItemID.SpectreMask:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases maximum mana by 60 and 9% reduced mana cost";
                        }
                    }
                    return;

                case ItemID.PharaohsRobe:
                case ItemID.PharaohsMask:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\nIncreases movement speed by 5%";
                        }
                    }
                    return;
                case ItemID.PirateHat:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\n10% increased summon damage\n30% increased whip range";
                        }
                    }
                    return;
                case ItemID.PirateShirt:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\n10% increased summon damage\n12% increased whip speed";
                        }
                    }
                    return;
                case ItemID.PiratePants:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\n10% increased summon damage and movement speed\n8% increased whip speed";
                        }
                    }
                    return;
                case ItemID.AncientArmorHat:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\n17% increased summon damage\nIncreases your maximum number of sentries by 1";
                        }
                    }
                    return;
                case ItemID.AncientArmorShirt:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\n5% increased summon damage\nIncreases your maximum number of sentries by 1";
                        }
                    }
                    return;
                case ItemID.AncientArmorPants:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\nIncreases movement speed by 10%\n3% increased summon damage";
                        }
                    }
                    return;
                case ItemID.RuneHat:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\n15% increased magic damage and critical strike chance";
                        }
                    }
                    return;
                case ItemID.RuneRobe:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Defense")
                        {
                            line.Text += "\nIncreases maximum mana by 100\nReduces mana costs by 21%";
                        }
                    }
                    return;              
                case ItemID.OrichalcumMask:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "13% increased melee critical strike chance";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "7% increased melee and movement speed";
                        }
                    }
                    return;
                case ItemID.DjinnsCurse:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nIncreases jump height and speed";
                        }
                    }
                    return;
                case ItemID.ChlorophytePlateMail:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "10% increased damage";
                        }
                    }
                    return;
                case ItemID.ChlorophyteGreaves:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "10% increased critical strike chance and movement speed";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                    return;

//////////////////////// OOA, ORDERED BY SQUIRE-MONK-APPRENTICE-HUNTRESS
                case ItemID.SquireGreaves:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "20% increased summon damage\n5% increased melee critical strike chance";
                        }

                    }
                    return;
                case ItemID.SquirePlating:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "10% increased melee and minion damage\nIncreases life regeneration";
                        }
                    }
                    return;
                case ItemID.MonkPants:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "15% increased melee critical strike chance\n10% increased summon damage and movement speed";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                    return;
                case ItemID.ApprenticeTrousers:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "20% increased magic critical strike chance\n10% increased move speed";
                        }
                    }
                    return;
                case ItemID.HuntressPants:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "10% increased minion damage and movement speed";
                        }
                    }
                    return;
                case ItemID.SquireAltHead:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nGreatly increased life regeneration";
                        }
                    }
                    return;
                case ItemID.SquireAltShirt:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "30% increased minion damage and greatly increased life regeneration";
                        }
                    }
                    return;
                case ItemID.SquireAltPants:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "20% increased minion damage and melee critical strike chance";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "10% increased movement speed";
                        }
                    }
                    return;
                case ItemID.MonkAltHead:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "20% increased melee speed and minion damage\nIncreases your maximum number of sentries by 2 ";
                        }
                    }
                    return;
                case ItemID.MonkAltShirt:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "20% increased melee and minion damage";
                        }
                    }
                    return;
                case ItemID.MonkAltPants:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "20% increased minion damage and melee critical strike chance";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "15% increased movement speed";
                        }
                    }
                    return;
                case ItemID.HuntressAltPants:
                case ItemID.ApprenticeAltPants:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "10% increased movement speed";
                        }
                    }
                    return;

//////////////////// END OF OOA
                
                case ItemID.CrystalNinjaHelmet:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "5% increased critical strike chance";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Reduces mana usage by 20%";
                        }
                    }
                    return;
                case ItemID.CrystalNinjaLeggings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "15% increased movement and melee speed";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                    return;
                case ItemID.NinjaHood:
                case ItemID.NinjaShirt:
  
                case ItemID.NinjaPants:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "6% increased critical strike chance";
                        }
                    }
                    return;
                case ItemID.MeteorHelmet:
                case ItemID.MeteorSuit:
                case ItemID.MeteorLeggings:
				     foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "11% increased magic damage";
                        }
                    }
                    return;
            }
        }
    }
}



