using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
namespace TRAEProject.NewContent.Items.FlamethrowerAmmo
{
    public class IchorGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ichor Gel");
            Tooltip.SetDefault("Highly Volatile");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 10);
            Item.rare = ItemRarityID.Pink;
            Item.width = 20;
            Item.height = 18;
            Item.shootSpeed = 3;
            Item.consumable = true;
            Item.shoot = ProjectileType<IchorGelP>();
            Item.ammo = AmmoID.Gel;
            Item.maxStack = 3000;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.Ichor)
                .AddIngredient(ItemID.Gel, 20)
                .AddTile(TileID.Solidifier)
                .Register();
        }
     }

    public class IchorGelP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ichorthrower");     //The English name of the Projectile
        }
        public override string Texture => "Terraria/Images/Item_0";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GoldenShowerFriendly);
            AIType = ProjectileID.GoldenShowerFriendly;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 90;            
			Projectile.penetrate = 1;
        }
        NPC hitEnemy = null;       
        public override void AI()
        {
            Projectile.localAI[0] += 1;
            if (Projectile.localAI[0] % 8f == 0f)
            {
                Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.Smoke, 0, -1);
            }
        }        
        public override void Kill(int timeLeft)
        {
            int spits = Main.rand.Next(1, 2);
            for (int i = 0; i < spits; i++)
            {
                float velX = Main.rand.NextFloat(-2f, 3f) * 3f;
                float velY = Main.rand.NextFloat(-4f, 4f) * 3f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, velX, velY, ProjectileType<Ichorthrower>(), Projectile.damage, 1f, Projectile.owner, 0f, 0f);
            }
            Vector2 spinningpoint1 = ((float)Main.rand.NextDouble() * 6.283185f).ToRotationVector2();
            Vector2 spinningpoint2 = spinningpoint1;
            float RandomNumberBetweenSixAndTen = Main.rand.Next(3, 5) * 2;
            int Fifteen = 5;
            float OneOrMinusOne = Main.rand.Next(2) == 0 ? 1f : -1f; // one in three chance of it being 1
            bool flag = true;
            for (int i = 0; i < Fifteen * RandomNumberBetweenSixAndTen; ++i) // makes 90 or 150 dusts total
            {
                if (i % Fifteen == 0)
                {
                    spinningpoint2 = spinningpoint2.RotatedBy(OneOrMinusOne * (6.28318548202515 / RandomNumberBetweenSixAndTen), default);
                    spinningpoint1 = spinningpoint2;
                    flag = !flag;
                }
                else
                {
                    float num4 = 6.283185f / (Fifteen * RandomNumberBetweenSixAndTen);
                    spinningpoint1 = spinningpoint1.RotatedBy(num4 * OneOrMinusOne * 3.0, default);
                }
                float num5 = MathHelper.Lerp(7.5f, 60f, i % Fifteen / Fifteen);
                int index2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 6, 6, DustID.Smoke, 0.0f, 0.0f, 100, default, 3f);
                Dust dust1 = Main.dust[index2];
                dust1.velocity = Vector2.Multiply(dust1.velocity, 0.1f);
                Dust dust2 = Main.dust[index2];
                dust2.velocity = Vector2.Add(dust2.velocity, Vector2.Multiply(spinningpoint1, num5));
                if (flag)
                    Main.dust[index2].scale = 0.9f;
                Main.dust[index2].noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public class Ichorthrower : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Ichorthrower");     //The English name of the Projectile
            }
            public override string Texture => "Terraria/Images/Item_0";
            public override void SetDefaults()
            {
                Projectile.CloneDefaults(ProjectileID.GoldenShowerFriendly);
                AIType = ProjectileID.GoldenShowerFriendly;
                Projectile.DamageType = DamageClass.Ranged;
                Projectile.penetrate = 2;
                Projectile.scale = 0.75f;
                Projectile.timeLeft = 45;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
                Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
                Projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage = 0.75f;
                Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Ichor;
                Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffDuration = 240;
            }
            public override bool OnTileCollide(Vector2 oldVelocity)
            {
                return false;
            }
        }
    }
}