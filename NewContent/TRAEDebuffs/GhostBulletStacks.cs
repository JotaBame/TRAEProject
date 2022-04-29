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
using TRAEProject.NewContent.Items.Weapons.Ammo;
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
            Vector2 velocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 0));
            velocity.Normalize();
            velocity *= Main.rand.Next(10, 101) * 0.5f;
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
                int d = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width, npc.height, DustID.SpectreStaff, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 2f);
                Main.dust[d].velocity *= 0.8f;
                Main.dust[d].velocity.Y -= 0.3f;
            }
        }
    }
}
