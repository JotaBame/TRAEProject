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
using TRAEProject.NewContent.Items.Weapons.Magic.ConfuseRay;
using Terraria.Audio;
using Terraria.GameContent;

namespace TRAEProject.NewContent.Items.Weapons.Magic.DreamEater
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
            Item.damage = 33;
            Item.useTime = 31;
            Item.useAnimation = 31;
            Item.mana = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 40);
            Item.shoot = ProjectileType<DreamEaterShot>();
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 6f;
            Item.shootSpeed = 9f;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item8;
        }
        int shotCount = 1;
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (shotCount == 1)
            {
                type = ProjectileType<DreamEaterShot1>();
                 return;
            }
 
            return;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            shotCount *= -1;
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
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
            Projectile.height = 38;
            Projectile.scale = 0.55f;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            DrawOffsetX = -7;
             Projectile.GetGlobalProjectile<ProjectileStats>().MaxBounces = 4;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 300;
         }
       
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            int num117 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width, Projectile.height, DustID.PinkTorch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 2f);
            Main.dust[num117].noGravity = true;
            Main.dust[num117].velocity.X *= 1f;
            Main.dust[num117].velocity.Y *= 1f;

 
        }
  
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //later change to only trigger on debuff hit
            DreamEaterDustHelper.DreamEaterShapeDust(Projectile.Center, 16f, 7f, DreamEaterDustHelper.PurpleDustID, 1f);

            if (Main.rand.NextBool(8))
             {
                SoundEngine.PlaySound(SoundID.Item45 with { MaxInstances = 0 });
                for (int i = 0; i < 25; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 3f);
                    Dust d = Dust.NewDustPerfect(target.Center, DustID.Shadowflame, speed * 4, Scale: 1.5f);
                    d.noGravity = true;
                }
                for (int i = 0; i < 25; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(2f, 2f);
                    Dust d = Dust.NewDustPerfect(target.Center, DustID.PinkTorch, speed * 2, Scale: 1.5f);
                    d.noGravity = true;
                }
                target.AddBuff(BuffID.ShadowFlame, 150);
            }            
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.Center);
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.Shadowflame, 1f);
                dust.noGravity = true;
            }
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, 179, 1f);
                dust.noGravity = true;
            }
        }
    }
    public class DreamEaterShot1 : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.scale = 0.55f;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            DrawOffsetX = -7;
            Projectile.GetGlobalProjectile<ProjectileStats>().MaxBounces = 4;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            int num117 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width, Projectile.height, DustID.PurpleTorch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 2f);
            Main.dust[num117].noGravity = true;
            Main.dust[num117].velocity.X *= 1f;
            Main.dust[num117].velocity.Y *= 1f;


        }
 
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //later change to only trigger on debuff hit
            DreamEaterDustHelper.DreamEaterShapeDust(Projectile.Center, 16f, 7f, DreamEaterDustHelper.PurpleDustID, 1f);

            if (Main.rand.NextBool(8))
            {

                SoundEngine.PlaySound(SoundID.Item45 with { MaxInstances = 0 });
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 3f);
                    Dust d = Dust.NewDustPerfect(target.Center, DustID.Shadowflame, speed * 4, Scale: 1.5f);
                    d.noGravity = true;
                }
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(2f, 2f);
                    Dust d = Dust.NewDustPerfect(target.Center, DustID.PinkTorch, speed * 2, Scale: 1.5f);
                    d.noGravity = true;
                }
                target.AddBuff(BuffID.ShadowFlame, 150);
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.Center);
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.Shadowflame, 1f);
                dust.noGravity = true;
            }
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, 179, 1f);
                dust.noGravity = true;
            }
        }
    }
}