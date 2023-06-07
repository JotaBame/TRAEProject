
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.FlamethrowerAmmo;

namespace TRAEProject.Changes.Weapons.Ranged
{
    public class Flamethrowers : GlobalItem
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
                case ItemID.Flamethrower:
                    item.damage = 9; // down from 35
                    item.shootSpeed = 5.5f; // down from 7
                    item.useAnimation = 40; // down from 30
                    item.useTime = 8; // up from 6
                    item.knockBack = 0.25f; // down from 0.3
                    item.shoot = ProjectileType<FlameP>();
                    break;
                case ItemID.ElfMelter:
                    item.damage = 24;
                    item.useAnimation = 30; // up from 30
                    item.useTime = 6; // up from 6
                    item.shoot = ProjectileType<FrostFlameP>();
                    break;
            }
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
        {
            if ((weapon.type == ItemID.Flamethrower || weapon.type == ItemID.ElfMelter))
            {
                return player.itemAnimation >= player.itemAnimationMax - 4;
            }

            return true;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.ElfMelter:
                case ItemID.Flamethrower:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "33% chance to not consume ammo";
                        }
                    }
                    return;
            }
        }
    }
    public class FlameP : FlamethrowerProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("CursedFlamethrower");     //The English name of the Projectile

        }
        public override string Texture => "Terraria/Images/Item_0";
        public override void FlamethrowerDefaults()
        {
            ColorMiddle = new Color(255, 80, 20, 200);
            ColorBack = new Color(255, 255, 20, 200);
            ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
            ColorSmoke = new Color(80, 80, 80, 100);
            dustID = DustID.Torch;
            Projectile.ArmorPenetration = 3;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.OnFire;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffDuration = 1200;
            Projectile.ai[0] = 1f;
            dieInWater = true;
        }
    }
    public class FrostFlameP : FlamethrowerProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("CursedFlamethrower");     //The English name of the Projectile

        }
        public override string Texture => "Terraria/Images/Item_0";
        public override void FlamethrowerDefaults()
        {
            ColorMiddle = new Color(255, 80, 20, 200);
            ColorBack = new Color(255, 255, 20, 200);
            ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
            ColorSmoke = new Color(80, 80, 80, 100);
            dustID = 135;
            Projectile.ArmorPenetration = 50;
            Projectile.penetrate = 5;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Frostburn2;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffDuration = 1200;
        }
    }
}