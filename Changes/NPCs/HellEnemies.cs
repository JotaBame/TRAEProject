using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.NPCs.Miniboss.Santa;
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
                    npc.damage = 50; // up from 30
                    npc.lifeMax = 400; // up from 250
                    return;
                case NPCID.BoneSerpentBody:
                    npc.damage = 30; // up from 15
                    npc.defense = 40; // up from 12
                    return;
                case NPCID.Demon:
                case NPCID.VoodooDemon:
                    npc.defense = 20; // up from 8
                    npc.noGravity = true;
                    return;
                case NPCID.FireImp:
                    npc.defense = 22; // up from 16
                    npc.lifeMax = 90; // up from 70
                    return;
            }
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.ZoneUnderworldHeight && !Main.remixWorld && !NPC.downedPlantBoss)
            {
                spawnRate = (int)(spawnRate * 0.67);
                maxSpawns = (int)(maxSpawns * 4 / 3);
            }
        }
        public override void AI(NPC npc)
        {
            if (npc.type == NPCID.Hellbat || npc.type == NPCID.Lavabat)
            {
                if (Math.Abs(npc.velocity.X) < 6.25f && Math.Abs(npc.velocity.X) > 1f)
                {
                    npc.velocity.X += 0.05f * npc.direction;
                }
                if (Math.Abs(npc.velocity.Y) < 3f )
                {
                    npc.velocity.Y += 0.03f * Math.Sign(npc.GetTargetData().Center.Y - npc.Center.Y);
                }
            }
            if (npc.type == NPCID.FireImp)
            {
                if (npc.ai[0] >= 450f && Main.netMode != 1)
                {
                    npc.ai[0] = 650f;
                    //When ai 0 reaches 650 it teleports.  when the fire imp is hit, ai[0 is set to 400. This is dumb but it's too annoying to change, so what i do is effectively shorten the cooldown to 450, still killed by focused fire but can slip away if it stops for a moment.

                }

            }
        }
        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon)
            {
                
                npc.TargetClosest();
       
                if (npc.wet)
                {
                    if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y *= 0.95f;
                    }
                    npc.velocity.Y -= 0.5f;
                    if (npc.velocity.Y < -4f)
                    {
                        npc.velocity.Y = -4f;
                    }
                }
                 else
                {
                    if (npc.direction == -1 && npc.velocity.X > -4f)
                    {
                        npc.velocity.X -= 0.1f;
                        if (npc.velocity.X > 4f)
                        {
                            npc.velocity.X -= 0.1f;
                        }
                        else if (npc.velocity.X > 0f)
                        {
                            npc.velocity.X += 0.05f;
                        }
                        if (npc.velocity.X < -4f)
                        {
                            npc.velocity.X = -4f;
                        }
                    }
                    else if (npc.direction == 1 && npc.velocity.X < 4f)
                    {
                        npc.velocity.X += 0.1f;
                        if (npc.velocity.X < -4f)
                        {
                            npc.velocity.X += 0.1f;
                        }
                        else if (npc.velocity.X < 0f)
                        {
                            npc.velocity.X -= 0.05f;
                        }
                        if (npc.velocity.X > 4f)
                        {
                            npc.velocity.X = 4f;
                        }
                    }
                    if (npc.directionY == -1 && (double)npc.velocity.Y > -1.5)
                    {
                        npc.velocity.Y -= 0.04f;
                        if ((double)npc.velocity.Y > 1.5)
                        {
                            npc.velocity.Y -= 0.05f;
                        }
                        else if (npc.velocity.Y > 0f)
                        {
                            npc.velocity.Y += 0.03f;
                        }
                        if ((double)npc.velocity.Y < -1.5)
                        {
                            npc.velocity.Y = -1.5f;
                        }
                    }
                    else if (npc.directionY == 1 && (double)npc.velocity.Y < 1.5)
                    {
                        npc.velocity.Y += 0.04f;
                        if ((double)npc.velocity.Y < -1.5)
                        {
                            npc.velocity.Y += 0.05f;
                        }
                        else if (npc.velocity.Y < 0f)
                        {
                            npc.velocity.Y -= 0.03f;
                        }
                        if ((double)npc.velocity.Y > 1.5)
                        {
                            npc.velocity.Y = 1.5f;
                        }
                    }
                }
                npc.ai[1] += 1f;
                if (npc.ai[1] > 200f)
                {
                    if (!Main.player[npc.target].wet && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                    {
                        npc.ai[1] = 0f;
                    }
                    float num217 = 0.2f;
                    float num218 = 0.1f;
                    float num219 = 4f;
                    float num220 = 1.5f;
                    if (npc.ai[1] > 1000f)
                    {
                        npc.ai[1] = 0f;
                    }
                    npc.ai[2] += 1f;
                    if (npc.ai[2] > 0f)
                    {
                        if (npc.velocity.Y < num220)
                        {
                            npc.velocity.Y += num218;
                        }
                    }
                    else if (npc.velocity.Y > 0f - num220)
                    {
                        npc.velocity.Y -= num218;
                    }
                    if (npc.ai[2] < -150f || npc.ai[2] > 150f)
                    {
                        if (npc.velocity.X < num219)
                        {
                            npc.velocity.X += num217;
                        }
                    }
                    else if (npc.velocity.X > 0f - num219)
                    {
                        npc.velocity.X -= num217;
                    }
                    if (npc.ai[2] > 300f)
                    {
                        npc.ai[2] = -300f;
                    }
                }
                if (Main.netMode == 1)
                {
                    return false;
                }
                npc.ai[0] += 1f;
                if (npc.Distance(Main.player[npc.target].Center) < 1200f)
                {


                    if (npc.ai[0] == 20f || npc.ai[0] == 40f || npc.ai[0] == 60f || npc.ai[0] == 80f)
                    {

                        float num228 = 0.2f;
                        Vector2 vector26 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        float num229 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector26.X + (float)Main.rand.Next(-100, 101);
                        float num230 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector26.Y + (float)Main.rand.Next(-100, 101);
                        float num231 = (float)Math.Sqrt(num229 * num229 + num230 * num230);
                        num231 = num228 / num231;
                        num229 *= num231;
                        num230 *= num231;
                        int num232 = 21;
                        int num233 = 44;
                        int num234 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector26.X, vector26.Y, num229, num230, num233, num232, 0f, Main.myPlayer);
                        Main.projectile[num234].timeLeft = 300;

                    }
                }
                else if (npc.ai[0] >= (float)(300 + Main.rand.Next(300)))
                {
                    npc.ai[0] = 0f;
                }


                return false;
            }

            return true;
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
     
            if (Main.remixWorld && spawnInfo.Player.ZoneUnderworldHeight && spawnInfo.PlayerFloorX > Main.maxTilesX / 3 && spawnInfo.PlayerFloorX < Main.maxTilesX * 2 / 3)
            {

                int[] removeThese = { NPCType<OniRoninNPC>(), NPCType<SalalavaNPC>(), NPCType<ObsidianBasiliskHead>(), NPCType<PhoenixNPC>(), NPCType<LavamanderNPC>(), NPCType<Lavalarva>(), NPCType<Froggabomba>(), NPCType<Boomxie>() };
                for (int k = 0; k < removeThese.Length; k++)
                {
                    pool.Remove(removeThese[k]);
                }
            }

            if (spawnInfo.Player.ZoneUnderworldHeight && NPC.downedPlantBoss && !Main.remixWorld)
            {
                pool.Remove(NPCID.RedDevil);
                pool.Add(NPCID.RedDevil, 0.25f);
 
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