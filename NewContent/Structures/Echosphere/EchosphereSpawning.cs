using System.Collections.Generic;
using System.Threading.Channels;
using Terraria;
using Terraria.ModLoader;
using TRAEProject.NewContent.NPCs.Echosphere.EchoLeviathan;
using TRAEProject.NewContent.NPCs.Echosphere.EchoLocator;
using TRAEProject.NewContent.NPCs.Echosphere.EchoSprite;
using TRAEProject.NewContent.NPCs.Echosphere.EchoStalker;
using TRAEProject.NewContent.NPCs.GraniteOvergrowth;

namespace TRAEProject.NewContent.Structures.Echosphere
{
    public class EchosphereSpawning : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (EchosphereSystem.PlayerInEchosphere(spawnInfo.Player))
            {
                pool.Clear();

                if (Main.hardMode)
                {
                    //idk how this really works...
                    if (!NPC.AnyNPCs(ModContent.NPCType<EchoLeviathanHead>()))
                    {
                        pool.Add(ModContent.NPCType<EchoLeviathanHead>(), 0.05f);
                    }
                    pool.Add(ModContent.NPCType<EchoLocator>(), 0.4f);
                    pool.Add(ModContent.NPCType<EchoStalkerHead>(), 0.2f);
                    pool.Add(ModContent.NPCType<EchoSprite>(), 0.3f);
                }
                else
                {
                    
                     if (!NPC.AnyNPCs(ModContent.NPCType<EchoStalkerHead>()))
                    {
                        pool.Add(ModContent.NPCType<EchoStalkerHead>(), 0.2f);

                    }



                }
            }
        }
    }
}
