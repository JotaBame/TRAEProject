using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace TRAEProject.NewContent.NPCs.Echosphere
{
    public static class EchosphereHelper
    {
        public static void SpectralDraw(NPC NPC, SpriteBatch spriteBatch, Vector2 screenPos, Texture2D texture)
        {
            InternalSpectralDraw(NPC, spriteBatch, screenPos, texture, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }
        public static void SpectralDrawMinusOneIsNoFlip(NPC NPC, SpriteBatch spriteBatch, Vector2 screenPos, Texture2D texture)
        {
            InternalSpectralDraw(NPC, spriteBatch, screenPos, texture, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        }
        public static void SpectralDrawVerticalFlip(NPC NPC, SpriteBatch spriteBatch, Vector2 screenPos, Texture2D texture)
        {
            InternalSpectralDraw(NPC, spriteBatch, screenPos, texture, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
        }
        public static void SearchForAirbornePlayers(NPC NPC)
        {
            if (Main.npc.IndexInRange(NPC.target))
            {
                Player player = Main.player[NPC.target];
                if (!Collision.SolidTiles(player.BottomLeft, player.width, 16))//current player is valid
                {
                    return;
                }
            }
            int target = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                //readability!!!!!!!!!!!!!!!!!!!!!!!
                if (!player.active || player.dead || Collision.SolidTiles(player.BottomLeft, player.width, 16) || target != -1 && player.DistanceSQ(NPC.Center) + player.aggro < Main.player[target].Distance(NPC.Center) + Main.player[target].aggro)
                {
                    continue;
                }
                target = i;
            }
            NPC.target = target;
        }
        public static void SpectralDraw(SpriteBatch spriteBatch, float opacity, float scale, float rotation, Vector2 drawPos, Texture2D texture, SpriteEffects spriteDir, Rectangle? frame, Vector2 origin)
        {
            Color drawColor = new Color(255, 52, 242, 0) * opacity * .5f;
            for (int i = 0; i < 4; i++)
            {
                float spectralRotation = Main.GlobalTimeWrappedHourly * 5 + i * .25f * MathF.Tau;
                Vector2 offset = spectralRotation.ToRotationVector2() * 2;
                if (MathF.Abs(offset.X) > MathF.Abs(offset.Y))
                {
                    offset.X = MathF.Max(MathF.Abs(offset.X), 2) * MathF.Sign(offset.X);
                }
                else
                {
                    offset.Y = MathF.Max(MathF.Abs(offset.Y), 2) * MathF.Sign(offset.Y);
                }
                offset = offset.RotatedBy(rotation);
                spriteBatch.Draw(texture, drawPos + offset, frame, drawColor, rotation, origin, scale, spriteDir, 0);
            }
        }
        private static void InternalSpectralDraw(NPC NPC, SpriteBatch spriteBatch, Vector2 screenPos, Texture2D texture, SpriteEffects spriteDir)
        {
            Color drawColor = new Color(255, 52, 242, 0) * NPC.Opacity * .5f;
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
                spriteBatch.Draw(texture, NPC.Center - screenPos + offset, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, spriteDir, 0);
            }
        }
    }
}
