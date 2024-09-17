
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using TRAEProject.Changes.Prefixes;
using System;

namespace TRAEProject.Changes.Weapon.Melee
{
    public class MiscMelee : GlobalItem
    {
        public override bool InstancePerEntity => true;
  
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {              
                case ItemID.ChainKnife:
                    item.crit = 12;
                    item.autoReuse = true;
                    item.value = Item.buyPrice(gold: 5);
                    return;
                case ItemID.HiveFive:
                    item.damage = 21; // down from 24 
                    return;
					case ItemID.EnchantedBoomerang:
                    item.damage = 20; // up from 17
                    return;
                case ItemID.IceBoomerang:
                    item.damage = 22; // up from 16
                    item.crit = 12; // up from 6%
                    return;
				case ItemID.LightDisc:
                    item.useTime = 12; // down from 14
                    item.useAnimation = 12; // up from 14
                    break;
                case ItemID.Trimarang:
                    item.damage = 24; // up from 16
                    return;
                case ItemID.PaladinsHammer:
                    item.damage = 102; // up from 90
                    return;
                case ItemID.VampireKnives:
                    item.damage = 24; // down from 29
            
                    break;
                case ItemID.PossessedHatchet:
                    item.damage = 102; // up from 80
                    return;
                case ItemID.BlueMoon:
                    item.damage = 32;
                    item.crit = 11;
                    return;
                case ItemID.Sunfury:
                    item.damage = 19;
                    item.crit = 0;
                    return;    
				case ItemID.Flairon:
                    item.noMelee = false;
                    return;
                case ItemID.DaoofPow:
                    item.damage = 40;
                    return;
                    // yoyo
                case ItemID.Rally:
                    item.value = Item.buyPrice(gold: 5);
                    return;
                 
                case ItemID.HelFire:
                    item.damage = 90;
                    item.knockBack = 3f;
                    return;
                case ItemID.Cascade:
                    item.damage = 27; // up from 27
                    return;
                case ItemID.Gradient:
                    item.damage = 29;
                    item.knockBack = 6f;
                    return;
                case ItemID.Chik:
                    //item.damage = 49;
                    item.knockBack = 0.5f;
                    return;
                case ItemID.Amarok:
                    //item.damage = 52;
                    item.knockBack = 5f;
                    return;
                case ItemID.Yelets:
                    item.damage = 65;
                    return;
                case ItemID.Code2:
                    item.damage = 61;
                    return;
                case ItemID.Kraken:
                    item.damage = 70; // vanilla value = 95
                    return;
                case ItemID.TheEyeOfCthulhu:
                    item.damage = 150; //vanilla value = 115
                    break;
    
         
            }
            return;
        }

        
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        { 
            if (player.HasBuff(BuffID.WeaponImbueNanites))
            {
                player.AddBuff(BuffType<NanoHealing>(), 60, false);
            }
            if (item.type == ItemID.Cutlass)
            {
                if (target.active && !target.dontTakeDamage && !target.friendly && target.lifeMax > 5 && !target.immortal && !target.SpawnedFromStatue)
                {
                    int amount = damageDone / 2;
                    player.QuickSpawnItem(player.GetSource_OnHit(target), ItemID.CopperCoin, amount);
                    return;
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if(ItemID.Sets.Yoyo[item.type])
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "Speed")
                    {
 
                        float range = ProjectileID.Sets.YoyosMaximumRange[item.shoot] * item.GetGlobalItem<YoyoStats>().range; 
                        if(Main.LocalPlayer.yoyoString)
                        {
                            range = range * 1.25f + 30f;
                        }
                        range *= (1f + Main.LocalPlayer.GetAttackSpeed(DamageClass.Melee) * 3f) / 4f / 16f;
                        range = MathF.Round(range);

                        float time = MathF.Round(ProjectileID.Sets.YoyosLifeTimeMultiplier[item.shoot], 1);
                        
                        float speed = ProjectileID.Sets.YoyosTopSpeed[item.shoot] * item.GetGlobalItem<YoyoStats>().speed;
                        speed *= (1f + Main.LocalPlayer.GetAttackSpeed(DamageClass.Melee) * 3f) / 4f;

                        string speedText = "";
                        if(speed < 10f)
                        {
                            speedText = "Very slow maneuvering speed";
                        }
                        if(speed >= 10f && speed < 13f)
                        {
                            speedText = "Slow maneuvering speed";
                        }
                        if(speed >= 13f && speed < 16f)
                        {
                            speedText = "Average maneuvering speed";
                        }
                        if(speed >= 16f && speed < 20f)
                        {
                            speedText = "Fast maneuvering speed";
                        }
                        if(speed >= 20f && speed < 24f)
                        {
                            speedText = "Very fast maneuvering speed";
                        }
                        if(speed >= 24f)
                        {
                            speedText = "Insanely fast maneuvering speed";
                        }

                        if (ProjectileID.Sets.YoyosLifeTimeMultiplier[item.shoot] < 0)
                            line.Text = "Infinite lifespan" + "\n" + speedText + "\n" + range + " tiles range";

                        else
                            line.Text = time + " seconds lifespan" + "\n" + speedText + "\n" + range + " tiles range";
                    }
                }

            }
            switch (item.type)
            {
                case ItemID.HelFire:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nIncinerate enemies with a ring of fire";
                        }
                    }
                    break;
                case ItemID.Cutlass:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nCreates money on enemy hits";
                        }
                    }
                    break;
                case ItemID.PalladiumPike:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nIncreases health regeneration after striking an enemy\nMore effective with Palladium armor equipped";
                        }
                    }
                    break;
                case ItemID.OrichalcumHalberd:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nCreates damaging petals on contact";
                        }
                    }
                    break;
                case ItemID.ChristmasTreeSword:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Shoots christmas decorations";
                        }
                    }
                    break;
                case ItemID.Chik:
                    break;
                case ItemID.FormatC:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nCharges power as it is held out";
                        }
                    }
                    break;
                case ItemID.Gradient:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nFires Bones at enemies";
                        }
                    }
                    break;
                case ItemID.Kraken:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nMuch faster than other yoyos";
                        }
                    }
                    break;
                case ItemID.Cascade:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nHighly Volatile";
                        }
                    }
                    break;
                     
                case ItemID.VampireKnives:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Throw life stealing daggers";
                        }
                    }
                    break;   
            }
        }
    }
}
