using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TRAEProject.Changes.NPCs.Boss.DestroyerChanges
{
    public class DestroyerBeam : ModSystem
    {
        private BlendState MaxBlend
        {
            get
            {
                return new BlendState
                {
                    ColorBlendFunction = BlendFunction.Add,
                    ColorDestinationBlend = Blend.One,//wtf is this
                    ColorSourceBlend = Blend.SourceAlpha,//wtf is this
                    AlphaBlendFunction = BlendFunction.Add,
                    AlphaDestinationBlend = Blend.SourceAlpha,//wtf is this
                    AlphaSourceBlend = Blend.SourceAlpha
                };
            }
        }
        static Asset<Texture2D> lightningLine;
        static Asset<Texture2D> lightningEnd;
        static List<DrawData> lines;
        static void AddSegmentToCache(Vector2 from, Vector2 to, Color color, float width)
        {
            if (lines == null)
            {
                lines = new List<DrawData>();
            }
            float rotation = (to - from).ToRotation();
            Vector2 origin = lightningLine.Size() / 2;//both textures the same size
            float length = (to - from).Length() / lightningLine.Height();
            Vector2 scale = new Vector2(width, length);
            lines.Add(new DrawData(lightningLine.Value, (from + to) / 2 - Main.screenPosition, null, color, rotation + MathF.PI / 2, origin, scale, default));
        }
        public static void AddLightningToCache(Vector2 from, Vector2 to, Color color, float width, int vertices, int randomSeedOffset, Vector2 normalForRandOffSetFrom, Vector2 normalForRandOffsetTo)
        {
            float beamDuration = 4;
            for (int j = 0; j < beamDuration; j++)
            {
                UnifiedRandom rnd = new UnifiedRandom((int)Main.timeForVisualEffects - j + randomSeedOffset);
                float opacity = Utils.Remap(1 - (j / beamDuration), 0, 1, .3f, 1);
                float[] positions = new float[vertices];
                for (int i = 0; i < vertices; i++)
                {
                    positions[i] = rnd.NextFloat(- (1f/vertices), 1 - (1f/vertices));
                }
                Array.Sort(positions);

                float sway = 80f;
                float jaggedness = 1f / sway;
                Vector2 tangent = to - from;
                Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
                float length = tangent.Length();
                Vector2 prevPoint = from;
                float prevDisplacement = 0f;
                float time = Main.GlobalTimeWrappedHourly * 10;
                for (int i = 1; i < positions.Length; i++)
                {
                    float currentPos = positions[i];
                    // used to prevent sharp angles by ensuring very close positions also have small perpendicular variation. 
                    float scale = (length * jaggedness) * (currentPos - positions[i - 1]);

                    // defines an envelope. Points near the middle of the bolt can be further from the central line. 
                    float envelope = currentPos > 0.95f ? 20 * (1 - currentPos) : 1;
                    float displacement = rnd.NextFloat(-sway, sway);//this is where to sample the sine wave combination
                    displacement -= (displacement - prevDisplacement) * (1 - scale);
                    displacement *= envelope;
                    if (displacement > 40)
                        displacement = 40;
                    if (displacement < -40)
                        displacement = -40;
                    Vector2 point = from + currentPos * tangent + displacement * normal;
                    if(i == 1)
                    {
                        prevPoint += normalForRandOffSetFrom * rnd.NextFloat(-17, 17);
                    }
                    AddSegmentToCache(prevPoint, point, color * opacity, width);
                    prevPoint = point;
                    prevDisplacement = displacement;
                }
                AddSegmentToCache(prevPoint, to + normalForRandOffsetTo * rnd.NextFloat(-20,20), color * opacity, width);
            }
           
        }

        static float RandomSineWave(UnifiedRandom rnd, float time)
        {
            int count = rnd.Next(3, 6);
            float amplitude = 1;
            float[] phases = new float[count];
            float[] amplitudes = new float[count];
            float[] frequencies = new float[count];

            for (int i = 0; i < count; i++)
            {
                phases[i] = rnd.NextFloat(MathF.Tau);
                amplitudes[i] = amplitude;
                frequencies[i] = rnd.NextFloat(2) * amplitude;
                amplitude *= rnd.NextFloat(0.6f, 0.45f);
            }
            return SampleSineWaveCombination(phases, amplitudes, frequencies, time);
        }
        static float SampleSineWaveCombination(float[] phases, float[] amplitides, float[] frequencies, float time)
        {
            float sum = 0;
            for (int i = 0; i < phases.Length; i++)
            {
                sum += MathF.Sin(time * frequencies[i] + phases[i]) * amplitides[i];
            }
            return sum;
        }
        public override void Load()
        {
            lightningLine = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/DestroyerChanges/GlowLine");
            lightningEnd = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/DestroyerChanges/GlowLineCap");
            On_Main.DoDraw_DrawNPCsBehindTiles += DrawDestroyerBeams;
        }

        private void DrawDestroyerBeams(On_Main.orig_DoDraw_DrawNPCsBehindTiles orig, Main self)
        {
            if (lines == null || lines.Count <= 0)
            {
                orig(self);
                return;
            }
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, MaxBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].Draw(Main.spriteBatch);
            }
            float opacity = 1;
            for (int i = 0; i < lines.Count; i++)
            {
                DrawData data = lines[i];
                opacity = (data.color.A / 255f);
                Main.spriteBatch.Draw(data.texture, data.position, null, Color.White * opacity, data.rotation, data.origin, new Vector2(data.scale.X * .5f, data.scale.Y), default, 0);
            }
            Texture2D lightningEnd = DestroyerBeam.lightningEnd.Value;
            for (int i = 0; i < lines.Count; i++)
            {
                DrawData data = lines[i];
                Vector2 scale = data.scale;
                scale.Y = 1;
                Vector2 offset = (data.rotation + MathF.PI / 2f).ToRotationVector2() * data.scale.Y * data.texture.Width / 2f;
                Main.spriteBatch.Draw(lightningEnd, data.position + offset, null, data.color, data.rotation, data.origin, scale, default, 0);
                Main.spriteBatch.Draw(lightningEnd, data.position - offset, null, data.color, data.rotation + MathF.PI, data.origin, scale, default, 0);
            }
            for (int i = 0; i < lines.Count; i++)
            {
                DrawData data = lines[i];
                opacity = (data.color.A / 255f);
                Vector2 scale = data.scale;
                scale.Y = 1;
                scale.X *= .5f;
                Vector2 offset = (data.rotation + MathF.PI / 2f).ToRotationVector2() * data.scale.Y * data.texture.Width / 2f;
                Main.spriteBatch.Draw(lightningEnd, data.position + offset, null, Color.White * opacity, data.rotation, data.origin, scale, default, 0);
                Main.spriteBatch.Draw(lightningEnd, data.position - offset, null, Color.White * opacity, data.rotation + MathF.PI, data.origin, scale, default, 0);
            }
            Main.spriteBatch.End();
            lines = null;
            orig(self);
        }
    }
}
