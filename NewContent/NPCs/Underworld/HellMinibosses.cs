using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.NPCs.Underworld.Boomxie;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.NPCs.Underworld.Phoenix;
using TRAEProject.NewContent.NPCs.Underworld.Salalava;
using TRAEProject.NewContent.NPCs.Underworld.Lavamander;
using TRAEProject.NewContent.NPCs.Underworld.ObsidianBasilisk;

namespace TRAEProject.NewContent.NPCs.Underworld
{ 
    public class HellMinibosses : GlobalNPC
    {

        public override bool InstancePerEntity => true;

        public static int[] MinibossList = new int[] { NPCType<PhoenixNPC>(), NPCType<SalalavaNPC>(), NPCType<PhoenixAsh>(), NPCType<ObsidianBasiliskHead>(), NPCType<ObsidianBasiliskBody>(), NPCType<ObsidianBasiliskTail>()};
        // important to include the phoenix ash, else an NPC may spawn while the phoenix is in its Ash phase
        public bool HellMinibossThatSpawnsInPairs = false;

        public override void SpawnNPC(int npc, int tileX, int tileY)
        {

            NPC npc1 = Main.npc[npc];

            if (npc1.GetGlobalNPC<HellMinibosses>().HellMinibossThatSpawnsInPairs)
            {
                int spawn = Main.rand.Next(MinibossList);
                while (spawn == npc1.type || spawn == NPCType<PhoenixAsh>() || spawn == NPCType<ObsidianBasiliskBody>() || spawn == NPCType<ObsidianBasiliskTail>()) // if it's about to spawn the phoenix ash or the same NPC, it rerolls until it doesnt
                {
                    spawn = Main.rand.Next(MinibossList);
                }
                NPC npc2 = NPC.NewNPCDirect(npc1.GetSource_FromAI(), (int)npc1.Center.X, (int)npc1.Center.Y, spawn);
            }
        }
    }
}