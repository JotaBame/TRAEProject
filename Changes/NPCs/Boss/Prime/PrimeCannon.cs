using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    public class PrimeCannon : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
            if(npc.type == NPCID.PrimeCannon)
            {
                npc.lifeMax = (int)(npc.lifeMax * ((float)PrimeStats.cannonHealth / 7000));
            }
        }
        public override void AI(NPC npc)
        {
            if(npc.type == NPCID.PrimeCannon && npc.ai[2] == 0)
            {
                npc.localAI[0] += 2f;
                if (Main.npc[(int)npc.ai[1]].ai[1] != 0f) 
                {
                    npc.localAI[0] += 2f;
                }
                if(!SkeletronPrime.KeepPhase1Arms(Main.npc[(int)npc.ai[1]]))
                {
                    npc.ai[2] += 10f;
                    if (npc.ai[2] > 50f || Main.netMode != NetmodeID.Server) 
                    {
                        npc.life = -1;
                        npc.HitEffect();
                        npc.active = false;
                        
                    }
                }
            }
        }
    }
}