using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.Changes;
using System;
using TRAEProject.Changes.Projectiles;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes.Items;
using Microsoft.Build.Construction;

namespace TRAEProject.NewContent.Items.Weapons.Magic.OnyxCurseDoll
{
    public class DreamEater : ModItem
    {
 
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Onyx Curse Doll");
            // Tooltip.SetDefault("Summons 3 fireballs to circle around you\nThe fireballs will drain 50 mana per second, affected by gear\nThey will curse nearby enemies, causing damage over time, lower damage or defense\nRight-click to uncast ");
        }
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 42;
            Item.damage = 38;
            Item.useAnimation = 27;
            Item.useTime = 27;
            Item.mana = 30;

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 40);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 4f;
            Item.shootSpeed = 7f;
            Item.noMelee = true;
            Item.shoot = ProjectileType<CurseDollWeaponflame>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item20;
 
        }
 
 
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Ebonwood, 10)
                .AddIngredient(ItemID.ShadowScale, 15)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddTile(TileID.Anvils)
                .Register();
			CreateRecipe(1).AddIngredient(ItemID.Shadewood, 10)
                .AddIngredient(ItemID.TissueSample, 15)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class DreamEaterShot : ModProjectile
    {
 
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 64;
            Projectile.scale = 0.5f;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 1;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.ShadowFlame;
			Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffChance = 10;
            Projectile.GetGlobalProjectile<ProjectileStats>().MaxBounces = 1;
            Projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 1800;
            Projectile.tileCollide = false;
        }
       
        public override void AI()
        {
 
             if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 1, Projectile.velocity.Y * -0.33f, 0, default, 0.7f);

            }
        }
        public override void OnKill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.Center);
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, 179, 1f);
                dust.noGravity = true;
            }
        }
    }
 
}