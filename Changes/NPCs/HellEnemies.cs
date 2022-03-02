using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.NPCs.Boomxie;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs
{
    public class HellEnemies: GlobalNPC
    {               
	
	public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Hellbat:
                    npc.damage = 40; 
                    npc.lifeMax = 70; 
                    return;
                case NPCID.LavaSlime:
                    npc.damage = 80; // up from 15
                    npc.lifeMax = 150; // up from 50
                    npc.knockBackResist = 0.4f; // up from 0%
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
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
        }
        public override void OnKill(NPC npc)
        {
            Vector2 zero = new Vector2(0, 0);
            if (npc.type == NPCID.BurningSphere && Main.expertMode)
            {
                Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), npc.Center, zero, ProjectileType<Boom>(), npc.damage, 0);
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