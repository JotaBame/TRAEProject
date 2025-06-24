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
    public class GhostBullet: ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ghost Bullet");
            // Tooltip.SetDefault("Goes through tiles and enemies\nLeaves up to 9 ghost bullets inside the enemy, releases them when killed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 12;
            Item.height = 15;
            Item.shootSpeed = 4;
            Item.consumable = true;
            Item.shoot = ProjectileType<GhostShot>();
            Item.ammo = AmmoID.Bullet;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.EmptyBullet, 100)
                .AddIngredient(ItemID.Ectoplasm, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class GhostShot: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("GhostSHot");     //The English name of the Projectile

        }
        public override void SetDefaults()
        {
            AIType = ProjectileID.Bullet;
            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.timeLeft = 1200;
            Projectile.alpha = 100;
            Projectile.GetGlobalProjectile<ScopeAndQuiver>().AffectedByReconScope = true;
			Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
             Projectile.penetrate = 2;
            Projectile.extraUpdates = 2;
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            GhostBulletStacks GB = TRAEDebuff.Apply<GhostBulletStacks>(target, 300, 9);
            if (GB != null)
            {
                GB.SetProjectile(Projectile);
            }
        }
        public override void OnKill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.position);
        }
    }
}


