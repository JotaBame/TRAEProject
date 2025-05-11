
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
 using System;
using static System.Net.Mime.MediaTypeNames;
using TRAEProject.Common.ModPlayers;
using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;
using Humanizer;
using TRAEProject.Changes.NPCs.Boss.Plantera;

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
                case ItemID.Beenade:
                    item.damage = 8; // down from 12				
                    item.useAnimation = 45; // up from 15
                    item.useTime = 45; // up from 15    
                    item.shootSpeed = 12f; // up from 6
                    item.autoReuse = true;
                    return;


                case ItemID.Harpoon:
                    item.shoot = ProjectileType<Harpoon>();
                    item.shootSpeed = 22f;
                    item.useAnimation = 36;
                    item.useTime = 36;
                    return;


                case ItemID.Revolver:
                    item.damage = 25; // up from 20
                    item.value = Item.buyPrice(gold: 25);
                    return;
                case ItemID.Boomstick:
                    item.damage = 11; // down from 14
                    return;
                case ItemID.PewMaticHorn:
                    item.useTime = 17;
                    item.useAnimation = 17;
                    break;
                case ItemID.Minishark:
                    item.value = Item.buyPrice(gold: 35);
                    return;
                case ItemID.Shotgun:
                    item.value = Item.buyPrice(gold: 45);
                    item.damage = 13; //down from 24
                    item.useAnimation = 45; // up from 45
                    item.useTime = 45; // up from 45
                    item.rare = ItemRarityID.Green;

                    break;
                case ItemID.BeesKnees:
                    item.damage = 19; // down from 23
                    item.useAnimation = 25; // up from 23
                    item.useTime = 25; // up from 23
                    return;
                case ItemID.QuadBarrelShotgun:
                    item.damage = 19; // down from 21
                    item.value = Item.buyPrice(gold: 90);


                    return;
                case ItemID.SuperStarCannon:
                    item.damage = 95; // up from 60
                    item.useAnimation = 16; // down from 16
                    item.useTime = 16;
                    return;
                case ItemID.Gatligator:
                    item.damage = 18; // down from 21

                    item.useTime = 6; // down from 7
                    item.useAnimation = 6;
                    return;
                case ItemID.Uzi:
                    item.damage = 22; // down from 30
                    item.value = Item.buyPrice(gold: 75);
                    item.rare = ItemRarityID.LightRed;

                    return;
                case ItemID.Toxikarp:
                    item.useTime = 14;
                    item.useAnimation = 14;
                    return;
                //case ItemID.OnyxBlaster:
                //    item.useTime = 50; // up from 48
                //    item.useAnimation = 50;
                //    return;
                case ItemID.DaedalusStormbow:
                    item.damage = 30;
                    return;
                case ItemID.PearlwoodBow:
                    item.damage = 25; // up from 12
                    item.useTime = 17; // down from 20
                    item.useAnimation = 17;
                    item.knockBack = 1f; // up from 0
                    item.autoReuse = true;
                    item.rare = ItemRarityID.LightRed;
                    return;
                case ItemID.Marrow:
                    item.damage = 100;
                    item.crit = 20;
                    item.useTime = 50;
                    item.useAnimation = 50;
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
                    item.useTime = 34; // down from 34
                    item.useAnimation = 34; // down from 34
                    return;
                case ItemID.NailGun:
                    item.damage = 115; // up from 85
                    item.knockBack = 1f; // up from 0
                    return;
                case ItemID.Tsunami:
                    item.damage = 70;
                    item.useTime = 40;
                    item.useAnimation = 40;
                    return;
                case ItemID.FairyQueenRangedItem:
                    item.damage = 24;
                    item.useTime = 2;// unchanged
                    item.useAnimation = 10; // down from 30
                    item.reuseDelay = 10; // up from 0
                                          // note that vanilla doesnt use reuseDelay for this, for whatever reason
                    return;

                case ItemID.DD2BetsyBow:
                    item.damage = 31; // down from 39
                    return;
                    ;

                case ItemID.ChainGun:
                    item.damage = 41; // up from 31
                    return;

                // AMMO
                case ItemID.VenomBullet:
                    item.damage = 19; // up from 15
                    item.knockBack = 7f;
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

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
 
            if (item.type == ItemID.ChainGun) // chain gun
            {
                 float mouseX = (float)Main.mouseX + Main.screenPosition.X - position.X;
                float mouseY = (float)Main.mouseY + Main.screenPosition.Y - position.Y;
                if (player.gravDir == -1)
                    mouseY *= -1f;
                velocity = new Vector2(mouseX, mouseY); // we have to do this because vanilla has special rules to define this thing's spread
                velocity.Normalize();  
                float shootSpeed = item.shootSpeed;

                player.PickAmmo(player.inventory[player.selectedItem], out type, out shootSpeed, out damage, out knockback, out var usedAmmoItemId, true);
                velocity *= shootSpeed;
 
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(10) * player.GetModPlayer<RangedStats>().spreadModifier);


                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                return false;
            }


            if (item.type == ItemID.Gatligator) // gatligator
            {
                float mouseX = (float)Main.mouseX + Main.screenPosition.X - position.X;
                float mouseY = (float)Main.mouseY + Main.screenPosition.Y - position.Y;
                if (player.gravDir == -1)
                    mouseY *= -1f;
                velocity = new Vector2(mouseX, mouseY); // we have to do this because vanilla has special rules to define this thing's spread
                velocity.Normalize();
                float shootSpeed = item.shootSpeed;

                player.PickAmmo(player.inventory[player.selectedItem], out type, out shootSpeed, out damage, out knockback, out var usedAmmoItemId, true);
                velocity *= shootSpeed;
 
                Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(12) * player.GetModPlayer<RangedStats>().spreadModifier);
                if (Main.rand.NextBool(3) && player.GetModPlayer<RangedStats>().spreadModifier == 1f)
                {
                    perturbedSpeed.X *= 1f + (float)Main.rand.Next(-30, 31) * 0.02f;
                    perturbedSpeed.Y *= 1f + (float)Main.rand.Next(-30, 31) * 0.02f;
                }

                Projectile.NewProjectile(source, position, perturbedSpeed , type, damage, knockback, player.whoAmI);
                return false;
            }






            if (item.type == ItemID.TacticalShotgun) //tact shotgun
            {
                for (int num131 = 0; num131 < 6; num131++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(11f /*14f is practically the same as vanilla spread, this is intentionally lowered to 11f*/) * player.GetModPlayer<RangedStats>().spreadModifier); // 
                     Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                 }
                return false;

            }

            if (item.type == ItemID.Shotgun) // shotgun
            {
                int num67 = Main.rand.Next(4, 6);
                for (int num68 = 0; num68 < num67; num68++)
                {
                    float num69 = velocity.X;
                    float num70 = velocity.Y;
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(11) * player.GetModPlayer<RangedStats>().spreadModifier);
                    Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                }
                return false;
            }
            if (item.type == ItemID.Boomstick) // boomstick
            {
                int num98 = Main.rand.Next(3, 5);
                for (int num99 = 0; num99 < num98; num99++)
                {
                    float num100 = velocity.X;
                    float num101 = velocity.Y;
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10) * player.GetModPlayer<RangedStats>().spreadModifier);
                    Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                }
                return false;
            }
            if (item.type == ItemID.OnyxBlaster) // onyx blaster
            {
                Vector2 vector55 = new Vector2(velocity.X, velocity.Y);
                float num198 = (float)Math.PI / 4f;
                for (int num199 = 0; num199 < 2; num199++)
                {
                    Projectile.NewProjectile(source, position, vector55 + vector55.SafeNormalize(Vector2.Zero).RotatedBy(num198 * (Main.rand.NextFloat() * 0.5f + 0.5f)) * Main.rand.NextFloatDirection() * 2f * player.GetModPlayer<RangedStats>().spreadModifier, type, damage, knockback, player.whoAmI);
                    Projectile.NewProjectile(source, position, vector55 + vector55.SafeNormalize(Vector2.Zero).RotatedBy((0f - num198) * (Main.rand.NextFloat() * 0.5f + 0.5f)) * Main.rand.NextFloatDirection() * 2f * player.GetModPlayer<RangedStats>().spreadModifier, type, damage, knockback, player.whoAmI);
                }
                Projectile.NewProjectile(source, position, velocity * 1.3f, 661, damage * 2, knockback, player.whoAmI);
                return false;
            }

            if (item.type == ItemID.QuadBarrelShotgun) // quad barrel
            {
                 Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
                for (int num72 = 0; num72 < 7; num72++)
                {
                    Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(33) * player.GetModPlayer<RangedStats>().spreadModifier);
                    Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                }
                return false;
            }
            if (item.type == ItemID.Tsunami) // TSUNAMI
            {
                float num42 = (float)Math.PI / 10f;
                if (player.GetModPlayer<RangedStats>().spreadModifier < 1) // stacking scopes on this just looks dumb
                    num42 = (float)Math.PI / 10f / 3;
                int projCount = 5;
                Vector2 vector17 = new Vector2(velocity.X, velocity.Y);
                vector17.Normalize();
                vector17 *= 40f;
                bool flag4 = Collision.CanHit(position, 0, 0, position + vector17, 0, 0);
                for (int num44 = 0; num44 < projCount; num44++)
                {
                    float num45 = (float)num44 - ((float)projCount - 1f) / 2f;
                    Vector2 vector18 = vector17.RotatedBy(num42 * num45);
                    if (!flag4)
                    {
                        vector18 -= vector17;
                    }
                    Vector2 vector19 = position + vector18;
                    Vector2 vector20 = (vector19 - player.Center).SafeNormalize(Vector2.Zero);
                    if (!Collision.CanHitLine(player.MountedCenter, 4, 4, vector19 - new Vector2(velocity.X, velocity.Y), 0, 0))
                    {
                        vector19 -= vector20 * 15f;
                    }
                    int num46 = Projectile.NewProjectile(source, vector19.X, vector19.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
                    Main.projectile[num46].noDropItem = true;
                }
                return false;
            }

            if (item.type == ItemID.Uzi)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(10) * player.GetModPlayer<RangedStats>().spreadModifier);

                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
                return false;
            }
            if (item.type == ItemID.ChlorophyteShotbow)
            {
                int num109 = 2;
                if (Main.rand.NextBool(3))
                {
                    num109++;
                }
                for (int num110 = 0; num110 < num109; num110++)
                {
                    float num111 = velocity.X;
                    float num112 =  velocity.Y;
                    if (num110 > 0)
                    {
                        num111 += (float)Main.rand.Next(-35, 36) * 0.04f * player.GetModPlayer<RangedStats>().spreadModifier;
                        num112 += (float)Main.rand.Next(-35, 36) * 0.04f * player.GetModPlayer<RangedStats>().spreadModifier;
                    }
                    if (num110 > 1)
                    {
                        num111 += (float)Main.rand.Next(-35, 36) * 0.04f * player.GetModPlayer<RangedStats>().spreadModifier;
                        num112 += (float)Main.rand.Next(-35, 36) * 0.04f * player.GetModPlayer<RangedStats>().spreadModifier;
                    }
                    if (num110 > 2)
                    {
                        num111 += (float)Main.rand.Next(-35, 36) * 0.04f * player.GetModPlayer<RangedStats>().spreadModifier;
                        num112 += (float)Main.rand.Next(-35, 36) * 0.04f * player.GetModPlayer<RangedStats>().spreadModifier; ;
                    }
                    Projectile.NewProjectile(source, position.X, position.Y, num111, num112, type, damage, knockback, player.whoAmI);
                 }
                return false;
            }

            if (item.type == ItemID.DaedalusStormbow)
            {
     
                Vector2 vector6 = new Vector2(velocity.X, velocity.Y);
                float mouseX = (float)Main.mouseX + Main.screenPosition.X - position.X;
                float mouseY = (float)Main.mouseY + Main.screenPosition.Y - position.Y - 1000f;
               player.itemRotation = (float)Math.Atan2(vector6.Y * (float)player.direction, vector6.X * (float)player.direction);

                int num13 = 3;
  
                if (Main.rand.Next(3) == 0)
                {
                    num13++;
                }
                for (int k = 0; k < num13; k++)
                {
                    position = new Vector2(position.X + (float)player.width * 0.5f + (float)(Main.rand.Next(201) * -player.direction) + ((float)Main.mouseX + Main.screenPosition.X - position.X), player.MountedCenter.Y - 600f );
                   
                    position.X = (position.X * 10f + player.Center.X)  / 11f + (float)Main.rand.Next(-100, 101);
                     position.Y -= 150 * k;
                    velocity.X = (float)Main.mouseX + Main.screenPosition.X - position.X;
                    velocity.Y = (float)Main.mouseY + Main.screenPosition.Y - position.Y;
                    if (velocity.Y < 0f)
                    {
                        velocity.Y *= -1f;
                    }
                    if (velocity.Y < 20f)
                    {
                        velocity.Y = 20f;
                    }
                   float num6 = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
                    num6 =  item.shootSpeed / num6;
                    velocity.X *= num6;
                    velocity.Y *= num6;
                    float num14 = velocity.X + (float)Main.rand.Next(-40, 41) * 0.03f * player.GetModPlayer<RangedStats>().spreadModifier;

                    if (player.GetModPlayer<RangedStats>().spreadModifier == 1)
                    {
                        num14 *= (float)Main.rand.Next(75, 150) * 0.01f;
                    }
                    else
                        num14 = velocity.X;
                    float speedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.03f * player.GetModPlayer<RangedStats>().spreadModifier;
                    if (player.GetModPlayer<RangedStats>().spreadModifier == 1)
                        position.X += Main.rand.Next(-50, 51);
                    int num15 = Projectile.NewProjectile(item.GetSource_FromThis(), position.X, position.Y, num14, speedY, type, damage, knockback, player.whoAmI);
                    Main.projectile[num15].noDropItem = true;
                }
                return false;
       
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
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
                case ItemID.Phantasm:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\n'Death by a thousand phantoms'";
                        }
                    }
                    return;
                case ItemID.NanoBullet:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "Increases life regeneration after striking an enemy";
                        }
                    }
                    return;
            }
        }
    }
}
