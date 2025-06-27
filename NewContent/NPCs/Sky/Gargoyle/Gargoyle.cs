using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Sky.Gargoyle
{
    public class Gargoyle : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            //don't make immune to confused, was really easy to add to AI
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 240;
            NPC.damage = 35;
            NPC.defense = 18;
            NPC.noGravity = true;
            NPC.width = 50;
            NPC.height = 50;
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.NPCDeath43;

            NPC.knockBackResist = 0.05f;
        }
        bool Passive => NPC.ai[0] == 0;

        static float BeforeItRisesAgain = 180f;
        static float Airtime = 1000f + BeforeItRisesAgain;

        public override void OnSpawn(IEntitySource source)
        {
            NPC.ai[0] = 1f;

            int num = 1;
            int num2 = 36;
            int j = (int)((NPC.position.X + (NPC.width / 2)) / 16f); // xSize in tiles
            int YsizeInTiles = (int)((NPC.position.Y + NPC.height) / 16f);
            for (int k = YsizeInTiles; k < YsizeInTiles + num2; k++)
            {

                Tile tile = Main.tile[j, k];
                Tile tileAbove = Main.tile[j, k - 1];
                if (tile.HasTile && !tile.IsActuated && !tile.CheckingLiquid && Main.tileSolid[tile.TileType]
                    && (!tileAbove.HasTile || !Main.tileSolid[tileAbove.TileType])
                    )
                {
                    NPC.ai[1] = 2f;
                    NPC.ai[0] = 0f;

                    NPC.position = new Vector2(j * 16f, k * 16f - NPC.height);
                    break;
                }
            }
        }
        public override void AI()
        {




            float maxVelX = 6;
            float accelX = .08f;
            float maxVelY = 3;
            float accelY = .1f;
            NPC.noGravity = true;
            NPC.GravityIgnoresLiquid = false;
            NPC.knockBackResist = 0.05f;

            if (NPC.ai[1] <= 0f)
                NPC.ai[1] = 1f;
            if (NPC.ai[1] >= 2f && NPC.ai[1] <= BeforeItRisesAgain)
                NPC.ai[1] += 1f;
            if (NPC.ai[1] >= Airtime)
            {
                NPC.ai[0] = 0f;
                NPC.ai[1] = 1f;

            }
            if (Passive)
            {
                NPC.velocity.X *= 0.99f;
                NPC.GravityIgnoresLiquid = true;
                NPC.knockBackResist = 0f;
                NPC.MaxFallSpeedMultiplier *= 1.6f;

                NPC.noGravity = false;
                NPC.TargetClosestUpgraded(false);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (NPC.velocity.Y == 0f && NPC.ai[1] == 1f)
                    {
                        NPC.velocity.X *= 0f;

                        SoundEngine.PlaySound(SoundID.Dig, NPC.position);
                        NPC.ai[1] = 2f;

                    }

                    if (NPC.velocity.Y >= NPC.maxFallSpeed)
                    {

                        NPC.ai[0] = 1f;
                        NPC.netUpdate = true;
                    }
                    else if (NPC.ai[1] >= BeforeItRisesAgain)
                    {
                        Rectangle rectangle = new Rectangle((int)Main.player[NPC.target].position.X, (int)Main.player[NPC.target].position.Y, Main.player[NPC.target].width, Main.player[NPC.target].height);
                        if (NPC.velocity.Y == 0f &&
                           (new Rectangle((int)NPC.position.X - 200, (int)NPC.position.Y - 200, NPC.width + 400, NPC.height + 400).Intersects(rectangle) || NPC.justHit))
                        {
                            SoundEngine.PlaySound(SoundID.Item109);

                            NPC.ai[0] = 1f;
                            NPC.velocity.Y -= 6f;
                            NPC.netUpdate = true;
                        }
                    }
                }
            }
            else if (!Main.player[NPC.target].dead)
            {
                NPC.ai[1] += 1f;

                if (NPC.collideX)
                {
                    NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
                    if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
                    {
                        NPC.velocity.X = 2f;
                    }
                    if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
                    {
                        NPC.velocity.X = -2f;
                    }
                }
                if (NPC.collideY)
                {
                    NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
                    if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
                    {
                        NPC.velocity.Y = 1f;
                    }
                    if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
                    {
                        NPC.velocity.Y = -1f;
                    }
                }
                NPC.TargetClosest();
                if (NPC.confused)
                {
                    NPC.direction *= -1;
                }
                if (NPC.direction == -1 && NPC.velocity.X > -maxVelX)
                {
                    NPC.velocity.X -= accelX;
                    if (NPC.velocity.X > maxVelX)
                    {
                        NPC.velocity.X -= accelX;
                    }
                    else if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X -= accelX * .5f;
                    }
                    if (NPC.velocity.X < -maxVelX)
                    {
                        NPC.velocity.X = -maxVelX;
                    }
                }
                else if (NPC.direction == 1 && NPC.velocity.X < maxVelX)
                {
                    NPC.velocity.X += 0.1f;
                    if (NPC.velocity.X < -maxVelX)
                    {
                        NPC.velocity.X += accelX;
                    }
                    else if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X += accelX * .5f;
                    }
                    if (NPC.velocity.X > maxVelX)
                    {
                        NPC.velocity.X = maxVelX;
                    }
                }
                float xDist = Math.Abs(NPC.position.X + NPC.width / 2 - (Main.player[NPC.target].position.X + Main.player[NPC.target].width / 2));

                float yDist = Main.player[NPC.target].position.Y - NPC.height / 2;
                if (NPC.confused)
                {
                    //xDist is inside abs function, so don't invert it
                    //yDist is not inside abs function, so invert for proper confusion
                    yDist *= -1;
                }
                if (xDist > 50f)
                {
                    yDist -= 100f;
                }
                if (NPC.position.Y < yDist)
                {
                    NPC.velocity.Y += accelY;
                    if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y += accelY * 0.2f;
                    }
                }
                else
                {
                    NPC.velocity.Y -= accelY;
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y -= accelY * 0.2f;
                    }
                }
                if (NPC.velocity.Y < -maxVelY)
                {
                    NPC.velocity.Y = -maxVelY;
                }
                if (NPC.velocity.Y > maxVelY)
                {
                    NPC.velocity.Y = maxVelY;
                }
            }
            if (NPC.wet)
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y *= 0.95f;
                }
                NPC.velocity.Y -= 0.5f;
                if (NPC.velocity.Y < -4f)
                {
                    NPC.velocity.Y = -4f;
                }
                NPC.TargetClosest();
            }
            NPC.rotation = NPC.velocity.X * .1f;
            NPC.spriteDirection = -NPC.direction;
        }
        public override void FindFrame(int frameHeight)
        {
            if (Passive && !NPC.IsABestiaryIconDummy)
            {
                NPC.frame.Y = 0;
            }
            else
            {
                NPC.frameCounter++;
                int frameSpeed = 6;
                int frameY = (int)(NPC.frameCounter / frameSpeed % (Main.npcFrameCount[Type] - 1) + 1) * frameHeight;
                NPC.frame.Y = frameY;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            float offSetX = 0f;
            float offSetY = 0f;
            if (Passive)
                offSetY = 16f;
            else offSetX = 16f * NPC.direction;

            Main.EntitySpriteDraw(texture, new(NPC.Center.X - screenPos.X - offSetX, NPC.Center.Y - screenPos.Y - offSetY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

            if (NPC.ai[0] != 0)
            {
                DrawEyeSparkles(screenPos, new(offSetX, offSetY));
            }
            return false;
        }

        private void DrawEyeSparkles(Vector2 screenPos, Vector2 off)
        {
            int frameSpeed = 6;
            int index = (int)(NPC.frameCounter / frameSpeed % (Main.npcFrameCount[Type] - 1));//-1 to compensate for idle frame being at the top of the sheet
            Vector2[] leftEyeOffsets = new Vector2[]
            {
                new(-33, -11),
                new(-33, -11),
                new(-33, -9),
                new(-33, -9),
                new(-33, -11)
            };
            Vector2[] rightEyeOffsets = new Vector2[]
            {
                new(-25, -11),
                new(-25, -11),
                new(-25, -9),
                new(-25, -9),
                new(-25, -11)
            };
            Vector2 offset = leftEyeOffsets[index];
            offset.X *= NPC.spriteDirection;
            offset = offset.RotatedBy(NPC.rotation);
            Vector2 fatness = new Vector2(0.8f) * (1 - NPC.ai[1] / Airtime);
            Vector2 scale = new Vector2(.75f, .4f);
            Vector2 drawpos = NPC.Center - screenPos - off;
            Color red = new Color(255, 20, 20, 0) * .3f;
            DrawSparkle(drawpos + offset, 0, red, red, scale, fatness);
            offset = rightEyeOffsets[index];
            offset.X *= NPC.spriteDirection;
            offset = offset.RotatedBy(NPC.rotation);
            DrawSparkle(drawpos + offset, 0, red, red, scale, fatness);
        }

        static void DrawSparkle(Vector2 drawpos, float rotation, Color outerColor, Color innerColor, Vector2 scale, Vector2 fatness)
        {
            SpriteEffects dir = default;
            Texture2D texture = TextureAssets.Extra[98].Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 yScale = new Vector2(fatness.X * 0.5f, scale.X);
            Vector2 xScale = new Vector2(fatness.Y * 0.5f, scale.Y);
            Main.EntitySpriteDraw(texture, drawpos, null, outerColor, MathF.PI / 2f + rotation, origin, yScale, dir);
            Main.EntitySpriteDraw(texture, drawpos, null, outerColor, 0f + rotation, origin, xScale, dir);
            Main.EntitySpriteDraw(texture, drawpos, null, innerColor, MathF.PI / 2f + rotation, origin, yScale * 0.6f, dir);
            Main.EntitySpriteDraw(texture, drawpos, null, innerColor, 0f + rotation, origin, xScale * 0.6f, dir);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.PotatoChips, 20));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("The magic that animates these statues can only remain active for a limited time. They perch in the islands in the sky, awaiting any intruders that come near.")
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Sky)
            {

                return 0.25f;

            }
            return 0f;
        }
    }
}
