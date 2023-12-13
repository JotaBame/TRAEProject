using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs
{

    public class EvilBiomeEnemies: GlobalNPC
    {               
	
	public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC npc)
        {
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.BloodCrawler:
                case NPCID.BloodCrawlerWall:
                    npcLoot.Add(ItemDropRule.Common(ItemID.WormTooth, 3, 1));
                    return;
                case NPCID.Crimera:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Leather, 9, 1));
                    return;
                case NPCID.FaceMonster:
                    npcLoot.Add(ItemDropRule.Common(ItemID.WormTooth, 4, 2, 5));
                    npcLoot.Add(ItemDropRule.Common(ItemID.Leather, 5, 1));
                    return;
                case NPCID.EaterofSouls:
                    npcLoot.Add(ItemDropRule.Common(ItemID.Leather, 10, 1));
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.AncientShadowHelmet; // compare more fields if needed
                    });
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.AncientShadowScalemail; // compare more fields if needed
                    });
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.AncientShadowGreaves; // compare more fields if needed
                    });
                    return;
                case NPCID.DevourerHead:
                    npcLoot.RemoveWhere(rule =>
                    {
                        if (rule is not CommonDrop drop) // Type of drop you expect here
                            return false;
                        return drop.itemId == ItemID.RottenChunk; // compare more fields if needed
                    });
                    npcLoot.Add(ItemDropRule.Common(ItemID.RottenChunk, 1, 1, 2));
                    npcLoot.Add(ItemDropRule.Common(ItemID.Leather, 4, 1));
                    return;
            }
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneCorrupt)
            {
                float spawnrate = Main.hardMode ? 0.02f : 0.125f;
                pool.Remove(NPCID.DevourerHead);
                pool.Add(NPCID.DevourerHead, spawnrate);
            }


        }
    }
    
}