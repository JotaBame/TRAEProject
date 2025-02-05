using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TRAEProject.NewContent.NPCs.Echosphere.EchoLeviathan;

namespace TRAEProject.NewContent.Projectiles.EchoLeviathanPortal
{
    internal class EchoLeviathanPortal : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.scale = 0;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
        static float EaseInOut(float progress)
        {
            return .5f - .5f * MathF.Cos(progress * MathF.PI);
        }
        public override void AI()
        {
            Projectile.alpha -= 10;
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            //ai0 is duration left, set when the projectile is spawned
            Projectile.ai[0]--;
            if (Projectile.ai[0] < 10)
            {
                Projectile.Opacity -= .1f;
            }
            if (Projectile.Opacity == 0)
            {
                Projectile.Kill();
            }
            Projectile.rotation -= Projectile.direction * (MathF.PI * 2f) / 120f;
            Projectile.scale = EaseInOut(Projectile.Opacity);
            Lighting.AddLight(Projectile.Center, 0.7f, 0.2f, 0.6f);
            CreateDust();
        }

        private void CreateDust()
        {
            for (int i = 0; i < 2; i++)
            {
                if (Main.rand.NextBool(2))
                {
                    Vector2 vector144 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * Projectile.scale;
                    Dust dust50 = Main.dust[Dust.NewDust(Projectile.Center - vector144 * 60, 0, 0, 86)];
                    dust50.noGravity = true;
                    dust50.position = Projectile.Center - vector144 * Main.rand.Next(20, 42);
                    dust50.velocity = vector144.RotatedBy(1.5707963705062866) * 10;
                    dust50.scale = 0.9f + Main.rand.NextFloat();
                    dust50.fadeIn = 0.5f;
                    dust50.customData = this;
                    vector144 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * Projectile.scale;
                    dust50 = Main.dust[Dust.NewDust(Projectile.Center - vector144 * 60, 0, 0, 90)];
                    dust50.noGravity = true;
                    dust50.position = Projectile.Center - vector144 * Main.rand.Next(20, 42);
                    dust50.velocity = vector144.RotatedBy(1.5707963705062866) * 8;
                    dust50.scale = 0.9f + Main.rand.NextFloat();
                    dust50.fadeIn = 0.5f;
                    dust50.customData = this;
                    dust50.color = Color.Crimson;
                }
                else
                {
                    Vector2 vector145 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * Projectile.scale;
                    Dust dust51 = Main.dust[Dust.NewDust(Projectile.Center - vector145 * 30, 0, 0, 240)];
                    dust51.noGravity = true;
                    dust51.position = Projectile.Center - vector145 * Main.rand.Next(40, 62);
                    dust51.velocity = vector145.RotatedBy(-1.5707963705062866) * 5;
                    dust51.scale = 0.9f + Main.rand.NextFloat();
                    dust51.fadeIn = 0.5f;
                    dust51.customData = this;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D back = ModContent.Request<Texture2D>("TRAEProject/NewContent/Projectiles/EchoLeviathanPortal/EchoLeviathanPortalBack").Value;
            Texture2D front = TextureAssets.Projectile[Type].Value;
            Vector2 drawPos = Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Color mainColor = Color.White;
            Vector2 frontOrigin = new Vector2(front.Width, front.Height) / 2f;
            float rotation = Projectile.rotation;
            Color secondryColor = mainColor * 0.8f;
            secondryColor.A /= 2;
            Color wobblingColor = Color.Lerp(mainColor, Color.Black, 0.5f);
            wobblingColor.A = mainColor.A;
            float wobblingColorOpacity = 0.95f + MathF.Sin(Projectile.rotation * 0.75f) * 0.1f;
            wobblingColor *= wobblingColorOpacity;
            float wobblingScale = 0.6f + Projectile.scale * 0.6f * wobblingColorOpacity;
            Vector2 backOrigin = back.Size() / 2f;
            Main.EntitySpriteDraw(back, drawPos, null, wobblingColor, 0f - rotation + 0.35f, backOrigin, wobblingScale * Projectile.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(back, drawPos, null, mainColor, 0f - rotation, backOrigin, Projectile.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(front, drawPos, null, secondryColor, (0f - rotation) * 0.7f, frontOrigin, Projectile.scale, SpriteEffects.None);
            Main.EntitySpriteDraw(back, drawPos, null, mainColor * 0.8f, rotation * 0.5f, backOrigin, Projectile.scale * 0.9f, SpriteEffects.FlipHorizontally);
            mainColor.A = 0;
            Main.EntitySpriteDraw(front, drawPos, null, mainColor, Projectile.rotation, frontOrigin, Projectile.scale, SpriteEffects.FlipHorizontally);
            return false;
        }
    }
}
