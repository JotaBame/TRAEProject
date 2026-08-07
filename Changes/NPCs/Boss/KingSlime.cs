using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.NPCs.Miniboss.Santa;
using TRAEProject.NewContent.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss
{
    public class KingSlime : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.KingSlime)
                npc.damage = 35; // down from 40
                if (npc.type == NPCID.SlimeSpiked)
                    npc.lifeMax = 30; // down from 50

        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.KingSlime && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Katana, 5));
            }    
        }

 
        public override bool PreAI(NPC npc)
        {
            if (GetInstance<BossConfig>().KingSlimeChanges)
            {
                if (npc.type == NPCID.KingSlime)
                {
                    float num = 1f;
                    float num2 = 1f;
                    bool flag = false;
                    bool flag2 = false;
                    bool flag3 = false;
                    float num3 = 2f;
                    if (Main.getGoodWorld)
                    {
                        num3 -= 1f - (float)npc.life / (float)npc.lifeMax;
                        num2 *= num3;
                    }
                    npc.aiAction = 0;
                    if (npc.ai[3] == 0f && npc.life > 0)
                    {
                        npc.ai[3] = npc.lifeMax;
                    }
                    if (npc.localAI[3] == 0f)
                    {
                        npc.localAI[3] = 1f;
                        flag = true;
                        if (Main.netMode != 1)
                        {
                            npc.ai[0] = -100f;
                            npc.TargetClosest();
                            npc.netUpdate = true;
                        }
                    }
                    if (npc.ai[1] == 4f && npc.velocity.Y == 0)
                    {
                        npc.ai[1] = 0f;
                        SoundEngine.PlaySound(SoundID.Item14, npc.position);
                        for (int l = (int)npc.position.X - 100; l < (int)npc.Center.X + 100; l += 20)
                        {
                            for (int m = 0; m < 4; m++)
                            {
                                int num5 = Dust.NewDust(new Vector2(npc.position.X - 20f, npc.position.Y + (float)npc.height), npc.width + 20, 4, 31, 0f, 0f, 100, default(Color), 1.5f);
                                Main.dust[num5].velocity *= 0.2f;
                            }
                            int num6 = Gore.NewGore(npc.GetSource_FromAI(), new Vector2(l - 20, npc.position.Y + (float)npc.height - 8f), default(Vector2), Main.rand.Next(61, 64));
                            Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(l - 20, npc.position.Y + (float)npc.height - 8f), Vector2.Zero, ProjectileType<KinglySmash>(), 10, 0f, Main.myPlayer);

                            Main.gore[num6].velocity *= 0.4f;
                        }
                        if (Main.netMode != 1)
                        {
                        }

                    }
                    int num4 = 3000;
                    if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > (float)num4)
                    {
                        npc.TargetClosest();
                        if (Main.player[npc.target].dead || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > (float)num4)
                        {
                            npc.EncourageDespawn(10);
                            if (Main.player[npc.target].Center.X < npc.Center.X)
                            {
                                npc.direction = 1;
                            }
                            else
                            {
                                npc.direction = -1;
                            }
                            if (Main.netMode != 1 && npc.ai[1] != 5f)
                            {
                                npc.netUpdate = true;
                                npc.ai[2] = 0f;
                                npc.ai[0] = 0f;
                                npc.ai[1] = 5f;
                                npc.localAI[1] = Main.maxTilesX * 16;
                                npc.localAI[2] = Main.maxTilesY * 16;
                            }
                        }
                    }
                    if (!Main.player[npc.target].dead && npc.timeLeft > 10 && npc.ai[2] >= 300f && npc.ai[1] < 5f && npc.velocity.Y == 0f)
                    {

                        npc.ai[2] = 0f;
                        npc.ai[0] = 0f;
                        npc.ai[1] = 5f;
                        if (Main.netMode != 1)
                        {
                            npc.TargetClosest(faceTarget: false);
                            Point point = npc.Center.ToTileCoordinates();
                            Point point2 = Main.player[npc.target].Center.ToTileCoordinates();
                            Vector2 vector = Main.player[npc.target].Center - npc.Center;
                            bool antiCheese = false;
                            if (npc.localAI[0] >= 360f || vector.Length() > 2000f)
                            {
                                if (npc.localAI[0] >= 360f)
                                {
                                    npc.localAI[0] = 360f;
                                }
                                antiCheese = true;
                            }
                            AI_015_KingSlime_FindTeleportSpot(npc, antiCheese, ref npc.localAI[1], ref npc.localAI[2]);
                        }
                    }
                    if (!Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0) || Math.Abs(npc.Top.Y - Main.player[npc.target].Bottom.Y) > 160f)
                    {
                        npc.ai[2] += 1f;
                        if (Main.netMode != 1)
                        {
                            npc.localAI[0] += 1f;
                        }
                    }
                    else if (Main.netMode != 1)
                    {
                        npc.localAI[0] -= 1f;
                        if (npc.localAI[0] < 0f)
                        {
                            npc.localAI[0] = 0f;
                        }
                    }
                    if (npc.timeLeft < 10 && (npc.ai[0] != 0f || npc.ai[1] != 0f))
                    {
                        npc.ai[0] = 0f;
                        npc.ai[1] = 0f;
                        npc.netUpdate = true;
                        flag2 = false;
                    }
                    if (npc.ai[1] == 5f)
                    {
                        flag2 = true;
                        npc.aiAction = 1;
                        npc.ai[0] += 1f;
                        num = MathHelper.Clamp((60f - npc.ai[0]) / 60f, 0f, 1f);
                        num = 0.5f + num * 0.5f;
                        if (npc.ai[0] >= 60f)
                        {
                            flag3 = true;
                        }
                        if (npc.ai[0] == 60f)
                        {
                            Gore.NewGore(npc.GetSource_FromAI(), npc.Center + new Vector2(-40f, -npc.height / 2), npc.velocity, 734);
                        }
                        if (npc.ai[0] >= 60f && Main.netMode != 1)
                        {
                            npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]);
                            npc.ai[1] = 6f;
                            npc.ai[0] = 0f;
                            npc.netUpdate = true;
                        }
                        if (Main.netMode == 1 && npc.ai[0] >= 120f)
                        {
                            npc.ai[1] = 6f;
                            npc.ai[0] = 0f;
                        }
                        if (!flag3)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                int num5 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                                Main.dust[num5].noGravity = true;
                                Main.dust[num5].velocity *= 0.5f;
                            }
                        }
                    }


                    else if (npc.ai[1] == 6f)
                    {
                        flag2 = true;
                        npc.aiAction = 0;
                        npc.ai[0] += 1f;
                        num = MathHelper.Clamp(npc.ai[0] / 30f, 0f, 1f);
                        num = 0.5f + num * 0.5f;
                        if (npc.ai[0] >= 30f && Main.netMode != 1)
                        {
                            npc.ai[1] = 0f;
                            npc.ai[0] = 0f;
                            npc.netUpdate = true;
                            npc.TargetClosest();
                        }
                        if (Main.netMode == 1 && npc.ai[0] >= 60f)
                        {
                            npc.ai[1] = 0f;
                            npc.ai[0] = 0f;
                            npc.TargetClosest();
                        }
                        for (int j = 0; j < 10; j++)
                        {
                            int num6 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
                            Main.dust[num6].noGravity = true;
                            Main.dust[num6].velocity *= 2f;
                        }
                    }
                    npc.dontTakeDamage = (npc.hide = flag3);
                    if (Main.masterMode && npc.velocity.Y > 0f && npc.ai[1] == 4f)
                    {

                        npc.GravityMultiplier *= 3f;
                            npc.MaxFallSpeedMultiplier *= 1.6f;
                      
                    }
                    if (npc.velocity.Y == 0f)
                    {
              
                        npc.velocity.X *= 0.8f; // changed from 0.8f
                        if (npc.velocity.X > -0.1f && npc.velocity.X < 0.1f)
                        {
                            npc.velocity.X = 0f;
                        }
                        if (!flag2)
                        {
                            npc.ai[0] += 2f;
                            float distance = npc.Distance(Main.player[npc.target].Center);
                            if (Main.masterMode && npc.ai[1] < 3f)
                                npc.ai[0] += 5f;
                            if (Main.expertMode && distance > 900f)
                                npc.ai[0] += 5f;
 
                            if ((float)npc.life < (float)npc.lifeMax * 0.8f)
                            {
                                npc.ai[0] += 1f;
                            }
                            if ((float)npc.life < (float)npc.lifeMax * 0.6f)
                            {
                                npc.ai[0] += 1f;
                            }
                            if ((float)npc.life < (float)npc.lifeMax * 0.4f)
                            {
                                npc.ai[0] += 2f;
                            }
                            if ((float)npc.life < (float)npc.lifeMax * 0.2f)
                            {
                                npc.ai[0] += 3f;
                            }
                            if ((float)npc.life < (float)npc.lifeMax * 0.1f)
                            {
                                npc.ai[0] += 4f;
                            }
                            if (npc.ai[0] >= 0f)
                            {
                                npc.netUpdate = true;
                                npc.TargetClosest();

                                if (npc.ai[1] == 3f) // high jump
                                {
                                    npc.velocity.Y = -13f;
                                    if (Main.masterMode)
                                    { 
                                        npc.velocity.Y -= 2f;
                                        if (distance < 160f)
                                        {
                                            npc.velocity.X += 1.5f * (float)npc.direction;

                                        }
                                        else if (distance < 320f)
                                        {
                                            npc.velocity.X += 3f * (float)npc.direction;

 
                                        }
                                        else
                                            npc.velocity.X += 7f * (float)npc.direction;

                                    }
                     

                                    npc.ai[0] = -200f;
                                    npc.ai[1] += 1f;


                                }
                                else if (npc.ai[1] == 2f) // low jump
                                {
                                    npc.velocity.Y = -6f;
                                    npc.velocity.X += 5.5f * (float)npc.direction; // up from 4.5
                                    npc.ai[0] = -120f;
                                    npc.ai[1] += 1f;
                                }
                                else // mid jump
                                {
                                    npc.velocity.Y = -8f;
                                    npc.velocity.X += 4f * (float)npc.direction;
                                    npc.ai[0] = -120f;
                                    npc.ai[1] += 1f;
                                }
                                if (Main.expertMode)
                                {
                                    float distanceMult = (distance / 600f);
                                    if (distanceMult > 2.5f)
                                        distanceMult = 2.5f;
                                    npc.velocity.X += 3.5f * (distanceMult + Main.player[npc.target].velocity.X / 10) * npc.direction;
                                }
                                if (Main.masterMode)
                                {
                                    npc.velocity.X +=  Main.player[npc.target].velocity.X / 4 * npc.direction;
                                    npc.velocity.Y -= 1f;
                                }
                            }
                            else if (npc.ai[0] >= -30f)
                            {
                                npc.aiAction = 1;
                            }
                        }
                    }

                    else if (npc.target < 255)
                    {
                        float num7 = 3f;
                        if (Main.getGoodWorld)
                        {
                            num7 = 6f;
                        }
                        if ((npc.direction == 1 && npc.velocity.X < num7) || (npc.direction == -1 && npc.velocity.X > 0f - num7))
                        {
                            if ((npc.direction == -1 && npc.velocity.X < 0.1f) || (npc.direction == 1 && npc.velocity.X > -0.1f))
                            {
                                npc.velocity.X += 0.2f * (float)npc.direction;
                            }
                            else
                            {
                                npc.velocity.X *= 0.93f;
                            }
                        }
                    }
                    int num8 = Dust.NewDust(npc.position, npc.width, npc.height, 4, npc.velocity.X, npc.velocity.Y, 255, new Color(0, 80, 255, 80), npc.scale * 1.2f);
                    Main.dust[num8].noGravity = true;
                    Main.dust[num8].velocity *= 0.5f;
                    if (npc.life <= 0)
                    {
                        return false;
                    }
                    float num9 = (float)npc.life / (float)npc.lifeMax;
                    num9 = num9 * 0.5f + 0.75f;
                    num9 *= num;
                    num9 *= num2;
                    if (num9 != npc.scale || flag)
                    {
                        npc.position.X += npc.width / 2;
                        npc.position.Y += npc.height;
                        npc.scale = num9;
                        npc.width = (int)(98f * npc.scale);
                        npc.height = (int)(92f * npc.scale);
                        npc.position.X -= npc.width / 2;
                        npc.position.Y -= npc.height;
                    }
                    if (Main.netMode == 1)
                    {
                        return false;
                    }
                    int num10 = (int)((float)npc.lifeMax * 0.05f);
                    if (!((float)(npc.life + num10) < npc.ai[3]))
                    {
                        return false;
                    }
                    npc.ai[3] = npc.life;
                    int num11 = Main.rand.Next(1, 4);
                    for (int k = 0; k < num11; k++)
                    {
                        int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 32));
                        int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 32));
                        int num12 = 1;
                        if (Main.expertMode && Main.rand.Next(4) == 0)
                        {
                            num12 = 535;
                        }
                        int num13 = NPC.NewNPC(npc.GetSource_FromThis(), x, y, num12);
                        Main.npc[num13].SetDefaults(num12);
                        Main.npc[num13].velocity.X = (float)Main.rand.Next(-15, 16) * 0.1f;
                        Main.npc[num13].velocity.Y = (float)Main.rand.Next(-30, 1) * 0.1f;
                        Main.npc[num13].ai[0] = -1000 * Main.rand.Next(3);
                        Main.npc[num13].ai[1] = 0f;
                        if (Main.netMode == 2 && num13 < Main.maxNPCs)
                        {
                            NetMessage.SendData(23, -1, -1, null, num13);
                        }
                    }
                    return false;
                }
                //if (npc.type == NPCID.SlimeSpiked && npc.ai[0] >= 40f)
                //{
                //    npc.ai[0] = -40f;
                //    if (npc.velocity.Y == 0f)
                //    {
                //        npc.velocity.X *= 0.9f;
                //    }
                //    if (Main.netMode != 1)
                //    {
                //        Vector2 vector5 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                //        float num37 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector5.X;
                //        float num38 = Main.player[npc.target].position.Y - vector5.Y - (float)Main.rand.Next(0, 200);
                //        float num39 = (float)Math.Sqrt(num37 * num37 + num38 * num38);
                //        num39 = 4.5f / num39;
                //        num37 *= num39;
                //        num38 *= num39;
                //       npc.localAI[0] = 50f;
                //        Projectile.NewProjectile(npc.GetSource_FromThis(), vector5.X, vector5.Y, num37, num38, 605, 9, 0f, Main.myPlayer);
                //    }
                //    return false;
                //}
            }

          
            return true;
        }
        private static int kingSlimePointCacheSize = 0;
        private static int kingSlimePointCacheSizeMax = 50;
        private static Point[] kingSlimePointCache = new Point[kingSlimePointCacheSizeMax];
        private bool AI_015_KingSlime_FindTeleportSpot(NPC npc, bool antiCheese, ref float teleportSpotX, ref float teleportSpotY)
        {
            if (antiCheese)
            {
                int num = Player.FindClosest(npc.position, npc.width, npc.height);
                teleportSpotX = Main.player[num].Bottom.X;
                teleportSpotY = Main.player[num].Bottom.Y;
                return false;
            }
            Point point = Main.player[npc.target].Center.ToTileCoordinates();
            if (BuildKingSlimeTeleportCache(npc, point.X, point.Y, 10, 7))
            {
                int num2 = Main.rand.Next(0, kingSlimePointCacheSize);
                int x = kingSlimePointCache[num2].X;
                int y = kingSlimePointCache[num2].Y;
                teleportSpotX = x * 16 + 8;
                teleportSpotY = y * 16;
                return true;
            }
            if (BuildKingSlimeTeleportCache(npc, point.X, point.Y, 6, 2))
            {
                int num3 = Main.rand.Next(0, kingSlimePointCacheSize);
                int x2 = kingSlimePointCache[num3].X;
                int y2 = kingSlimePointCache[num3].Y;
                teleportSpotX = x2 * 16 + 8;
                teleportSpotY = y2 * 16;
                return true;
            }
            int num4 = Player.FindClosest(npc.position, npc.width, npc.height);
            teleportSpotX = Main.player[num4].Bottom.X;
            teleportSpotY = Main.player[num4].Bottom.Y;
            return false;
        }
        private bool BuildKingSlimeTeleportCache(NPC npc, int tileTargetX, int tileTargetY, int outerRange, int innerRange)
        {
            kingSlimePointCacheSize = 0;
            AddKingSlimeTeleportCacheTiles(npc, tileTargetX - outerRange, tileTargetX - innerRange, tileTargetY - outerRange, tileTargetY + outerRange);
            AddKingSlimeTeleportCacheTiles(npc, tileTargetX + innerRange, tileTargetX + outerRange, tileTargetY - outerRange, tileTargetY + outerRange);
            AddKingSlimeTeleportCacheTiles(npc, tileTargetX - innerRange, tileTargetX + innerRange, tileTargetY - outerRange, tileTargetY - innerRange);
            AddKingSlimeTeleportCacheTiles(npc, tileTargetX - innerRange, tileTargetX + innerRange, tileTargetY + innerRange, tileTargetY + outerRange);
            return kingSlimePointCacheSize > 0;
        }
        private void AddKingSlimeTeleportCacheTiles(NPC npc, int x0, int x1, int y0, int y1)
        {
            for (int i = x0; i <= x1; i++)
            {
                for (int j = y0; j <= y1; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if (!tile.HasUnactuatedTile || kingSlimePointCacheSize >= kingSlimePointCacheSizeMax || ((!Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]) && !TileID.Sets.Platforms[tile.TileType]))
                    {
                        continue;
                    }
                    Tile testTile = Main.tile[i, j - 1];
                    if (!WorldGen.SolidTile(testTile))
                    {
                        Vector2 position = new Vector2(i * 16 + 8, j * 16 - npc.height / 2);
                        if (tile.LiquidType != LiquidID.Lava && Collision.CanHitLine(position, 0, 0, Main.player[npc.target].Center, 0, 0))
                        {
                            kingSlimePointCache[kingSlimePointCacheSize].X = i;
                            kingSlimePointCache[kingSlimePointCacheSize].Y = j;
                            kingSlimePointCacheSize++;
                        }
                    }
                }
            }
        }
        //public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        //{
        //    if (GetInstance<BossConfig>().KingSlimeChanges)
        //    {
        //        if (npc.type == NPCID.KingSlime && npc.ai[1] == 4f && npc.velocity.Y > 0 && Main.masterMode)
        //        {
                
        //            Texture2D texture = TextureAssets.Npc[npc.type].Value;
        //            float num92 = 1f;
        //            int num89 = Main.npcFrameCount[npc.type];
        //            int num90 = npc.frame.Y / npc.frame.Height;
        //            Microsoft.Xna.Framework.Rectangle rectangle8 = texture.Frame(2, 16, num90 / num89, num90 % num89);
        //            rectangle8.Inflate(0, -2);
        //            Vector2 origin10 = rectangle8.Size() * new Vector2(0.5f, 1f);
        //            Color color21 = Microsoft.Xna.Framework.Color.Lerp(Microsoft.Xna.Framework.Color.White, npc.color, 0.5f);
        //            for (int num93 = 7; num93 >= 0; num93--)
        //            {
        //                float num94 = 1f - (float)num93 / 8f;
        //                Vector2 vector21 = npc.oldPos[num93] + new Vector2((float)npc.width * 0.5f, npc.height);
        //                //vector21 += npc.Center;
        //                vector21 -= screenPos;
        //                Color color22 = color21 * num94;
        //                Main.NewText("draw:" + vector21 + "Center:" + npc.Center);
        //                spriteBatch.Draw(texture, vector21, rectangle8, color22, npc.rotation, origin10, npc.scale, SpriteEffects.FlipHorizontally, 1f);
        //            }
        //        }
        //    }
        //    return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        //}
    }
}
