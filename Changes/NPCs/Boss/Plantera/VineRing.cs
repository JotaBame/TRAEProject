using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TRAEProject.Changes.NPCs.Boss.Plantera
{
    class VineRing : ModProjectile
    {
        public const float Radius = 600f;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Vine Ring");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.alpha = 250;
            //28 32
        }
        Player player;
        
        public override void AI()
        {
            if(Projectile.alpha > 0)
            {
                Projectile.alpha--;
                Projectile.Center = Main.player[Projectile.owner].Center;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 targetCenter = targetHitbox.Center.ToVector2();
            return (Projectile.Center - targetCenter).Length() > Radius-16 && (Projectile.Center - targetCenter).Length() < Radius+10;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D vine = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Plantera/ThornyVine").Value;
            float height = vine.Height;
            for(float arcLength =0; arcLength < Radius * MathF.PI * 2f; arcLength += height)
            {
                float theta = arcLength / Radius;
                Vector2 pos = Projectile.Center + TRAEMethods.PolarVector(Radius, theta) ;
                Color light = Lighting.GetColor((int)pos.X / 16, (int)pos.Y / 16);
                Color color = new Color(light.R, light.G, light.B, Projectile.alpha);
                Main.EntitySpriteDraw(vine, pos - Main.screenPosition, null, color, theta, Vector2.UnitX * 14, 1f, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}