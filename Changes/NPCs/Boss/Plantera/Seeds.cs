using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss.Plantera
{
	class Seeds : GlobalProjectile
	{
		public override bool InstancePerEntity => true;
		int down = 360;
        public override void SetDefaults(Projectile projectile)
        {
			if (projectile.type == 276)
            {
				projectile.timeLeft = 120;
            }
				//base.SetDefaults(projectile);
        }
        public override void Kill(Projectile projectile, int timeLeft)
        {
            if(projectile.type == 276 && projectile.localAI[1] == 1)
            {

				SoundEngine.PlaySound(SoundID.NPCHit, projectile.Center);
				Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ProjectileType<HostileCloud>(), projectile.damage, 0);
            }
        }
        public override bool PreAI(Projectile projectile)
		{
			if (projectile.type == 275 || projectile.type == 276)
			{
				projectile.extraUpdates = 3;
				if (projectile.ai[1] == 0f)
				{
					projectile.ai[1] = 1f;
					SoundEngine.PlaySound(SoundID.Item17, projectile.position);
				}
				if (projectile.alpha > 0)
				{
					projectile.alpha -= 30;
				}
				if (projectile.alpha < 0)
				{
					projectile.alpha = 0;
				}
				/*
				down-=2;
				if(down < 60)
                {
					down = 60;
                }
				Vector2 target = Main.player[Player.FindClosest(projectile.Center, 1, 1)].Center;
				float angularDiff = TRAEMethods.AngularDifference(projectile.velocity.ToRotation(), (target - projectile.Center).ToRotation());
				if(angularDiff < (float)Math.PI / 2)
                {
					float direction = projectile.velocity.ToRotation();
					direction.SlowRotation((target - projectile.Center).ToRotation(), (float)Math.PI / down);
					projectile.velocity = TRAEMethods.PolarVector(projectile.velocity.Length(), direction);
				}
				*/
				projectile.tileCollide = false;
				if (projectile.timeLeft > 720)
				{
					projectile.timeLeft = 720;
				}
				/*
				float num206 = 18f;
				int num207 = Player.FindClosest(projectile.Center, 1, 1);
				Vector2 value17 = Main.player[num207].Center - projectile.Center;
				value17.Normalize();
				value17 *= num206;
				int num208 = 70;
				projectile.velocity = (projectile.velocity * (num208 - 1) + value17) / num208;
				if (projectile.velocity.Length() < 14f)
				{
					projectile.velocity.Normalize();
					projectile.velocity *= 4f;
				}
				projectile.tileCollide = false;
				if (projectile.timeLeft > 180)
				{
					projectile.timeLeft = 180;
				}
				*/
				projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2;
				return false;
			}
			return base.PreAI(projectile);
		}
	}
}
