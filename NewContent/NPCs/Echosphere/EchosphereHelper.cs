using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace TRAEProject.NewContent.NPCs.Echosphere
{
    public static class EchosphereHelper
    {
        static int BlurCountOld => 17;
        static int BlurCountINew => 5;
        static int BlurCountJNew => 5;
        static int BlurCountNew => BlurCountINew * BlurCountJNew;//actually 21 in practice because the corners of the kernel get discarded as their value is too low to matter
        static Vector2 GetBlurOffsetOld(int i, float time)
        {
            return new Vector2(i * 3f % 5f).RotatedBy(i / 5.5f * MathF.Tau + time);
        }
        static bool GetBlurOffsetAndOpacity(int i, int j, float time, out float opacity, out Vector2 offset)
        {
            opacity = gaussianWeights5x5[i, j];
            if (opacity < 0.01f)//opacity too low too mater, don't draw.
            {
                opacity = 0;
                offset = Vector2.Zero;
                return false;
            }
            opacity *= 2.5f;//make it brighter because it was too faint
            Vector2 dir = new(i - 2, j - 2);
            dir *= 4;
            offset = dir.RotatedBy(time);
            return true;
        }
        static float[,] gaussianWeights5x5 = new float[,]
        {
            { 1/273f, 4/273f, 7 / 273f, 4 / 273f, 1/273f },
            { 4/273f, 16 / 273f, 26/273f, 16 / 273f, 4 / 273f},
            { 7/273f, 26 / 273f, 41 /273f, 26 / 273f, 7/273f},
            { 4/273f, 16 / 273f, 26/273f, 16/273f, 4/273f },
            { 1 / 273f, 4 / 273f, 7/273f, 4/273f, 1/273f }
        };

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
        public static void DrawEchoWormSegmentWithBlurOld(Texture2D blurTexture, Texture2D segmentTexture, Vector2 drawPos, float blurOpacity, float rotation, Vector2 origin, float scale, SpriteEffects spriteFX, float nonBlurOpacity, Color nonBlurColor)
        {
            if (nonBlurOpacity > 0)
            {
                Main.EntitySpriteDraw(segmentTexture, drawPos, null, nonBlurColor * nonBlurOpacity, rotation, origin, scale, spriteFX);
            }
            if (blurOpacity > 0)
            {
                Color additiveWhite = new(255, 255, 255, 0);
                float time = (float)(Main.timeForVisualEffects * 0.1f);
                for (float i = 0; i < BlurCountOld; i++)
                {
                    Vector2 blurOffset = GetBlurOffsetOld((int)i, time);
                    Main.EntitySpriteDraw(blurTexture, drawPos + blurOffset, null, additiveWhite * blurOpacity, rotation, origin, scale, spriteFX);
                }
            }
        }
        public static void DrawEchoWormBlurOld(Texture2D texture, Vector2 drawPos, float blurOpacity, float rotation, Vector2 origin, float scale, SpriteEffects spriteFX)
        {
            if (blurOpacity > 0)
            {
                Color additiveWhite = new(255, 255, 255, 0);
                float time = (float)(Main.timeForVisualEffects * 0.1f);
                for (float i = 0; i < 17; i++)
                {
                    Vector2 blurOffset = new Vector2(i * 3f % 5f).RotatedBy(i / 5.5f * MathF.Tau + time);
                    Main.EntitySpriteDraw(texture, drawPos + blurOffset, null, additiveWhite * blurOpacity, rotation, origin, scale, spriteFX);
                }
            }
        }
        public static void DrawEchoWormSegmentWithBlur(Texture2D blurTexture, Texture2D segmentTexture, Vector2 drawPos, float blurOpacity, float rotation, Vector2 origin, float scale, SpriteEffects spriteFX, float nonBlurOpacity, Color nonBlurColor)
        {
            if (nonBlurOpacity > 0)
            {
                Main.EntitySpriteDraw(segmentTexture, drawPos, null, nonBlurColor * nonBlurOpacity, rotation, origin, scale, spriteFX);
            }
            if (blurOpacity > 0)
            {
                Color additiveWhite = new(255, 255, 255, 0);
                float time = (float)(Main.timeForVisualEffects * 0.1f);
                for (int i = 0; i < BlurCountINew; i++)
                {
                    for (int j = 0; j < BlurCountJNew; j++)
                    {
                        if (GetBlurOffsetAndOpacity(i, j, time, out float opacityMult, out Vector2 blurOffset))
                        {
                            Main.EntitySpriteDraw(blurTexture, drawPos + blurOffset, null, additiveWhite * blurOpacity * opacityMult, rotation, origin, scale, spriteFX);
                        }
                    }
                }
            }
        }
        public static void DrawEchoWormBlur(Texture2D texture, Vector2 drawPos, float blurOpacity, float rotation, Vector2 origin, float scale, SpriteEffects spriteFX)
        {
            if (blurOpacity > 0)
            {
                Color additiveWhite = new(255, 255, 255, 0);
                float time = (float)(Main.timeForVisualEffects * 0.1f);
                for (int i = 0; i < BlurCountINew; i++)
                {
                    for (int j = 0; j < BlurCountJNew; j++)
                    {
                        if (GetBlurOffsetAndOpacity(i, j, time, out float opacityMult, out Vector2 blurOffset))
                        {
                            Main.EntitySpriteDraw(texture, drawPos + blurOffset, null, additiveWhite * blurOpacity * opacityMult, rotation, origin, scale, spriteFX);
                        }
                    }
                }
            }
        }
        public static void SearchForSpaceLayerPlayers(NPC npc)
        {
            //first check if current target is valid and in space
            //first search players for only space layer
            //if none found, set target as invalid
            int target = -1;
            if (npc.HasValidTarget)
            {
                target = npc.target;
                Player player = Main.player[target];
                if (!player.ZoneSkyHeight)
                {
                    target = -1;
                }
                else
                {
                    return;//current target is already valid and in space. Don't need to search for another
                }
            }
            if(target == -1)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (!player.active || player.dead || !player.ZoneSkyHeight || target != -1 && player.DistanceSQ(npc.Center) + player.aggro < Main.player[target].DistanceSQ(npc.Center) + Main.player[target].aggro)
                    {
                        continue;
                    }
                    target = i;
                }
            }
            npc.target = target;
        }
        public static int SearchForSpaceLayerPlayers(Vector2 from)
        {
            int target = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (!player.active || player.dead || !player.ZoneSkyHeight || target != -1 && player.DistanceSQ(from) + player.aggro < Main.player[target].DistanceSQ(from) + Main.player[target].aggro)
                {
                    continue;
                }
                target = i;
            }
            return target;
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
                if (!player.active || player.dead || Collision.SolidTiles(player.BottomLeft, player.width, 16) || target != -1 && player.DistanceSQ(NPC.Center) + player.aggro < Main.player[target].DistanceSQ(NPC.Center) + Main.player[target].aggro)
                {
                    continue;
                }
                target = i;
            }
            NPC.target = target;
        }
        public static int SearchForAirbornePlayers(Vector2 from)
        {
            int target = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (!player.active || player.dead || Collision.SolidTiles(player.BottomLeft, player.width, 16) || target != -1 && player.DistanceSQ(from) + player.aggro < Main.player[target].DistanceSQ(from) + Main.player[target].aggro)
                {
                    continue;
                }
                target = i;
            }
            return target;
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
