using TRAEProject.NewContent.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Common;

namespace TRAEProject.Changes.NPCs
{
    public class GeneralNPCChanges : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC npc)
        {
            switch (npc.type)
            {
    
                case NPCID.PirateCaptain:
                    npc.lifeMax = 750;
                    break;
                case NPCID.JungleCreeper:
                case NPCID.JungleCreeperWall:
                    {
                        npc.lifeMax = 110; // from 400
                        npc.damage = 25; // from 100
                        npc.defense = 12; // from 40
                    }
                    break;
                case NPCID.GigaZapper:
                    npc.knockBackResist = 0f;
                    break;
                case NPCID.VortexSoldier:
                    npc.damage = 110; // up from 90
                    npc.lifeMax = 550; // down from 700
                    npc.knockBackResist = 0.15f; // buffed from 0.6
                    break;
                case NPCID.VortexRifleman:
                    npc.lifeMax = 800; // unchanged
                    break;
                case NPCID.SolarDrakomire:
                    npc.lifeMax = 1250; // up from 800;
                    npc.defense = 48;
                    npc.knockBackResist = 0f; // nerfed from 0.2
                    break;
                case NPCID.SolarCorite:
                    npc.lifeMax = 600; // down from 600 
                    npc.knockBackResist = 0.6f; // nerfed from 0.2
                    break;
                case NPCID.SolarSolenian:
                    npc.lifeMax = 800; // unchanged from 800 
                    npc.damage = 90; // unchanged from 90
                    npc.knockBackResist = 0.8f; // nerfed from 0.4
                    break;
                case NPCID.SolarSroller:
                    npc.damage = 60; // down from 80
                    npc.knockBackResist = 0.35f; // nerfed from 1
                    break;
                case NPCID.SolarCrawltipedeHead:
                    npc.lifeMax = 25000; 
                    npc.damage = 100; // down from 150
                    break;
                case NPCID.SolarCrawltipedeBody:            
                    npc.damage = 50; // down from 100
                    break;
                case NPCID.SolarCrawltipedeTail:
                    npc.damage = 50; // unchanged
                    break;
                case NPCID.NebulaHeadcrab:
                    npc.damage = 60; // down from 70
                    npc.lifeMax = 480; // up from 330
                    npc.defense = 34; // unchanged from 34
                    npc.knockBackResist = 0.85f;
                    break;
                case NPCID.StardustCellSmall:
                    npc.lifeMax = 300; // unchanged from 300
                    npc.damage = 40;
                    break;
                case NPCID.StardustCellBig:
                    npc.lifeMax = 400; // up from 300
                    npc.damage = 40;
                    break;
                case NPCID.StardustJellyfishBig:
                    npc.lifeMax = 2000; // up from 1500
                    break;
                case NPCID.StardustSpiderBig:
                    npc.scale = 1.5f;
                    npc.width = 63;
                    npc.height = 54;
                    npc.knockBackResist = 0.05f;
                    break;
                case NPCID.StardustWormHead:
                    npc.lifeMax = 3300;
                    npc.defense = 50;
                    break;
                case NPCID.MoonLordHead:
                    npc.width = 51;
                    npc.height = 81;
                    break;
                case NPCID.DuneSplicerHead:
                    npc.lifeMax = 3500;
                    npc.noGravity = true;
                    break;  
				case NPCID.DuneSplicerBody:
                    npc.damage = 30;
                    npc.noGravity = true;
                    break;
            }
        }
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            switch (npc.type)
            {
                case NPCID.StardustCellBig:
                    {
                        int length = Main.rand.Next(90, 180);
                        target.AddBuff(BuffType<Celled>(), length, false);
                    }
                    break;
                case NPCID.SolarSroller:
                    if (Main.expertMode && npc.ai[0] == 6.0 /*&& npc.ai[3] > 0 && npc.ai[3] < 4.0*/)
                    {
                        int length = Main.rand.Next(240, 420);
                        target.AddBuff(BuffID.Dazed, length, false);
                    }
                    break;
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)/* tModPorter Note:bossAdjustment -> balance (bossAdjustment is different, see the docs for details) */
        {

            switch (npc.type)
            {
                case NPCID.SolarFlare:
                    npc.lifeMax = 200;
                    return;
                case NPCID.SolarSpearman:
                    npc.knockBackResist = 0f;
                    return;
                case NPCID.StardustSpiderSmall:
                    npc.knockBackResist = 0f;
                    return;
            }
        }
        bool onlyOnce = false;
        //public override bool PreAI(NPC npc)
        //{
        //    if (npc.type == NPCID.MoonLordFreeEye &&!onlyOnce)
        //    {
        //        for (int i = 0; i < 200; i++)
        //        {
        //            if (Main.npc[i].type == NPCID.MoonLordFreeEye)
        //            {
        //                for (int l = 0; l < 4; l++)
        //                {
        //                    npc.ai[l] = Main.npc[i].ai[l];
        //                }

        //            }
        //        }
        //        onlyOnce = true;
        //        return true;
        //    }
        //    return default;
        //}

        public float braintimer = 0;
        public override void AI(NPC npc)
        {
            switch (npc.type)
            {

                case NPCID.SolarCorite:
                    npc.knockBackResist = 0.2f;
                    return;
                case NPCID.SolarSolenian:
                    npc.reflectsProjectiles = false;
                    npc.takenDamageMultiplier = 1f;
                    return;
                case NPCID.StardustSpiderSmall:
                    int spidres = Main.rand.Next(2, 4);
                    if (npc.ai[2] < spidres && Main.expertMode)
                    {
                        int spider = NPC.NewNPC(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, NPCID.StardustSpiderSmall);
                        Main.npc[spider].velocity = npc.velocity;
                        Main.npc[spider].ai[2] = spidres;
                        npc.ai[2] += 1f;
                    }
                    return;
    
                case NPCID.LunarTowerNebula:
                    if (Main.expertMode)
                    {
                        braintimer += 1f;
                        if (braintimer == 1200f && npc.HasPlayerTarget)
                        {
                            for (int i = 0; i < Main.rand.Next(5, 7); i++)
                            {
                                NPC.NewNPC(npc.GetSource_Loot(), (int)npc.position.X, (int)npc.position.Y, NPCID.NebulaHeadcrab);
                                for (int x = 0; x < Main.rand.Next(10, 15); x++)
                                {
                                    Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 243, 0f, 0f, 0, default, 1);
                                    dust.noLight = true;
                                    dust.velocity *= 0.5f;
                                }
                            }
                            braintimer = 0f;
                        }
                    }
                    return;
                case NPCID.DuneSplicerHead:
                    if(npc.velocity.Length() > 2f)
                    {
                        npc.velocity = npc.velocity.SafeNormalize(Vector2.UnitY) * 5f;
                    }
                    return;
            }
            switch (npc.aiStyle)
            {
                case 85: // Stardust Cell AND Brain Suckler
                    if (npc.type == NPCID.StardustCellBig && Main.expertMode) //only for the Cell, on Expert
                    {
                        npc.noTileCollide = false;
                        if (npc.ai[0] == 0f)
                        {
                            npc.TargetClosest(true);
                            if (Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                            {
                                npc.ai[0] = 1f;
                            }
                            else
                            {
                                Vector2 value31 = Main.player[npc.target].Center - npc.Center;
                                ref float x = ref value31.Y;
                                x -= (Main.player[npc.target].height / 4);
                                float num1256 = value31.Length();
                                if (num1256 > 800f)
                                {
                                    npc.ai[0] = 2f;
                                }
                                else
                                {
                                    Vector2 center33 = npc.Center;
                                    center33.X = Main.player[npc.target].Center.X;
                                    Vector2 vector216 = center33 - npc.Center;
                                    if (vector216.Length() > 8f && Collision.CanHit(npc.Center, 1, 1, center33, 1, 1))
                                    {
                                        npc.ai[0] = 3f;
                                        npc.ai[1] = center33.X;
                                        npc.ai[2] = center33.Y;
                                        Vector2 center34 = npc.Center;
                                        center34.Y = Main.player[npc.target].Center.Y;
                                        if (vector216.Length() > 8f && Collision.CanHit(npc.Center, 1, 1, center34, 1, 1) && Collision.CanHit(center34, 1, 1, Main.player[npc.target].position, 1, 1))
                                        {
                                            npc.ai[0] = 3f;
                                            npc.ai[1] = center34.X;
                                            npc.ai[2] = center34.Y;
                                        }
                                    }
                                    else
                                    {
                                        center33 = npc.Center;
                                        center33.Y = Main.player[npc.target].Center.Y;
                                        vector216 = center33 - npc.Center;
                                        if (vector216.Length() > 8f && Collision.CanHit(npc.Center, 1, 1, center33, 1, 1))
                                        {
                                            npc.ai[0] = 3f;
                                            npc.ai[1] = center33.X;
                                            npc.ai[2] = center33.Y;
                                        }
                                    }
                                    if (npc.ai[0] == 0f)
                                    {
                                        npc.localAI[0] = 0f;
                                        value31.Normalize();
                                        value31 *= 0.5f;
                                        npc.velocity += value31;
                                        npc.ai[0] = 4f;
                                        npc.ai[1] = 0f;
                                    }
                                }
                            }
                        }
                        if (npc.ai[0] == 1f)  // MAY BE THE ONLY PART THAT SHOULD BE HERE
                        {
                            npc.rotation += npc.direction * 0.3f;
                            Vector2 value32 = Main.player[npc.target].Center - npc.Center;
                            float num1257 = value32.Length();
                            float num1258 = 14f; //changed from 5.5f
                            num1258 += num1257 / 100f;
                            int num1259 = 50;
                            value32.Normalize();
                            value32 *= num1258;
                            npc.velocity = (npc.velocity * (num1259 - 1) + value32) / num1259;
                            if (!Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                            {
                                npc.ai[0] = 0f;
                                npc.ai[1] = 0f;
                            }
                        }
                        else if (npc.ai[0] == 2f)
                        {
                            npc.rotation = npc.velocity.X * 0.1f;
                            npc.noTileCollide = true;
                            Vector2 value33 = Main.player[npc.target].Center - npc.Center;
                            float num1261 = value33.Length();
                            float scaleFactor11 = 3f; // MAYBE THIS ONE OR THE ONE BELOW AFFECTS SPEED TOO
                            int num1262 = 3;
                            value33.Normalize();
                            value33 *= scaleFactor11;
                            npc.velocity = (npc.velocity * (float)(num1262 - 1) + value33) / (float)num1262;
                            if (num1261 < 600f && !Collision.SolidCollision(npc.position, npc.width, npc.height))
                            {
                                npc.ai[0] = 0f;
                            }
                        }
                        else if (npc.ai[0] == 3f)
                        {
                            npc.rotation = npc.velocity.X * 0.1f;
                            Vector2 value34 = new Vector2(npc.ai[1], npc.ai[2]);
                            Vector2 value35 = value34 - npc.Center;
                            float num1263 = value35.Length();
                            float num1264 = 2f; // MAY AFFECT SPEED
                            float num1265 = 3f; // MAY AFFECT SPEED
                            value35.Normalize();
                            value35 *= num1264;
                            npc.velocity = (npc.velocity * (num1265 - 1f) + value35) / num1265;
                            if (npc.collideX || npc.collideY)
                            {
                                npc.ai[0] = 4f;
                                npc.ai[1] = 0f;
                            }
                            if (num1263 < num1264 || num1263 > 800f || Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                            {
                                npc.ai[0] = 0f;
                            }
                        }
                        else if (npc.ai[0] == 4f)
                        {
                            npc.rotation = npc.velocity.X * 0.1f;
                            if (npc.collideX)
                            {
                                ref float x1 = ref npc.velocity.X;
                                x1 *= -0.8f;
                            }
                            if (npc.collideY)
                            {
                                ref float x2 = ref npc.velocity.Y;
                                x2 *= -0.8f;
                            }
                            Vector2 value36;
                            if (npc.velocity.X == 0f && npc.velocity.Y == 0f)
                            {
                                value36 = Main.player[npc.target].Center - npc.Center;
                                ref float x3 = ref value36.Y;
                                x3 -= Main.player[npc.target].height / 4;
                                value36.Normalize();
                                npc.velocity = value36 * 0.1f;
                            }
                            float scaleFactor12 = 2f; // MAY AFFECT SPEED
                            float num1266 = 20f;  // MAY AFFECT SPEED
                            value36 = npc.velocity;
                            value36.Normalize();
                            value36 *= scaleFactor12;
                            npc.velocity = (npc.velocity * (num1266 - 1f) + value36) / num1266;
                            ref float x = ref npc.ai[1];
                            x += 1f;
                            if (npc.ai[1] > 180f)
                            {
                                npc.ai[0] = 0f;
                                npc.ai[1] = 0f;
                            }
                            if (Collision.CanHit(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                            {
                                npc.ai[0] = 0f;
                            }
                            ref float x4 = ref npc.localAI[0];
                            x4 += 1f;
                            if (npc.localAI[0] >= 5f && !Collision.SolidCollision(npc.position - new Vector2(10f, 10f), npc.width + 20, npc.height + 20))
                            {
                                npc.localAI[0] = 0f;
                                Vector2 center35 = npc.Center;
                                center35.X = Main.player[npc.target].Center.X;
                                if (Collision.CanHit(npc.Center, 1, 1, center35, 1, 1) && Collision.CanHit(npc.Center, 1, 1, center35, 1, 1) && Collision.CanHit(Main.player[npc.target].Center, 1, 1, center35, 1, 1))
                                {
                                    npc.ai[0] = 3f;
                                    npc.ai[1] = center35.X;
                                    npc.ai[2] = center35.Y;
                                }
                                else
                                {
                                    center35 = npc.Center;
                                    center35.Y = Main.player[npc.target].Center.Y;
                                    if (Collision.CanHit(npc.Center, 1, 1, center35, 1, 1) && Collision.CanHit(Main.player[npc.target].Center, 1, 1, center35, 1, 1))
                                    {
                                        npc.ai[0] = 3f;
                                        npc.ai[1] = center35.X;
                                        npc.ai[2] = center35.Y;
                                    }
                                }
                            }
                        }
                        else if (npc.ai[0] == 5f)
                        {
                            Player player7 = Main.player[npc.target];
                            if (!player7.active || player7.dead)
                            {
                                npc.ai[0] = 0f;
                                npc.ai[1] = 0f;
                                npc.netUpdate = true;
                            }
                            else
                            {
                                npc.Center = ((player7.gravDir == 1f) ? player7.Top : player7.Bottom) + new Vector2((float)(player7.direction * 4), 0f);
                                npc.gfxOffY = player7.gfxOffY;
                                npc.velocity = Vector2.Zero;
                                player7.AddBuff(163, 59, false);
                            }
                        }
                        if (npc.type == NPCID.StardustCellBig)
                        {
                            npc.rotation = 0f;
                            int num2;
                            for (int num1267 = 0; num1267 < 200; num1267 = num2 + 1)
                            {
                                if (num1267 != npc.whoAmI && Main.npc[num1267].active && Main.npc[num1267].type == npc.type && Math.Abs(npc.position.X - Main.npc[num1267].position.X) + Math.Abs(npc.position.Y - Main.npc[num1267].position.Y) < (float)npc.width)
                                {
                                    if (npc.position.X < Main.npc[num1267].position.X)
                                    {
                                        ref float x = ref npc.velocity.X;
                                        x -= 0.05f;
                                    }
                                    else
                                    {
                                        ref float x = ref npc.velocity.X;
                                        x += 0.05f;
                                    }
                                    if (npc.position.Y < Main.npc[num1267].position.Y)
                                    {
                                        ref float x = ref npc.velocity.Y;
                                        x -= 0.05f;
                                    }
                                    else
                                    {
                                        ref float x = ref npc.velocity.Y;
                                        x += 0.05f;
                                    }
                                }
                                num2 = num1267;
                            }
                        }
                    }
                    return;

            }
        }
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            switch (npc.type)
            {

                case NPCID.SolarSpearman:
                    for (int i = 0; i < 5; i++)
                    {
                        int num = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 174, 0f, 0f, 100, default, 2f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].noLight = true;
                    }
                    return;
            }
        }
        public override void OnKill(NPC npc)
        {
            //if (npc.type == NPCID.WallofFlesh)
            //{
            //    NPC.downedMechBoss1 = true;
            //    NPC.downedMechBoss2 = true;
            //    NPC.downedMechBoss3 = true;
            //    NPC.downedMechBossAny = true;
            //}
            if (npc.type == NPCID.Plantera)
            {
                TRAEWorld.downedOvergrowth = true;
            }

        }
    }
}