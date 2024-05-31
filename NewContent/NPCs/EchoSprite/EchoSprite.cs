using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Projectiles.EchoSpriteProj;

namespace TRAEProject.NewContent.NPCs.EchoSprite
{
    /// <summary>
    /// UNTESTED
    /// </summary>
    public class EchoSprite : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailCacheLength[Type] = 10;
            NPCID.Sets.TrailingMode[Type] = 1;
            Main.npcFrameCount[Type] = 3;
        }
        public override void SetDefaults()
        {
            NPC.width = 10;
            NPC.height = 16;
            NPC.defense = 33;
            NPC.lifeMax = 400;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            int firerate = 120;
            float distToTargetPosRequired = 600;//so it doesn't shoot from offscreen
            Movement(out float distToTargetPos);

            NPC.ai[0]++;
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
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * shootSpeed, projID, 100 / 2, 0, Main.myPlayer);
                    }
                    SoundEngine.PlaySound(SoundID.Item125 with { PitchVariance = 0.3f, MaxInstances = 8 }, NPC.Center);//pew pew (phantasmal bolt when shot from true eoc)
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
            int offsetDirection = 1;
            if (NPC.Center.X < player.position.X + player.width)
            {
                offsetDirection = -1;
            }
            float offsetX = player.Center.X - offsetDirection * -400 - NPC.Center.X;
            float offsetY = player.Center.Y - NPC.Center.Y;

            //out parameter assignment
            distToTargetPos = (player.Center - NPC.Center).Distance(new Vector2(offsetDirection * -400, 0));

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
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            texture = ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoSprite/EchoSpriteTrail").Value;
            DrawTrail(screenPos, drawColor);
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
                offset = offset.RotatedBy(NPC.rotation * NPC.spriteDirection);
                float rotation = NPC.rotation;
                if (i == NPC.oldPos.Length - 1)
                    rotation -= MathF.PI / 2 * NPC.spriteDirection;
                offset += NPC.position;
                Main.EntitySpriteDraw(texture, offset, null, drawColor, rotation, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
        }
    }
}
