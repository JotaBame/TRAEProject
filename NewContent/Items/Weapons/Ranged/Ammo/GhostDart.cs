using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.NewContent.TRAEDebuffs;
using TRAEProject.Changes.Accesory;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Ammo
{
    public class GhostDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ghost Dart");
            // Tooltip.SetDefault("Goes through tiles and enemies\nSplits into 6 when close to a target");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 14;
            Item.height = 28;
            Item.shootSpeed = 4;
            Item.consumable = true;
            Item.shoot = ProjectileType<GhostDartShot>();
            Item.ammo = AmmoID.Dart;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.Ectoplasm, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class GhostDartShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("GhostSHot");     //The English name of the Projectile

        }
        public override void SetDefaults()
        {
            AIType = ProjectileID.PoisonDartBlowgun;
            Projectile.CloneDefaults(ProjectileID.PoisonDartBlowgun);
            Projectile.timeLeft = 300;
            Projectile.alpha = 50;
            Projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().homingRange = 600;
            Projectile.GetGlobalProjectile<ScopeAndQuiver>().AffectedByAlphaScope = true;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Lighting.AddLight(Projectile.Center, 0.4f, 0.4f, 0.4f);

        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector2 direction = Projectile.velocity.RotatedBy(360 - 45 * (i - 1)) * 9f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction, ProjectileType<GhostDartSubShot>(), (int)(Projectile.damage * 0.5f), 1f, Projectile.owner);
            }
        }
    }
    public class GhostDartSubShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("GhostSHot");     //The English name of the Projectile

        }
        public override void SetDefaults()
        {
            AIType = ProjectileID.PoisonDartBlowgun;
            Projectile.CloneDefaults(ProjectileID.PoisonDartBlowgun);
            Projectile.timeLeft = 15;
            Projectile.alpha = 50;
            Projectile.scale = 1f;    
			Projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().homingRange = 600;
			Projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
            Projectile.ArmorPenetration = 100;
            Projectile.GetGlobalProjectile<ScopeAndQuiver>().AffectedByAlphaScope = true;
            Projectile.penetrate = 3;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.SpectreStaff, 1f);
                dust.noGravity = true;
            }
        }
    }
}


