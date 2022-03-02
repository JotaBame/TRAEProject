using System;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Common;

namespace TRAEProject
{
    public class TRAEMethods
    {
        public static void SpawnProjectilesFromAbove(Player player1, Vector2 Base, int projectileCount, int spreadX, int spreadY, int[] offsetCenter, float velocity, int type, int damage, float knockback, int player)
        {
            for (int i = 0; i < projectileCount; ++i)
            {
                // where the projectile spawns
                float x2 = Base.X  + Main.rand.Next(-spreadX, spreadX);
                float y = Base.Y - Main.rand.Next((int)(spreadY * 0.8), (int)(spreadY * 1.2));
                ///
                //Calculate Velocity
                Vector2 vector17 = new Vector2(x2, y);
                float X = Base.X - vector17.X;
                float Y = Base.Y + (Main.rand.Next(offsetCenter) * 100) - vector17.Y;
                float squareRoot = (float)Math.Sqrt(X * X + Y * Y);
                squareRoot = velocity / squareRoot;
                X *= squareRoot;
                Y *= squareRoot;
                ///
                // Spawn the projectile
                int Projectile = Terraria.Projectile.NewProjectile(player1.GetProjectileSource_Misc(type), x2, y, X, Y, type, damage, knockback, player);
                // once the projectile reaches the base's position, it will no longer go through tiles.
                Main.projectile[Projectile].localAI[1] += Base.Y;
           
            }
            return;
        }
        public static void Explode(Projectile projectile, int ExplosionRadius, float ExplosionDamage) // Doesn't set any special effects
        {
            projectile.GetGlobalProjectile<ProjectileStats>().explodes = false; // without this, the projectile will keep exploding infinitely
            projectile.timeLeft = 3; // Explosion will stay active for 3 frames          
            projectile.alpha = 255; // Projectile becomes invisible
            projectile.damage = (int)(projectile.damage * ExplosionDamage); // Damage is multiplied by a certian amount if necessary
            projectile.tileCollide = false; // if set to true, the explosion won't go through blocks (and probably be messed up)
            // Adjust the explosion's hitbox so that it spawns at the center of the projectile.
            projectile.position.X += projectile.width / 2;
            projectile.position.Y += projectile.height / 2;
            projectile.width = projectile.height = ExplosionRadius;
            projectile.position.X -= projectile.width / 2;
            projectile.position.Y -= projectile.height / 2;
            projectile.position.X += projectile.width / 2;
            projectile.position.Y += projectile.height / 2;
            projectile.width = projectile.height = ExplosionRadius;
            projectile.position.X -= (projectile.width / 2);
            projectile.position.Y -= (projectile.height / 2);
        }
        public static void DefaultExplosion(Projectile projectile)
        {
            SoundEngine.PlaySound(SoundID.Item14, projectile.position);
            float num846 = 3f;
            for (int num847 = 0; num847 < 50; num847++)
            {
                Dust dust53 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                dust53.velocity = (dust53.position - projectile.Center).SafeNormalize(Vector2.Zero);
                Dust dust = dust53;
                dust.velocity *= 2f + (float)Main.rand.Next(5) * 0.1f;
                dust53.velocity.Y -= num846 * 0.5f;
                dust53.color = Color.Black * 0.9f;
                if (Main.rand.Next(2) == 0)
                {
                    dust53.scale = 0.5f;
                    dust53.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    dust53.color = Color.Black * 0.8f;
                }
            }
            for (int num848 = 0; num848 < 30; num848++)
            {
                Dust dust54 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100);
                dust54.noGravity = true;
                dust54.fadeIn = 1.4f;
                dust54.velocity = (dust54.position - projectile.Center).SafeNormalize(Vector2.Zero);
                Dust dust = dust54;
                dust.velocity *= 5.5f + (float)Main.rand.Next(61) * 0.1f;
                dust54.velocity.Y -= num846 * 0.5f;
                dust54 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100);
                dust54.velocity = (dust54.position - projectile.Center).SafeNormalize(Vector2.Zero);
                dust54.velocity.Y -= num846 * 0.25f;
                dust = dust54;
                dust.velocity *= 1.5f + (float)Main.rand.Next(5) * 0.1f;
                dust54.fadeIn = 0f;
                dust54.scale = 0.6f;
                dust54 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                dust54.noGravity = num848 % 2 == 0;
                dust54.velocity = (dust54.position - projectile.Center).SafeNormalize(Vector2.Zero);
                dust = dust54;
                dust.velocity *= 3f + (float)Main.rand.Next(21) * 0.2f;
                dust54.velocity.Y -= num846 * 0.5f;
                dust54.fadeIn = 1.2f;
                if (!dust54.noGravity)
                {
                    dust54.scale = 0.4f;
                    dust54.fadeIn = 0f;
                }
                else
                {
                    dust = dust54;
                    dust.velocity *= 2f + (float)Main.rand.Next(5) * 0.2f;
                    dust54.velocity.Y -= num846 * 0.5f;
                }
            }
        }
        public static Vector2 PolarVector(float radius, float theta)
        {
            return new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * radius;
        }
        /// <summary>
        /// give an angle to shoot at to attempt to hit a moving target, returns NaN when this is impossible
        /// </summary>
        public static float PredictiveAim(Vector2 shootFrom, float shootSpeed, Vector2 targetPos, Vector2 targetVelocity, out float travelTime)
        {
            float angleToTarget = (targetPos - shootFrom).ToRotation();
            float targetTraj = targetVelocity.ToRotation();
            float targetSpeed = targetVelocity.Length();
            float dist = (targetPos - shootFrom).Length();

            //imagine a tirangle between the shooter, its target and where it think the target will be in the future
            // we need to find an angle in the triangle z this is the angle located at the target's corner
            float z = (float)Math.PI + (targetTraj - angleToTarget);

            //with this angle z we can now use the law of cosines to find time
            //the side opposite of z is equal to shootSpeed * time
            //the other sides are dist and targetSpeed * time
            // putting these values into law of cosines gets (shootSpeed * time)^2 = (targetSpeed * time)^2 + dist^2 -2*targetSpeed*time*cos(z)
            //we can rearange it to (shootSpeed^2 - targetSpeed^2)time^2 + 2*targetSpeed*dist*cos(z)*time - dist^2 = 0, this is a quadratic!

            //here we use the quadratic formula to find time
            float a = shootSpeed * shootSpeed - targetSpeed * targetSpeed;
            float b = 2 * targetSpeed * dist * (float)Math.Cos(z);
            float c = -(dist * dist);
            float time = (-b + (float)Math.Sqrt(b * b - 4 * a * c)) / (2 * a);

            //we now know the time allowing use to find all sides of the tirangle, now we use law of Sines to calculate the angle to shoot at.
            float calculatedShootAngle = angleToTarget - (float)Math.Asin((targetSpeed * time * (float)Math.Sin(z)) / (shootSpeed * time));
            travelTime = time;
            return calculatedShootAngle;
        }
        public delegate bool SpecialCondition(NPC possibleTarget);

        //used for homing projectile
        public static bool ClosestNPC(ref NPC target, float maxDistance, Vector2 position, bool ignoreTiles = false, int overrideTarget = -1, SpecialCondition specialCondition = null)
        {
            //very advance users can use a delegate to insert special condition into the function like only targetting enemies not currently having local iFrames, but if a special condition isn't added then just return it true
            if (specialCondition == null)
            {
                specialCondition = delegate (NPC possibleTarget) { return true; };
            }
            bool foundTarget = false;
            //If you want to prioritse a certain target this is where it's processed, mostly used by minions that haave a target priority
            if (overrideTarget != -1)
            {
                if ((Main.npc[overrideTarget].Center - position).Length() < maxDistance && !Main.npc[overrideTarget].immortal && (Collision.CanHit(position, 0, 0, Main.npc[overrideTarget].Center, 0, 0) || ignoreTiles) && specialCondition(Main.npc[overrideTarget]))
                {
                    target = Main.npc[overrideTarget];
                    return true;
                }
            }
            //this is the meat of the targetting logic, it loops through every NPC to check if it is valid the miniomum distance and target selected are updated so that the closest valid NPC is selected
            for (int k = 0; k < Main.npc.Length; k++)
            {
                NPC possibleTarget = Main.npc[k];
                float distance = (possibleTarget.Center - position).Length();
                if (distance < maxDistance && possibleTarget.active && possibleTarget.chaseable && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && (Collision.CanHit(position, 0, 0, possibleTarget.Center, 0, 0) || ignoreTiles) && specialCondition(possibleTarget))
                {
                    target = Main.npc[k];
                    foundTarget = true;

                    maxDistance = (target.Center - position).Length();
                }
            }
            return foundTarget;
        }
        public static float AngularDifference(float angle1, float angle2)
        {
            angle1 = PolarVector(1f, angle1).ToRotation();
            angle2 = PolarVector(1f, angle2).ToRotation();
            if (Math.Abs(angle1 - angle2) > Math.PI)
            {
                return (float)Math.PI * 2 - Math.Abs(angle1 - angle2);
            }
            return Math.Abs(angle1 - angle2);
        }
    }
}
