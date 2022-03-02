using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TRAEProject.Common;

namespace TRAEProject.NewContent.TRAEDebuffs
{
    public class Corrupted : TRAEDebuff
    {
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            drawColor.R = 172;
            drawColor.G = 145;
            drawColor.B = 153;
            if (Main.rand.Next(4) < 1)
            {
                int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width, npc.height, 184, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.8f;
                Main.dust[dust].velocity.Y -= 0.3f;
                if (Main.rand.NextBool(4))
                {
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].scale *= 0.3f;
                }
            }
        }
        float ScourgeTime = 0f;
        float TinyEaterDelay = 90f;
        int TinyEaterCount = 2;
        public override void Update(NPC npc)
        {
            ++ScourgeTime;
            if (ScourgeTime >= TinyEaterDelay)
            {
                ScourgeTime = 0;
                for (int i = 0; i < TinyEaterCount; ++i)
                {
                    float velX = Main.rand.Next(-35, 36) * 0.1f;
                    float velY = Main.rand.Next(-35, 36) * 0.1f;
                    Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), npc.Center.X, npc.Center.Y, velX, velY, ProjectileID.TinyEater, 52, 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }

        public override void CheckDead(NPC npc)
        {
            for (int i = 0; i < 3; ++i)
            {
                float velX = Main.rand.Next(-35, 36) * 0.2f;
                float velY = Main.rand.Next(-35, 36) * 0.2f;
                Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), npc.position.X, npc.position.Y, velX, velY, ProjectileID.TinyEater, 52, 0f, Main.myPlayer, 0f, 0f);
            }
        }
    }
}
