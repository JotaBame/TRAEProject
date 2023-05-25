using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TRAEProject
{
    public class ParticleSystem : ModSystem
    {
        public const int maxSparkles = 1300;
        public const int maxSmoke = 500;
        public const int maxGlowBalls = 1600;
        public static Sparkle[] sparkle = new Sparkle[maxSparkles + 1];
        public static Smoke[] smoke = new Smoke[maxSmoke + 1];
        public static GlowBall[] glowBall = new GlowBall[maxGlowBalls + 1];
        public override void Load()
        {
            for (int i = 0; i < sparkle.Length; i++)
                sparkle[i] = new Sparkle(i);
            for (int i = 0; i < smoke.Length; i++)
                smoke[i] = new Smoke(i);
            for (int i = 0; i < glowBall.Length; i++)
                glowBall[i] = new GlowBall(i);
        }
        public override void Unload()
        {
            sparkle = null;
            smoke = null;
            glowBall = null;
        }
        public override void PostDrawTiles()
        {
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            int iterationsToGoThrough = (int)MathF.Max(maxSmoke, maxGlowBalls);
            iterationsToGoThrough = (int)MathF.Max(iterationsToGoThrough, maxSparkles);
            for (int i = 0; i < iterationsToGoThrough; i++)
            {
                if (i < maxSparkles && sparkle[i].Active)
                    sparkle[i].DrawWhitePart();
                if (i < maxGlowBalls && glowBall[i].Active)
                    glowBall[i].DrawWhitePart();
            }
            for (int i = 0; i < iterationsToGoThrough; i++)
            {
                if (i < maxSparkles && sparkle[i].Active)
                    sparkle[i].Draw();
                if (i < maxGlowBalls && glowBall[i].Active)
                    glowBall[i].Draw();
                if (i < maxSmoke && smoke[i].Active)
                    smoke[i].Draw();
            }
            Main.spriteBatch.End();
        }
        //I just chose PostUpdateDusts arbitrarily, it can be any update method that runs globally
        public override void PostUpdateDusts()
        {
            int iterationsToGoThrough = (int)MathF.Max(maxSmoke, maxGlowBalls);
            iterationsToGoThrough = (int)MathF.Max(iterationsToGoThrough, maxSparkles);
            for (int i = 0; i < iterationsToGoThrough; i++)
            {
                if (i < maxSparkles && sparkle[i].Active)
                    sparkle[i].Update();
                if (i < maxGlowBalls && glowBall[i].Active)
                    glowBall[i].Update();
                if (i < maxSmoke && smoke[i].Active)
                    smoke[i].Update();
            }
            sparkle[maxSparkles].Active = false;
            smoke[maxSmoke].Active = false;
            glowBall[maxGlowBalls].Active = false;
        }
    }

    public class BaseParticle
    {
        public int WhoAmI { get; protected set; }
        public Color Color { get; set; }
        public int TimeLeft { get; set; }
        public Vector2 Scale { get; set; }
        public float Opacity { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }
        public float Friction { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Rotation { get; set; }
        public bool Active { get => TimeLeft > 0; set => TimeLeft = value ? TimeLeft : 0; }
    }
}
