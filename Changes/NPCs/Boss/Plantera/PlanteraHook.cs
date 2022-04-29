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
    class PlanteraHook : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plantera's Hook");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 5;
            Projectile.timeLeft = 180 * (1 + Projectile.extraUpdates);
            //28 32
        }
        Vector2? anchor = null;
        Player player;
        public override void Kill(int timeLeft)
        {
            Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)Main.rand.Next(Projectile.width), Projectile.position.Y + (float)Main.rand.Next(Projectile.height)), Projectile.velocity, 390, Projectile.scale);
            Gore.NewGore(Projectile.GetSource_Death(), new Vector2(Projectile.position.X + (float)Main.rand.Next(Projectile.width), Projectile.position.Y + (float)Main.rand.Next(Projectile.height)), Projectile.velocity, 391, Projectile.scale);


            float dist = (Projectile.Center - (Vector2)anchor).Length();
            float rot = ((Vector2)anchor - Projectile.Center).ToRotation();
            for (int k = 0; k < dist; k += 28)
            {
                Vector2 pos = Projectile.Center + TRAEMethods.PolarVector(k, rot);
                Dust.NewDust(pos - Vector2.One * 16, 16, 16, DustID.Plantera_Green);
            }

            
        }
        public override void AI()
        {
            
            if(anchor == null)
            {
                anchor = Projectile.Center;
                player = Main.player[Player.FindClosest(Projectile.Center, 1, 1)];
            }
            else
            {
                if (TRAEMethods.AngularDifference(((Vector2)anchor - Projectile.Center).ToRotation(), (player.Center - Projectile.Center).ToRotation()) < (float)Math.PI/2)
                {
                    Projectile.tileCollide = true;
                }
            }
            if(Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
            }
            Projectile.frame = Projectile.velocity == Vector2.Zero ? 1 : 0;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (anchor != null)
            {
                float p = 0;
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), (Vector2)anchor, Projectile.Center, 16, ref p);
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D vine = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Plantera/ThornyVine").Value;
            float dist = (Projectile.Center - (Vector2)anchor).Length();
            float rot = ((Vector2)anchor - Projectile.Center).ToRotation();
            for (int k = 0; k < dist; k += vine.Height)
            {
                Vector2 pos = Projectile.Center + TRAEMethods.PolarVector(k, rot);
                Main.EntitySpriteDraw(vine, pos - Main.screenPosition, null, Lighting.GetColor((int)pos.X / 16, (int)pos.Y / 16), rot + (float)Math.PI / 2f, new Vector2(vine.Width / 2, vine.Height), 1f, SpriteEffects.None, 0);
            }

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                        new Rectangle(0, Projectile.frame * 50, texture.Width, 50), lightColor, Projectile.rotation,
                        new Vector2(28, 32), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
