using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
namespace TRAEProject.NewContent.Projectiles
{
   public class PhantomTentacle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = 40;
		    Projectile.height = 40;
			Projectile.MaxUpdates = 3;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
        }
        public override void AI()
        {
			Projectile.ai[0] += 10f;
			if (Projectile.ai[0] > 500f)
			{
				Projectile.Kill();
			}
			for (int i = 0; i < 5; i++)
			{
				int GhostTentacle = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, 175, 0f, 0f, 100);
				Main.dust[GhostTentacle].position = (Main.dust[GhostTentacle].position + Projectile.Center) / 2f;
				Main.dust[GhostTentacle].noGravity = true;
				Dust dust = Main.dust[GhostTentacle];
				dust.velocity *= 0.1f;
				if (i == 1)
				{
					dust = Main.dust[GhostTentacle];
					dust.position += Projectile.velocity / 2f;
				}
				float ScaleMult = (1000f - Projectile.ai[0]) / 500f;
				dust = Main.dust[GhostTentacle];
				dust.scale *= ScaleMult + 0.1f;
				Projectile.velocity.Y += 0.008f;
			}
			//for (int i = 0; i < 4; i++)
			//{
			//	Vector2 ProjectilePosition = Projectile.Center;
			//	ProjectilePosition -= Projectile.velocity * (i * 0.25f);
			//	Projectile.alpha = 255;
			//	int dust2 = Dust.NewDust(ProjectilePosition, 1, 1, 175, 0f, 0f, 0, default, 1f);
			//	Main.dust[dust2].noGravity = true;
			//	Main.dust[dust2].position = ProjectilePosition;
			//	Main.dust[dust2].scale = Main.rand.Next(70, 100) * 0.010f;
			//	Main.dust[dust2].velocity *= 0.2f;
			//}
		}
	}
}