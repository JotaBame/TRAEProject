using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    public class PrimeRail : ModNPC
    {
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            database.Entries.Remove(bestiaryEntry);
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossAdjustment * balance);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                PrimeStats.ArmGore(NPC);
            }
        }
        int timer = 0;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPC.type] = 6;
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 52;
            NPC.damage = 30;
            NPC.defense = 24;
            NPC.lifeMax = PrimeStats.railHealth;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.netAlways = true;
            NPC.aiStyle = -1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            PrimeStats.RenderBones(NPC, spriteBatch, screenPos, 1);

            drawColor = NPC.GetNPCColorTintedByBuffs(drawColor);
            for (int num93 = 9; num93 >= 0; num93 -= 2)
            {
                Color alpha9 = NPC.GetAlpha(drawColor);
                alpha9.R = (byte)(alpha9.R * (10 - num93) / 20);
                alpha9.G = (byte)(alpha9.G * (10 - num93) / 20);
                alpha9.B = (byte)(alpha9.B * (10 - num93) / 20);
                alpha9.A = (byte)(alpha9.A * (10 - num93) / 20);
                spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value,
                NPC.oldPos[num93] + NPC.Size * 0.5f - screenPos,
                NPC.frame, alpha9, NPC.rotation, new Vector2(20f, 20f), NPC.scale, spriteEffects, 0f);
            }
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos,
                        NPC.frame, drawColor, NPC.rotation,
                        new Vector2(20f, 20f), 1f, spriteEffects, 0f);
            spriteBatch.Draw(ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Prime/PrimeRail_Glow").Value, NPC.Center - screenPos,
                        NPC.frame, Color.White, NPC.rotation,
                        new Vector2(20f, 20f), 1f, spriteEffects, 0f);
            if (timer > PrimeStats.railChargeTime - PrimeStats.railWarnTime)
            {
                DrawLaser(spriteBatch, NPC.Center + TRAEMethods.PolarVector(30, NPC.rotation), NPC.rotation, MathF.Max(0.1f, 1f - (float)(PrimeStats.railChargeTime - timer) / PrimeStats.railWarnTime), 3000);
            }

            return false;
        }
        float aimToward = 0;
        
        public override void AI()
        { 
            NPC.damage = 0;
            NPC.spriteDirection = -1;
            NPC prime = Main.npc[(int)NPC.ai[1]];
            if (!prime.active || prime.aiStyle != 32 || (NPC.ai[0] == 0 && !SkeletronPrime.KeepPhase2Arms(prime)))
            {
                NPC.ai[2] += 10f;
                if (NPC.ai[2] > 50f || Main.netMode != NetmodeID.Server)
                {
                    NPC.life = -1;
                    NPC.HitEffect();
                    NPC.active = false;
                }
            }
            if (prime.ai[1] == 3f)
                NPC.EncourageDespawn(10);

            float yOffset = -100;
            float xOffset = -180;
            if (NPC.ai[0] != 0)
            {
                yOffset = 0;
                xOffset = -306;
            }
            if (NPC.position.Y > prime.position.Y + yOffset)
            {
                if (NPC.velocity.Y > 0f)
                    NPC.velocity.Y *= 0.96f;

                NPC.velocity.Y -= 0.1f;
                if (NPC.velocity.Y > 3f)
                    NPC.velocity.Y = 3f;
            }
            else if (NPC.position.Y < prime.position.Y + yOffset)
            {
                if (NPC.velocity.Y < 0f)
                    NPC.velocity.Y *= 0.96f;

                NPC.velocity.Y += 0.1f;
                if (NPC.velocity.Y < -3f)
                    NPC.velocity.Y = -3f;
            }

            if (NPC.position.X + NPC.width / 2 > prime.position.X + prime.width / 2 + xOffset)
            {
                if (NPC.velocity.X > 0f)
                    NPC.velocity.X *= 0.96f;

                NPC.velocity.X -= 0.14f;
                if (NPC.velocity.X > 8f)
                    NPC.velocity.X = 8f;
            }

            if (NPC.position.X + NPC.width / 2 < prime.position.X + prime.width / 2 + xOffset)
            {
                if (NPC.velocity.X < 0f)
                    NPC.velocity.X *= 0.96f;

                NPC.velocity.X += 0.14f;
                if (NPC.velocity.X < -8f)
                    NPC.velocity.X = -8f;
            }
            float railVel = PrimeStats.railVel;
            bool predictiveAim = PrimeStats.railUsesPredictiveAim;
            float stopAimingTime = PrimeStats.stopAimingTime;
            int railExtraUpdates = PrimeStats.railExtraUpdates;
            bool holdFireWhenRaged = true;
            float rotSpeed = (100 / 60f) * (180f / MathF.PI);
            
            NPC.TargetClosest(false);
            if (timer > PrimeStats.railChargeTime - PrimeStats.railWarnTime)
            {
                int num = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.TheDestroyer, 0f, 0f, 100, default, 1f);
                Main.dust[num].noGravity = true;
                Main.dust[num].noLight = true;
            }
            if (timer <= PrimeStats.railChargeTime  - stopAimingTime)
            {
                aimToward = TRAEMethods.PredictiveAimWithOffset(NPC.Center, railVel * (1 + railExtraUpdates), Main.player[NPC.target].Center + Main.player[NPC.target].velocity * stopAimingTime, Main.player[NPC.target].velocity, 30);
                if (float.IsNaN(aimToward) || !predictiveAim)
                {
                    aimToward = (Main.player[NPC.target].Center + Main.player[NPC.target].velocity * stopAimingTime - NPC.Center).ToRotation();
                }
            }
            else if(holdFireWhenRaged)
            {
                NPC.velocity = Vector2.Zero;
            }


            timer++;
            if(holdFireWhenRaged && timer >= PrimeStats.railChargeTime - PrimeStats.railWarnTime && prime.ai[1] != 0f)
            {
                timer = PrimeStats.railChargeTime - PrimeStats.railWarnTime;
            }
            NPC.rotation.SlowRotation(aimToward, rotSpeed);


            if (timer >= PrimeStats.railChargeTime)
            {
                timer = 0;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + TRAEMethods.PolarVector(30, NPC.rotation), TRAEMethods.PolarVector(railVel, NPC.rotation), ModContent.ProjectileType<RailShot>(), PrimeStats.railDamage, 0, Main.myPlayer);
                    DeathRailShootDust(TRAEMethods.PolarVector(railVel * 1.4f, NPC.rotation), NPC.Center + TRAEMethods.PolarVector(30, NPC.rotation));
                }
            }
        }
        public static void DeathRailShootDust(Vector2 shootVelocity, Vector2 origin)
        {
            shootVelocity.Normalize();
            int dustAmount = 24;//change to what you want, this is the line
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 dustVel = shootVelocity * Utils.Remap(i, 0, dustAmount, 2, 10);//arbitrary numbers, change 2 ad 10 to your liking
                float scale = Utils.Remap(i, 0, dustAmount, 2, 1);
                GlowyRedDust(origin, dustVel, scale);
            }
            dustAmount = 10;//change to what you want, this is the ring
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 offset = Utils.Remap(i, 0, dustAmount, 0, MathF.Tau).ToRotationVector2();
                offset.X *= .5f;
                offset = offset.RotatedBy(shootVelocity.ToRotation());
                GlowyRedDust(origin + offset, offset * 2, 2);
            }
            Vector2 sparkleScale = new(1.5f, 3f);
            for (int i = -1; i < 2; i += 2)
            {
                Sparkle.NewSparkle(origin, Color.Red, sparkleScale, shootVelocity.RotatedBy(MathF.PI / 2) * i * 5, 20, sparkleScale, rotation: shootVelocity.ToRotation(), friction: .9f);
                Sparkle.NewSparkle(origin, Color.Red, sparkleScale, shootVelocity.RotatedBy(MathF.PI / 2) * i * 5, 20, sparkleScale, rotation: shootVelocity.ToRotation(), friction: .9f);
            }
        }
        static void GlowyRedDust(Vector2 pos, Vector2 vel, float scale)
        {
            Dust dust = Dust.NewDustPerfect(pos, DustID.TheDestroyer, vel, 0, Color.White with { A = 0 }, scale);
            dust.noGravity = true;
        }
        public static void DrawLaser(SpriteBatch spriteBatch, Vector2 pos, float dir, float opacity, float length)
        {
            Vector2 segPos = pos - Main.screenPosition;
            Texture2D blankTexture = TextureAssets.Extra[178].Value;
            Vector2 texScale = new Vector2(length, 10 * opacity);
            Color col = Color.Red with { A = 0 };
            spriteBatch.Draw(blankTexture, segPos, new Rectangle(0, 0, 1, 1), col * opacity * 0.75f, dir, new Vector2(0, 0.5f), texScale, SpriteEffects.None, 0);
            texScale.Y *= .35f;
            col = Color.White with { A = 0 };
            spriteBatch.Draw(blankTexture, segPos, new Rectangle(0, 0, 1, 1), col * opacity * 0.75f, dir, new Vector2(0, 0.5f), texScale, SpriteEffects.None, 0);
        }
    }
    public class RailShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20 * (PrimeStats.railExtraUpdates + 1);
            ProjectileID.Sets.TrailingMode[Type] = 0;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;//renders further away(trail wont get cut off)
        }
        public override void SetDefaults()
        {

            Projectile.extraUpdates = PrimeStats.railExtraUpdates;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = 1;
            Projectile.tileCollide = false;//so it doesn't awkwardly disappear
            AIType = ProjectileID.DeathLaser;
            Projectile.hostile = true;
            Projectile.penetrate = 3;
            Projectile.light = 0.75f;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.timeLeft = 2400;
        }

        public override bool PreAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() - MathF.PI * .5f;
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[0] += 1f;
 
                SoundEngine.PlaySound(SoundID.Item67 with { Pitch = -1f, Volume = 2 }, Projectile.Center);
                //SoundEngine.PlaySound(SoundID.Item158 with { Pitch = 1, Volume = 2 }, Projectile.Center);
                //SoundEngine.PlaySound(SoundID.Item158 with { Pitch = -1, Volume = 2 }, Projectile.Center);

                return false;
            }
            if (Projectile.localAI[2] > 0)
            {
                Projectile.damage = -1;
                Projectile.localAI[2]++;
                if (Projectile.localAI[2] >= 10 * Projectile.MaxUpdates)
                {
                    Projectile.Kill();
                }
            }
            else if (Collision.SolidTiles(Projectile.position, 1, 1))
            {
                Projectile.localAI[2]++;
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 8f)
            {
                //for (int i = 0; i < 2; i++)
                //{
                //    int type = DustID.TheDestroyer;
                //    int Dust = Terraria.Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, type, 0f, 0f, 100);
                //    Main.dust[Dust].position = (Main.dust[Dust].position + Projectile.Center) / 2f;
                //    Main.dust[Dust].noGravity = true;
                //    Dust dust = Main.dust[Dust];
                //    dust.velocity *= 0.1f;
                //    if (i == 1)
                //    {
                //        dust = Main.dust[Dust];
                //        dust.position += Projectile.velocity / 2f;
                //    }
                //    float ScaleMult = (1000f - Projectile.ai[0]) / 500f;
                //    dust = Main.dust[Dust];
                //    dust.scale *= ScaleMult + 0.1f;
                //}
            }
            return false;
        }
        static float Easing(float progress)
        {
            return .5f - MathF.Cos(progress * MathF.PI) * .5f;
        }
        static float ExpoEasing(float progress, float exponent = 2)
        {
            return MathF.Pow(progress, exponent);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float fade = Utils.GetLerpValue(Projectile.MaxUpdates * 10, 0, Projectile.localAI[2], true);
            Projectile.scale = 1.5f;
            Texture2D tex = TextureAssets.Extra[98].Value;
            Vector2 origin = new Vector2(35.5f, 56.5f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 offset = Projectile.velocity.SafeNormalize(Vector2.UnitX) * 14;//just what looks good
            //Draw(drawPos - offset * .35f, 1, Projectile.rotation, tex.Size() / 2, new Vector2(Projectile.scale * 1.5f, Projectile.scale));
            for (int i = -1; i < 2; i += 2)
            {
                for (float j = 0; j < .99f; j += .334f)
                {
                    float animProgress = (float)(Main.timeForVisualEffects / 20f + j) % 1;
                    float opacity = Utils.GetLerpValue(0, .3f, animProgress, true) * Utils.GetLerpValue(1, .75f, animProgress, true);
                    opacity = Easing(opacity);
                    animProgress *= animProgress;
                    float rotationOffset = MathHelper.Lerp(i, 0, animProgress);
                    float length = MathHelper.Lerp(1f, 3, animProgress);
                    Vector2 scale = new Vector2(Projectile.scale * 1.2f, Projectile.scale * length);
                    Draw(drawPos + offset, opacity * fade, rotationOffset + Projectile.rotation, origin, scale);
                }
            }
            for (int i = 0; i < Projectile.oldPos.Length; i += 2)
            {
                Vector2 pos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
                float opacity = Utils.GetLerpValue(Projectile.oldPos.Length, 0, i);

                Draw(pos, opacity * fade, Projectile.rotation, tex.Size() / 2, Vector2.One);
            }
            Projectile.scale = 1;
            lightColor = Color.White;
            return false;
        }
        static void Draw(Vector2 drawPos, float opacity, float rotation, Vector2 origin, Vector2 scale)
        {
            Texture2D tex = TextureAssets.Extra[98].Value;
            Color col = new Color(255, 0, 0, 0);
            Main.EntitySpriteDraw(tex, drawPos, null, col * opacity, rotation, origin, scale, SpriteEffects.None);
            scale.X *= .5f;
            scale.Y *= .8f;
            Main.EntitySpriteDraw(tex, drawPos, null, new Color(255, 255, 255, 0) * opacity, rotation, origin, scale, SpriteEffects.None);
        }
    }
}