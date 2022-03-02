
using TRAEProject.Buffs;
using TRAEProject.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

namespace TRAEProject.Changes.Weapons
{
    public class Ranged : GlobalItem
    {
        public override bool InstancePerEntity => true;
  
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void SetDefaults(Item item)
        {
			if (item.ammo > 0 && item.type != ItemID.SilverCoin && item.type != ItemID.GoldCoin && item.type != ItemID.CopperCoin && item.type != ItemID.PlatinumCoin && item.type != ItemID.PlatinumCoin && item.type != ItemID.Ale && item.type != ItemID.SandBlock)
            {
                item.maxStack = 9999;
            }
            switch (item.type)
            {

                /// RANGED 
                case ItemID.Boomstick:
                    item.damage = 11; // down from 14
                    return;
			    case ItemID.Beenade: 
                    item.damage = 8; // down from 12				
					item.useAnimation = 42; // up from 15
                    item.useTime = 42; // up from 15    
					item.shootSpeed = 12f; // up from 6
                    item.autoReuse = true;
					return;
                case ItemID.Revolver:
                    item.damage = 30; // up from 20
                    item.value = 250000; // 25 gold
                    return;
                case ItemID.Flamethrower:
                    item.damage = 14; // down from 35
                    item.shootSpeed = 5.33f; // down from 7
                    item.useAnimation = 60; // down from 30
                    item.useTime = 10; // up from 6
                    item.knockBack = 0.25f; // down from 0.3
                    return;
                case ItemID.EldMelter:
                    item.damage = 30;
                    item.useAnimation = 60; // up from 30
                    item.useTime = 6; // up from 6
                    return;
                case ItemID.Harpoon:
                    item.shoot = ProjectileType<Harpoon>();
                    item.shootSpeed = 22f;
                    item.useAnimation = 36;
                    item.useTime = 36;
                    return;
                case ItemID.FlintlockPistol:
                    item.shootSpeed = 7f;
<<<<<<< Updated upstream:Changes/Weapon/Ranged.cs
                    item.autoReuse = true; //These are minishark's stats, BUT this weapon doesn't autofire
                    return;
=======
                    item.autoReuse = true;
                    return;     
>>>>>>> Stashed changes:Changes/Weapon/Ranged/GeneralChanges.cs
                case ItemID.Minishark:
                    item.value = Item.buyPrice(gold: 50);
                    return; 
                case ItemID.BeesKnees:
                 item.damage = 16;
                    item.useAnimation = 26;
                    item.useTime = 26;
                    return;
                case ItemID.QuadBarrelShotgun:
                    item.value = Item.buyPrice(gold: 50);
                    item.autoReuse = true;
                    return;
                case ItemID.Gatligator:
                    item.value = Item.buyPrice(gold: 75);
                    return;
                case ItemID.Uzi:
                    item.value = Item.buyPrice(platinum: 1);
					   item.shootSpeed = 5f;
                    item.useTime = 7;
                    item.useAnimation = 7;
                    return;
                case ItemID.Handgun:
                    item.damage = 20; // up from 17
                    return;
                case ItemID.Toxikarp:
                    item.useTime = 14;
                    item.useAnimation = 14;
                    return;
                case ItemID.DaedalusStormbow:
                    item.damage = 30;
                    return;
                case ItemID.Marrow:
                    item.damage = 100;
                    item.crit = 20;
                    item.useTime = 50;
                    item.useAnimation = 50;
                    item.autoReuse = true;
                    return;
<<<<<<< Updated upstream:Changes/Weapon/Ranged.cs
                case ItemID.Megashark:
                    item.damage = 20; // down from 26
                    return;
=======

>>>>>>> Stashed changes:Changes/Weapon/Ranged/GeneralChanges.cs
                case ItemID.TheUndertaker:
                case ItemID.SniperRifle:
                    item.autoReuse = true;
                    return;
                case ItemID.JackOLanternLauncher:
                    item.shootSpeed = 14f; // up from 7
                    return;
                case ItemID.CandyCornRifle:
                    item.damage = 60; // up from 44
                    return;
                case ItemID.TacticalShotgun:
                    item.damage = 34; // up from 29
                    item.useTime = 32; // down from 34
                    item.useAnimation = 32; // down from 34
                    return;
                case ItemID.VenusMagnum:
                    item.damage = 100; // up from 50
                    item.useTime = 24; // up from 8
                    item.useAnimation = 24; // up from 8
                    item.autoReuse = true; 
                    return;
                case ItemID.StakeLauncher:
                    item.useTime = 15;
                    item.useAnimation = 15;
                    return;
                case ItemID.NailGun:
                    item.damage = 110; // up from 85
                    return;
                case ItemID.Tsunami:
                    item.damage = 79;
                    item.useTime = 40;
                    item.useAnimation = 40;
                    return;
                case ItemID.FairyQueenRangedItem:
                    item.damage = 40; // down from 50
                    return;
                case ItemID.ChainGun:
                    item.damage = 41; // up from 31
                    return;
                case ItemID.Phantasm:
                    item.damage = 40; // down from 50
                    return;
                case ItemID.VortexBeater:
                    item.damage = 42;
                    return;
                // AMMO
                case ItemID.MeteorShot:
                    item.damage = 7;
                    return;
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
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            switch (item.type)
            {
                case ItemID.Uzi:
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(40));
                        velocity.X = perturbedSpeed.X;
                        velocity.Y = perturbedSpeed.Y;
                        return true;
                    }                 
            }
            return true;
        }
        public override bool CanConsumeAmmo(Item weapon, Player player)
        {
            if (weapon.CountsAsClass(DamageClass.Ranged) && player.ammoPotion)
            {
                if (weapon.type != ItemID.StarCannon && weapon.type != ItemID.Clentaminator && weapon.type != ItemID.CoinGun)
                {
                    return false;
                }
            }
            if ((weapon.type == ItemID.Flamethrower || weapon.type == ItemID.EldMelter) && player.itemTime < player.itemTimeMax)
            {
                return false;
            }

            return true;
        }
        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
        {
            if (weapon.type == ItemID.VortexBeater && (ammo.type == ItemID.MusketBall || ammo.type == ItemID.EndlessMusketPouch))
            {
                type = ProjectileType<LilRocket>();
            }
        }
      
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {                
                case ItemID.VenusMagnum:       
				case ItemID.EldMelter:
				case ItemID.Flamethrower:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text += "\n33% chance to not consume ammo";
                        }
                    }
                    return;
                case ItemID.ChainGun:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text = "66% chance to not consume ammo";
                        }
                    }
                    return;            
            }
        }
    }
}
