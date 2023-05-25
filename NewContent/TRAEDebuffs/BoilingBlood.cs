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
    public class BoilingBlood : TRAEDebuff
    {
        int BoilingBloodDMG = 10;
        public void SetDamage(int damage)
        {
            BoilingBloodDMG = damage;
        }
        public override void Update(NPC npc)
        {
            
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            npc.lifeRegen -= BoilingBloodDMG * 2;
            damage = BoilingBloodDMG;
        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Main.rand.NextBool(15))
            {
                int d = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width, npc.height, DustID.Smoke, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default, 2f);
                Main.dust[d].velocity *= 0.8f;
                Main.dust[d].velocity.Y -= 0.3f;
            }
            if (Main.rand.NextBool(12))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.CrimtaneWeapons, 0f, 0f, 0, default, Main.rand.Next(8, 12) * 0.1f);
                dust.noLight = true;
                dust.velocity *= 0.5f;
            }
        }
    }
}
