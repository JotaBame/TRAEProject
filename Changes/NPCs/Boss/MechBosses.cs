using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss
{
    public class MechBosses : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
            switch (npc.type)
            {
                //case NPCID.TheDestroyer:


                //    npc.lifeMax = (int)(npc.lifeMax * ((float)30000 / 80000));

                //    break;
            //    case NPCID.SkeletronPrime:
          

				        //npc.lifeMax = (int)(npc.lifeMax  * ((float)24000 / 28000));
            //            npc.defense = 20;
                   
            //        break;
            //    case NPCID.PrimeVice:
                   
				        //npc.lifeMax = (int)(npc.lifeMax  * (float)(12000 / 9000));
                    
            //        break;
            //    case NPCID.PrimeLaser:
                    
            //            npc.lifeMax = 6000;
				        //npc.lifeMax = (int)(npc.lifeMax  * ((float)6000 / 6000));
            //            npc.damage = 90;
                    
            //        break;
            //    case NPCID.PrimeSaw:
				        //npc.lifeMax = (int)(npc.lifeMax  * ((float)6000 / 9000));
            //            npc.damage = 90;
                    
            //        break;
                
            }
        }

        static bool IsNPCTypeDestroyer(NPC npc) => npc.type >= NPCID.TheDestroyer && npc.type <= NPCID.TheDestroyerTail;
        static int GetAmountOfIframes(Projectile projectile)
        {
            if (projectile.stopsDealingDamageAfterPenetrateHits)
                return int.MaxValue;
            if (projectile.usesOwnerMeleeHitCD)
                return Main.player[projectile.owner].itemAnimation;
            if (projectile.usesIDStaticNPCImmunity)
                return projectile.idStaticNPCHitCooldown;
            if (projectile.usesLocalNPCImmunity)
                return projectile.localNPCHitCooldown < 1 ? 10 : projectile.localNPCHitCooldown / projectile.MaxUpdates;
            return 10;
        }

        //public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        //{
        //    if (IsNPCTypeDestroyer(npc))
        //        destroyerIframes[projectile.whoAmI] = GetAmountOfIframes(projectile);
        //}
        //static int[] destroyerIframes = new int[Main.maxProjectiles + Main.maxPlayers];//ok so basically the first 1000 slots are for projs and the latter 255 slots are for players
        //public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        //{
        //    if (IsNPCTypeDestroyer(npc))
        //        destroyerIframes[player.whoAmI + Main.maxProjectiles] = player.itemAnimation;
        //}
        //public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
        //{
        //    if (!IsNPCTypeDestroyer(npc) || (destroyerIframes[player.whoAmI + Main.maxProjectiles] < 1))
        //        return null;
        //    return false;
        //}
        /// <summary>
        /// THIS ASSUMES THAT YOU CHECK IF THE NPC IS A DESTROYER BEFOREHAND
        /// </summary>
        //static bool IsDestroyerImmuneToThis(Projectile projectile, NPC npc)
        //{
        //    if (!projectile.friendly || projectile.DistanceSQ(npc.Center) > 40000)//checking distance for optimization
        //        return true;
        //    if (projectile.usesIDStaticNPCImmunity)
        //        if (destroyerIframes[projectile.whoAmI] < 1 && projectile.friendly)
        //        {
        //            for (int i = 0; i < Main.maxNPCs; i++)
        //            {
        //                if (!npc.active || !IsNPCTypeDestroyer(Main.npc[i]))
        //                    continue;
        //                if (Projectile.perIDStaticNPCImmunity[projectile.type][i] > 0)
        //                    return true;
        //            }
        //        }
        //    if (projectile.usesLocalNPCImmunity || projectile.usesOwnerMeleeHitCD || projectile.stopsDealingDamageAfterPenetrateHits)
        //        return destroyerIframes[projectile.whoAmI] > 1;
        //    for (int i = 0; i < Main.maxProjectiles; i++)//attempt at mimmicking global iframes
        //    {
        //        if (destroyerIframes[i] > 0)
        //            return true;
        //    }
        //    return false;
        //}
        //public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
        //{
        //    if (!IsNPCTypeDestroyer(npc) || !IsDestroyerImmuneToThis(projectile, npc))
        //        return null;
        //    return false;
        //}
        //public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => IsNPCTypeDestroyer(entity);
        ///// <summary>
        ///// CALL THIS ON PRE AI OF GLOBAL NPC
        ///// </summary>
        //static void UpdateDestroyerIframes(NPC npc)
        //{
        //    if (npc.type == NPCID.TheDestroyer)
        //    {
        //        for (int i = 0; i < destroyerIframes.Length; i++)
        //        {
        //            if (i < 1000 && !Main.projectile[i].active)
        //                destroyerIframes[i] = 0;
        //            destroyerIframes[i]--;
        //        }
        //    }
        //}
        //public override bool PreAI(NPC npc)
        //{
        //    if (IsNPCTypeDestroyer(npc))
        //    {
        //        UpdateDestroyerIframes(npc);
        //    }
        //    return true;
        //}
    }  
}
