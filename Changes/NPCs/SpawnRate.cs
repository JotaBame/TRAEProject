using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.DreadItems.RedPearl;
using TRAEProject.NewContent.NPCs.Underworld.Beholder;

namespace TRAEProject.Changes.NPCs
{
    public class SpawnRate : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            //if(Main.CurrentFrameFlags.AnyActiveBossNPC)
            //{
            //    spawnRate = 0;
            //    maxSpawns = 0;
            //}
            //if(NPC.AnyNPCs(NPCID.BloodNautilus) || NPC.AnyNPCs(ModContent.NPCType<BeholderNPC>()))
            //{
            //    spawnRate = 0;
            //    maxSpawns = 0;
            //}
            if (player.GetModPlayer<PearlEffects>().spawnUp)
            {
                spawnRate = (int)(spawnRate * 0.75);
                maxSpawns = (int)(maxSpawns * 2f);
            }
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
{
    if (!TRAEWorld.downedAMech && npc.type == NPCID.Steampunker)
        npc.life = 0;
}

    }
}
