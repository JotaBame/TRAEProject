using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.NPCs.Underworld.Boomxie;
using TRAEProject.NewContent.NPCs.Underworld.Froggabomba;
using TRAEProject.NewContent.NPCs.Underworld.Lavamander;
using TRAEProject.NewContent.NPCs.Underworld.ObsidianBasilisk;
using TRAEProject.NewContent.NPCs.Underworld.OniRonin;
using TRAEProject.NewContent.NPCs.Underworld.Phoenix;
using TRAEProject.NewContent.NPCs.Underworld.Salalava;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs
{
    public class HellEnemies : GlobalNPC
    {

        public override bool InstancePerEntity => true;
        public bool despawn = false;
        public override void SetDefaults(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Hellbat:
                    npc.damage = 40;
                    npc.lifeMax = 70;
                    return;
                    
                case NPCID.LavaSlime:
                    if (!Main.remixWorld)
                    { 
                        npc.damage = 50; // up from 15
                    npc.lifeMax = 150; // up from 50
                    npc.knockBackResist = 0.4f; // up from 0%
                    }
                    return;
                case NPCID.BoneSerpentHead:
                    npc.damage = 70; // up from 30
                    npc.lifeMax = 400; // up from 250
                    return;
                case NPCID.BoneSerpentBody:
                    npc.damage = 30; // up from 15
                    npc.defense = 40; // up from 12
                    return;
                case NPCID.Demon:
                case NPCID.VoodooDemon:
                    npc.defense = 20; // up from 8
                    npc.knockBackResist = 0.4f; // up from 0.8
                    return;
                case NPCID.FireImp:
                    npc.defense = 22; // up from 16
                    npc.lifeMax = 90; // up from 70
                    return;
            }
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)  
        {
            if (player.lavaWet && player.ZoneUnderworldHeight && !Main.remixWorld)
            {
                spawnRate = (int)(spawnRate * 1.5f);
            }
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
  

            if (Main.remixWorld && spawnInfo.Player.ZoneUnderworldHeight && spawnInfo.PlayerFloorX > Main.maxTilesX / 3 && spawnInfo.PlayerFloorX < Main.maxTilesX * 2 / 3)
            {

                int[] removeThese =  { NPCType<OniRoninNPC>(), NPCType<SalalavaNPC>(), NPCType<ObsidianBasiliskHead>(), NPCType<PhoenixNPC>(), NPCType<LavamanderNPC>(), NPCType<Lavalarva>(), NPCType<Froggabomba>(), NPCType<Boomxie>()};
                for (int k = 0; k < removeThese.Length; k++)
                {
                    pool.Remove(removeThese[k]);
                 }
            }
 
            if (spawnInfo.Player.ZoneUnderworldHeight && NPC.downedPlantBoss && !Main.remixWorld)
            {
                pool.Remove(NPCID.RedDevil);
                pool.Add(NPCID.RedDevil, 0.25f);
            }
            if (spawnInfo.Player.ZoneUnderworldHeight && NPC.downedPlantBoss)
            {
                int[] lowerTheseSpawnRates = { NPCID.LavaSlime, NPCID.FireImp, NPCID.Hellbat };
                for (int k = 0; k < lowerTheseSpawnRates.Length; k++)
                {
                    pool.Remove(lowerTheseSpawnRates[k]);
                    pool.Add(lowerTheseSpawnRates[k], 0.02f);
                }

            }
 

        }

        public override void OnKill(NPC npc)
        {
            Vector2 zero = new Vector2(0, 0);
            if (npc.type == NPCID.BurningSphere && Main.expertMode)
            {
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, zero, ProjectileType<Boom>(), 30, 0);
            }
        }
    }
    public class DemonScytheChange : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileID.DemonSickle)
            {
                projectile.tileCollide = false;
            }
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            if (projectile.type == ProjectileID.UnholyTridentHostile)
            {
                modifiers.SourceDamage.Base /= 2;
            }
           }
    }
}