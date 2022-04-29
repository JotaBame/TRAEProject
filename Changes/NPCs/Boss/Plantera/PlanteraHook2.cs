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

namespace TRAEProject.Changes.NPCs.Boss.Plantera
{
    class PlanteraHook2 : ModProjectile
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
            Projectile.timeLeft = 600;
            //28 32
        }
        Vector2? anchor = null;
        int timer =0;
        public override void Kill(int timeLeft)
        {
            Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + (float)Main.rand.Next(Projectile.width), Projectile.position.Y + (float)Main.rand.Next(Projectile.height)), Projectile.velocity, 390, Projectile.scale);
            Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + (float)Main.rand.Next(Projectile.width), Projectile.position.Y + (float)Main.rand.Next(Projectile.height)), Projectile.velocity, 391, Projectile.scale);
        }
        public override void AI()
        {
            if(anchor == null)
            {
                anchor = Projectile.Center;
            }
            else
            {
                timer++;
                if(timer > 60)
                {
                    Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * 0.001f;
                    Projectile.frame = 1;
                    Projectile.hostile = true;

                }
                else
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D vine = TextureAssets.Chain26.Value;
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
