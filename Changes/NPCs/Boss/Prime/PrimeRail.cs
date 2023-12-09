using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    public class PrimeRail : ModNPC
    {
        const float shootVel = 8f;
        public const int railExtraUpdates = 4;
        const int chargeTime = 600;
        const int warnTime = 180;
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
            NPC.damage = 29;
            NPC.defense = 20;
            NPC.lifeMax = 8000;
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

            float side = 1;
            Vector2 vector7 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f - 5f * side, NPC.position.Y + 20f);
            for (int k = 0; k < 2; k++) 
            {
                float num22 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - vector7.X;
                float num23 = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - vector7.Y;
                float num24 = 0f;
                if (k == 0) 
                {
                    num22 -= 200f * side;
                    num23 += 130f;
                    num24 = (float)Math.Sqrt(num22 * num22 + num23 * num23);
                    num24 = 92f / num24;
                    vector7.X += num22 * num24;
                    vector7.Y += num23 * num24;
                }
                else 
                {
                    num22 -= 50f * side;
                    num23 += 80f;
                    num24 = (float)Math.Sqrt(num22 * num22 + num23 * num23);
                    num24 = 60f / num24;
                    vector7.X += num22 * num24;
                    vector7.Y += num23 * num24;
                }

                float rotation7 = (float)Math.Atan2(num23, num22) - 1.57f;
                Color color7 = Lighting.GetColor((int)vector7.X / 16, (int)(vector7.Y / 16f));
                spriteBatch.Draw(TextureAssets.BoneArm2.Value, new Vector2(vector7.X - screenPos.X, vector7.Y - screenPos.Y), new Rectangle(0, 0, TextureAssets.BoneArm.Width(), TextureAssets.BoneArm.Height()), color7, rotation7, new Vector2((float)TextureAssets.BoneArm.Width() * 0.5f, (float)TextureAssets.BoneArm.Height() * 0.5f), 1f, SpriteEffects.None, 0f);
                if (k == 0) 
                {
                    vector7.X += num22 * num24 / 2f;
                    vector7.Y += num23 * num24 / 2f;
                }
                else if (!Main.gamePaused) 
                {
                    vector7.X += num22 * num24 - 16f;
                    vector7.Y += num23 * num24 - 6f;
                    int num25 = Dust.NewDust(new Vector2(vector7.X, vector7.Y), 30, 10, DustID.Torch, num22 * 0.02f, num23 * 0.02f, 0, default(Microsoft.Xna.Framework.Color), 2.5f);
                    Main.dust[num25].noGravity = true;
                }
            }

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
            if(timer > chargeTime - warnTime)
            {
                DrawLaser(spriteBatch, NPC.Center + TRAEMethods.PolarVector(30, NPC.rotation), NPC.rotation, MathF.Max(0.1f, 1f - (float)(chargeTime - timer) / warnTime), 3000);
            }
            
            return false;
        }
        public override void AI()
        {
            NPC.spriteDirection = -1;
            if (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].aiStyle != 32 || (NPC.ai[0] == 0 && !SkeletronPrime.KeepPhase2Arms(Main.npc[(int)NPC.ai[1]])))
            {
                NPC.ai[2] += 10f;
                if (NPC.ai[2] > 50f || Main.netMode != NetmodeID.Server) 
                {
                    NPC.life = -1;
                    NPC.HitEffect();
                    NPC.active = false;
                }
            }
            if (Main.npc[(int)NPC.ai[1]].ai[1] == 3f)
                    NPC.EncourageDespawn(10);

            float yOffset = -100;
            float xOffset = -180;
            if(NPC.ai[0] != 0)
            {
                yOffset = 0;
                xOffset = -306;
            }
            if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y + yOffset) 
            {
                if (NPC.velocity.Y > 0f)
                    NPC.velocity.Y *= 0.96f;

                NPC.velocity.Y -= 0.1f;
                if (NPC.velocity.Y > 3f)
                    NPC.velocity.Y = 3f;
            }
            else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y + yOffset) 
            {
                if (NPC.velocity.Y < 0f)
                    NPC.velocity.Y *= 0.96f;

                NPC.velocity.Y += 0.1f;
                if (NPC.velocity.Y < -3f)
                    NPC.velocity.Y = -3f;
            }

            if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) + xOffset) 
            {
                if (NPC.velocity.X > 0f)
                    NPC.velocity.X *= 0.96f;

                NPC.velocity.X -= 0.14f;
                if (NPC.velocity.X > 8f)
                    NPC.velocity.X = 8f;
            }

            if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) + xOffset) 
            {
                if (NPC.velocity.X < 0f)
                    NPC.velocity.X *= 0.96f;

                NPC.velocity.X += 0.14f;
                if (NPC.velocity.X < -8f)
                    NPC.velocity.X = -8f;
            }

            NPC.TargetClosest(false);
            float aimToward = TRAEMethods.PredictiveAimWithOffset(NPC.Center, shootVel * (1 + railExtraUpdates), Main.player[NPC.target].Center, Main.player[NPC.target].velocity, 30);
            if(float.IsNaN(aimToward))
            {
                aimToward = (Main.player[NPC.target].Center - NPC.Center).ToRotation();
            }
            NPC.rotation.SlowRotation(aimToward, MathF.PI / 120f);
            timer++;
            if(timer >= chargeTime)
            {
                timer = 0;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + TRAEMethods.PolarVector(30, NPC.rotation), TRAEMethods.PolarVector(shootVel, NPC.rotation), ModContent.ProjectileType<RailShot>(), 80, 0, Main.myPlayer);
                    DeathRailShootDust(TRAEMethods.PolarVector(shootVel * 1.4f, NPC.rotation), NPC.Center + TRAEMethods.PolarVector(30, NPC.rotation));
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
            Vector2 sparkleScale = new(1.5f ,3f);
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
            spriteBatch.Draw(blankTexture, segPos, new Rectangle(0, 0, 1, 1), Color.Red * opacity  * 0.75f, dir, new Vector2(0, 0.5f), texScale, SpriteEffects.None, 0);
        }
    }
    public class RailShot : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.DeathLaser;
            Projectile.hostile = true;
            Projectile.penetrate = 3;
            Projectile.light = 0.75f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = PrimeRail.railExtraUpdates;
            Projectile.scale = 1.8f;
            Projectile.timeLeft = 2700;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return base.PreDraw(ref lightColor);
        }
    }
}