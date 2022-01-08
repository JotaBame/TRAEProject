using TRAEProject.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NPCs.Boomxie;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject
{
    public class HellEnemies: GlobalNPC
    {               
	
	public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC npc)
        {
            switch (npc.type)
            {
				case NPCID.LavaSlime:
                    npc.damage = 40; // up from 15
                    npc.lifeMax = 50; // up from 50
                    npc.knockBackResist = 0.7f; // up from 0%
                    return;
                case NPCID.BoneSerpentHead:
                    npc.damage = 70; // up from 30
                    npc.lifeMax = 250; // up from 250
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
        public override void ScaleExpertStats(NPC npc, int numPlayers, float bossLifeScale)
        {           
            switch (npc.type)
            {
                case NPCID.Hellbat:
                    npc.lifeMax = 75;
                    return;
            }
        }
        public bool runonce = false;
        public override void AI(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Hellbat:
                    if (Main.expertMode && !runonce)
                    {
                        for (int i = 0; i < 2; ++i)
                        {
                            NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, NPCID.Hellbat);
                            if (Main.rand.Next(3) == 0)
							{
								NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, NPCID.Hellbat);
							}
							runonce = true;
                        }
                    }
                    return;
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
        }
        public override void OnKill(NPC npc)
        {
            Vector2 zero = new Vector2(0, 0);
            if (npc.type == NPCID.BurningSphere)
            {
                Projectile.NewProjectile(npc.GetProjectileSpawnSource(), npc.Center, zero, ProjectileType<Boom>(), npc.damage, 0);
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
    }
}