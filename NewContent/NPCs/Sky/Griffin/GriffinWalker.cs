using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Sky.Griffin
{
    internal class GriffinWalker : GriffinFlier
    {
        static float MaxWalkSpeed => 20;
        static float JumpSpeed => 6;
        static float WalkAcceleration => 0.2f;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
        }
        //ai3 is timer for how long touching walls/for turnaround
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.noGravity = false;
            DrawOffsetY = -4;

            //159 vampire, 158 vampire bat id

            if (player.position.Y + player.height == NPC.position.Y + NPC.height)
            {
                NPC.directionY = -1;
            }
            bool flag = false;
            bool flag5 = false;
            bool flag6 = false;
            if (NPC.velocity.X == 0f)
            {
                flag6 = true;
            }
            if (NPC.justHit)
            {
                flag6 = false;
            }
            int num56 = 60;
            bool flag7 = false;

            if (NPC.velocity.Y == 0f && (NPC.velocity.X > 0f && NPC.direction < 0 || NPC.velocity.X < 0f && NPC.direction > 0))
            {
                flag7 = true;
            }
            if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= num56 || flag7)
            {
                NPC.ai[3] += 1f;
            }
            else if ((double)Math.Abs(NPC.velocity.X) > 0.9 && NPC.ai[3] > 0f)
            {
                NPC.ai[3] -= 1f;
            }
            if (NPC.ai[3] > num56 * 10)
            {
                NPC.ai[3] = 0f;
            }
            if (NPC.justHit)
            {
                NPC.ai[3] = 0f;
            }
            if (NPC.ai[3] == num56)
            {
                NPC.netUpdate = true;
            }
            if (player.Hitbox.Intersects(NPC.Hitbox))
            {
                NPC.ai[3] = 0f;
            }

            if (NPC.ai[3] < num56)
            {
                NPC.TargetClosest();
                if (NPC.directionY > 0 && player.Center.Y <= NPC.Bottom.Y)
                {
                    NPC.directionY = -1;
                }
            }
            else if (!(NPC.ai[2] > 0f))
            {
                if (NPC.velocity.X == 0f)
                {
                    if (NPC.velocity.Y == 0f)
                    {
                        NPC.ai[0] += 1f;
                        if (NPC.ai[0] >= 2f)
                        {
                            NPC.direction *= -1;
                            NPC.spriteDirection = NPC.direction;
                            NPC.ai[0] = 0f;
                        }
                    }
                }
                else
                {
                    NPC.ai[0] = 0f;
                }
                if (NPC.direction == 0)
                {
                    NPC.direction = 1;
                }
            }

            Movement();

            if (NPC.velocity.Y == 0f || flag)
            {
                int num181 = (int)(NPC.position.Y + NPC.height + 7f) / 16;
                int num182 = (int)(NPC.position.Y - 9f) / 16;
                int num183 = (int)NPC.position.X / 16;
                int num184 = (int)(NPC.position.X + NPC.width) / 16;
                int num185 = (int)(NPC.position.X + 8f) / 16;
                int num186 = (int)(NPC.position.X + NPC.width - 8f) / 16;
                bool flag22 = false;
                for (int i = num185; i <= num186; i++)
                {
                    if (i >= num183 && i <= num184 && Main.tile[i, num181] == null)
                    {
                        flag22 = true;
                        continue;
                    }
                    if (Main.tile[i, num182] != null && Main.tile[i, num182].HasUnactuatedTile && Main.tileSolid[Main.tile[i, num182].TileType])
                    {
                        flag5 = false;
                        break;
                    }
                    if (!flag22 && i >= num183 && i <= num184 && Main.tile[i, num181].HasUnactuatedTile && Main.tileSolid[Main.tile[i, num181].TileType])
                    {
                        flag5 = true;
                    }
                }
                if (!flag5 && NPC.velocity.Y < 0f)
                {
                    NPC.velocity.Y = 0f;
                }
                if (flag22)
                {
                    return;
                }
            }
            if (NPC.velocity.Y >= 0f)
            {
                int num188 = 0;
                if (NPC.velocity.X < 0f)
                {
                    num188 = -1;
                }
                if (NPC.velocity.X > 0f)
                {
                    num188 = 1;
                }
                Vector2 vector39 = NPC.position;
                vector39.X += NPC.velocity.X;
                int num189 = (int)((vector39.X + NPC.width / 2 + (NPC.width / 2 + 1) * num188) / 16f);
                int num190 = (int)((vector39.Y + NPC.height - 1f) / 16f);
                if (WorldGen.InWorld(num189, num190, 4))
                {
                    //unsure of what to do with these
                    //if (Main.tile[num189, num190] == null)
                    //{
                    //    Main.tile[num189, num190] = new Tile();
                    //}
                    //if (Main.tile[num189, num190 - 1] == null)
                    //{
                    //    Main.tile[num189, num190 - 1] = new Tile();
                    //}
                    //if (Main.tile[num189, num190 - 2] == null)
                    //{
                    //    Main.tile[num189, num190 - 2] = new Tile();
                    //}
                    //if (Main.tile[num189, num190 - 3] == null)
                    //{
                    //    Main.tile[num189, num190 - 3] = new Tile();
                    //}
                    //if (Main.tile[num189, num190 + 1] == null)
                    //{
                    //    Main.tile[num189, num190 + 1] = new Tile();
                    //}
                    //if (Main.tile[num189 - num188, num190 - 3] == null)
                    //{
                    //    Main.tile[num189 - num188, num190 - 3] = new Tile();
                    //}
                    if (num189 * 16 < vector39.X + NPC.width && num189 * 16 + 16 > vector39.X && (Main.tile[num189, num190].HasUnactuatedTile && !Main.tile[num189, num190].TopSlope && !Main.tile[num189, num190 - 1].TopSlope && Main.tileSolid[Main.tile[num189, num190].TileType] && !Main.tileSolidTop[Main.tile[num189, num190].TileType] || Main.tile[num189, num190 - 1].IsHalfBlock && Main.tile[num189, num190 - 1].HasUnactuatedTile) && (!Main.tile[num189, num190 - 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num189, num190 - 1].TileType] || Main.tileSolidTop[Main.tile[num189, num190 - 1].TileType] || Main.tile[num189, num190 - 1].IsHalfBlock && (!Main.tile[num189, num190 - 4].HasUnactuatedTile || !Main.tileSolid[Main.tile[num189, num190 - 4].TileType] || Main.tileSolidTop[Main.tile[num189, num190 - 4].TileType])) && (!Main.tile[num189, num190 - 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num189, num190 - 2].TileType] || Main.tileSolidTop[Main.tile[num189, num190 - 2].TileType]) && (!Main.tile[num189, num190 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num189, num190 - 3].TileType] || Main.tileSolidTop[Main.tile[num189, num190 - 3].TileType]) && (!Main.tile[num189 - num188, num190 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num189 - num188, num190 - 3].TileType]))
                    {
                        float num191 = num190 * 16;
                        if (Main.tile[num189, num190].IsHalfBlock)
                        {
                            num191 += 8f;
                        }
                        if (Main.tile[num189, num190 - 1].IsHalfBlock)
                        {
                            num191 -= 8f;
                        }
                        if (num191 < vector39.Y + NPC.height)
                        {
                            float num192 = vector39.Y + NPC.height - num191;
                            float num193 = 16.1f;
                            if (num192 <= num193)
                            {
                                NPC.gfxOffY += NPC.position.Y + NPC.height - num191;
                                NPC.position.Y = num191 - NPC.height;
                                NPC.stepSpeed = num192 < 9f ? 1f : 2f;
                            }
                        }
                    }
                }
            }
            if (flag5)
            {
                int num194 = (int)((NPC.position.X + NPC.width / 2 + 15 * NPC.direction) / 16f);
                int num195 = (int)((NPC.position.Y + NPC.height - 15f) / 16f);
                //if (Main.tile[num194, num195] == null)
                //{
                //    Main.tile[num194, num195] = new Tile();
                //}
                //if (Main.tile[num194, num195 - 1] == null)
                //{
                //    Main.tile[num194, num195 - 1] = new Tile();
                //}
                //if (Main.tile[num194, num195 - 2] == null)
                //{
                //    Main.tile[num194, num195 - 2] = new Tile();
                //}
                //if (Main.tile[num194, num195 - 3] == null)
                //{
                //    Main.tile[num194, num195 - 3] = new Tile();
                //}
                //if (Main.tile[num194, num195 + 1] == null)
                //{
                //    Main.tile[num194, num195 + 1] = new Tile();
                //}
                //if (Main.tile[num194 + NPC.direction, num195 - 1] == null)
                //{
                //    Main.tile[num194 + NPC.direction, num195 - 1] = new Tile();
                //}
                //if (Main.tile[num194 + NPC.direction, num195 + 1] == null)
                //{
                //    Main.tile[num194 + NPC.direction, num195 + 1] = new Tile();
                //}
                //if (Main.tile[num194 - NPC.direction, num195 + 1] == null)
                //{
                //    Main.tile[num194 - NPC.direction, num195 + 1] = new Tile();
                //}
                //Main.tile[num194, num195 + 1].IsHalfBlock;
                OpenDoorsAndFallThroughPlatforms(player, flag6, num56, num194, num195);
            }
            else
            {
                NPC.ai[1] = 0f;
                NPC.ai[2] = 0f;
            }
            NPC.spriteDirection = NPC.direction;
            CheckForTransformation();
        }

        private void OpenDoorsAndFallThroughPlatforms(Player player, bool flag6, int num56, int num194, int num195)
        {
            if (Main.tile[num194, num195 - 1].HasUnactuatedTile && (Main.tile[num194, num195 - 1].TileType == 10 || Main.tile[num194, num195 - 1].TileType == 388))
            {
                NPC.ai[2] += 1f;
                NPC.ai[3] = 0f;
                if (NPC.ai[2] >= 60f)
                {
                    NPC.velocity.X = 0.5f * -NPC.direction;
                    int doorOpenTimerSpeed = 5;
                    NPC.ai[1] += doorOpenTimerSpeed;
                    NPC.ai[2] = 0f;
                    bool flag25 = false;
                    if (NPC.ai[1] >= 10f)
                    {
                        flag25 = true;
                        NPC.ai[1] = 10f;
                    }
                    WorldGen.KillTile(num194, num195 - 1, fail: true);
                    if ((Main.netMode != NetmodeID.MultiplayerClient || !flag25) && flag25 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (Main.tile[num194, num195 - 1].TileType == 10)
                        {
                            bool flag26 = WorldGen.OpenDoor(num194, num195 - 1, NPC.direction);
                            if (!flag26)
                            {
                                NPC.ai[3] = num56;
                                NPC.netUpdate = true;
                            }
                            if (Main.netMode == NetmodeID.Server && flag26)
                            {
                                NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, num194, num195 - 1, NPC.direction);
                            }
                        }
                        if (Main.tile[num194, num195 - 1].TileType == 388)
                        {
                            bool flag27 = WorldGen.ShiftTallGate(num194, num195 - 1, closing: false);
                            if (!flag27)
                            {
                                NPC.ai[3] = num56;
                                NPC.netUpdate = true;
                            }
                            if (Main.netMode == NetmodeID.Server && flag27)
                            {
                                NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, num194, num195 - 1);
                            }
                        }

                    }
                }
            }
            else
            {
                int num197 = NPC.spriteDirection;
                if (NPC.velocity.X < 0f && num197 == -1 || NPC.velocity.X > 0f && num197 == 1)
                {
                    if (NPC.height >= 32 && Main.tile[num194, num195 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num194, num195 - 2].TileType])
                    {
                        if (Main.tile[num194, num195 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num194, num195 - 3].TileType])
                        {
                            NPC.velocity.Y = -8f;
                            NPC.netUpdate = true;
                        }
                        else
                        {
                            NPC.velocity.Y = -7f;
                            NPC.netUpdate = true;
                        }
                    }
                    else if (Main.tile[num194, num195 - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[num194, num195 - 1].TileType])
                    {

                        NPC.velocity.Y = -JumpSpeed;

                        NPC.netUpdate = true;
                    }
                    else if (NPC.position.Y + NPC.height - num195 * 16 > 20f && Main.tile[num194, num195].HasUnactuatedTile && !Main.tile[num194, num195].TopSlope && Main.tileSolid[Main.tile[num194, num195].TileType])
                    {
                        NPC.velocity.Y = -JumpSpeed * (5f / 6f);
                        NPC.netUpdate = true;
                    }
                    else if (NPC.directionY < 0 && (!Main.tile[num194, num195 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num194, num195 + 1].TileType]) && (!Main.tile[num194 + NPC.direction, num195 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num194 + NPC.direction, num195 + 1].TileType]))
                    {
                        NPC.velocity.Y = -JumpSpeed * (8f / 6f);
                        NPC.velocity.X *= 1.5f;
                        NPC.netUpdate = true;
                    }
                    else
                    {
                        NPC.ai[1] = 0f;
                        NPC.ai[2] = 0f;
                    }
                    if (NPC.velocity.Y == 0f && flag6 && NPC.ai[3] == 1f)
                    {
                        NPC.velocity.Y = -JumpSpeed * (5f / 6f);
                    }
                    if (NPC.velocity.Y == 0f && Main.expertMode && player.Bottom.Y < NPC.Top.Y && Math.Abs(NPC.Center.X - player.Center.X) < player.width * 3 && Collision.CanHit(NPC, player))
                    {
                        if (NPC.velocity.Y == 0f)
                        {
                            int num200 = 6;
                            if (player.Bottom.Y > NPC.Top.Y - num200 * 16)
                            {
                                NPC.velocity.Y = -JumpSpeed * (7.9f / 6f);
                            }
                            else
                            {
                                int num201 = (int)(NPC.Center.X / 16f);
                                int num202 = (int)(NPC.Bottom.Y / 16f) - 1;
                                for (int i = num202; i > num202 - num200; i--)
                                {
                                    if (Main.tile[num201, i].HasUnactuatedTile && TileID.Sets.Platforms[Main.tile[num201, i].TileType])
                                    {
                                        NPC.velocity.Y = -JumpSpeed * (7.9f / 6f);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Movement()
        {
            if (NPC.velocity.X > 0f && NPC.direction < 0 || NPC.velocity.X < 0f && NPC.direction > 0)
            {
                NPC.velocity.X *= 0.95f;
            }
            if (NPC.velocity.X < -MaxWalkSpeed || NPC.velocity.X > MaxWalkSpeed)
            {
                if (NPC.velocity.Y == 0f)
                {
                    NPC.velocity *= 0.8f;
                }
            }
            else if (NPC.velocity.X < MaxWalkSpeed && NPC.direction == 1)
            {
                if (NPC.velocity.Y == 0f && NPC.velocity.X < 0f)
                {
                    NPC.velocity.X *= 0.99f;
                }
                NPC.velocity.X += WalkAcceleration;
                if (NPC.velocity.X > MaxWalkSpeed)
                {
                    NPC.velocity.X = MaxWalkSpeed;
                }
            }
            else if (NPC.velocity.X > -MaxWalkSpeed && NPC.direction == -1)
            {
                if (NPC.velocity.Y == 0f && NPC.velocity.X > 0f)
                {
                    NPC.velocity.X *= 0.99f;
                }
                NPC.velocity.X -= WalkAcceleration;
                if (NPC.velocity.X < -MaxWalkSpeed)
                {
                    NPC.velocity.X = -MaxWalkSpeed;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            int frameSpeed = 4;
            NPC.frame.Y = (int)NPC.frameCounter / frameSpeed % Main.npcFrameCount[Type] * frameHeight;
        }
        private void CheckForTransformation()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player player = Main.player[NPC.target];
                Vector2 vector31 = NPC.Center;
                float num129 = player.Center.X - vector31.X;
                float num130 = player.Center.Y - vector31.Y;
                if (MathF.Sqrt(num129 * num129 + num130 * num130) > 300f)
                {
                    NPC.Transform(ModContent.NPCType<GriffinFlier>());
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            screenPos -= new Vector2(0, DrawOffsetY + NPC.gfxOffY);
            Rectangle frame = NPC.frame;
            if (NPC.velocity.Y != 0)
            {
                texture = TextureAssets.Npc[ModContent.NPCType<GriffinFlier>()].Value;
                frame = texture.Frame(1, Main.npcFrameCount[ModContent.NPCType<GriffinFlier>()]);
            }
            spriteBatch.Draw(texture, NPC.Center - screenPos, frame, drawColor, NPC.rotation, frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
    }
}
