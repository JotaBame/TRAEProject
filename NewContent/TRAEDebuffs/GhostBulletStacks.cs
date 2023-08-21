using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TRAEProject.Common;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;
namespace TRAEProject.NewContent.TRAEDebuffs
{
    public class GhostBulletStacks : TRAEDebuff
    {
        Projectile projectile = null;
        public void SetProjectile(Projectile pRojectile)
        {
            projectile = pRojectile;
        }
        public void GhostBoom(NPC npc)
        {

            int[] array = new int[10];
            int num6 = 0;
            int Range = 700;
            int num8 = 20;
            Vector2 velocity;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(this, false))
                {
                    float DistanceBetweenProjectileAndEnemy = (projectile.Center - Main.npc[j].Center).Length();
                    if (DistanceBetweenProjectileAndEnemy > num8 && DistanceBetweenProjectileAndEnemy < Range && Collision.CanHitLine(projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                    {
                        array[num6] = j;
                        num6++;
                        if (num6 >= 9)
                        {
                            break;
                        }
                    }
                }
            }
            if (num6 > 0)
            {
                num6 = Main.rand.Next(num6);
                Vector2 value2 = Main.npc[array[num6]].Center - projectile.Center;
                float scaleFactor2 = projectile.velocity.Length();
                value2.Normalize();
                velocity = value2 * scaleFactor2;
                projectile.netUpdate = true;
            }
            else
            {
                velocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 0));
                velocity.Normalize();
                velocity *= Main.rand.Next(10, 101) * 0.5f;
            }


            Projectile.NewProjectile(projectile.GetSource_FromThis(), npc.position, velocity, ProjectileType<GhostShot>(), projectile.damage, 4f);
        }
        public override void CheckDead(NPC npc)
        {
            GhostBoom(npc);
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Main.rand.Next(6) < 1)
            {
                int d = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width, npc.height, DustID.SpectreStaff, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 2f);
                Main.dust[d].velocity *= 0.8f;
                Main.dust[d].velocity.Y -= 0.3f;
            }
        }
    }
}
