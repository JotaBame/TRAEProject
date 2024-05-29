using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Projectiles.KinnaraFeather
{
    public class KinnaraFeather : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathF.PI / 2;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
            }
        }
    }
}
