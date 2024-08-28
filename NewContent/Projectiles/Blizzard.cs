using Microsoft.Xna.Framework;
using TRAEProject;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
namespace TRAEProject.NewContent.Projectiles
{
   public class Blizzard : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 3600;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;

        }
        public virtual bool? CanHitNPC(Projectile projectile, NPC target)
        {
            return false;
        }
        // Note, this Texture is actually just a blank texture, FYI.

        readonly int fireRate = 12;
        readonly int[] offSetCenter = {5, 6, 7};
        readonly int projectilesPerShot = 4;
        readonly int projectileType = ProjectileID.Blizzard;
        readonly float velocity = 12;
        readonly int SpreadX = 400;
        readonly int SpreadY = 800;
        public override void AI()
        {
            Projectile.localAI[0] += 1f;

            Player player = Main.player[Projectile.owner];

            if (player.statMana >= (int)(12 * player.manaCost) && Projectile.localAI[0] > fireRate)
            {
				player.statMana -= (int)(12 * player.manaCost);
                Projectile.localAI[0] -= fireRate;
                TRAEMethods.SpawnProjectilesFromAbove(player, Projectile.position, projectilesPerShot, SpreadX, SpreadY, offSetCenter, velocity, projectileType, Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }
}