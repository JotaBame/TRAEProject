
using TRAEProject.Buffs;
using TRAEProject.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

namespace TRAEProject.Changes.Weapon
{
    public class PreHardmodeSwords : GlobalItem
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
                // WOODEN 
                case ItemID.WoodenSword:
                    item.damage = 9; // up from 7
                    item.scale = 1.15f; // up from 1
                    item.useTime = 22; // down from 25
                    item.useAnimation = 22;
                    return;
                case ItemID.BorealWoodSword:
                case ItemID.PalmWoodSword:
                    item.damage = 10; // up from 8
                    item.scale = 1.15f; // up from 1
                    item.useTime = 22; // down from 23
                    item.useAnimation = 22;
                    return;
                case ItemID.RichMahoganySword:
                case ItemID.EbonwoodSword:
                case ItemID.ShadewoodSword:
                    item.damage = 12; // up from 10
                    item.scale = 1.15f; // up from 1
                    return;
                case ItemID.PearlwoodSword:
                    item.scale = 1.15f; // up from 1
                    return;
                case ItemID.CactusSword:
                    item.damage = 9; // up from 8
                    item.useTime = 25; // down from 32
                    item.useAnimation = 25;
                    return;
                // ORES
                case ItemID.CopperBroadsword:
                    item.damage = 13;
                    item.scale = 1.25f;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    return;
                case ItemID.TinBroadsword:
                    item.damage = 14;
                    item.scale = 1.25f;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    return;
                case ItemID.IronBroadsword:
                    item.damage = 15;
                    item.scale = 1.25f;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    return;
                case ItemID.LeadBroadsword:
                    item.damage = 16;
                    item.scale = 1.25f;
                    item.useAnimation = 20;
                    item.useTime = 20;
                    return;
                case ItemID.SilverBroadsword:
                    item.damage = 17;
                    item.scale = 1.25f;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    return;
                case ItemID.TungstenBroadsword:
                    item.damage = 18;
                    item.scale = 1.25f;
                    item.useTime = 20;
                    item.useAnimation = 20;
                    return;
                case ItemID.GoldBroadsword:
                    item.damage = 19;
                    item.scale = 1.25f;
                    item.useTime = 18;
                    item.useAnimation = 18;
                    return;
                case ItemID.PlatinumBroadsword:
                    item.damage = 20;
                    item.scale = 1.25f;
                    item.useTime = 18;
                    item.useAnimation = 18;
                    return;
                
                    
                    // SHORTSWORDS
                
                
                case ItemID.CopperShortsword:
                case ItemID.TinShortsword:
                case ItemID.IronShortsword:
                case ItemID.LeadShortsword:
                case ItemID.SilverShortsword:
                case ItemID.TungstenShortsword:
                case ItemID.GoldShortsword:
                case ItemID.PlatinumShortsword:
                    item.autoReuse = true;
                    return;
                case ItemID.Gladius:
                    item.autoReuse = true;
                    item.knockBack = 5f; // up from 0.5
                    return;
                
                    
                    // OTHER
               
                
                
                case ItemID.AntlionClaw:
                    item.useTime = 11;
                    item.useAnimation = 11;
                    return;
                case ItemID.ZombieArm:
                    item.damage = 15; // up from 12
                    item.useTime = 20;
                    item.useAnimation = 20;
                    item.knockBack = 8.25f;
                    item.autoReuse = true;
                    return;
                case ItemID.BoneSword:
                    item.damage = 24;
                    item.scale = 1.33f;
                    item.value = 500000;
<<<<<<< Updated upstream:Changes/Weapon/PreHardmodeSwords.cs
                    return;
=======
                    break;
                //case ItemID.Beekeeper:
                //    item.damage = 24;
                //    item.scale = 1.33f;
                //    item.value = 500000;
                //    break;
>>>>>>> Stashed changes:Changes/Weapon/Melee/PreHardmodeSwords.cs
                //phaseblades
                case ItemID.PurplePhaseblade:
                    item.damage = 29;
                    item.crit = 8;
                    item.knockBack = 0.1f;
                    item.useTime = 25;
                    item.useAnimation = 25;
                    item.shoot = ProjectileType<Blank>();
                    item.autoReuse = true;
                    item.useTurn = false;
                    return;
                case ItemID.YellowPhaseblade:
                    item.damage = 29;
                    item.crit = 10;
                    item.knockBack = 0.1f;
                    item.useTime = 25;
                    item.useAnimation = 25;
                    item.shoot = ProjectileType<Blank>();
                    item.autoReuse = true;
                    item.useTurn = false;
                    return;
                case ItemID.BluePhaseblade:
                    item.damage = 29;
                    item.crit = 12;
                    item.knockBack = 0.1f;
                    item.useTime = 25;
                    item.useAnimation = 25;
                    item.shoot = ProjectileType<Blank>();
                    item.autoReuse = true;
                    item.useTurn = false;
                    return;
                case ItemID.GreenPhaseblade:
                    item.damage = 29;
                    item.crit = 14;
                    item.knockBack = 0.1f;
                    item.useTime = 25;
                    item.useAnimation = 25;
                    item.autoReuse = true;
                    item.useTurn = false;
                    return;
                case ItemID.RedPhaseblade:
                    item.damage = 29;
                    item.crit = 16;
                    item.knockBack = 0.1f;
                    item.useTime = 25;
                    item.useAnimation = 25;
                    item.autoReuse = true;
                    item.useTurn = false;
                    return;
                case ItemID.OrangePhaseblade:
                    item.damage = 29;
                    item.crit = 17;
                    item.knockBack = 0.1f;
                    item.useTime = 25;
                    item.useAnimation = 25;
                    item.autoReuse = true;
                    item.useTurn = false;
                    return;
                case ItemID.WhitePhaseblade:
                    item.damage = 29;
                    item.crit = 18;
                    item.knockBack = 0.1f;
                    item.useTime = 25;
                    item.useAnimation = 25;
                    item.autoReuse = true;
                    item.useTurn = false;
                    return;


                // NIGHT'S EDGE & COMPONENTS
                
                
                case ItemID.LightsBane:
                    item.damage = 21;
                    item.useTime = 17;
                    item.useAnimation = 17;
                    item.autoReuse = true;
                    item.shoot = ProjectileType<Blank>();
                    return;
                case ItemID.BloodButcherer:
                    item.damage = 24;
                    item.useTime = 25;
                    item.useAnimation = 25;
                    item.shoot = ProjectileType<Blank>();
                    return;
                case ItemID.Katana:
                case ItemID.Muramasa:
                    item.crit = 20;
                    item.useTime = 16;
                        item.useAnimation = 16;
                    item.shoot = ProjectileType<Blank>();
                    item.useTurn = false;
                    return;
                case ItemID.BladeofGrass:
                    item.useTime = 30;
                    item.useAnimation = 30;
                    item.shoot = ProjectileType<Blank>();
                    return;
                case ItemID.FieryGreatsword:
                    item.useTime = 42;
                    item.useAnimation = 42;
                    item.autoReuse = true;
                    return;
                case ItemID.NightsEdge:
                    item.shoot = ProjectileType<NightsBeam>();
                    item.damage = 38;
                    item.useTime = 60;
                    item.useAnimation = 30;
                    item.autoReuse = true;
                    item.shootSpeed = 7f;
                    return;
                case ItemID.PurpleClubberfish:
                    item.scale = 1.4f;
                    item.damage = 34;
                    item.autoReuse = true;
                    return;
            }
            return;
        }
        
        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {          
            if (item.type == ItemID.BloodButcherer)
            {
                if (Main.rand.Next(2) == 0) // 50% chance
                {
                    player.HealEffect(1, true);
                    player.statLife += 1;
                    return;
                }
            }
            if (item.type == ItemID.BladeofGrass)
            {
                if (Main.rand.Next(3) == 0)
                {
                    float PositionX = player.position.X - Main.rand.Next(-50, 50);
                    float PositionY = player.position.Y - Main.rand.Next(-50, 50);
                    Projectile.NewProjectile(player.GetProjectileSource_Item(item), PositionX, PositionY, 0, 0, ProjectileID.SporeTrap, damage, knockBack, player.whoAmI);
                    return;
                }
            }
            if (item.type == ItemID.FieryGreatsword)
            {
                target.AddBuff(BuffType<Heavyburn>(), 120, false);
                return;
            }
        }
        /// SHOOT STUFF

<<<<<<< Updated upstream:Changes/Weapon/PreHardmodeSwords.cs
      
        public override bool Shoot(Item item, Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
=======
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
>>>>>>> Stashed changes:Changes/Weapon/Melee/PreHardmodeSwords.cs
        {
            Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            switch (item.type)
            {
                case ItemID.NightsEdge:
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8);
                        return true;
            }
            return true;       
        }
        public override void HoldItem(Item item, Player player)
        {
            if (item.type == ItemID.NightsEdge)
            {
                if (player.itemTime == 1 && player.whoAmI == Main.myPlayer)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.MaxMana);
                    for (int i = 0; i < 5; i++)
                    {
                        Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 173, 0f, 0f, 255, default, Main.rand.Next(20, 26) * 0.1f);
                        dust.noLight = true;
                        dust.noGravity = true;
                        dust.velocity *= 0.5f;
                    }
                    return;
                }
            }
            return;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.BloodButcherer:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Material")
                        {
                            line.text += "\nChance to heal the user on contact";
                        }
                    }
                    return;
                case ItemID.BladeofGrass:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.text += "\nCreates a spore on contact";
                        }
                    }
                    return;
            }
        }
    }
}
