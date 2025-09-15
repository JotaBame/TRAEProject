using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using static AssGen.Assets;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.Magic.EchoStaff
{
    public class EchoStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.DefaultToStaff(ProjectileType<EchoStaffProj>(), pushForwardSpeed: 5, singleShotTime: 25, manaPerShot: 13);
            Item.width = 44;
            Item.height = 44;
            Item.damage = 43;
            Item.crit = 3;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 7);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item124 with { PitchVariance = 0.3f, MaxInstances = 1,  };
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f * 2;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }
        int timer = 0;

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {

            if (timer < 80)
                timer++;
            if (timer >= 80)
            {
                if (Main.rand.NextBool(4))
                {
                    int num117 = Dust.NewDust(new Vector2(Item.position.X, Item.position.Y + 2f), Item.width, Item.height, DustID.PinkTorch, Item.velocity.X * 0.2f, Item.velocity.Y * 0.2f, 100, default, 2f);
                    Main.dust[num117].noGravity = true;
                    Main.dust[num117].velocity.X *= 1f;
                    Main.dust[num117].velocity.Y *= 1f;
                }
              
                maxFallSpeed = 0;
            }


        }
        public class EchoStaffProj : ModProjectile
        {
            public override string Texture => "Terraria/Images/Item_0";
            public override void SetDefaults()
            {
                Projectile.extraUpdates = 2;
                Projectile.penetrate = 3;
                Projectile.friendly = true;
                Projectile.width = Projectile.height = 24;
                Projectile.ignoreWater = true;
                Projectile.tileCollide = true;
            }
            public override void AI()
            {
                Projectile.localAI[0] += .1f;
                float yOffset = MathF.Sin(Projectile.localAI[0]) * 20;
                Vector2 dustOffset = new Vector2(0, yOffset).RotatedBy(Projectile.velocity.ToRotation());
                float t = MathF.Abs(MathF.Sin(Projectile.localAI[0] / 2));
                t *= t;
                float scale = MathHelper.Lerp(2.5f, 1.25f, t);
                Color color = Color.White * t;
                color.A = 0;
                Dust.NewDustPerfect(Projectile.Center + dustOffset, DustID.PinkTorch, Vector2.Zero, 0, Color.White, scale).noGravity = true;
                t = MathF.Abs(MathF.Sin((Projectile.localAI[0] + MathF.PI) / 2));
                t *= t;
                color.A = 0;
                scale = MathHelper.Lerp(2.5f, 1.25f, t);
                Dust.NewDustPerfect(Projectile.Center - dustOffset, DustID.PinkTorch, Vector2.Zero, 0, Color.White, scale).noGravity = true;
            }
            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                Main.player[Projectile.owner].ApplyDamageToNPC(target, Projectile.damage, Projectile.knockBack, hit.HitDirection);
            }
            public override bool PreDraw(ref Color lightColor)
            {
                return false;
            }
        }
    }
}