using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Projectiles.EchoSpriteProj
{
    /// <summary>
    /// UNTESTED
    /// </summary>
    public class EchoSpriteProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_0";
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 2;
            Projectile.hostile = true;
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
