using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Weapons.Magic.DreamEater.Dusts;

namespace TRAEProject.NewContent.Items.Weapons.Magic.DreamEater
{
    public static class DreamEaterDustHelper
    {
        public static int PurpleDustID => ModContent.DustType<DreamEaterDustPurple>();
        public static void DustCircle(Vector2 center, float radius, float density, int type, float scale)
        {
            int dustCount = (int)MathF.Round(radius * density);
            for (int i = 0; i < dustCount; i++)
            {
                float progress = (float)i / dustCount;
                Vector2 offset = (progress * MathF.Tau).ToRotationVector2() * radius;
                Dust.NewDustPerfect(center + offset, type, Vector2.Zero, 0, new Color(255, 255, 255, 128), scale);
            }
        }
        public static void DreamEaterShapeDust(Vector2 center, float radius, float density, int type, float scale)
        {
            float velocityMult = 0.3f;
            int dustCount = (int)MathF.Round(radius * density);
            for (int i = 0; i < dustCount; i++)
            {
                float progress = (float)i / dustCount;
                Vector2 offset = (progress * MathF.Tau).ToRotationVector2() * radius;
                Dust.NewDustPerfect(center + offset, type, offset * velocityMult, 0, new Color(255, 255, 255, 128), scale);
            }
            dustCount = (int)MathF.Round(radius * density);
            radius *= 0.8f;
            for (int i = 0; i < dustCount; i++)
            {
                float progress = (float)i / dustCount;
                Vector2 offset = (progress * MathF.Tau + (MathF.PI / dustCount)).ToRotationVector2() * radius;
                Dust.NewDustPerfect(center + offset, type, offset * velocityMult, 0, new Color(255, 255, 255, 128), scale);
            }
            int armsCount = 7;
            float spiralness = armsCount / MathF.PI;
            int armDustCount = (int)(dustCount / MathF.PI);
            for (int i = 0; i < armsCount; i++)
            {
                for (int j = 0; j < armDustCount; j++)
                {
                    float armProgress = (float)j / armDustCount;
                    float dist = EaseOut(armProgress) * radius;
                    armProgress = EaseIn(armProgress);
                    float angle = Utils.Remap(i, 0, armsCount, 0, MathF.Tau);
                    angle += armProgress * spiralness;
                    Vector2 offset = angle.ToRotationVector2() * dist;
                    Dust.NewDustPerfect(center + offset, type, offset * velocityMult, 0, new Color(255, 255, 255, 128), scale);
                }
                for (int j = 0; j < armDustCount; j++)
                {
                    float armProgress = (float)j / armDustCount;
                    float dist = EaseOut(armProgress) * radius;
                    armProgress = EaseIn(armProgress);
                    float angle = Utils.Remap(i, 0, armsCount, 0, MathF.Tau);
                    angle -= armProgress * spiralness;
                    Vector2 offset = angle.ToRotationVector2() * dist;
                    Dust.NewDustPerfect(center + offset, type, offset * velocityMult, 0, new Color(255, 255, 255, 128), scale);
                }
            }
        }
        static float EaseOut(float x)
        {
            x = 1 - x;
            x *= x;
            x = 1 - x;
            return x;
        }
        static float EaseIn(float x)
        {
            x *= x;
            return x;
        }
        public static void PolygonShapeDust(Vector2 pos, float radius, float rotation, int sides, int dustID, Color dustColor)
        {
            if (sides < 3)
                return; // Not enough sides for a polygon

            // Generate vertices
            Vector2[] vertices = new Vector2[sides];
            float angleIncrement = MathHelper.TwoPi / sides;

            for (int i = 0; i < sides; i++)
            {
                float angle = i * angleIncrement + rotation;
                vertices[i] = pos + radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            }

            // Spawn dust along edges
            for (int i = 0; i < sides; i++)
            {
                Vector2 start = vertices[i];
                Vector2 end = vertices[(i + 1) % sides];
                float dist = (end - start).Length();
                float increment = 4f / dist;
                for (float j = 0; j < 1f; j += increment)
                {
                    float t = j;
                    Vector2 point = Vector2.Lerp(start, end, t);
                    Vector2 vel = point - pos;
                    vel *= 0.1f;
                    Dust dust = Dust.NewDustPerfect(pos, dustID, vel, 0, dustColor, 1f);
                    dust.noGravity = true;
                    dust.scale *= 1.2f;
                }
            }
        }

    }
}
