using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Projectiles
{
  public  class Blank : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 1;
            Projectile.penetrate = 0;
            
        }
        public override void AI()
        {
            Projectile.damage = 0;
        }
    }
}