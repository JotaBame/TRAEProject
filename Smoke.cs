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
    public class Smoke : BaseParticle
    {
        public float AngularVelocity { get; set; }
        Texture2D currentTexture = null;
        SpriteEffects spriteEffect;
        public Smoke(int index)
        {
            Color = Color.White;
            TimeLeft = 0;
            Scale = Vector2.One;
            Opacity = 1;
            Velocity = Vector2.Zero;
            Position = Vector2.Zero;
            Friction = 1;
            Acceleration = Vector2.Zero;
            Rotation = 0;
            WhoAmI = index;
            AngularVelocity = 0;
            spriteEffect = SpriteEffects.None;
        }
        /// <summary>
        /// spawns a sparkle
        /// </summary>
        /// <param name="duration">how long it lasts in frames</param>
        /// <param name="friction">velocity and acceleration get multiplied by this every frame</param>
        /// <returns></returns>
        public static Smoke NewSmoke(Vector2 position, Vector2? scale = null, Vector2? velocity = null, int duration = 100, Vector2? acceleration = null, float opacity = 1, float rotation = 0, float angularVelocity = 0,float friction = 1)
        {
            scale ??= Vector2.One * (Main.rand.NextFloat() * 0.15f + 0.95f);
            velocity ??= Main.rand.NextVector2Circular(4,4);
            acceleration ??= new Vector2(0, -Main.rand.NextFloat() * 0.5f);
            int finalIndex = ParticleSystem.maxSmoke;
            for (int i = 0; i < ParticleSystem.maxSmoke; i++)
            {
                if (ParticleSystem.smoke[i].Active)
                    continue;
                finalIndex = i;
                ParticleSystem.smoke[i].Position = position;
                ParticleSystem.smoke[i].Color = Color.Lerp(Color.Black, Color.Gray, Main.rand.NextFloat()) * (Main.rand.NextFloat() * 0.25f + 0.5f);
                ParticleSystem.smoke[i].Scale = scale.Value;
                ParticleSystem.smoke[i].Velocity = velocity.Value;
                ParticleSystem.smoke[i].TimeLeft = duration;
                ParticleSystem.smoke[i].Acceleration = acceleration.Value;
                ParticleSystem.smoke[i].Opacity = opacity;
                ParticleSystem.smoke[i].Rotation = rotation;
                ParticleSystem.smoke[i].AngularVelocity = angularVelocity;
                ParticleSystem.smoke[i].Friction = friction;
                ParticleSystem.smoke[i].currentTexture = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/smoke_0" + Main.rand.Next(1, 9)).Value;
                ParticleSystem.smoke[i].spriteEffect = (SpriteEffects)Main.rand.Next(3);
                break;
            }
            return ParticleSystem.smoke[finalIndex];
        }
        public void Update()
        {
            TimeLeft--;
            Position += Velocity;
            Velocity += Acceleration;
            Velocity *= Friction;
            Acceleration *= Friction;
            Rotation += AngularVelocity;
            AngularVelocity *= Friction;
            if (TimeLeft <= 15)
                Opacity -= 1f / 15;
        }
        public void Draw()
        {
            Main.EntitySpriteDraw(currentTexture, Position - Main.screenPosition, null, Color * Opacity, Rotation, currentTexture.Size() / 2, Scale, spriteEffect);
        }
    }
}
