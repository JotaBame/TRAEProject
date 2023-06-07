
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

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
                case ItemID.VampireKnives:
                    item.damage = 29; // up from 29
                    return;
					case ItemID.EnchantedBoomerang:
                    item.damage = 20; // up from 17
                    return;
                case ItemID.IceBoomerang:
                    item.damage = 22; // up from 16
                    item.crit = 12; // up from 6%
                    return;

                case ItemID.PaladinsHammer:
                    item.damage = 102; // up from 90
                    return;
                case ItemID.PossessedHatchet:
                    item.damage = 102; // up from 80
                    return;
                case ItemID.BlueMoon:
                    item.damage = 32;
                    item.crit = 11;
                    return;
                case ItemID.Sunfury:
                    item.damage = 16;
                    item.crit = 4;
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
                    item.damage = 39;
                    item.knockBack = 7f;
                    return;
                case ItemID.Cascade:
                    item.damage = 27; // up from 27
                    return;
                case ItemID.Gradient:
                    item.damage = 29;
                    item.knockBack = 6f;
                    return;
                case ItemID.Chik:
                    item.damage = 39;
                    item.knockBack = 1f;
                    return;
                case ItemID.Kraken:
                    item.damage = 88; // vanilla value = 95
                    return;
    
         
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
            switch (item.type)
            {
                case ItemID.Cutlass:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nCreates money on enemy hits";
                        }
                    }
                    return;
                case ItemID.PalladiumPike:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nIncreases health regeneration after striking an enemy";
                        }
                    }
                    return;
                case ItemID.OrichalcumHalberd:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nCreates damaging petals on contact";
                        }
                    }
                    return;
                case ItemID.ChristmasTreeSword:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Shoots christmas decorations";
                        }
                    }
                    return;
                case ItemID.Chik:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nCauses an explosion of crystal shards on hit";
                        }
                    }
                    return;
                case ItemID.FormatC:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nCharges power as it is held out";
                        }
                    }
                    return;
                case ItemID.Gradient:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nFires Bones at enemies";
                        }
                    }
                    return;
                case ItemID.Kraken:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nReleases a tentacle made out of lost souls while held out";
                        }
                    }
                    return;
                case ItemID.Cascade:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nHighly Volatile";
                        }
                    }
                    return;
                case ItemID.Sunfury:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nInflicts a heavy burn on enemies";
                        }
                    }
                    return;               
                case ItemID.VampireKnives:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Throw life stealing daggers";
                        }
                    }
                    return;
   
            }
        }
    }
}
