using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Weapon.Summon.Minions
{
    public class Tempest
    {
        public static void AI(Projectile projectile)
        {
            //linking the minion to the player
            Player player = Main.player[projectile.owner];
            if (Main.player[projectile.owner].dead)
            {
                Main.player[projectile.owner].sharknadoMinion = false;
            }
            if (Main.player[projectile.owner].sharknadoMinion)
            {
                projectile.timeLeft = 2;
            }

            //sharknados push each other when too close
            float disturbanceVelocity = 0.1f;
            float disturbanceDistance = projectile.width;
            disturbanceDistance *= 2f;
            for (int m = 0; m < 1000; m++)
            {
                if (m != projectile.whoAmI && Main.projectile[m].active && Main.projectile[m].owner == projectile.owner && Main.projectile[m].type == projectile.type && Math.Abs(projectile.position.X - Main.projectile[m].position.X) + Math.Abs(projectile.position.Y - Main.projectile[m].position.Y) < disturbanceDistance)
                {
                    if (projectile.position.X < Main.projectile[m].position.X)
                    {
                        projectile.velocity.X -= disturbanceVelocity;
                    }
                    else
                    {
                        projectile.velocity.X += disturbanceVelocity;
                    }
                    if (projectile.position.Y < Main.projectile[m].position.Y)
                    {
                        projectile.velocity.Y -= disturbanceVelocity;
                    }
                    else
                    {
                        projectile.velocity.Y += disturbanceVelocity;
                    }
                }
            }

            //manage alpha
            projectile.tileCollide = false;
            if (Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.alpha += 20;
                if (projectile.alpha > 150)
                {
                    projectile.alpha = 150;
                }
            }
            else
            {
                projectile.alpha -= 50;
                if (projectile.alpha < 60)
                {
                    projectile.alpha = 60;
                }
            }

            //movement
            if (!Collision.CanHitLine(projectile.Center, 1, 1, Main.player[projectile.owner].Center, 1, 1))
            {
                projectile.ai[0] = 1f;
            }
            float speed = 16f;
            Vector2 diff = player.Center - projectile.Center + new Vector2(0f, -20f);
            float num25 = diff.Length();
            if (num25 > 200f)
            {
                speed = 32f;
            }
            if (num25 < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.ai[0] = 0f;
                projectile.netUpdate = true;
            }
            if (num25 > 2000f)
            {
                projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
                projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.width / 2);
            }

            if (Math.Abs(diff.X) > 40f || Math.Abs(diff.Y) > 10f)
            {
                diff.Normalize();
                diff *= speed;
                diff *= new Vector2(1.25f, 0.65f);
                projectile.velocity = (projectile.velocity * 20f + diff) / 21f;
            }
            else
            {
                if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                {
                    projectile.velocity.X = -0.15f;
                    projectile.velocity.Y = -0.05f;
                }
                projectile.velocity *= 1.01f;
            }

            //animation and dust
            int num27 = 2;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 6 * num27)
            {
                projectile.frameCounter = 0;
            }
            projectile.frame = projectile.frameCounter / num27;
            if (Main.rand.Next(5) == 0)
            {
                int num28 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 217, 0f, 0f, 100, default(Color), 2f);
                Main.dust[num28].velocity *= 0.3f;
                Main.dust[num28].noGravity = true;
                Main.dust[num28].noLight = true;
            }
            if (projectile.velocity.X > 0f)
            {
                projectile.spriteDirection = (projectile.direction = -1);
            }
            else if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = (projectile.direction = 1);
            }

            //attack cooldown
            projectile.ai[1] += 1f;
            if (Main.rand.Next(3) != 0)
            {
                projectile.ai[1] += 1f;
            }
            if (projectile.ai[1] > 50f)
            {

                if (TRAEMethods.ClosestNPC(ref projectile.GetGlobalProjectile<MinionChanges>().target, 500 * 16, player.Center, false, player.MinionAttackTargetNPC))
                {

                    if (Main.myPlayer == projectile.owner)
                    {
                        float shootSpeed = 20;
                        float calculatedShootAngle = TRAEMethods.PredictiveAim(projectile.Center, shootSpeed, projectile.GetGlobalProjectile<MinionChanges>().target.Center, projectile.GetGlobalProjectile<MinionChanges>().target.velocity, out _);
                        if (!float.IsNaN(calculatedShootAngle))
                        {
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, TRAEMethods.PolarVector(shootSpeed, calculatedShootAngle), ProjectileID.MiniSharkron, projectile.damage, projectile.knockBack, projectile.owner);
                        }



                    }
                    projectile.ai[1] = 0f;
                    projectile.netUpdate = true;
                }

            }
        }
    }
}
