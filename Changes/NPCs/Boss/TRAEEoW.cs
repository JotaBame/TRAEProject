using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.NPCs.Miniboss.Santa;
using TRAEProject.NewContent.Items.Accesories.LifeCuffs;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss
{
    public class EaterOfWorlds : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void SetStaticDefaults()
        {
            if (GetInstance<BossConfig>().EoWChanges)
            {
                NPCID.Sets.ImmuneToRegularBuffs[NPCID.EaterofWorldsHead] = true;
                NPCID.Sets.ImmuneToRegularBuffs[NPCID.EaterofWorldsBody] = true;
                NPCID.Sets.ImmuneToRegularBuffs[NPCID.EaterofWorldsTail] = true;
            }
        }
        public override void SetDefaults(NPC npc)
        {
            if (GetInstance<BossConfig>().EoWChanges)
            {
                if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
                {
                    npc.lifeMax = 180; // up from 150

                }
                if (npc.type == NPCID.EaterofWorldsBody)
                {
                    npc.defense = 8;
                }
            }

        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (npc.type == NPCID.VileSpitEaterOfWorlds && GetInstance<BossConfig>().EoWChanges)
            {

                return false;
            }
            return true;
        }
        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            if (Main.masterMode && GetInstance<BossConfig>().EoWChanges)
            {
                if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
                {
                    npc.lifeMax = (int)((float)(npc.lifeMax * 0.88f));
                }
            }
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if ((npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail) && GetInstance<BossConfig>().EoWChanges)
            {
                npc.localAI[1] -= damageDone * 4;
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (GetInstance<BossConfig>().EoWChanges && npc.type >= 13 && npc.type <= 15)
            {
                if (projectile.type == ProjectileID.VampireFrog)
                    modifiers.FinalDamage *= 0.6f;
                if (projectile.type == ProjectileID.VilethornTip || projectile.type == ProjectileID.VilethornBase)
                    modifiers.FinalDamage *= 0.33f;
                if (projectile.type == ProjectileID.Grenade || projectile.type == ProjectileID.StickyGrenade || projectile.type == ProjectileID.BouncyGrenade || projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.GiantBee || projectile.type == ProjectileID.Beenade)
                    modifiers.FinalDamage *= 5f;
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (GetInstance<BossConfig>().EoWChanges)
            {
                if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
                {
                    if (Main.netMode != 1 && Main.expertMode)
                    {
                        npc.localAI[1]++;
                        int healTime = 240;

                        if (npc.localAI[1] >= healTime)
                        {
                            npc.localAI[1] = 0;
                            float percentageHealed = 0.02f;

                            int healAmount = npc.lifeMax - npc.life;
                            if (healAmount > (int)(npc.lifeMax * percentageHealed))
                                healAmount = (int)(npc.lifeMax * percentageHealed);
                            if (healAmount > 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    NPC.HealEffect(npc.Hitbox, healAmount, true);
                                    npc.netUpdate = true;
                                }
                                npc.life += healAmount;
                            }
                        }
                    }

                    if (Main.netMode != 1 && Main.masterMode)
                    {
                        //if (npc.type == NPCID.EaterofWorldsBody && ((double)(npc.position.Y / 16f) < Main.worldSurface || Main.getGoodWorld))
                        //{
                        //    int num7 = (int)(npc.Center.X / 16f);
                        //    int num8 = (int)(npc.Center.Y / 16f);
                        //    // vile spit
                        //    if (WorldGen.InWorld(num7, num8) && Main.tile[num7, num8].WallType == 0)
                        //    {
                        //        int num9 = 900;
                        //        if (Main.getGoodWorld)
                        //        {
                        //            num9 /= 2;
                        //        }
                        //        if (Main.rand.Next(num9) == 0)
                        //        {
                        //            npc.TargetClosest();
                        //            if (Collision.CanHitLine(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                        //            {
                        //                //NPC.NewNPC(GetSpawnSourceForNPCFromNPCAI(), (int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), 666, 0, 0f, 1f);
                        //            }
                        //        }
                        //    }
                        //}
                        /*else */
                        if (npc.type == NPCID.EaterofWorldsHead)
                        {
                            npc.localAI[2]++;
                            if (npc.localAI[2] >= 300)
                            {
                                npc.localAI[2] = 0;
                                npc.TargetClosest();
                                if (Collision.CanHitLine(npc.Center, 1, 1, Main.player[npc.target].Center, 1, 1))
                                {
                                    NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2) + npc.velocity.X), (int)(npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y), NPCID.VileSpitEaterOfWorlds, 0, 0f, 1f);
                                }
                            }
                        }
                    }
                    bool flag = false;
                    float num11 = 0.2f;
                    if (npc.type >= NPCID.EaterofWorldsHead && npc.type <= NPCID.EaterofWorldsTail)
                    {
                        npc.realLife = -1;
                    }
                    else if (npc.ai[3] > 0f)
                    {
                        npc.realLife = (int)npc.ai[3];
                    }
                    if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
                        npc.alpha = 0;
                    if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || (flag && (double)Main.player[npc.target].position.Y < Main.worldSurface * 16.0))
                    {
                        npc.TargetClosest();
                    }
                    if (Main.player[npc.target].dead || (flag && (double)Main.player[npc.target].position.Y < Main.worldSurface * 16.0))
                    {
                        npc.EncourageDespawn(300);
                        if (flag)
                        {
                            npc.velocity.Y += num11;
                        }
                    }

                    else if ((npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody) && npc.ai[0] == 0f)
                    {
                        if (npc.type == NPCID.EaterofWorldsHead)
                        {


                            if (npc.type == NPCID.EaterofWorldsHead)
                            {
                                npc.ai[2] = NPC.GetEaterOfWorldsSegmentsCount();
                            }

                            int bodyS = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), npc.type + 1, npc.whoAmI);
                            npc.ai[0] = bodyS;

                            Main.npc[(int)npc.ai[0]].CopyInteractions(npc);
                        }
                        else if (npc.type == NPCID.EaterofWorldsBody)

                        {
                            if (npc.ai[2] > 0f)
                            {
                                int bodyS = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), npc.type, npc.whoAmI);
                                npc.ai[0] = bodyS;
                                Main.npc[(int)npc.ai[0]].CopyInteractions(npc);
                            }
                            else
                            {

                                int tailS = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), NPCID.EaterofWorldsTail, npc.whoAmI);
                                npc.ai[0] = tailS;

                                Main.npc[(int)npc.ai[0]].CopyInteractions(npc);
                            }
                        }


                        Main.npc[(int)npc.ai[0]].ai[1] = npc.whoAmI;
                        Main.npc[(int)npc.ai[0]].ai[2] = npc.ai[2] - 1f;
                        npc.netUpdate = true;
                        return true;
                    }


                    if (!Main.npc[(int)npc.ai[1]].active && !Main.npc[(int)npc.ai[0]].active)
                    {
                        npc.life = 0;
                        npc.HitEffect();
                        npc.checkDead();
                        npc.active = false;
                        NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                        return false;
                    }
                    if (npc.type == NPCID.EaterofWorldsHead && !Main.npc[(int)npc.ai[0]].active)
                    {
                        npc.life = 0;
                        npc.HitEffect();
                        npc.checkDead();
                        npc.active = false;
                        NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, npc.whoAmI, -1f);
                        return false;
                    }


                    if (npc.type == NPCID.EaterofWorldsTail && !Main.npc[(int)npc.ai[1]].active)
                    {
                        npc.life = 0;
                        npc.HitEffect();
                        npc.checkDead();
                        npc.active = false;
                        NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                        return false;
                    }
                    if (npc.type == NPCID.EaterofWorldsBody && (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != npc.aiStyle))
                    {
                        npc.type = NPCID.EaterofWorldsHead;
                        int num38 = npc.whoAmI;
                        float num39 = (float)npc.life / (float)npc.lifeMax;
                        float num40 = npc.ai[0];
                        npc.SetDefaultsKeepPlayerInteraction(npc.type);
                        npc.life = (int)((float)npc.lifeMax * num39);
                        npc.ai[0] = num40;
                        npc.TargetClosest();
                        npc.netUpdate = true;
                        npc.whoAmI = num38;
                        npc.alpha = 0;
                    }
                    if (npc.type == NPCID.EaterofWorldsBody && (!Main.npc[(int)npc.ai[0]].active || Main.npc[(int)npc.ai[0]].aiStyle != npc.aiStyle))
                    {
                        npc.type = NPCID.EaterofWorldsTail;
                        int num41 = npc.whoAmI;
                        float num42 = (float)npc.life / (float)npc.lifeMax;
                        float num43 = npc.ai[1];
                        npc.SetDefaultsKeepPlayerInteraction(npc.type);
                        npc.life = (int)((float)npc.lifeMax * num42);
                        npc.ai[1] = num43;
                        npc.TargetClosest();
                        npc.netUpdate = true;
                        npc.whoAmI = num41;
                        npc.alpha = 0;
                    }

                    if (!npc.active && Main.netMode == 2)
                    {
                        NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f);
                    }

                    int num44 = (int)(npc.position.X / 16f) - 1;
                    int num45 = (int)((npc.position.X + (float)npc.width) / 16f) + 2;
                    int num46 = (int)(npc.position.Y / 16f) - 1;
                    int num47 = (int)((npc.position.Y + (float)npc.height) / 16f) + 2;
                    if (num44 < 0)
                    {
                        num44 = 0;
                    }
                    if (num45 > Main.maxTilesX)
                    {
                        num45 = Main.maxTilesX;
                    }
                    if (num46 < 0)
                    {
                        num46 = 0;
                    }
                    if (num47 > Main.maxTilesY)
                    {
                        num47 = Main.maxTilesY;
                    }
                    bool flag2 = false;

                    if (!flag2)
                    {
                        Vector2 vector2 = default;
                        for (int num48 = num44; num48 < num45; num48++)
                        {
                            for (int num49 = num46; num49 < num47; num49++)
                            {
                                Tile tile = Main.tile[num48, num49];
                                if (tile == null || ((!(tile.HasTile && !tile.IsActuated) || (!Main.tileSolid[tile.TileType] && (!Main.tileSolidTop[tile.TileType] || tile.TileFrameY != 0))) && tile.LiquidAmount <= 64))
                                {
                                    continue;
                                }
                                vector2.X = num48 * 16;
                                vector2.Y = num49 * 16;
                                if (npc.position.X + (float)npc.width > vector2.X && npc.position.X < vector2.X + 16f && npc.position.Y + (float)npc.height > vector2.Y && npc.position.Y < vector2.Y + 16f)
                                {
                                    flag2 = true;
                                    if (Main.rand.NextBool(100) && npc.type != NPCID.LeechHead && tile.IsActuated && Main.tileSolid[tile.TileType])
                                    {
                                        WorldGen.KillTile(num48, num49, fail: true, effectOnly: true);
                                    }
                                }
                            }
                        }
                    }
                    if (!flag2 && npc.type == NPCID.EaterofWorldsHead)
                    {
                        Rectangle rectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                        int num50 = 1000;
                        bool flag3 = true;
                        for (int num51 = 0; num51 < 255; num51++)
                        {
                            if (Main.player[num51].active)
                            {
                                Rectangle rectangle2 = new Rectangle((int)Main.player[num51].position.X - num50, (int)Main.player[num51].position.Y - num50, num50 * 2, num50 * 2);
                                if (rectangle.Intersects(rectangle2))
                                {
                                    flag3 = false;
                                    break;
                                }
                            }
                        }
                        if (flag3)
                        {
                            flag2 = true;
                        }
                    }
                    float speed = 10f;
                    float acceleration = 0.07f;


                    if (Main.expertMode)
                    {
                        speed = 12f;
                        acceleration = 0.15f;
                    }

                    if (Main.masterMode)
                    {
                        speed = 15f;
                        if (npc.Distance(Main.player[npc.target].Center) > 450f)
                            acceleration *= 1.6f;
                    }


                    if (Main.getGoodWorld)
                    {
                        speed += 4f;
                        acceleration += 0.05f;
                    }
                    if ((double)(npc.position.Y / 16f) < Main.worldSurface)
                    {
                        acceleration *= 1.45f;


                    }
                    Vector2 vector5 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                    float playerCenterXTile = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2);
                    float playerCenterYTile = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2);

                    playerCenterXTile = (int)(playerCenterXTile / 16f) * 16;
                    playerCenterYTile = (int)(playerCenterYTile / 16f) * 16;
                    vector5.X = (int)(vector5.X / 16f) * 16;
                    vector5.Y = (int)(vector5.Y / 16f) * 16;
                    playerCenterXTile -= vector5.X;
                    playerCenterYTile -= vector5.Y;



                    float num68 = (float)Math.Sqrt(playerCenterXTile * playerCenterXTile + playerCenterYTile * playerCenterYTile);
                    if (npc.ai[1] > 0f && npc.ai[1] < (float)Main.npc.Length)
                    {
                        try
                        {
                            vector5 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                            playerCenterXTile = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - vector5.X;
                            playerCenterYTile = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - vector5.Y;
                        }
                        catch
                        {
                        }
                        npc.rotation = (float)Math.Atan2(playerCenterYTile, playerCenterXTile) + 1.57f;
                        num68 = (float)Math.Sqrt(playerCenterXTile * playerCenterXTile + playerCenterYTile * playerCenterYTile);
                        int num69 = npc.width;
                        if (npc.type >= NPCID.EaterofWorldsHead && npc.type <= NPCID.EaterofWorldsTail)
                        {
                            num69 = (int)((float)num69 * npc.scale);
                        }
                        if (Main.getGoodWorld && npc.type >= NPCID.EaterofWorldsHead && npc.type <= NPCID.EaterofWorldsTail)
                        {
                            num69 = 62;
                        }
                        num68 = (num68 - (float)num69) / num68;
                        playerCenterXTile *= num68;
                        playerCenterYTile *= num68;
                        npc.velocity = Vector2.Zero;
                        npc.position.X += playerCenterXTile;
                        npc.position.Y += playerCenterYTile;
                    }


                    else
                    {

                        if (!flag2)
                        {
                            npc.TargetClosest();
                            float gravity = 0.11f; // vanilla value
                            if (Main.expertMode)
                                gravity = 0.095f;

                            npc.velocity.Y += gravity;

                            if (npc.velocity.Y > speed)
                            {
                                npc.velocity.Y = speed;
                            }
                            if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)speed * 0.4)
                            {
                                if (npc.velocity.X < 0f)
                                {
                                    npc.velocity.X -= acceleration * 1.1f;
                                }
                                else
                                {
                                    npc.velocity.X += acceleration * 1.1f;
                                }
                            }
                            else if (npc.velocity.Y == speed)
                            {
                                if (npc.velocity.X < playerCenterXTile)
                                {
                                    npc.velocity.X += acceleration;
                                }
                                else if (npc.velocity.X > playerCenterXTile)
                                {
                                    npc.velocity.X -= acceleration;
                                }
                            }
                            else if (npc.velocity.Y > 4f)
                            {
                                if (npc.velocity.X < 0f)
                                {
                                    npc.velocity.X += acceleration * 0.9f;
                                }
                                else
                                {
                                    npc.velocity.X -= acceleration * 0.9f;
                                }
                            }
                        }
                        else
                        {
                            num68 = (float)Math.Sqrt(playerCenterXTile * playerCenterXTile + playerCenterYTile * playerCenterYTile);
                            float num71 = Math.Abs(playerCenterXTile);
                            float num72 = Math.Abs(playerCenterYTile);
                            float num73 = speed / num68;
                            playerCenterXTile *= num73;
                            playerCenterYTile *= num73;
                            bool flag4 = false;
                            if (npc.type == NPCID.EaterofWorldsHead && ((!Main.player[npc.target].ZoneCorrupt && !Main.player[npc.target].ZoneCrimson) || Main.player[npc.target].dead))
                            {
                                flag4 = true;
                            }
                            if (flag4)
                            {
                                bool flag5 = true;
                                for (int num74 = 0; num74 < 255; num74++)
                                {
                                    if (Main.player[num74].active && !Main.player[num74].dead && Main.player[num74].ZoneCorrupt)
                                    {
                                        flag5 = false;
                                    }
                                }
                                if (flag5)
                                {
                                    if (Main.netMode != 1 && (double)(npc.position.Y / 16f) > (Main.rockLayer + (double)Main.maxTilesY) / 2.0)
                                    {
                                        npc.active = false;
                                        int num75 = (int)npc.ai[0];
                                        while (num75 > 0 && num75 < 200 && Main.npc[num75].active && Main.npc[num75].aiStyle == npc.aiStyle)
                                        {
                                            int num76 = (int)Main.npc[num75].ai[0];
                                            Main.npc[num75].active = false;
                                            npc.life = 0;
                                            if (Main.netMode == 2)
                                            {
                                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num75);
                                            }
                                            num75 = num76;
                                        }
                                        if (Main.netMode == 2)
                                        {
                                            NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                                        }
                                    }
                                    playerCenterXTile = 0f;
                                    playerCenterYTile = speed;
                                }
                            }
                            bool flag6 = false;

                            if (!flag6)
                            {
                                if ((npc.velocity.X > 0f && playerCenterXTile > 0f) || (npc.velocity.X < 0f && playerCenterXTile < 0f) || (npc.velocity.Y > 0f && playerCenterYTile > 0f) || (npc.velocity.Y < 0f && playerCenterYTile < 0f))
                                {
                                    if (npc.velocity.X < playerCenterXTile)
                                    {
                                        npc.velocity.X += acceleration;
                                    }
                                    else if (npc.velocity.X > playerCenterXTile)
                                    {
                                        npc.velocity.X -= acceleration;
                                    }
                                    if (npc.velocity.Y < playerCenterYTile)
                                    {
                                        npc.velocity.Y += acceleration;
                                    }
                                    else if (npc.velocity.Y > playerCenterYTile)
                                    {
                                        npc.velocity.Y -= acceleration;
                                    }
                                    if ((double)Math.Abs(playerCenterYTile) < (double)speed * 0.2 && ((npc.velocity.X > 0f && playerCenterXTile < 0f) || (npc.velocity.X < 0f && playerCenterXTile > 0f)))
                                    {
                                        if (npc.velocity.Y > 0f)
                                        {
                                            npc.velocity.Y += acceleration * 2f;
                                        }
                                        else
                                        {
                                            npc.velocity.Y -= acceleration * 2f;
                                        }
                                    }
                                    if ((double)Math.Abs(playerCenterXTile) < (double)speed * 0.2 && ((npc.velocity.Y > 0f && playerCenterYTile < 0f) || (npc.velocity.Y < 0f && playerCenterYTile > 0f)))
                                    {
                                        if (npc.velocity.X > 0f)
                                        {
                                            npc.velocity.X += acceleration * 2f;
                                        }
                                        else
                                        {
                                            npc.velocity.X -= acceleration * 2f;
                                        }
                                    }
                                }
                                else if (num71 > num72)
                                {
                                    if (npc.velocity.X < playerCenterXTile)
                                    {
                                        npc.velocity.X += acceleration * 1.1f;
                                    }
                                    else if (npc.velocity.X > playerCenterXTile)
                                    {
                                        npc.velocity.X -= acceleration * 1.1f;
                                    }
                                    if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)speed * 0.5)
                                    {
                                        if (npc.velocity.Y > 0f)
                                        {
                                            npc.velocity.Y += acceleration;
                                        }
                                        else
                                        {
                                            npc.velocity.Y -= acceleration;
                                        }
                                    }
                                }
                                else
                                {
                                    if (npc.velocity.Y < playerCenterYTile)
                                    {
                                        npc.velocity.Y += acceleration * 1.1f;
                                    }
                                    else if (npc.velocity.Y > playerCenterYTile)
                                    {
                                        npc.velocity.Y -= acceleration * 1.1f;
                                    }
                                    if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)speed * 0.5)
                                    {
                                        if (npc.velocity.X > 0f)
                                        {
                                            npc.velocity.X += acceleration;
                                        }
                                        else
                                        {
                                            npc.velocity.X -= acceleration;
                                        }
                                    }
                                }
                            }
                        }
                        if (npc.type == NPCID.EaterofWorldsHead)
                        {
                            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

                            if (flag2)
                            {
                                if (npc.localAI[0] != 1f)
                                {
                                    npc.netUpdate = true;
                                }
                                npc.localAI[0] = 1f;
                            }
                            else
                            {
                                if (npc.localAI[0] != 0f)
                                {
                                    npc.netUpdate = true;
                                }
                                npc.localAI[0] = 0f;
                            }
                            if (((npc.velocity.X > 0f && npc.oldVelocity.X < 0f) || (npc.velocity.X < 0f && npc.oldVelocity.X > 0f) || (npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f) || (npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f)) && !npc.justHit)
                            {
                                npc.netUpdate = true;
                            }
                        }
                        // this section makes 0 sense to me but i'll keep it just in case
                        if (npc.type < NPCID.EaterofWorldsHead || npc.type > NPCID.EaterofWorldsTail || (npc.type != NPCID.EaterofWorldsHead && (npc.type == NPCID.EaterofWorldsHead || Main.npc[(int)npc.ai[1]].alpha >= 85)))
                        {
                            return false;
                        }

                        if (npc.alpha > 0 && npc.life > 0)
                        {
                            for (int num80 = 0; num80 < 2; num80++)
                            {
                                int num81 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Demonite, 0f, 0f, 100, default(Color), 2f);
                                Main.dust[num81].noGravity = true;
                                Main.dust[num81].noLight = true;
                            }
                        }
                        if ((npc.position - npc.oldPosition).Length() > 2f)
                        {
                            npc.alpha -= 42;
                            if (npc.alpha < 0)
                            {
                                npc.alpha = 0;
                            }
                        }
                    }
                    return false;
                }
                if (npc.type == NPCID.VileSpitEaterOfWorlds)
                {
                    Rectangle rectangle = npc.Hitbox;
                    for (int index1 = 0; index1 < 255; index1++)
                    {
                        Player player = Main.player[index1];
                        if (index1 >= 0 && player.active && !player.dead)
                        {


                            if (rectangle.Intersects(player.Hitbox))
                            {
                                player.AddBuff(BuffID.Weak, Main.rand.Next(180, 240));
                                player.AddBuff(BuffID.OgreSpit, 150);

                                SoundEngine.PlaySound(SoundID.NPCDeath1 with { MaxInstances = 0 });
                                for (int i = 0; i < 25; ++i)
                                {
                                    Vector2 position10 = new Vector2(player.position.X, player.position.Y);
                                    Dust dust = Dust.NewDustDirect(position10, player.width, player.height, DustID.CorruptGibs, 0f, 0f, 100, default, 1.5f);
                                    dust.velocity *= 3f;
                                }
                                npc.life = 0;

                            }
                        }
                    }
                    int num81 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.WhiteTorch, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num81].noGravity = true;
                    return true;


                }
            }
            return true;
        }
        public override bool PreKill(NPC npc)
        {
            if ((npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail) && Main.expertMode && GetInstance<BossConfig>().EoWChanges)
            {
                NPCLoader.blockLoot.Add(ItemID.Heart);
            }
            return true;
        }
    }
 
}