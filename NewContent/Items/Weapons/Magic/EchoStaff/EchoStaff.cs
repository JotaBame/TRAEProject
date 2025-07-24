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
            Item.DefaultToStaff(ProjectileType<EchoStaffProj>(), 14, 25, 12);
            Item.width = 44;
            Item.height = 44;
            Item.damage = 43;
			Item.crit = 3;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 7);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item125 with { PitchVariance = 0.3f, MaxInstances = 8 };
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }
    }
    public class EchoStaffProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_0";
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
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
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}