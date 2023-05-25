using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TRAEProject.Common;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace TRAEProject.NewContent.TRAEDebuffs
{
    public class BamFire : TRAEDebuff
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            npc.lifeRegen -= 400;
            damage = npc.lifeRegen / -4;
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Main.rand.NextBool(4))
            {
                Dust dust6 = Dust.NewDustDirect(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, 229, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1f);
                dust6.noGravity = true;
                dust6.velocity *= 1.8f;
                dust6.velocity.Y -= 0.5f;
            }
            //if (Main.rand.NextBool(2))
            //{
            //    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 229, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 100, default, 3f);
            //    Main.dust[dust].noGravity = true;
            //    Main.dust[dust].velocity *= 2.4f;
            //    Main.dust[dust].velocity.Y -= 0.4f;
            //    if (Main.rand.NextBool(4))
            //    {
            //        Main.dust[dust].noGravity = false;
            //        Main.dust[dust].scale *= 0.9f;
            //    }
            //}
        }
    }
}
