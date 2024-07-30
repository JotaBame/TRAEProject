using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Projectiles.EchoSpriteProj;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoSprite
{
    /// <summary>
    /// UNTESTED
    /// </summary>
    public class EchoSprite : ModNPC
    {
        const float Phi = 1.61803398875f;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[Type] = 10;
            NPCID.Sets.TrailingMode[Type] = 1;
            Main.npcFrameCount[Type] = 3;
        }
        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 32;
            NPC.defense = 33;
            NPC.lifeMax = 400;
            NPC.noGravity = true;
        }
        ref float TurnaroundTimer => ref NPC.ai[1];
        ref float IdleMovementTimer => ref NPC.localAI[0];
        static bool SolidTile(Vector2 worldPos)
        {
            Tile tile = Main.tile[(int)(worldPos.X / 16), (int)(worldPos.Y / 16)];
            return tile.HasTile && tile.HasUnactuatedTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];
        }
        public override void AI()
        {
            EchosphereHelper.SearchForAirbornePlayers(NPC);
            if (NPC.target < 0 || NPC.target >= Main.maxPlayers)
            {
                NPC.ai[0] = 0;
                NPC.dontTakeDamage = true;
                NPC.Opacity = .5f;
                IdleMovementTimer++;
                NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(MathF.Sin(IdleMovementTimer * 0.2f) * .02f, MathF.Cos(IdleMovementTimer * Phi * 0.2f)), .1f);
                if (Main.rand.NextFloat() < 0.8f)//8% chance of dust
                {
                    Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.PinkTorch, NPC.velocity.X, NPC.velocity.Y, 0, default, 1.5f);
                    d.noGravity = true;
                }
                NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
                if ((int)IdleMovementTimer % 30 == 0)
                {
                    for (float i = 0; i < 0.999f; i += 1f / 12)
                    {
                        Vector2 rotation = (i * MathF.Tau).ToRotationVector2();
                        rotation.Y *= .5f;
                        Dust d = Dust.NewDustPerfect(NPC.Center + new Vector2(0, 12) + rotation, DustID.PinkTorch, rotation);
                        d.scale += .75f;
                        d.noGravity = true;
                    }
                }
                return;
            }
            NPC.dontTakeDamage = false;
            NPC.Opacity = 1;
            Player player = Main.player[NPC.target];
            int firerate = 120;
            if (NPC.confused)
            {
                //I AM PANICKING AAA WHERE DO I SHOOT
                firerate /= 7;
            }
            float distToTargetPosRequired = 600;//so it doesn't shoot from offscreen
            Movement(out float distToTargetPos);
            TurnaroundTimer++;
            TurnaroundTimer %= 600;
            NPC.ai[0]++;
            NPC.spriteDirection = MathF.Sign(NPC.Center.X - player.Center.X);
            if (Main.expertMode && NPC.life < NPC.lifeMax * 0.8f)//from spaz code
            {
                NPC.ai[0] += 0.6f;
            }
            if (Main.getGoodWorld)//from spaz code
            {
                NPC.ai[0] += 0.4f;
            }
            if (distToTargetPos < distToTargetPosRequired)
            {
                if (NPC.ai[0] >= firerate)
                {
                    NPC.ai[0] %= firerate;
                    int projID = ModContent.ProjectileType<EchoSpriteProj>();
                    float shootSpeed = 12f;
                    if (Main.expertMode)//from spaz code
                    {
                        shootSpeed = 14;
                    }
                    shootSpeed /= ContentSamples.ProjectilesByType[projID].MaxUpdates;
                    Vector2 projVel = NPC.DirectionTo(player.Center) * shootSpeed;
                    if (NPC.confused)
                    {
                        //AAAA HELP I AM AN IDIOT I CANT AIM
                        projVel = -projVel;
                        projVel = projVel.RotatedByRandom(2);
                        projVel *= Main.rand.NextFloat(0.5f, 2f);
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int damage = 100;
                        if (NPC.confused)
                        {
                            damage = 40;
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, projVel, projID, damage / 2, 0, Main.myPlayer);
                    }
                    //pew pew (phantasmal bolt when shot from true eoc)
                    SoundEngine.PlaySound(SoundID.Item125 with { PitchVariance = 0.3f, MaxInstances = 8 }, NPC.Center);
                    for (float i = 0; i < 0.999f; i += 1f / 32)
                    {
                        Vector2 rotation = (i * MathF.Tau).ToRotationVector2();
                        rotation.X *= .5f;
                        rotation = rotation.RotatedBy(projVel.ToRotation());

                        Dust d = Dust.NewDustPerfect(NPC.Center + rotation * 8, DustID.PinkTorch, rotation * 5 + projVel);
                        d.scale += 1;
                        d.noGravity = true;
                    }
                }
            }
            else
            {
                NPC.ai[0] = 0;
            }
        }

        private void Movement(out float distToTargetPos)
        {
            Player player = Main.player[NPC.target];
            float moveSpeed = 12f;
            float acceleration = 0.4f;
            if (Main.getGoodWorld)//from spaz code. leaving it in cuz why not ig
            {
                moveSpeed *= 1.15f;
                acceleration *= 1.15f;
            }

            Vector2 offset = new Vector2(400, 0);
            float offsetRotation = Utils.GetLerpValue(200, 300, TurnaroundTimer, true) * Utils.GetLerpValue(600, 500, TurnaroundTimer, true);
            offsetRotation *= MathF.PI;
            offset = offset.RotatedBy(offsetRotation);
            offset.Y *= .5f;

            float offsetX = player.Center.X - NPC.Center.X + offset.X;
            float offsetY = player.Center.Y - NPC.Center.Y + offset.Y;
            //out parameter assignment
            distToTargetPos = (NPC.Center - player.Center).Distance(offset);

            float normalizingFactor = moveSpeed / MathF.Sqrt(offsetX * offsetX + offsetY * offsetY);
            offsetX *= normalizingFactor;
            offsetY *= normalizingFactor;

            if (NPC.velocity.X < offsetX)
            {
                NPC.velocity.X += acceleration;
                if (NPC.velocity.X < 0f && offsetX > 0f)
                {
                    NPC.velocity.X += acceleration;
                }
            }
            else if (NPC.velocity.X > offsetX)
            {
                NPC.velocity.X -= acceleration;
                if (NPC.velocity.X > 0f && offsetX < 0f)
                {
                    NPC.velocity.X -= acceleration;
                }
            }
            if (NPC.velocity.Y < offsetY)
            {
                NPC.velocity.Y += acceleration;
                if (NPC.velocity.Y < 0f && offsetY > 0f)
                {
                    NPC.velocity.Y += acceleration;
                }
            }
            else if (NPC.velocity.Y > offsetY)
            {
                NPC.velocity.Y -= acceleration;
                if (NPC.velocity.Y > 0f && offsetY < 0f)
                {
                    NPC.velocity.Y -= acceleration;
                }
            }

        }

        public override void FindFrame(int frameHeight)
        {

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            drawColor *= NPC.Opacity;
            if (NPC.Opacity != 1)
            {
                drawColor = new Color(255, 52, 242, 0) * NPC.Opacity * .5f;
                for (int i = 0; i < 4; i++)
                {
                    float rotation = Main.GlobalTimeWrappedHourly * 5 + i * .25f * MathF.Tau;
                    Vector2 offset = rotation.ToRotationVector2() * 2;
                    if (MathF.Abs(offset.X) > MathF.Abs(offset.Y))
                    {
                        offset.X = MathF.Max(MathF.Abs(offset.X), 2) * MathF.Sign(offset.X);
                    }
                    else
                    {
                        offset.Y = MathF.Max(MathF.Abs(offset.Y), 2) * MathF.Sign(offset.Y);
                    }
                    offset = offset.RotatedBy(NPC.rotation);
                    spriteBatch.Draw(texture, NPC.Center - screenPos + offset, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                    DrawTrail(screenPos + offset, drawColor);
                }
            }
            else
            {
                spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                DrawTrail(screenPos, drawColor);
            }
            return false;
        }

        private void DrawTrailSum(Vector2 screenPos, Color drawColor, Texture2D texture)
        {
            Vector2[] dotPositions = new Vector2[10] { new(0, 0), new(2, 0), new(2, 0), new(2, 2), new(2, 0), new(2, 0), new(2, -2), new(2, 0), new(0, 0), new(-1, -4) };
            Vector2 offset = default;
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                offset += dotPositions[i];
                Vector2 adjustedOffset = offset;
                adjustedOffset.X += NPC.velocity.X * Utils.Remap(i, 0, NPC.oldPos.Length - 1, 0, .9f);
                adjustedOffset.Y -= NPC.velocity.Y * Utils.Remap(i, 0, NPC.oldPos.Length - 1, 0, .9f);

                adjustedOffset *= NPC.scale;
                adjustedOffset.X *= NPC.spriteDirection;
                adjustedOffset -= screenPos;
                adjustedOffset = adjustedOffset.RotatedBy(NPC.rotation * NPC.spriteDirection);
                float rotation = NPC.rotation;
                if (i == NPC.oldPos.Length - 1)
                    rotation -= MathF.PI / 2 * NPC.spriteDirection;
                adjustedOffset += NPC.position;
                Main.EntitySpriteDraw(texture, adjustedOffset, null, drawColor, rotation, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
        }

        void DrawTrail(Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoSprite/EchoSpriteTrail").Value;
            Vector2[] dotPositions = new Vector2[10] { new(0, 0), new(2, 0), new(4, 0), new(6, 2), new(8, 2), new(10, 2), new(12, 0), new(14, 0), new(14, 0), new(13, -4) };
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 offset = dotPositions[i];
                offset.X += NPC.velocity.X * Utils.Remap(i, 0, NPC.oldPos.Length - 1, 0, .9f);
                offset.Y -= NPC.velocity.Y * Utils.Remap(i, 0, NPC.oldPos.Length - 1, 0, .9f);

                offset *= NPC.scale;
                offset.X *= NPC.spriteDirection;
                offset -= screenPos;
                offset = offset.RotatedBy(NPC.rotation);
                float rotation = NPC.rotation;
                if (i == NPC.oldPos.Length - 1)
                    rotation -= MathF.PI / 2 * NPC.spriteDirection;
                offset += NPC.position;
                offset += NPC.Size / 2;
                offset.X -= 4;
                offset.Y -= 10;
                if (NPC.spriteDirection == 1)
                {
                    offset.X += 10;
                }
                Main.EntitySpriteDraw(texture, offset, null, drawColor, rotation, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
        }
    }
}
