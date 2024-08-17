using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Plantera
{
   public class HostileCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {

            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
			Projectile.tileCollide = false;
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.aiStyle = 44;
			Projectile.hostile = true;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
		}
        public override void AI()
        {
            Projectile.alpha += 2;
            Projectile.scale = 0.97f;
            if (Projectile.alpha >= 240)
            {
                Projectile.Kill();
            }
        }
    }
}
