using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace TRAEProject.NewContent.Structures.Echosphere
{
    public class EchosphereEdgeSparkle(int index)
    {
        public static Color Purple => Color.Purple;
        public int WhoAmI { get; protected set; } = index;
        public int TimeLeft = 0;
        public Vector2 Scale = Vector2.One;
        public float Opacity = 1;
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 Position = Vector2.Zero;
        public bool Active { get => TimeLeft > 0; set => TimeLeft = value ? TimeLeft : 0; }
        public void Update()
        {
            TimeLeft--;
            if (TimeLeft <= 30f)
            {
                Opacity -= 1f / 30f;
            }
            else if (Opacity < 1f)
            {
                Opacity += 1f / 10f;
            }
            if (Opacity >= 1f)
            {
                PlayerCollision();
            }
            Position += Velocity;
        }

        private void PlayerCollision()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player plr = Main.player[i];
                if (plr.dead || !plr.active)
                {
                    continue;
                }

                Vector2 plrCenter = plr.Center;
                Vector2 plrDeltaPos = plr.position - plr.oldPosition;
                float radius = plr.Hitbox.Size().Length() * .8f;//hit detection circle size
                Vector2 deltaPos = Position - plrCenter;

                //tried, didn't look good
                //deltaPos.Y *= .5f; //compensate for sparkles being taller

                float distToPlayer = deltaPos.Length();
                if (distToPlayer < radius)
                {
                    Vector2 offsetDirection = (Position - plrCenter);
                    offsetDirection.SafeNormalize(Vector2.UnitY);


                        //wasn't necessary
                    //float pushDistance = radius - distToPlayer;
                    // Position += offsetDirection * pushDistance;

                    float playerSpeed = plrDeltaPos.Length();
                    Velocity += offsetDirection * playerSpeed;
                    if (Velocity.Length() > playerSpeed)
                    {
                        Velocity.Normalize();
                        Velocity *= playerSpeed;
                    }
                }
            }
        }

        private void PlayerCollisionAttempt()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player plr = Main.player[i];
                if (plr.dead || !plr.active)
                {
                    continue;
                }
                Vector2 plrPos = plr.position;
                Vector2 plrBottomRight = plr.BottomRight;
                Vector2 plrCenter = plr.Center;
                Vector2 plrDeltaPos = plr.position - plr.oldPosition;
                float radius = plr.Hitbox.Size().Length() * .4f;//take the dimensions of the player's rectangle hitbox and combine them to approximate a circle hitbox.

                //if intersects player hitbox
                if (Position.Distance(plrCenter) < radius)
                {
                    //push particles outside 
                    Vector2 offsetDirection = (plrCenter - Position);
                    offsetDirection.SafeNormalize(Vector2.UnitY);
                    Position = plrCenter + offsetDirection * radius;
                    float playerSpeed = plrDeltaPos.Length();
                    Velocity += offsetDirection * playerSpeed;
                    if (Velocity.LengthSquared() > plrDeltaPos.LengthSquared())
                    {
                        Velocity.Normalize();
                        Velocity *= playerSpeed;
                    }
                }
            }
        }

        private void PushAroundParticles_Rect(Player plr, Vector2 plrPos, Vector2 plrBottomRight)
        {
            if (Position.X < plrBottomRight.X && Position.X > plrPos.X && Position.Y > plrPos.Y && Position.Y < plrBottomRight.Y)
            {
                // Main.NewText("intersected hitbox");
                Vector2 plrDeltaPos = plr.position - plr.oldPosition;
                Position += plrDeltaPos;
                Velocity += plrDeltaPos;
                if (Velocity.LengthSquared() > plrDeltaPos.LengthSquared())
                {
                    Velocity.Normalize();
                    Velocity *= plrDeltaPos.Length();
                }
            }
        }

        public void Draw()
        {
            Vector2 drawpos = Position - Main.screenPosition;
            Color bigShineColor = Purple * 0.75f;
            bigShineColor.A = (byte)(Main.dayTime ? (100 * Opacity) : 0);
            Vector2 origin = Sparkle.sparkleTexture.Size() / 2f;
            float brightness = MathF.Cos(TimeLeft * 0.4f) * 0.1f + .9f;
            Vector2 scaleX = new Vector2(0.5f, Scale.X) * brightness;
            Vector2 scaleY = new Vector2(0.5f, Scale.Y) * brightness;
            bigShineColor *= brightness;
            Main.EntitySpriteDraw(Sparkle.sparkleTexture, drawpos, null, bigShineColor * Opacity, MathHelper.PiOver2, origin, scaleX, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(Sparkle.sparkleTexture, drawpos, null, bigShineColor * Opacity, 0, origin, scaleY, SpriteEffects.None, 0);
        }
        public void DrawWhitePart()
        {
            Vector2 drawpos = Position - Main.screenPosition;
            Vector2 origin = Sparkle.sparkleTexture.Size() / 2f;
            Color smallShineColor = Color.White * .75f;
            smallShineColor.A = 0;
            float brightness = MathF.Cos(TimeLeft * 0.4f) * 0.1f + .9f;
            Vector2 scaleX = new Vector2(0.5f, Scale.X) * brightness;
            Vector2 scaleY = new Vector2(0.5f, Scale.Y) * brightness;
            smallShineColor *= brightness;
            Main.EntitySpriteDraw(Sparkle.sparkleTexture, drawpos, null, smallShineColor * Opacity, MathHelper.PiOver2, origin, scaleX * 0.6f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(Sparkle.sparkleTexture, drawpos, null, smallShineColor * Opacity, 0, origin, scaleY * 0.6f, SpriteEffects.None, 0);
        }
      
    }
}