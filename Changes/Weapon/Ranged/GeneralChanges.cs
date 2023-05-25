
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

namespace TRAEProject.Changes.Weapons
{
    public class RangedItems : GlobalItem
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

                case ItemID.Boomstick:
                    item.damage = 11; // down from 14
                    return;
                case ItemID.Beenade:
                    item.damage = 8; // down from 12				
                    item.useAnimation = 45; // up from 15
                    item.useTime = 45; // up from 15    
                    item.shootSpeed = 12f; // up from 6
                    item.autoReuse = true;
                    return;
                case ItemID.Revolver:
                    item.damage = 30; // up from 20
                    item.value = 250000; // 25 gold
                    return;

                case ItemID.Harpoon:
                    item.shoot = ProjectileType<Harpoon>();
                    item.shootSpeed = 22f;
                    item.useAnimation = 36;
                    item.useTime = 36;
                    return;
                case ItemID.PewMaticHorn:
                    item.useTime = 17;
                    item.useAnimation = 17;
                    break;
                case ItemID.BeesKnees:
                    item.damage = 19;
                    return;
                case ItemID.Minishark:
                    item.value = Item.buyPrice(gold: 35);
                    return;
                case ItemID.QuadBarrelShotgun:
                    item.value = Item.buyPrice(gold: 50);
                    item.autoReuse = true;
                    return;
                case ItemID.Gatligator:
                    item.damage = 18; // down from 21

                    item.useTime = 7; // down from 7
                    item.useAnimation = 7;
                    item.value = Item.buyPrice(platinum: 1, gold: 50);
                    return;
                case ItemID.Uzi:
                    item.damage = 22; // up from 30
                    item.value = Item.buyPrice(platinum: 1, gold: 50);
                    item.shootSpeed = 5f;
                    item.useTime = 7;
                    item.useAnimation = 7;
                    return;
                case ItemID.Toxikarp:
                    item.useTime = 14;
                    item.useAnimation = 14;
                    return;
                case ItemID.DaedalusStormbow:
                    item.damage = 30;
                    return;
                case ItemID.PearlwoodBow:
                    item.damage = 25; // up from 12
                    item.useTime = 17; // down from 20
                    item.useAnimation = 17;
                    item.knockBack = 1f; // up from 0
                    item.autoReuse = true;
                    item.rare = ItemRarityID.Pink;
                    return;
                case ItemID.Marrow:
                    item.damage = 100;
                    item.crit = 20;
                    item.useTime = 50;
                    item.useAnimation = 50;
                    item.autoReuse = true;
                    return;
                case ItemID.TheUndertaker:
                    item.autoReuse = true;
                    return;
                case ItemID.JackOLanternLauncher:
                    item.shootSpeed = 14f; // up from 7
                    return;
                case ItemID.CandyCornRifle:
                    item.damage = 60; // up from 44
                    return;
                case ItemID.SniperRifle:
                    item.damage = 200; // up from 185
                    item.autoReuse = true;
                    return;
                case ItemID.TacticalShotgun:
                    item.damage = 38; // up from 29
                    item.useTime = 29; // down from 34
                    item.useAnimation = 29; // down from 34
                    return;
                case ItemID.NailGun:
                    item.damage = 115; // up from 85
                    item.knockBack = 1f; // up from 0
                    return;
                case ItemID.Tsunami:
                    item.damage = 79;
                    item.useTime = 40;
                    item.useAnimation = 40;
                    return;
                case ItemID.Phantasm:
                    item.damage = 60;
                    return;
                case ItemID.FairyQueenRangedItem:
                    item.damage = 40; // down from 50
                    return;
                case ItemID.ChainGun:
                    item.damage = 41; // up from 31
                    return;

                case ItemID.VortexBeater:
                    item.damage = 42;
                    return;
                // AMMO
                case ItemID.CrystalBullet:
                    item.damage = 7; // down from 9
                    return;
                case ItemID.NanoBullet:
                    item.damage = 10; // unchanged
                    item.knockBack = 7f;
                    return;
                case ItemID.MoonlordBullet:
                    item.damage = 50; // up from 20
                    return;
                case ItemID.FlamingArrow:
                    item.shootSpeed = 5f;
                    item.knockBack = 6f;
                    return;
                case ItemID.UnholyArrow:
                case ItemID.HolyArrow:
                    item.damage = 10;
                    return;
                case ItemID.HellfireArrow:
                    item.damage = 14;
                    item.shootSpeed = 3.5f;
                    return;
                case ItemID.ChlorophyteArrow:
                    item.damage = 10; // down from 16
                    item.knockBack = 2f; // down from 3.5
                    return;
            }
        }
        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
        {
            if (weapon.type == ItemID.VortexBeater && (ammo.type == ItemID.MusketBall || ammo.type == ItemID.SilverBullet || ammo.type == ItemID.TungstenBullet || ammo.type == ItemID.EndlessMusketPouch))
            {
                type = ProjectileType<LilRocket>();
            }
            if (weapon.type == ItemID.PearlwoodBow && (ammo.type == ItemID.WoodenArrow || ammo.type == ItemID.EndlessQuiver))
            {
                type = ProjectileID.HolyArrow;
            }
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (item.type == ItemID.PhoenixBlaster)
            {
                return true;
            }
            return base.AltFunctionUse(item, player);
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.PhoenixBlaster)
            {
                if (player.altFunctionUse == 2)
                {
                    item.useAmmo = AmmoID.Flare;
                }
                if (player.altFunctionUse != 2)
                {
                    item.useAmmo = AmmoID.Bullet;
                }
            }
            return base.CanUseItem(item, player);
        }


        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.PearlwoodBow:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nWooden arrows turn into Holy arrows";
                        }
                    }
                    return;
                case ItemID.PhoenixBlaster:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nRight Click to shoot flares";
                        }
                    }
                    return;
                case ItemID.VenusMagnum:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n33% chance to not consume ammo";
                        }
                    }
                    return;
                case ItemID.ChainGun:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "66% chance to not consume ammo";
                        }
                    }
                    return;
                case ItemID.VortexBeater:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nConverts Musket Balls into homing rockets";
                        }
                    }
                    return;
            }
        }
    }
}
