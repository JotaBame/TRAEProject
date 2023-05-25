using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using TRAEProject.Changes.Items;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;
namespace TRAEProject.NewContent.Projectiles
{
    public class ToxicCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
			Projectile.frame = Main.rand.Next(3);
            // DisplayName.SetDefault("ToxicCloud");     //The English name of the Projectile
        }
        public override void SetDefaults()
        {
			Projectile.width = 40;
			Projectile.height = 38;
            Projectile.scale = 1.15f;
            Projectile.friendly = true; 
			Projectile.GetGlobalProjectile<MagicProjectile>().DrainManaOnHit = TRAEMagicItem.FlaskOnHitCost;
			Projectile.GetGlobalProjectile<MagicProjectile>().DrainManaPassively = TRAEMagicItem.FlaskPassiveCost;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
			Projectile.alpha = 0;
            //Projectile.aiStyle = 92;
            Projectile.tileCollide = false;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 12;
			Projectile.extraUpdates = 0;
            
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int length = Main.rand.Next(3, 5) * 60;
            target.AddBuff(BuffID.Venom, length, false);
        }
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false; // return false because we are handling collision
		}
		public override void AI()
		{
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] > 600f)
			{
				Projectile.ai[0] += 3f;
			}
			if (Projectile.alpha > 240)
			{
				Projectile.Kill();
			}
			Projectile.alpha = (int)(100.0 + Projectile.ai[0]);
			Projectile.rotation += Projectile.velocity.X * 0.1f;
			Projectile.rotation += Projectile.direction * 0.033f;
			Projectile.velocity *= 0.99f;
			Rectangle rectangle5 = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
			for (int num780 = 0; num780 < 1000; num780++)
			{
				if (num780 == Projectile.whoAmI || !Main.projectile[num780].active || Main.projectile[num780].type < 511 || Main.projectile[num780].type > 513)
				{
					continue;
				}
				Rectangle value47 = new Rectangle((int)Main.projectile[num780].position.X, (int)Main.projectile[num780].position.Y, Main.projectile[num780].width, Main.projectile[num780].height);
				if (!rectangle5.Intersects(value47))
				{
					continue;
				}
				Vector2 vector59 = Main.projectile[num780].Center - Projectile.Center;
				if (vector59.X == 0f && vector59.Y == 0f)
				{
					if (num780 < Projectile.whoAmI)
					{
						vector59.X = -1f;
						vector59.Y = 1f;
					}
					else
					{
						vector59.X = 1f;
						vector59.Y = -1f;
					}
				}
				vector59.Normalize();
				vector59 *= 0.005f;
				Projectile.velocity -= vector59;
				Projectile Projectile2 = Main.projectile[num780];
				Projectile2.velocity += vector59;
			}
		}
	}
}


