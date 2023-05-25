using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject;

using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss.Dreadnautilus
{ 
    public class BloodSnipe : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 26;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

        }
        bool runOnce = true;
        public override void AI()
        {
            if(runOnce)
            {
                SoundEngine.PlaySound(SoundID.Item171 with { MaxInstances = 0 }, Projectile.Center);
                runOnce = false;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
            Projectile.ai[0]--;
            if(Projectile.ai[0] <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item171 with { MaxInstances = 0 }, Projectile.Center);
                for (int i =0; i < 8; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,  TRAEMethods.PolarVector(10, Projectile.rotation + ((float)i / 8f) * (float)Math.PI * 2f), ProjectileID.BloodNautilusShot, (int)(Projectile.damage * 0.8f), 0);
                }
                Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Dreadnautilus/BloodSnipe").Value;
            for (int i = 18; i > 0; i += -1)
            {
                Color color = Color.Lerp(lightColor, lightColor * 0.5f, (float)i / (float)0);
               // Color color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                Vector2 value18 = Projectile.oldPos[i];
                if (value18 == Vector2.Zero)
                {
                    continue;
                }
                Vector2 position3 = value18 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Main.EntitySpriteDraw(texture, position3, null, color, Projectile.rotation, texture.Size() * 0.5f, 0.7f, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}
