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
using TRAEProject.Common;
using TRAEProject.Common.ModPlayers;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Projectiles.Spears
{
    class NorthPole : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 164f;
            stabStart = 147f;
            stabEnd = -10;
            swingAmount = (float)Math.PI / 24;
        }
        public override void OnMaxReach(float direction)
        {
            Vector2 center = Projectile.Center + TRAEMethods.PolarVector(-28.3f * Projectile.scale, direction);
            for(int i =0; i < 5; i++)
            {
                float rot = (float)Math.PI * (i / 4f) - (float)Math.PI / 2f + direction;
                Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), center, TRAEMethods.PolarVector(10, rot), ProjectileType<NorthStar>(), (int)(Projectile.damage * 0.25f), 0f, Projectile.owner);
            }
        }
    }
    public class NorthPoleThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 164f;
            holdAt = 92f;
            floatTime = 24;
            Projectile.penetrate = -1;
        }
        float counter = 0;
        public override void ThrownUpdate()
        {
            counter += Projectile.velocity.Length();
            if (counter > 99f)
            {
                counter -= 99f;
                Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.NorthPoleSnowflake, (int)(Projectile.damage * 0.7f), 0f, Projectile.owner, 0f, Main.rand.Next(3));
            }
        }
    }
    public class Snowflake : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if(projectile.type == ProjectileID.NorthPoleSnowflake)
            {
                projectile.timeLeft = 120;
            }
        }
        public override void AI(Projectile projectile)
        {
            if (projectile.type == ProjectileID.NorthPoleSnowflake)
            {
                projectile.velocity.X = 0;
                NPC target = null;
                if(TRAEMethods.ClosestNPC(ref target, 1000, projectile.Center, specialCondition: delegate (NPC possibleTarget) { return possibleTarget.Top.Y > projectile.Bottom.Y; }))
                {
                    float xSpeed = Math.Min(4, projectile.velocity.Y);
                    xSpeed = Math.Min(xSpeed, Math.Abs(target.Center.X - projectile.Center.X) * 0.1f);
                    if (Math.Abs(target.Center.X - projectile.Center.X) < xSpeed)
                    {
                        projectile.Center = new Vector2(target.Center.X, projectile.Center.Y);
                    }
                    else
                    {
                        projectile.velocity.X = Math.Sign(target.Center.X - projectile.Center.X) * xSpeed;
                    }
                }
                else
                {
                }
                projectile.rotation += projectile.velocity.Y * projectile.velocity.X * (float)Math.PI / 120f;
                //projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            }
        }
    }
    public class NorthStar : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 20;
            Projectile.width = Projectile.height = 18;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.extraUpdates = 1;
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int num427 = 4; num427 < 10; num427++)
            {
                float num428 = Projectile.oldVelocity.X * (30f / (float)num427);
                float num429 = Projectile.oldVelocity.Y * (30f / (float)num427);
                int num430 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num428, Projectile.oldPosition.Y - num429), 8, 8, 197, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default(Color), 1.2f);
                Main.dust[num430].noGravity = true;
                Dust dust = Main.dust[num430];
                dust.velocity *= 0.5f;
            }
        }
    }
}
