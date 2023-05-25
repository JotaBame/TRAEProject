
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.NewContent.TRAEDebuffs;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.FlamethrowerAmmo
{
    public class CrystalGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Gel");
            // Tooltip.SetDefault("Splits and ignores 25 defense");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99; AmmoID.Sets.IsSpecialist[Item.type] = true;

        }
        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Pink;
            Item.width = 24;
            Item.height = 22;
            Item.shootSpeed = 0f;
            Item.consumable = true;
            Item.shoot = ProjectileType<CrystalGelP>();
            Item.ammo = AmmoID.Gel;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.GelBalloon)
                .AddIngredient(ItemID.Gel, 20)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
    public class CrystalGelP : FlamethrowerProjectile
    {
        public override string Texture => "Terraria/Images/Item_0";
        public override void FlamethrowerDefaults()
        {
            dustID = DustID.BlueTorch;
            scalemodifier = 0.9f;
            dustScale = 0.5f;
            ColorMiddle = new Color(82, 158, 218, 75);
            ColorBack = new Color(91, 163, 245, 75);
            ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
            ColorSmoke = new Color(60, 80, 115, 75);
            Projectile.ArmorPenetration = 25;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.15f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            float rotation = MathHelper.ToRadians(120);
            for (int i = 0; i < 2; ++i)
            {
                int projToShoot = i == 0 ? ProjectileType<CrystalGelSplitPink>() : ProjectileType<CrystalGelSplitPurple>();
                Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (2 - 1))) * 0.2f; // Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X + perturbedSpeed.X, Projectile.velocity.Y + perturbedSpeed.Y, projToShoot, (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0f, ai2: 1f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.buffImmune[BuffID.OnFire])
            {
                TRAEDebuff.Apply<CrystalFire>(target, 300, 1);
            }
        }
        public override void AI()
        {
            Color color = new Color(100, 100, 255);
            CrystalGelSparkleSpawning(60, color, this, dustScale, 0.17f);
            base.AI();
        }

        public static void CrystalGelSparkleSpawning(float timeBeforeItSlowsDown, Color color, FlamethrowerProjectile flame, float dustScale, float sparkleChancePerFrame)
        {
            if (Main.rand.NextFloat() > sparkleChancePerFrame)
                return;
            color.A = 100;
            Sparkle sparkle = Sparkle.NewSparkle(flame.Projectile.Center + Main.rand.NextVector2Circular(60, 60) * Utils.Remap(flame.Projectile.localAI[0], 0f, 72f, 0.5f, 1f), color, new Vector2(0.7f, 1.15f) * dustScale * 0.8f, flame.Projectile.velocity * 0.2f + new Vector2(0, 0.9f), 35, Vector2.One * dustScale * 2f, -flame.Projectile.velocity * 0.01f + new Vector2(0, -0.15f), 1, 0, 0.97f);
            if (Main.rand.NextBool(4))
            {
                sparkle.Scale *= 2f;
                sparkle.Velocity *= 2f;
            }
            sparkle.Scale *= 1.5f;
            sparkle.Velocity *= 1.2f;
            sparkle.Velocity += flame.Projectile.velocity * 1f * Utils.Remap(flame.Projectile.localAI[0], 0f, 60 * 0.75f, 1f, 0.1f) * Utils.Remap(flame.Projectile.localAI[0], 0f, (float)timeBeforeItSlowsDown * 0.1f, 0.1f, 1f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            base.PreDraw(ref lightColor);
            return false;
        }
    }
    public class CrystalGelSplitPurple : FlamethrowerProjectile
    {
        public override string Texture => "Terraria/Images/Item_0";
        public override void FlamethrowerDefaults()
        {
            dustID = DustID.PurpleTorch;
            scalemodifier = 0.5f; 

            ColorMiddle = new Color(154, 72, 183, 100);
                ColorBack = new Color(194, 89, 255, 100);
                ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
                ColorSmoke = new Color(75, 40, 75, 100);
            dustScale = 0.5f;
            Projectile.ArmorPenetration = 25;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.15f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.buffImmune[BuffID.OnFire])
            {
                TRAEDebuff.Apply<CrystalFire>(target, 300, 1);
            }
        }
        public override void AI()
        {
            Color color = new Color(128, 0, 255);
            float timeBeforeItSlowsDown = 60;
            CrystalGelP.CrystalGelSparkleSpawning(timeBeforeItSlowsDown, color, this, dustScale, 0.1f);
            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            base.PreDraw(ref lightColor);
            return false;
        }
    }
    public class CrystalGelSplitPink : FlamethrowerProjectile
    {
        public override string Texture => "Terraria/Images/Item_0";
        public override void FlamethrowerDefaults()
        {
            dustID = DustID.PinkTorch;
            scalemodifier = 0.5f;
            dustScale = 0.5f;

            ColorMiddle = new Color(255, 88, 178, 100);
                ColorBack = new Color(254, 132, 231, 100);
                ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
                ColorSmoke = new Color(100, 60, 90, 100);         
            Projectile.ArmorPenetration = 25;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.15f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.buffImmune[BuffID.OnFire])
            {
                TRAEDebuff.Apply<CrystalFire>(target, 300, 1);
            }
        }
        public override void AI()
        {
            base.AI();
            Color color = Color.Magenta;
            float timeBeforeItSlowsDown = 60;
            color.A = 100;
            CrystalGelP.CrystalGelSparkleSpawning(timeBeforeItSlowsDown, color, this, dustScale, 0.1f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            base.PreDraw(ref lightColor);
            return false;
        }
    }
}



