using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using static Terraria.Main;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

using Terraria.GameContent.Drawing;
using TRAEProject.Common;
using TRAEProject.Changes.Items;
using System.Collections.Generic;
using TRAEProject;
using TRAEProject.Changes.Accesory;
using TRAEProject.Changes.Weapon.Melee.MeowmereEffect;

namespace TRAEProject.Changes.Weapon.Melee
{
    public abstract class SwordAura : ModProjectile
    {
        public override string Texture => "TRAEProject/Changes/Weapon/Melee/Aura";

        public virtual void AuraDefaults()
        {

        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(16);

            Projectile.aiStyle = 190;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.usesOwnerMeleeHitCD = true;
            Projectile.ownerHitCheckDistance = 300f;
            AuraDefaults();
        }
        public override bool PreAI()
        {
            SwingAI();
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {

            float coneLength = 95 * Projectile.scale;
            float maximumAngle = MathF.PI / 2.5f;
            float coneRotation = Projectile.rotation - 0.6f * Projectile.direction;
            return IntersectsConeFastInaccurate(targetHitbox, Projectile.Center, coneLength, coneRotation, maximumAngle) && Projectile.localAI[0] > 1;
        }

        public static bool IntersectsConeFastInaccurate(Rectangle targetRect, Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle)
        {
            Vector2 point = coneCenter + coneRotation.ToRotationVector2() * coneLength;
            Vector2 spinningpoint = targetRect.ClosestPointInRect(point) - coneCenter;
            float num = spinningpoint.RotatedBy(0f - coneRotation).ToRotation();
            if (num < 0f - maximumAngle || num > maximumAngle)
            {
                return false;
            }
            return spinningpoint.Length() < coneLength;
        }

        protected float scaleIncrease = 0f;
        protected Color frontColor = Color.White;
        protected Color middleColor = Color.White;
        protected Color backColor = Color.White;
        protected bool once = false;

        public override bool PreDraw(ref Color lightColor)
        {
            // Color[] palette = new Color[] { new Color(204, 204, 0), Color.Yellow, Color.White };  KEEP THIS FOR ADA SWORD

            Color[] palette = new Color[] { frontColor, middleColor, backColor };
            SwordAura.DrawProj_BladeAura(Projectile, palette);
            return false;
        }

        public void SwingAI()
        {
            //if (Projectile.localAI[0] == 0f)
            //{
            //    SoundEngine.PlaySound(SoundID.Item60 with { Volume = 0.65f }, Projectile.position);
            //}
            Projectile.localAI[0] += 1f;
            Player player = Main.player[Projectile.owner];
            float progress = Projectile.localAI[0] / Projectile.ai[1];
            float whichSide = Projectile.ai[0];
            float velToRot = Projectile.velocity.ToRotation();
            float realRotation = MathF.PI * whichSide * progress + velToRot + whichSide * MathF.PI + player.fullRotation;
            Projectile.rotation = realRotation;
            float baseScale = 1f;

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = (baseScale + progress * scaleIncrease) * Projectile.ai[2];
            float rotationWithDeviation = Projectile.rotation + Main.rand.NextFloatDirection() * (MathF.PI / 2f) * 0.7f;
            Vector2 edgePosition = Projectile.Center + rotationWithDeviation.ToRotationVector2() * 85f * Projectile.scale;
            Vector2 vector8 = (rotationWithDeviation + Projectile.ai[0] * (MathF.PI / 2f)).ToRotationVector2();
            Lighting.AddLight(Projectile.Center, middleColor.ToVector3());
            Projectile.scale *= 1;
            if (progress > 1)
            {
                Projectile.Kill();
            }
        }
        public static void DrawProj_BladeAura(Projectile proj, Color[] colorArray)
        {
            Vector2 auraDrawPos = proj.Center - screenPosition;
            Asset<Texture2D> texture = TextureAssets.Projectile[proj.type];
            Rectangle frame = texture.Frame(1, 4);
            Vector2 origin = frame.Size() / 2f;
            float auraScale = proj.scale * 1.1f;
            SpriteEffects effects = ((!(proj.ai[0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None);
            float progress = proj.localAI[0] / proj.ai[1];
            float remappedProgress = Utils.Remap(progress, 0f, 0.6f, 0f, 1f) * Utils.Remap(progress, 0.6f, 1f, 1f, 0f);
            //remappedProgress = progress;
            //Main.NewText($"{remappedProgress}, {Utils.Remap(progress, 0f, 0.8f, 0f, 1f)}, {Utils.Remap(progress, 0.8f, 1f, 1f, 0f)}");
            float num4 = 0.975f;
            float lightingMultiplier = Lighting.GetColor(proj.Center.ToTileCoordinates()).ToVector3().Length() / MathF.Sqrt(3);
            lightingMultiplier = 0.5f + lightingMultiplier * 0.5f;
            lightingMultiplier = Utils.Remap(lightingMultiplier, 0.2f, 1f, 0f, 1f);
            Color blue = colorArray[2];//new Color(45, 124, 205);//blue
            Color lime = colorArray[0];//new Color(181, 230, 29);//yellowlime
            Color green = colorArray[1];//new Color(34, 177, 76);//green
            Color whiteOverlay = Color.White * remappedProgress * 0.5f;
            whiteOverlay.A = (byte)((float)(int)whiteOverlay.A * (1f - lightingMultiplier));
            Color color5 = whiteOverlay * lightingMultiplier * 0.5f;
            color5.G = (byte)((float)(int)color5.G * lightingMultiplier);
            color5.B = (byte)((float)(int)color5.R * (0.25f + lightingMultiplier * 0.75f));
            DrawAuraLayers_WTFIsThis(proj, auraDrawPos, texture, frame, origin, auraScale, effects, progress, remappedProgress, num4, lightingMultiplier, blue, lime, green, color5);
            for (float num6 = 0f; num6 < 12f; num6 += 1f)
            {
                float num7 = proj.rotation + proj.ai[0] * (num6 - 2f) * (MathF.PI * -2f) * 0.025f + Utils.Remap(progress, 0f, 1f, 0f, MathF.PI / 4f) * proj.ai[0];
                Vector2 drawpos = auraDrawPos + num7.ToRotationVector2() * ((float)texture.Width() * 0.5f - 6f) * auraScale;
                float num8 = num6 / 12f;
                DrawPrettyStarSparkle(proj.Opacity, SpriteEffects.None, drawpos, new Color(255, 255, 255, 0) * remappedProgress * num8, green, progress, 0f, 0.5f, 0.5f, 1f, num7, new Vector2(0f, Utils.Remap(progress, 0f, 1f, 3f, 0f)) * auraScale, Vector2.One * auraScale);
            }
            Vector2 drawpos2 = auraDrawPos + (proj.rotation + Utils.Remap(progress, 0f, 1f, 0f, MathF.PI / 4f) * proj.ai[0]).ToRotationVector2() * ((float)texture.Width() * 0.5f - 4f) * auraScale;
            DrawPrettyStarSparkle(proj.Opacity, SpriteEffects.None, drawpos2, new Color(255, 255, 255, 0) * remappedProgress * 0.5f, green, progress, 0f, 0.5f, 0.5f, 1f, 0f, new Vector2(2f, Utils.Remap(progress, 0f, 1f, 4f, 1f)) * auraScale, Vector2.One * auraScale * 1.5f);
        }

        public static void DrawAuraLayers_WTFIsThis(Projectile proj, Vector2 drawPos, Asset<Texture2D> texture, Rectangle frame, Vector2 origin, float scale, SpriteEffects effects, float progress, float remappedProgress, float num4, float lightingMultiplier, Color blue, Color lime, Color green, Color color5)
        {

            if(proj.type == MeowmereAura.ID)//method is not instance, so gotta do this to kinda act like an override.
            {   
                MeowmereAuraEffectDrawer.DrawBladeAura(spriteBatch, proj, frame, progress, effects, proj.rotation, origin, scale, drawPos, texture.Value, remappedProgress);
                return;
            }


            spriteBatch.Draw(texture.Value, drawPos, frame, blue * lightingMultiplier * remappedProgress, proj.rotation + proj.ai[0] * (MathF.PI / 4f) * -1f * (1f - progress), origin, scale * 0.95f, effects, 0f);
            spriteBatch.Draw(texture.Value, drawPos, frame, color5 * 0.15f, proj.rotation + proj.ai[0] * 0.01f, origin, scale, effects, 0f);
            spriteBatch.Draw(texture.Value, drawPos, frame, green * lightingMultiplier * remappedProgress * 0.3f, proj.rotation, origin, scale, effects, 0f);
            spriteBatch.Draw(texture.Value, drawPos, frame, lime * lightingMultiplier * remappedProgress * 0.5f, proj.rotation, origin, scale * num4, effects, 0f);
            spriteBatch.Draw(texture.Value, drawPos, texture.Frame(1, 4, 0, 3), Color.White * 0.6f * remappedProgress, proj.rotation + proj.ai[0] * 0.01f, origin, scale, effects, 0f);
            spriteBatch.Draw(texture.Value, drawPos, texture.Frame(1, 4, 0, 3), Color.White * 0.5f * remappedProgress, proj.rotation + proj.ai[0] * -0.05f, origin, scale * 0.8f, effects, 0f);
            spriteBatch.Draw(texture.Value, drawPos, texture.Frame(1, 4, 0, 3), Color.White * 0.4f * remappedProgress, proj.rotation + proj.ai[0] * -0.1f, origin, scale * 0.6f, effects, 0f);
        }

        public static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawpos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D texture = TextureAssets.Extra[98].Value;
            Color bigShineColor = shineColor * opacity * 0.5f;
            bigShineColor.A = 0;
            Vector2 origin = texture.Size() / 2f;
            Color smallShineColor = drawColor * 0.5f;
            float brightness = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 vector = new Vector2(fatness.X * 0.5f, scale.X) * brightness;
            Vector2 vector2 = new Vector2(fatness.Y * 0.5f, scale.Y) * brightness;
            bigShineColor *= brightness;
            smallShineColor *= brightness;
            Main.EntitySpriteDraw(texture, drawpos, null, bigShineColor, MathHelper.PiOver2 + rotation, origin, vector, dir, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, bigShineColor, rotation, origin, vector2, dir, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, smallShineColor, MathHelper.PiOver2 + rotation, origin, vector * 0.6f, dir, 0);
            Main.EntitySpriteDraw(texture, drawpos, null, smallShineColor, rotation, origin, vector2 * 0.6f, dir, 0);
        }
    }
    public class KeybrandAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.6f;
            frontColor = new Color(204, 204, 0);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Vector2 positionInWorld = target.Hitbox.ClosestPointInRect(Projectile.Center);
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings
            {
                PositionInWorld = positionInWorld
            }, Projectile.owner);
            float lifepercent = (float)(target.life) / (float)(target.lifeMax);
            if (lifepercent > 0.1f)
                modifiers.FinalDamage *= 1 + 5/3 * (float)(1 - lifepercent);
            else
                modifiers.FinalDamage *= 2.5f;

        }

    }
    public class BeekeeperAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.25f;
            frontColor = Color.Yellow;
            middleColor = Color.LightGoldenrodYellow;
            backColor = Color.Black;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!once)
            {
                int num3 = Main.rand.Next(1, 4);
                for (int j = 0; j < num3; j++)
                {
                    float num4 = (float)(hit.HitDirection * 2) + (float)Main.rand.Next(-35, 36) * 0.02f;
                    float num5 = (float)Main.rand.Next(-35, 36) * 0.02f;
                    num4 *= 0.2f;
                    num5 *= 0.2f;
                    int num6 = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, new Vector2(num4, num5), ProjectileID.Bee, damageDone / 3, 0, Projectile.owner);
                }
                once = true;

            }
        }
    }
    public class BreakerAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.9f;
            frontColor = Color.LightGreen;
            middleColor = Color.DarkGray;
            backColor = Color.DarkGreen;
            Projectile.hide = true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.life > (int)(target.lifeMax * 0.9f))
                modifiers.FinalDamage *= 2.5f;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
    public class PearlwoodAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.33f;
            frontColor = Color.LightGoldenrodYellow;
            middleColor = Color.Yellow;
            backColor = new Color(192, 176, 138);
        }
    }
    public class ShortAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = -0.6f;
            frontColor = Color.IndianRed;
            middleColor = Color.IndianRed;
            backColor = Color.IndianRed;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (player.GetModPlayer<OnHitEffects>().BaghnakhHeal <= (int)(player.GetModPlayer<OnHitEffects>().LastHitDamage * 0.35))
            {

                float healAmount = (float)(damageDone * 0.05f);
                if (healAmount < 1)
                    healAmount = 1;
                player.GetModPlayer<OnHitEffects>().BaghnakhHeal += (int)healAmount;
                player.Heal((int)healAmount);
            }
            player.stealth = 1;
        }
    }
    public class CobaltAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.1f;
            frontColor = Color.Blue;
            middleColor = Color.LightBlue;
            backColor = Color.LightCyan;
        }
    }
    public class PallaAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.1f;
            frontColor = Color.Orange;
            middleColor = Color.Orange;
            backColor = Color.Orange;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            player[Projectile.owner].AddBuff(BuffID.RapidHealing, 300);
        }
    }
    public class MythrilAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.1f;
            frontColor = Color.DarkGreen;
            middleColor = Color.DarkGreen;
            backColor = Color.PaleGreen;
        }


        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.CritDamage += 0.25f;
        }
    }
    public class OrichalcumAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.1f;
            frontColor = Color.HotPink;
            middleColor = Color.HotPink;
            backColor = Color.LightPink;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!once)
            {

                once = true;
                Player player = Main.player[Projectile.owner];
                int direction = player.direction;
                float k = Main.screenPosition.X;
                if (direction < 0)
                {
                    k += (float)Main.screenWidth;
                }
                float y2 = Main.screenPosition.Y;
                y2 += (float)Main.rand.Next(Main.screenHeight);
                Vector2 vector = new Vector2(k, y2);
                float num2 = target.Center.X - vector.X;
                float num3 = target.Center.Y - vector.Y;
                num2 += (float)Main.rand.Next(-50, 51) * 0.1f;
                num3 += (float)Main.rand.Next(-50, 51) * 0.1f;
                float num4 = MathF.Sqrt(num2 * num2 + num3 * num3);
                num4 = 24f / num4;
                num2 *= num4;
                num3 *= num4;
                Projectile.NewProjectile(player.GetSource_FromThis(), k, y2, num2, num3, 221, 36, 0f, player.whoAmI);
            }
        }
    }
    public class AdamantiteAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.33f;
            frontColor = Color.Red;
            middleColor = Color.IndianRed;
            backColor = Color.White;
        }

    }
    public class TitaniumAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.1f;
            frontColor = Color.Gray;
            middleColor = Color.LightGray;
            backColor = Color.White;
        }

    }
    public class SaberAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.4f;
            frontColor = Color.LimeGreen;
            middleColor = Color.Black;
            backColor = Color.DarkGreen;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, TRAEMethods.PolarVector(Main.rand.NextFloat() * 2f, Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI)), ProjectileID.SporeCloud, (int)(.5f * damageDone), 0, Projectile.owner);
        }

    }
    public class ClaymoreAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 1f;
            frontColor = Color.LimeGreen;
            middleColor = Color.Black;
            backColor = Color.DarkGreen;
        }

    }
    public class BrandAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.33f; frontColor = Color.MediumVioletRed;
            middleColor = Color.MediumVioletRed;
            backColor = Color.Orange;
        }

    }
    public class DrgngnAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.33f;
            frontColor = Color.Red;
            middleColor = Color.White;
            backColor = Color.Red;
        }
    }
    public class InfluxAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0f; 
            frontColor = Color.LightCyan;
            middleColor = Color.LightCyan;
            backColor = Color.LightCyan;
        }

    }
    public class StarWrathAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = 0.75f;
            frontColor = Color.Red;
            middleColor = Color.DarkOrange;
            backColor = new Color(251, 191, 119);
        }

    }
    public class MeowmereAura : SwordAura
    {
        public static int ID => ModContent.ProjectileType<MeowmereAura>();

        public override void AuraDefaults()
        {
            scaleIncrease = 0.65f;
            frontColor = Main.DiscoColor;
            middleColor = Main.DiscoColor;
            backColor = Main.DiscoColor;
        }

    }
    public class EnchantedAura : SwordAura
    {
        public override void AuraDefaults()
        {
            scaleIncrease = -0.2f;
            frontColor = Color.White;
            middleColor = Color.LightBlue;
            backColor = Color.LightBlue;
        }

    }
}

