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
    public class Sparkle : BaseParticle
    {
        readonly static Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
        public Vector2 Fatness { get; set; }
        public Sparkle(int index)
        {
            Color = Color.White;
            TimeLeft = 0;
            Scale = Vector2.One;
            Fatness = Vector2.One;
            Opacity = 1;
            Velocity = Vector2.Zero;
            Position = Vector2.Zero;
            Friction = 1;
            Acceleration = Vector2.Zero;
            Rotation = 0;
            WhoAmI = index;
        }
        /// <summary>
        /// spawns a sparkle
        /// </summary>
        /// <param name="duration">how long it lasts in frames</param>
        /// <param name="fatness">this can be used to stretch the sparkle</param>
        /// <param name="friction">velocity and acceleration get multiplied by this every frame</param>
        /// <returns></returns>
        public static Sparkle NewSparkle(Vector2 position, Color color, Vector2? scale = null, Vector2? velocity = null, int duration = 100, Vector2? fatness = null, Vector2? acceleration = null, float opacity = 1, float rotation = 0, float friction = 1)
        {
            scale ??= Vector2.One;
            velocity ??= Vector2.Zero;
            fatness ??= Vector2.One;
            acceleration ??= Vector2.Zero;
            int finalIndex = ParticleSystem.maxSparkles;
            for (int i = 0; i < ParticleSystem.maxSparkles; i++)
            {
                if (ParticleSystem.sparkle[i].Active)
                    continue;
                finalIndex = i;
                ParticleSystem.sparkle[i].Position = position;
                ParticleSystem.sparkle[i].Color = color;
                ParticleSystem.sparkle[i].Scale = scale.Value;
                ParticleSystem.sparkle[i].Velocity = velocity.Value;
                ParticleSystem.sparkle[i].TimeLeft = duration;
                ParticleSystem.sparkle[i].Fatness = fatness.Value;
                ParticleSystem.sparkle[i].Acceleration = acceleration.Value;
                ParticleSystem.sparkle[i].Opacity = opacity;
                ParticleSystem.sparkle[i].Rotation = rotation;
                ParticleSystem.sparkle[i].Friction = friction;
                break;
            }
            return ParticleSystem.sparkle[finalIndex];
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
            Vector2 drawpos = Position - Main.screenPosition;
            Color bigShineColor = Color * 0.5f;
            bigShineColor.A = (byte)(Main.dayTime ? (100 * Opacity) : 0);
            Vector2 origin = sparkleTexture.Size() / 2f;
            float brightness = MathF.Cos(TimeLeft * 0.4f) * 0.25f + 0.75f;
            Vector2 scaleX = new Vector2(Fatness.X * 0.5f, Scale.X) * brightness;
            Vector2 scaleY = new Vector2(Fatness.Y * 0.5f, Scale.Y) * brightness;
            bigShineColor *= brightness;
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigShineColor * Opacity, MathHelper.PiOver2 + Rotation, origin, scaleX, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, bigShineColor * Opacity, Rotation, origin, scaleY, SpriteEffects.None, 0);
        }
        public void DrawWhitePart()
        {
            Vector2 drawpos = Position - Main.screenPosition;
            Vector2 origin = sparkleTexture.Size() / 2f;
            Color smallShineColor = Color.White * 0.5f;
            smallShineColor.A = 0;
            float brightness = MathF.Cos(TimeLeft * 0.4f) * 0.25f + 0.75f;
            Vector2 scaleX = new Vector2(Fatness.X * 0.5f, Scale.X) * brightness;
            Vector2 scaleY = new Vector2(Fatness.Y * 0.5f, Scale.Y) * brightness;
            smallShineColor *= brightness;
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallShineColor * Opacity, MathHelper.PiOver2 + Rotation, origin, scaleX * 0.6f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(sparkleTexture, drawpos, null, smallShineColor * Opacity, Rotation, origin, scaleY * 0.6f, SpriteEffects.None, 0);
        }
    }
}
