using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;


namespace TRAEProject.NewContent.Projectiles
{
    public class HelAura : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_0";

        // these are the defaults for all gels
        public override void SetDefaults()
        {
            Projectile.width = 196;
            Projectile.height = 196;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        float scaleFactor = 2f;
        public override void AI()
        {
            Projectile parent = Main.projectile[(int)Projectile.ai[0]];
            if(!parent.active || parent.type != ProjectileID.HelFire)
            {
                Projectile.Kill();
                return;
            }
            else
            {
                Projectile.Center = parent.Center;
            }
            Projectile.localAI[0] += 1f;
            Projectile.rotation += MathF.PI / 120f;
            Projectile.timeLeft = 2;
            scaleFactor = 2f + 0.2f * MathF.Sin(MathF.PI * Projectile.localAI[0] / 120f);
            if(Projectile.localAI[0] < 60f)
            {
                scaleFactor *= Projectile.localAI[0] / 60f;
            }
            int dustTimer = 60;
            if (dustTimer > 0 && Projectile.localAI[0] >= dustTimer && Main.rand.NextFloat() < 0.25f)
            {
                Vector2 center = Main.player[Projectile.owner].Center;
                Vector2 vector = (Projectile.Center - center).SafeNormalize(Vector2.Zero).RotatedByRandom(0.19634954631328583) * 7f;
                Dust dust2 = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(50f, 50f) - vector * 2f, 4, 4, DustID.RedTorch, 0f, 0f, 150, new Color(80, 80, 80));
                dust2.noGravity = true;
                dust2.velocity = vector;
                dust2.scale *= 1.1f + Main.rand.NextFloat() * 0.2f;
                dust2.customData = -0.3f - 0.15f * Main.rand.NextFloat();
            }
        }
        public void DrawFlamethrower(Color color1, Color color2, Color color3, Color color4)
        {
            float num13 = Utils.Remap(Projectile.localAI[0], 0f, (int)Projectile.localAI[0] % 72, 0f, 1f);
            float num11 = Utils.Remap(Projectile.localAI[0], 60, (int)Projectile.localAI[0] % 72, 1f, 0f);

            Texture2D texture = TextureAssets.Projectile[ProjectileID.Flames].Value;
            Vector2 drawAt = Projectile.Center - Main.screenPosition;
            Rectangle rectangle = texture.Frame(1, 7, 0, 3);

            Main.EntitySpriteDraw(texture, drawAt, rectangle, Color.OrangeRed, Projectile.rotation, rectangle.Size() / 2f, scaleFactor, SpriteEffects.None);

            
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Color ColorMiddle = new Color(255, 50, 50, 200);
            Color ColorBack = new Color(240, 170, 23, 200);
            Color ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
            Color ColorSmoke = new Color(65, 65, 65, 100);
            DrawFlamethrower(ColorMiddle, ColorBack, ColorLerp, ColorSmoke);
            return base.PreDraw(ref lightColor);
        }
    }
}