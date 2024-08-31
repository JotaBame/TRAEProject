using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.SolarEclipse
{

    public class Nailhead: GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            if(npc.type == NPCID.Nailhead)
            {
                npc.localAI[3] = 30f; //shuts off vanilla nail spawning
            }
            return base.PreAI(npc);
        }
        public override void AI(NPC npc)
        {
            if(npc.type == NPCID.Nailhead)
            {
                //Main.NewText(npc.localAI[0] + ", " + npc.localAI[1] + ", " + npc.localAI[2] + ", " + npc.localAI[3]);
                if(npc.localAI[2] > 0)
                {
                    npc.localAI[2] -= 1f;
                } 
            }
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if(npc.type == NPCID.Nailhead && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if(npc.localAI[2] <= 0)
                {
                    npc.localAI[2] = 60;
                    for(int i =0; i < 5; i++)
                    {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + Vector2.UnitY * -12, (Vector2.UnitY * -8).RotatedBy((i - 2) * MathF.PI * (1f/9f)),  ProjectileID.Nail, (int)((double)npc.damage * 0.15f), 1f, Main.myPlayer);
                    }
                }
            }
        }
    }
    public class NailheadNail : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if(projectile.type == ProjectileID.Nail)
            {
                projectile.timeLeft = 5 * 60;
            }
        }
    }
}