using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRAEProject.Changes.Items;
using Terraria;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Weapon
{
    class HardmodeSwords : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public bool FetidHeal = false;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void SetDefaults(Item item)
        {
            switch(item.type)
            {
                case ItemID.CobaltSword:
                    item.damage = 42; // up from 39
                    item.useTime = 16; // down from 22
                    item.useAnimation = 16;  // down from 22
                    item.scale = 1.59f;
                    item.useTurn = false;
                    item.crit = 40;
                    item.SetNameOverride("Cobalt Katana");
                    break;
                case ItemID.PalladiumSword:
                    item.damage = 61; // up from 45
                    item.useTime = 20;
                    item.useAnimation = 20;
                    item.scale = 1.67f;
                    item.useTurn = false;
                    item.SetNameOverride("Palladium Falcata");
                    break;
                case ItemID.MythrilSword:
                    item.damage = 140;
                    item.useTime = 40;
                    item.useAnimation = 40;
                    item.scale = 2.4f;
                    item.SetNameOverride("Mythril Zweihänder");
                    break;
                case ItemID.OrichalcumSword:
                    item.damage = 50;
                    item.useTime = 25;
                    item.useAnimation = 25;
                    item.scale = 1.66f;
                    item.SetNameOverride("Orichalcum Flamberge");
                    break;
                case ItemID.AdamantiteSword:
                    item.damage = 60;
                    item.useTime = 17;
                    item.useAnimation = 17;
                    item.scale = 1.75f;
                    item.SetNameOverride("Adamantite Broadsword");
                    break;
                case ItemID.TitaniumSword:
                    item.scale = 1.75f;
                    item.SetNameOverride("Titanium Falchion");
                    break;
                case ItemID.Excalibur:
                case ItemID.TrueExcalibur:
                    item.damage = 70;
                    item.useTime = 16;
                    item.useAnimation = 16;
                    item.scale = 1.8f;
                    break;
                case ItemID.ChlorophyteSaber:
                    item.damage = 50;
                    item.scale = 1.75f;
                    item.useTime = 16;
                    item.useAnimation = 16;
                    item.useTurn = false;
                    item.shoot = ProjectileID.None;
                    break;
                case ItemID.ChlorophyteClaymore:
                    item.damage = 150;
                    item.useTime = 50;
                    item.useAnimation = 40;
                    item.scale = 2.4f;
                    item.autoReuse = true;
                    item.useTurn = false;
                    break;
                case ItemID.BeamSword: // REVISIT
                    item.useTime = 60;
                    item.useAnimation = 15;
                    item.autoReuse = true;
                    item.useTurn = false;
                    return;
                case ItemID.TrueNightsEdge: // REVISIT
                    item.autoReuse = true;
                    item.damage = 100;
                    item.useTime = 30;
                    item.useAnimation = 30;      
					item.scale = 1.75f;
                    return;
                //phasesabars
                case ItemID.PurplePhasesaber:
                case ItemID.YellowPhasesaber:
                case ItemID.BluePhasesaber:
                case ItemID.GreenPhasesaber:
                case ItemID.RedPhasesaber:
                case ItemID.OrangePhasesaber:
                case ItemID.WhitePhasesaber:
                    item.damage = 60;
                    item.crit = 24;
                    item.knockBack = 1f;
                    item.useTime = 17;
                    item.useAnimation = 17;
                    item.autoReuse = true;
                    item.useTurn = false;
                    break;
                case ItemID.BreakerBlade:
                    item.scale = 1.5f; // up from 1.05f
                    item.damage = 80;
                    item.useTime = 60;
                    item.useAnimation = 60;
                    item.height = 90;
                    item.width = 80;
                    break;
                case ItemID.Cutlass:
                    //item.damage = 60;
                    //item.knockBack = 1f;
                    //item.useTime = 13;
                    //item.useAnimation = 13;
                    //item.autoReuse = true;
                    //item.useTurn = false;
                    break;
                case ItemID.Seedler:
                    item.scale = 1.3f;
                    //item.useTime = 64; // up from 23
                    //item.useAnimation = 16; // down from 23
                    break;
                case ItemID.Keybrand:
                    item.scale = 1.7f;
                    item.damage = 100;
                    break;
                case ItemID.TerraBlade: // REVISIT
                    item.damage = 100;
                    item.scale = 1.6f;
                    item.useTime = 16;
                    item.useAnimation = 16;
                    break;
                case ItemID.TheHorsemansBlade:
                    item.damage = 150;
                    item.scale = 1.75f;
                    break;
			case ItemID.DD2SquireDemonSword:
                    item.useTurn = false;
                    item.scale = 1.7f;
                    item.shoot = ProjectileType<Blank>();
                    return;
                case ItemID.FetidBaghnakhs:
                    FetidHeal = true;
                    item.useTurn = false;
                    item.shoot = ProjectileType<Blank>();
                    item.damage = 60;
                    item.useTime = 12;
                    item.useAnimation = 12;
                    return;
                case ItemID.PsychoKnife:
                    FetidHeal = true;
                    item.shoot = ProjectileType<Blank>();
                    item.useTime = 12;
                    item.useAnimation = 12;
                    return;
                case ItemID.ChristmasTreeSword: // REVISIT
                    item.useTime = 31;
                    item.damage = 76;
                    item.useAnimation = 31;
                    item.knockBack = 4f;
                    item.autoReuse = true;
                    return;
                case ItemID.DD2SquireBetsySword:
                    item.scale = 1.7f;
                    return;
            }
        }
        public override bool? CanHitNPC(Item item, Player player, NPC target)
        {
            if(item.type == ItemID.TheHorsemansBlade)
            {
                if(target.SpawnedFromStatue)
                {
                    return false;
                }
            }
            return base.CanHitNPC(item, player, target);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (FetidHeal && player.GetModPlayer<OnHitItems>().BaghnakhHeal <= player.GetModPlayer<OnHitItems>().LastHitDamage)
            {
                float healAmount = (float)(damage * 0.1f);
                player.GetModPlayer<OnHitItems>().BaghnakhHeal += (int)healAmount;
                player.HealEffect((int)healAmount, true);
                player.statLife += (int)healAmount;
            }
            switch (item.type)
            {
                case ItemID.PalladiumSword:
                    player.AddBuff(BuffID.RapidHealing, 300);
                    return;
                case ItemID.OrichalcumSword:
                    int direction = player.direction;
                    float k = Main.screenPosition.X;
                    if (direction < 0)
                    {
                        k += (float)Main.screenWidth;
                    }
                    float y2 = Main.screenPosition.Y;
                    y2 += (float)Main.rand.Next(Main.screenHeight);
                    Vector2 vector = new Vector2(k, y2);
                    float num2 = target.Center.X - vector.X;
                    float num3 = target.Center.Y - vector.Y;
                    num2 += (float)Main.rand.Next(-50, 51) * 0.1f;
                    num3 += (float)Main.rand.Next(-50, 51) * 0.1f;
                    float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
                    num4 = 24f / num4;
                    num2 *= num4;
                    num3 *= num4;
                    Projectile.NewProjectile(player.GetProjectileSource_SetBonus(5), k, y2, num2, num3, 221, 36, 0f, player.whoAmI);
                    return;
                case ItemID.ChlorophyteSaber:
                    Projectile.NewProjectile(player.GetProjectileSource_Item(item), target.Center, TRAEMethods.PolarVector(Main.rand.NextFloat() * 2f, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI)), ProjectileID.SporeCloud, (int)(.8f * damage), 0, player.whoAmI);
                    break;
            }
        }
        public override void HoldItem(Item item, Player player)
        {
            switch (item.type)
            {
                case ItemID.TitaniumSword:
                    player.onHitTitaniumStorm = true;
                    return;
            }
        }
    }
}
