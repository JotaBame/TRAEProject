using Microsoft.Xna.Framework;
using TRAEProject;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
namespace TRAEProject.NewContent.Projectiles
{
    class Blizzard : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;

        }
        public virtual bool? CanHitNPC(Projectile projectile, NPC target)
        {
            return false;
        }
        // Note, this Texture is actually just a blank texture, FYI.

        readonly int fireRate = 15;
        readonly int[] offSetCenter = {3, 4, 5};
        readonly int projectilesPerShot = 4;
        readonly int projectileType = ProjectileID.Blizzard;
        readonly float velocity = 10;
        readonly int SpreadX = 500;
        readonly int SpreadY = 800;
        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > fireRate)
            {
                Projectile.localAI[0] -= fireRate;
                TRAEMethods.SpawnProjectilesFromAbove(Main.player[Projectile.owner], Projectile.position, projectilesPerShot, SpreadX, SpreadY, offSetCenter, velocity, projectileType, Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }
}