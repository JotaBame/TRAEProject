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
    public class SpikedBleed : TRAEDebuff
    {
 
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            npc.lifeRegen -= 12 * 2;
            damage = 2;
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
 
            if (Main.rand.NextBool(12))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Blood, 0f, 0f, 0, default, Main.rand.Next(8, 12) * 0.1f);
                dust.noLight = true;
                dust.velocity *= 0.5f;
            }
        }
    }
}
