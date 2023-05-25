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
    public class GlowBall : BaseParticle
    {   static Texture2D texture = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/GlowBallSmallPremultiplied", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        public GlowBall(int index)
        {
            Color = Color.White;
            TimeLeft = 0;
            Scale = Vector2.One;
            Opacity = 1;
            Velocity = Vector2.Zero;
            Position = Vector2.Zero;
            Friction = 1;
            Acceleration = Vector2.Zero;
            WhoAmI = index;
        }
        /// <summary>
        /// spawns a glowing ball
        /// </summary>
        /// <param name="duration">how long it lasts in frames</param>
        /// <param name="friction">velocity and acceleration get multiplied by this every frame</param>
        /// <returns>the glow ball it just spawed</returns>
        public static GlowBall NewGlowBall(Vector2 position, Color color, Vector2? scale = null, Vector2? velocity = null, int duration = 100, Vector2? acceleration = null, float opacity = 1, float friction = 1)
        {
            scale ??= Vector2.One;
            velocity ??= Vector2.Zero;
            acceleration ??= Vector2.Zero;
            int finalIndex = ParticleSystem.maxGlowBalls;
            for (int i = 0; i < ParticleSystem.maxGlowBalls; i++)
            {
                if (ParticleSystem.glowBall[i].Active)
                    continue;
                finalIndex = i;
                ParticleSystem.glowBall[i].Position = position;
                ParticleSystem.glowBall[i].Color = color;
                ParticleSystem.glowBall[i].Scale = scale.Value;
                ParticleSystem.glowBall[i].Velocity = velocity.Value;
                ParticleSystem.glowBall[i].TimeLeft = duration;
                ParticleSystem.glowBall[i].Acceleration = acceleration.Value;
                ParticleSystem.glowBall[i].Opacity = opacity;
                ParticleSystem.glowBall[i].Friction = friction;
                break;
            }
            return ParticleSystem.glowBall[finalIndex];
        }
        public void Update()
        {
            TimeLeft--;
            Position += Velocity;
            Velocity += Acceleration;
            Velocity *= Friction;
            Acceleration *= Friction;
            if (TimeLeft <= 15)
                Opacity -= 1f / 15;
        }
        public void Draw()
        {
            Main.EntitySpriteDraw(texture, Position - Main.screenPosition, null, Color * Opacity, Rotation, texture.Size() / 2, Scale, SpriteEffects.None);
        }
        public void DrawWhitePart()
        {
            Main.EntitySpriteDraw(texture, Position - Main.screenPosition, null, Color.White * Opacity, Rotation, texture.Size() / 2, Scale * 0.5f, SpriteEffects.None);
        }
    }
}
