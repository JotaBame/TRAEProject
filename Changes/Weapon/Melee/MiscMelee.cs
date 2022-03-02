
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
                    item.value = 50000;
                    return;
                case ItemID.ChainGuillotines:
                    item.crit = 20;
                    return;
                case ItemID.VampireKnives:
                    item.damage = 41; // up from 29
                    return;
                case ItemID.IceBoomerang:
                    item.damage = 22; // up from 16
                    item.crit = 12; // up from 6%
                    return;
                case ItemID.Flamarang:
                    item.damage = 45; // up from 32
                    return;
                case ItemID.LightDisc:
                    item.damage = 100; // up from 57
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
                    item.value = 50000;
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

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (item.type == ItemID.BreakerBlade)
            {
                if (target.life >= target.lifeMax * 0.9)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, target.position);
                    for (int i = 0; i < 20; ++i)
                    {
                        int Fire = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Torch, 0f, 0f, 100, default(Color), 3f);
                        Main.dust[Fire].noGravity = true;
                        Main.dust[Fire].velocity *= 4f;
                        int Fire2 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Torch, 0f, 0f, 100, default(Color), 2f);
                        Main.dust[Fire2].velocity *= 2f;
                    }
                    return;
                }
            }
        }
        
        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        { 
            if (player.HasBuff(BuffID.WeaponImbueNanites))
            {
                player.AddBuff(BuffType<NanoHealing>(), 60, false);
            }
            if (item.type == ItemID.Cutlass)
            {
                if (target.active && !target.dontTakeDamage && !target.friendly && target.lifeMax > 5 && !target.immortal && !target.SpawnedFromStatue)
                {
                    int amount = damage / 2;
                    player.QuickSpawnItem(player.GetItemSource_OnHit(target, item.type),ItemID.CopperCoin, amount);
                    return;
                }
            }
        }
        /// SHOOT STUFF
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            switch (item.type)
            {
                case ItemID.ChristmasTreeSword:
                    {
                        int chance = Main.rand.Next(5);
                        int numberOrnaments = 3 + Main.rand.Next(3, 4); // 4 or 5 shots
                        int numberLights = 1 + Main.rand.Next(1, 2); // 4 or 5 shots
                        for (int i = 0; i < numberLights; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(24));
                            Projectile.NewProjectile(player.GetProjectileSource_Item(item), position, perturbedSpeed * 0.95f, ProjectileType<LightsLong>(), damage, knockback * 1.5f, player.whoAmI);
                        }
                        for (int i = 0; i < numberOrnaments; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(30));
                            Projectile.NewProjectile(player.GetProjectileSource_Item(item), position, perturbedSpeed * 1.2f, ProjectileID.OrnamentFriendly, (int)(damage * 0.9), knockback, player.whoAmI);
                        }
                        if (chance == 0)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 57, 0f, 0f, 255, default, Main.rand.Next(20, 26) * 0.1f);
                                dust.noLight = true;
                                dust.noGravity = true;
                                dust.velocity *= 0.5f;
                            }
                            Projectile.NewProjectile(player.GetProjectileSource_Item(item), position, velocity * 1.8f, ProjectileType<Star1>(), (int)(damage * 1.8), knockback, player.whoAmI);
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item25);
                            return false;
                        }
                        return false;
                    }
            }
            return true;       
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.Cutlass:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.text += "\nCreates money on enemy hits";
                        }
                    }
                    return;
                case ItemID.PalladiumPike:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.text += "\nIncreases health regeneration after striking an enemy";
                        }
                    }
                    return;
                case ItemID.OrichalcumHalberd:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.text += "\nCreates damaging petals on contact";
                        }
                    }
                    return;
                case ItemID.ChristmasTreeSword:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Shoots christmas decorations";
                        }
                    }
                    return;
                case ItemID.Chik:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.text += "\nCauses an explosion of crystal shards on hit";
                        }
                    }
                    return;
                case ItemID.FormatC:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.text += "\nCharges power as it is held out";
                        }
                    }
                    return;
                case ItemID.Gradient:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.text += "\nFires Bones at enemies";
                        }
                    }
                    return;
                case ItemID.Kraken:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.text += "\nReleases a tentacle made out of lost souls while held out";
                        }
                    }
                    return;
                case ItemID.Cascade:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.text += "\nHighly Volatile";
                        }
                    }
                    return;
                case ItemID.Sunfury:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.text += "\nInflicts a heavy burn on enemies";
                        }
                    }
                    return;               
                case ItemID.VampireKnives:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Throw life stealing daggers";
                        }
                    }
                    return;
                case ItemID.ChainGuillotines:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "Heals on a critical hit";
                        }
                    }
                    return;
            }
        }
    }
}
