using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.FlamethrowerAmmo;

namespace TRAEProject.Changes.NPCs.Boss.TwinsChanges
{
    public static class SpazPhase3
    {
        const float flameTime = 400f;
        public static int CauldronFireRate => Main.masterMode ? 13 : 13;
        public static int CauldronDuration => 280;
        public static int ChargeCount => Main.masterMode ? 3 : 3;
        public static int ChargeDuration => 60;
        public static int ChargeDecelerateStart => ChargeDuration / 2;
        public static void Header(NPC npc)
        {
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest();
            }
            bool playerDead = Main.player[npc.target].dead;
            float angVel = MathF.PI / 240f + MathF.PI / 180f * (1f - npc.ai[2] / flameTime) * (1f - npc.ai[2] / flameTime);
            npc.rotation.SlowRotation((Main.player[npc.target].Center - npc.Center).ToRotation() - MathF.PI / 2, angVel);

            if (Main.rand.NextBool(5))
            {
                int index = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + npc.height * 0.25f), npc.width, (int)(npc.height * 0.5f), 5, npc.velocity.X, 2f);
                Main.dust[index].velocity.X *= 0.5f;
                Main.dust[index].velocity.Y *= 0.1f;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient && !Main.dayTime && !playerDead && npc.timeLeft < 10)
            {
                for (int i = 0; i < 200; i++)
                {
                    if (i != npc.whoAmI && Main.npc[i].active && (Main.npc[i].type == NPCID.Retinazer || Main.npc[i].type == NPCID.Spazmatism))
                    {
                        npc.DiscourageDespawn(Main.npc[i].timeLeft - 1);
                    }
                }
            }
            npc.reflectsProjectiles = false;
            if (Main.dayTime || playerDead)
            {
                npc.velocity.Y -= 0.04f;
                npc.EncourageDespawn(10);
                return;
            }
        }
        public static void Flame(NPC npc)
        {
            Header(npc);
            float maxSpeed = 8f;
            float distModifier = MathF.Min(1000f, (Main.player[npc.target].Center - npc.Center).Length()) / 1000f;
            float speed = maxSpeed - maxSpeed * (npc.ai[2] / flameTime);
            speed = distModifier * (maxSpeed - speed) + speed;
            float acceleration = 0.4f - 0.3f * (npc.ai[2] / flameTime);
            acceleration = distModifier * (0.4f - acceleration) + acceleration;
            int num480 = 1;
            if (npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width)
            {
                num480 = -1;
            }
            Vector2 vector46 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float num481 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 + num480 * 180 - vector46.X;
            float num482 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector46.Y;
            float num483 = MathF.Sqrt(num481 * num481 + num482 * num482);

            num483 = speed / num483;
            num481 *= num483;
            num482 *= num483;
            if (npc.velocity.X < num481)
            {
                npc.velocity.X += acceleration;
                if (npc.velocity.X < 0f && num481 > 0f)
                {
                    npc.velocity.X += acceleration;
                }
            }
            else if (npc.velocity.X > num481)
            {
                npc.velocity.X -= acceleration;
                if (npc.velocity.X > 0f && num481 < 0f)
                {
                    npc.velocity.X -= acceleration;
                }
            }
            if (npc.velocity.Y < num482)
            {
                npc.velocity.Y += acceleration;
                if (npc.velocity.Y < 0f && num482 > 0f)
                {
                    npc.velocity.Y += acceleration;
                }
            }
            else if (npc.velocity.Y > num482)
            {
                npc.velocity.Y -= acceleration;
                if (npc.velocity.Y > 0f && num482 < 0f)
                {
                    npc.velocity.Y -= acceleration;
                }
            }

            npc.ai[2] += 1f;
            if (npc.ai[2] >= flameTime)
            {
                npc.ai[1] = 1f;
                npc.ai[2] = 0f;
                npc.ai[3] = 0f;
                npc.target = 255;
                npc.netUpdate = true;
            }

            if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
            {
                float shootSpeed = GetFlameShootSpeed(npc);
                Vector2 shootFrom = npc.Center + TRAEMethods.PolarVector(32 - shootSpeed, npc.rotation + MathF.PI / 2);
                Vector2 vel = TRAEMethods.PolarVector(shootSpeed, npc.rotation + MathF.PI / 2);
                //GlowBall.NewGlowBall(shootFrom, Color.YellowGreen with { A = 0}, Vector2.One * .7f, Vector2.Normalize(vel).RotatedByRandom(.2f) * 16 + npc.velocity, 20, null, 1, .9f);
                npc.localAI[2] += 1f;
                if (npc.localAI[2] > 22f)
                {
                    npc.localAI[2] = 0f;
                    SoundEngine.PlaySound(SoundID.Item34 with { MaxInstances = 0 }, npc.position);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.localAI[1] += 6;
                    if (npc.localAI[1] > 8f)
                    {
                        npc.localAI[1] = 0f;
                        int dmg = npc.GetAttackDamage_ForProjectiles(30f, 27f);
                        Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), shootFrom, vel, ModContent.ProjectileType<SpamatizmFire>(), dmg, 0f, Main.myPlayer);
                    }
                }
            }
        }
        static float GetFlameShootSpeed(NPC npc)
        {
            //max multiplier is 5, min is 0
            float multiplier = 1 + 4f * (npc.ai[2] / flameTime);
            return 6f * multiplier;
        }
        public static void Update(NPC npc)
        {
            bool dead2 = Main.player[npc.target].dead;
            if (Main.netMode != 1 && !Main.dayTime && !dead2 && npc.timeLeft < 10)
            {
                for (int num403 = 0; num403 < 200; num403++)
                {
                    if (num403 != npc.whoAmI && Main.npc[num403].active && (Main.npc[num403].type == 125 || Main.npc[num403].type == 126))
                    {
                        npc.DiscourageDespawn(Main.npc[num403].timeLeft - 1);
                    }
                }
            }
            if (Main.dayTime || dead2)
            {
                npc.velocity.Y -= 0.04f;
                npc.EncourageDespawn(10);
                return;
            }
            npc.HitSound = SoundID.NPCHit4;
            npc.damage = (int)(npc.defDamage * 1.5);
            npc.defense = npc.defDefense + 8; // 18 defense, lower than phase 2 because doing ret first is taking longer every single time
            if (npc.ai[1] == 3)
            {
                Cauldron(npc);
            }
            else if (npc.ai[1] == 0)
            {
                Flame(npc);
            }
            else
            {
                Charge(npc);
            }
        }
        public static void Cauldron(NPC npc)
        {
            float angVel = MathF.PI / 12f;
            npc.rotation.SlowRotation(MathF.PI, angVel);
            float accY = 0.2f;
            float maxVelY = 5f;
            float accX = 0.2f;
            float maxVelX = 20f;

            npc.TargetClosest(false);
            Player player = Main.player[npc.target];

            npc.velocity.X += accX * Math.Sign(player.Center.X - npc.Center.X);
            if (Math.Abs(npc.velocity.X) > maxVelX)
            {
                npc.velocity.X = maxVelX * Math.Sign(player.Center.X - npc.Center.X);
            }

            npc.velocity.Y += accY * Math.Sign(player.Center.Y - npc.Center.Y);
            if (Math.Abs(npc.velocity.Y) > maxVelY)
            {
                npc.velocity.Y = maxVelY * Math.Sign(player.Center.Y - npc.Center.Y);
            }

            if (Math.Sign(npc.velocity.X) != Math.Sign(player.Center.X - npc.Center.X))
            {
                npc.velocity.X *= 0.98f;
            }
            npc.ai[2]++;
            int timer = CauldronFireRate; // make it 10 on master 
            if ((int)npc.ai[2] % timer == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int attackDamage_ForProjectiles7 = npc.GetAttackDamage_ForProjectiles(30f, 27f);
                Vector2 shootFrom = npc.Center + TRAEMethods.PolarVector(6, npc.rotation + MathF.PI / 2);
                Vector2 vel = TRAEMethods.PolarVector(12f + npc.velocity.Y, npc.rotation + MathF.PI / 2) + TRAEMethods.PolarVector(Main.rand.Next(-4, 5), npc.rotation);
                Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), shootFrom, vel, ModContent.ProjectileType<BouncingFlames>(), attackDamage_ForProjectiles7, 0f, Main.myPlayer);
            }
            if (npc.ai[2] >= CauldronDuration)
            {
                npc.ai[2] = 0;
                npc.ai[1] = 0;
            }


        }
        public static void Charge(NPC npc)
        {
            // Lighting.AddLight(npc.Center, new Vector3(0.8f));
            if (npc.ai[1] == 1f)
            {
                SoundEngine.PlaySound(SoundID.Roar with { MaxInstances = 0 }, npc.Center);
                //SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                npc.TargetClosest();
                npc.rotation = (Main.player[npc.target].Center - npc.Center).ToRotation() - MathF.PI / 2;
                float num489 = 24f;
                //if (Main.masterMode)
                //{
                //    num489 += 5f;
                //}
                Vector2 vector48 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float num490 = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 - vector48.X;
                float num491 = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - vector48.Y;
                float num492 = MathF.Sqrt(num490 * num490 + num491 * num491);
                num492 = num489 / num492;
                npc.velocity.X = num490 * num492;
                npc.velocity.Y = num491 * num492;
                npc.ai[1] = 2f;
            }
            else
            {
                if (npc.ai[1] != 2f)
                {
                    return;
                }
                npc.ai[2] += 1f;
                if (Main.masterMode)
                {
                    npc.ai[2] += 0.5f; // make it 0.5 on master
                }
                if (npc.ai[2] >= ChargeDecelerateStart)
                {
                    npc.velocity.X *= 0.93f;
                    npc.velocity.Y *= 0.93f;
                    if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                    {
                        npc.velocity.X = 0f;
                    }
                    if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
                    {
                        npc.velocity.Y = 0f;
                    }
                }
                else
                {
                    npc.rotation = MathF.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
                }
                if (npc.ai[2] >= ChargeDuration)
                {
                    npc.ai[3] += 1f;
                    npc.ai[2] = 0f;
                    npc.target = 255;
                    npc.rotation = (Main.player[npc.target].Center - npc.Center).ToRotation() - MathF.PI / 2;
                    if (npc.ai[3] >= ChargeCount) // five on master
                    {
                        npc.ai[1] = 3f;
                        npc.ai[3] = 0f;
                    }
                    else
                    {
                        npc.ai[1] = 1f;
                    }
                }
            }
        }
        public static void Phase3Draw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Texture2D texture = TextureAssets.Npc[npc.type].Value;
            Color color = npc.GetNPCColorTintedByBuffs(drawColor);
            Vector2 halfSize = new(55f, 107f);
            float num35 = 0f;
            float num36 = Main.NPCAddHeight(npc);
            Vector2 Pos = new(
                npc.position.X - screenPos.X + npc.width / 2 - TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale,
                npc.position.Y - screenPos.Y + npc.height - TextureAssets.Npc[npc.type].Height() * npc.scale / Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35);

            if (npc.ai[0] == 4f)
            {
                float prog = npc.ai[1] / 100f;
                float c = 1f * prog * 0.6f;
                Color color2 = new Color(93 / 255f, 174f / 255, 70 / 255f) * c;
                Twins.RestartSpritebatchForShaderDraw(spriteBatch, 1f, color2);
                for (int i = 0; i < 4; i++)
                {
                    float rot = i / 4f * 2f * MathF.PI;
                    rot += prog * MathF.PI / 2f;
                    float radius = (1f - prog) * 200;

                    Vector2 effectPos = Pos + TRAEMethods.PolarVector(radius, rot);
                    spriteBatch.Draw(texture, effectPos, npc.frame, color2, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                }
                Twins.RestartSpritebatchForVanillaDraw(spriteBatch);

            }
            if (npc.ai[0] == 5f)
            {
                float prog = npc.ai[2] / 0.5f;
                float c = 1f * prog * 0.6f;
                Color color2 = new Color(93 / 255f, 174f / 255, 70 / 255f) * c;
                Twins.RestartSpritebatchForShaderDraw(spriteBatch, 1f, color2);
                for (int i = 0; i < 16; i++)
                {
                    float rot = i / 16f * 2f * MathF.PI;
                    float radius = (1f - prog) * 8000;
                    Vector2 effectPos = Pos + TRAEMethods.PolarVector(radius, rot);
                    spriteBatch.Draw(texture, effectPos, npc.frame, color2, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                }
                Twins.RestartSpritebatchForVanillaDraw(spriteBatch);
            }
            float opacityForMotionBlur = 0f;
            if (npc.ai[0] > 5 && npc.ai[1] == 2)
            {
                opacityForMotionBlur = Utils.GetLerpValue(ChargeDuration - 3, ChargeDuration - 7, npc.ai[2], true);
            }
            float trailOpacityMult = 1f;// - opacityForMotionBlur;
            if (npc.ai[0] > 5f)
            {
                if (trailOpacityMult > 0.00001f)
                {
                    Color baseColor = new Color(93 / 255f, 174f / 255, 70 / 255f) * 0.3f;
                    baseColor = new Color(0.1f, 0.2f, 0.1f);
                    Color endColor = new(0.05f, 0.15f, 0.05f);
                    endColor = new Color(179f / 255, 252f / 255f, 0, 1f) * 0.6f;
                    endColor.A = 255;
                    Twins.RestartSpritebatchForShaderDraw(spriteBatch, 1f, baseColor);
                    for (int i = npc.oldPos.Length - 1; i >= 0; i--)
                    {
                        float c = ((float)i / npc.oldPos.Length);
                        float opacity = 1 - c;
                        opacity *= trailOpacityMult;
                        Color color2 = Color.Lerp(endColor, baseColor, opacity) * opacity; //Color.Lerp(baseColor, endColor,c);
                        float interpAmount = Utils.Remap(i, 0, 4, 1f / npc.oldPos.Length, 1f);
                        Twins.ModifyShaderParams(interpAmount, color2);
                        Vector2 effectPos = Pos - npc.position + npc.oldPos[i];
                        /* new Vector2(
                        npc.oldPos[i].X - screenPos.X + (float)(npc.width / 2) - (float)TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale, 
                        npc.oldPos[i].Y - screenPos.Y + (float)npc.height - (float)TextureAssets.Npc[npc.type].Height() * npc.scale / (float)Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35); */
                        //due to the shader, this color parameter on the trail is completely irrelevant
                        spriteBatch.Draw(texture, effectPos, npc.frame, Color.White, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                    }
                    Twins.RestartSpritebatchForVanillaDraw(spriteBatch);
                }
            }

            DrawSpazmatismInnerMouthGlow(npc, screenPos);
            if (opacityForMotionBlur > 0.0001f)
            {
                int imageHalfCount = 28;
                for (int i = -imageHalfCount; i < imageHalfCount + 1; i++)
                {
                    float opacity = 1f - (MathF.Abs(i) / imageHalfCount);
                    opacity *= 0.15f;
                    Vector2 vel = npc.velocity * 0.25f;
                    float maxLength = 8;
                    if (vel.Length() > maxLength)
                    {
                        vel.Normalize();
                        vel *= maxLength;
                    }
                    Vector2 offset = vel * i * (6f / maxLength);
                    spriteBatch.Draw(texture, Pos + offset, npc.frame, color * opacity, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                }
            }
            else
            {
                spriteBatch.Draw(texture, Pos, npc.frame, color, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
            }
        }
        public static void DrawSpazmatismInnerMouthGlow(NPC npc, Vector2 screenPos)
        {
            float opacity = 0f;
            if ((npc.ai[0] == 6 || npc.ai[0] == 3) && npc.ai[1] == 0) //phase 2 spewing flames
            {
                float max = 300;//vanilla phase 2 flamethrower attack duration
                if (npc.ai[0] == 6)
                {
                    max = flameTime;
                }
                opacity = Utils.GetLerpValue(-1, 4, npc.ai[2], true) * Utils.GetLerpValue(max - 1, max - 5, npc.ai[2], true);
            }
            if (npc.ai[0] == 6 && npc.ai[1] == 3)
            {
                float max = CauldronDuration;
                float relativeTimer = npc.ai[2] % CauldronFireRate;

                opacity = Utils.GetLerpValue(-1, 4, npc.ai[2], true) * Utils.GetLerpValue(max - 1, max - 5, npc.ai[2], true);
                opacity *= Utils.Remap(relativeTimer, 2, CauldronFireRate - 1, 1.2f, .25f);
            }
            if (opacity == 0)
            {
                return;
            }
            Vector2 halfSize = new(55f, 107f);
            float num35 = 0f;
            float num36 = Main.NPCAddHeight(npc);
            Vector2 Pos = new(
                npc.position.X - screenPos.X + npc.width / 2 - TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale,
                npc.position.Y - screenPos.Y + npc.height - TextureAssets.Npc[npc.type].Height() * npc.scale / Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35);

            Color cursedFlamesLime = new(179f / 255, 252f / 255f, 0, 1f);
            cursedFlamesLime *= opacity;
            Twins.FadeLightBall(Pos + (npc.rotation + MathF.PI / 2f).ToRotationVector2() * 28, cursedFlamesLime, 1.2f);
        }
        public static void Start(NPC npc)
        {
            if (npc.ai[0] == 4f)
            {

                npc.ai[2] += 0.005f;
                if (npc.ai[2] > 0.5f)
                {
                    npc.ai[2] = 0.5f;
                }
            }
            else
            {
                npc.ai[2] -= 0.005f;
                if (npc.ai[2] < 0f)
                {
                    npc.ai[2] = 0f;
                }
            }
            //npc.rotation += npc.ai[2];
            npc.ai[1] += 1f;
            if (npc.ai[1] >= 100f)
            {
                npc.ai[0] += 1f;
                npc.ai[1] = 0f;
                if (npc.ai[0] == 6f)
                {
                    npc.ai[2] = 0f;
                }
                else
                {
                    //SoundEngine.PlaySound(3, (int)npc.position.X, (int)npc.position.Y);
                    for (int num477 = 0; num477 < 20; num477++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
                    }
                    //SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);

                    SoundEngine.PlaySound(SoundID.ForceRoarPitched with { MaxInstances = 0 }, npc.Center);
                }
            }
            Dust.NewDust(npc.position, npc.width, npc.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
            npc.velocity.X *= 0.98f;
            npc.velocity.Y *= 0.98f;
            if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
            {
                npc.velocity.X = 0f;
            }
            if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
            {
                npc.velocity.Y = 0f;
            }
        }
    }
    public class BouncingFlames : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 8;
            Projectile.hostile = true;
            Projectile.light = 0.8f;
            Projectile.alpha = 100;
            AIType = 95;
            Projectile.timeLeft = 240;
        }
    }
    public class SpamatizmFire : FlamethrowerProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Flames;
        public override void FlamethrowerDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.GetGlobalProjectile<Common.ProjectileStats>().DamageFalloff = 0;
            ColorMiddle = new Color(50, 255, 0, 100);
            ColorBack = new Color(167, 255, 65, 100);
            ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
            ColorSmoke = new Color(70, 70, 70, 100);
            dustID = DustID.CursedTorch;
            dustAmount = 0;
            maxScale = 0.8f;
            Projectile.light = 1;
        }
        public override void AI()
        {
            minScale = Utils.Remap(Projectile.velocity.Length(), 6, 30, 0.25f, 0.8f, false);
            base.AI();
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.Next(3) < 2)
                target.AddBuff(BuffID.CursedInferno, Main.rand.Next(120, 180));
        }
        public override void Load()
        {
            On_Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float += ChangeEyeFireToSpazFire;
        }
        int ChangeEyeFireToSpazFire(On_Projectile.orig_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float orig, Terraria.DataStructures.IEntitySource spawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1, float ai2)
        {
            if (Type == ProjectileID.EyeFire && NPC.AnyNPCs(NPCID.Spazmatism))
            {
                Type = ModContent.ProjectileType<SpamatizmFire>();
            }
            return orig(spawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
        }
    }
}