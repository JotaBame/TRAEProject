using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace TRAEProject.Changes.Items.RuneSetEffects
{
    public static class RuneSetWeaponEffects
    {
        
        public static bool CrystalSerpentAIAddon(Projectile proj)
        {
            if (proj == null) return false;
            if (RuneSetHelper.IsFastThenSlow(proj.ai[2]))
            {
                proj.MaxUpdates = ContentSamples.ProjectilesByType[proj.type].MaxUpdates * 2;
                if (proj.timeLeft > 240)
                    proj.timeLeft = 240;
                RuneSetHelper.SlowDownnnn(proj, ref proj.localAI[2]);
            }
            if (RuneSetHelper.IsHoming(proj.ai[2]))
            {
                RuneSetGlobalProj gProj = proj.GetGlobalProjectile<RuneSetGlobalProj>();
                RuneSetHelper.Homing(proj, 20, ref gProj.amethystHomingTarget, ref proj.localAI[2], .1f, 500);
            }
            if (RuneSetHelper.IsSwirlTwins(proj.ai[2], out int twinSign))
            {
                //crystal serpent AI has no timer, so let's use localAI2
                Vector2 posOffset = RuneSetHelper.GetDoubledPositioningOffset(proj.localAI[2], proj.velocity.ToRotation(), proj.Center, .5f, 16f);
                proj.position += posOffset * twinSign;
                proj.localAI[2]++;
            }
            return true;
        }

        public static bool CrystalStormAIAddon(Projectile proj)
        {
            if (RuneSetHelper.IsFastThenSlow(proj.ai[2]))
            {
                proj.MaxUpdates = ContentSamples.ProjectilesByType[proj.type].MaxUpdates * 2;
                RuneSetHelper.SlowDownnnn(proj, ref proj.localAI[2]);
            }
            if (proj.soundDelay == 0)
            {
                if (RuneSetHelper.IsHoming(proj.ai[2]))
                {
                    proj.localAI[1] = proj.velocity.Length();
                }
                proj.soundDelay = 10000;
            }
            if (RuneSetHelper.IsSwirlTwins(proj.ai[2], out int twinSign))
            {
                //in crystal storm AI, ai1 is used as timer
                Vector2 posOffset = RuneSetHelper.GetDoubledPositioningOffset(proj.ai[1], proj.velocity.ToRotation(), proj.Center, 0.1f, 16f);
                proj.position += posOffset * twinSign;

            }
            if (RuneSetHelper.IsHoming(proj.ai[2]))
            {
                RuneSetHelper.Homing(proj, proj.localAI[1], ref proj.ai[0], ref proj.localAI[2], .1f, 1000);
            }
            return true;
        }
        public static bool CrystalStormExtraDraw(Projectile proj, ref Color lightColor)
        {
            SpriteEffects dir = SpriteEffects.None;
            if (proj.spriteDirection == -1)
            {
                dir = SpriteEffects.FlipHorizontally;
            }
            Color color = proj.GetAlpha(lightColor);
            Vector2 screenPos = Main.screenPosition;
            float num153 = (TextureAssets.Projectile[proj.type].Width() - proj.width) * 0.5f + proj.width * 0.5f;
            for (int i = 0; i < 10; i++)
            {
                float trailFade = (9 - i) / 9f;
                color.R = (byte)(color.R * trailFade);
                color.G = (byte)(color.G * trailFade);
                color.B = (byte)(color.B * trailFade);
                color.A = (byte)(color.A * trailFade);
                Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.oldPos[i].X - screenPos.X + num153, proj.oldPos[i].Y - screenPos.Y + proj.height / 2 + proj.gfxOffY), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), color, proj.rotation, new Vector2(num153, proj.height / 2), trailFade * proj.scale, dir);
            }
            Main.EntitySpriteDraw(TextureAssets.Projectile[proj.type].Value, new Vector2(proj.position.X - screenPos.X + num153, proj.position.Y - screenPos.Y + proj.height / 2 + proj.gfxOffY), new Rectangle(0, 0, TextureAssets.Projectile[proj.type].Width(), TextureAssets.Projectile[proj.type].Height()), color, proj.rotation, new Vector2(num153, proj.height / 2), proj.scale, dir);
            if (RuneSetHelper.BiggerHitbox(proj.ai[2]))
            {
                DrawBigHitboxSparkle(proj.Center - screenPos, new Color(202, 97,255, 128), .5f);
            }
            return false;
        }
        public static void DrawBigHitboxSparkle(Vector2 drawPos, Color color, float opacity = 1, float rotationOffset = 0, float scale = 1f)
        {
            Texture2D tex = TextureAssets.Extra[ExtrasID.SharpTears].Value;
            float scaleOScillation = 1f + (float)Math.Cos(Main.GlobalTimeWrappedHourly * (MathF.PI * 2f) * 4f) * 0.2f;
            scaleOScillation *= scale;
            Vector2 origin = tex.Size() / 2;
            color *= opacity;
            SpriteEffects dir = SpriteEffects.None;
            Main.EntitySpriteDraw(tex, drawPos, null, color * 0.5f, rotationOffset + (float)Math.PI / 4f, origin, new Vector2(1f, 2f) * scaleOScillation, dir);
            Main.EntitySpriteDraw(tex, drawPos, null, color * 0.5f, rotationOffset + (float)Math.PI / 4f, origin, new Vector2(2f, 1f) * scaleOScillation, dir);
            Main.EntitySpriteDraw(tex, drawPos, null, color * 0.5f, rotationOffset - (float)Math.PI / 4f, origin, new Vector2(1f, 2f) * scaleOScillation, dir);
            Main.EntitySpriteDraw(tex, drawPos, null, color * 0.5f, rotationOffset - (float)Math.PI / 4f, origin, new Vector2(2f, 1f) * scaleOScillation, dir);
        }
        public static void CrystalStormExplode(Projectile proj)
        {
            int sideLength = 9;
            Vector2 center = proj.Center;
            for (int i = 0; i < sideLength; i++)
            {  
                for (int j = 0; j < 4; j++)
                {     
                    Vector2 offset = new(i, sideLength);
                    offset.X -= sideLength / 2;
                    offset = offset.RotatedBy(MathF.PI * 0.25f);
                    offset = offset.RotatedBy((j / (float)4) * MathF.Tau);
                    Vector2 dustPos = center + offset;
                    Vector2 dustVel = center - dustPos;
                    Dust.NewDustPerfect(dustPos, DustID.PurpleCrystalShard, -dustVel).noGravity = true;
                }
            }
            SoundEngine.PlaySound(SoundID.Item4 with { MaxInstances = 0 }, proj.Center);
            HitboxIncreaseExplosion(proj, 16 * 5);
        }
        public static void CrystalSerpentExplode(Projectile proj)
        {
            if (RuneSetHelper.IsFastThenSlow(proj.ai[2]))
            {
                proj.MaxUpdates = ContentSamples.ProjectilesByType[proj.type].MaxUpdates * 2;
            }
            if (RuneSetHelper.IsHoming(proj.ai[2]))
            {
                RuneSetHelper.Homing(proj, 20, ref proj.ai[1], ref proj.localAI[2], .1f, 500);
            }
            if (RuneSetHelper.IsSwirlTwins(proj.ai[2], out int twinSign))
            {
                //crystal serpent AI has no timer, so let's use localAI2
                Vector2 posOffset = RuneSetHelper.GetDoubledPositioningOffset(proj.localAI[2], proj.velocity.ToRotation(), proj.Center, .5f, 16f);
                proj.position += posOffset * twinSign;
                proj.localAI[2]++;
            }
        }

        internal static bool CrystalSerpentExtraDraw(Projectile projectile, ref Color lightColor)
        {
            if (RuneSetHelper.BiggerHitbox(projectile.ai[2]))
            {
                DrawBigHitboxSparkle(projectile.Center - Main.screenPosition, Color.HotPink with { A = 128 });
            }
            return true;
        }
        public static void MeteorStaffAI(Projectile proj)
        {

            if (proj.localAI[1] == 0)
            {
                proj.localAI[1] = proj.velocity.Length();
            }
            if (RuneSetHelper.IsFastThenSlow(proj.ai[2]))
            {
                proj.MaxUpdates = ContentSamples.ProjectilesByType[proj.type].MaxUpdates * 2; 
                RuneSetHelper.SlowDownnnn(proj, ref proj.localAI[2], whenToStop: 28, deceleration: 0.975f, newLifeTime: 270);
            }
            if (RuneSetHelper.IsHoming(proj.ai[2]))
            {
                RuneSetGlobalProj gProj = proj.GetGlobalProjectile<RuneSetGlobalProj>();
                proj.localAI[2]++;
                RuneSetHelper.Homing(proj, proj.localAI[1], ref gProj.amethystHomingTarget, ref proj.localAI[2], .1f, 500);
            }
            if (RuneSetHelper.IsSwirlTwins(proj.ai[2], out int twinSign))
            {
                int maxTimeLeft = ContentSamples.ProjectilesByType[proj.type].timeLeft;
                Vector2 posOffset = RuneSetHelper.GetDoubledPositioningOffset(maxTimeLeft-proj.timeLeft, proj.velocity.ToRotation(), proj.Center, .25f, 8f);
                proj.position += posOffset * twinSign;
                proj.localAI[2]++;
            }
        }
        public static void MeteorStaffKill(Projectile proj)
        {
            int explodeHitboxSize = 128;
            if (RuneSetHelper.AoEExplosion(proj.ai[2]))
            {
                explodeHitboxSize *= 2;
            }

            SoundEngine.PlaySound(SoundID.Item89, proj.position);
            proj.position.X += proj.width / 2;
            proj.position.Y += proj.height / 2;
            proj.width = (int)(explodeHitboxSize * proj.scale);
            proj.height = (int)(explodeHitboxSize * proj.scale);
            proj.position.X -= proj.width / 2;
            proj.position.Y -= proj.height / 2;
            for (int i = 0; i < 8; i++)
            {
                Dust.NewDust(new Vector2(proj.position.X, proj.position.Y), proj.width, proj.height, 31, 0f, 0f, 100, default(Color), 1.5f);
            }
            for (int i = 0; i < 32; i++)
            {
                int num464 = Dust.NewDust(new Vector2(proj.position.X, proj.position.Y), proj.width, proj.height, 6, 0f, 0f, 100, default(Color), 2.5f);
                Main.dust[num464].noGravity = true;
                Dust dust2 = Main.dust[num464];
                dust2.velocity *= 3f;
                num464 = Dust.NewDust(new Vector2(proj.position.X, proj.position.Y), proj.width, proj.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                dust2 = Main.dust[num464];
                dust2.velocity *= 2f;
                Main.dust[num464].noGravity = true;
            }
            for (int i = 0; i < 2; i++)
            {
                int num466 = Gore.NewGore(proj.GetSource_Death(), proj.position + new Vector2((float)(proj.width * Main.rand.Next(100)) / 100f, (float)(proj.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, default(Vector2), Main.rand.Next(61, 64));
                Gore gore2 = Main.gore[num466];
                gore2.velocity *= 0.3f;
                Main.gore[num466].velocity.X += (float)Main.rand.Next(-10, 11) * 0.05f;
                Main.gore[num466].velocity.Y += (float)Main.rand.Next(-10, 11) * 0.05f;
            }
            if (proj.owner == Main.myPlayer)
            {
                proj.localAI[1] = -1f;
                proj.maxPenetrate = 0;
                proj.Damage();
            }
            for (int i = 0; i < 5; i++)
            {
                int num468 = Utils.SelectRandom<int>(Main.rand, 6, 259, 158);
                int num469 = Dust.NewDust(proj.position, proj.width, proj.height, num468, 2.5f * proj.direction, -2.5f);
                Main.dust[num469].alpha = 200;
                Dust dust2 = Main.dust[num469];
                dust2.velocity *= 2.4f;
                dust2 = Main.dust[num469];
                dust2.scale += Main.rand.NextFloat();
            }
        }
        public static void FrostStaffAIAddon(Projectile proj)
        {
            if (RuneSetHelper.IsFastThenSlow(proj.ai[2]))
            {
                proj.MaxUpdates = ContentSamples.ProjectilesByType[proj.type].MaxUpdates * 2;
                RuneSetHelper.SlowDownnnn(proj, ref proj.localAI[2]);
            }
            if (RuneSetHelper.IsHoming(proj.ai[2]))
            {
                RuneSetHelper.Homing(proj, 20, ref proj.ai[1], ref proj.localAI[2], .1f, 500);
            }
            if (RuneSetHelper.IsSwirlTwins(proj.ai[2], out int twinSign))
            {
                //crystal serpent AI has no timer, so let's use localAI2
                Vector2 posOffset = RuneSetHelper.GetDoubledPositioningOffset(proj.localAI[2], proj.velocity.ToRotation(), proj.Center, .5f, 16f);
                proj.position += posOffset * twinSign;
                proj.localAI[2]++;
            }
        }
        public static void HitboxIncreaseExplosion(Projectile proj, int newHitboxSideLength)
        {
            Rectangle oldHitbox = proj.Hitbox;
            int oldPen = proj.penetrate;
            proj.penetrate = 9999;
            proj.Hitbox = Utils.CenteredRectangle(proj.Center, new Vector2(newHitboxSideLength));
            proj.Damage();
            proj.Hitbox = oldHitbox;
            proj.penetrate = oldPen;
        }
        
    }
}
