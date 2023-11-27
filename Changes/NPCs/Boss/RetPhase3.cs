using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss
{
    public static class RetPhase3
    {
        static int tpAnimTime = 8;
        static int tpTime = 16;
        static int tpCount = 6;
        static int shotTime = 48;
        static int rapidShotTime = 7;
        static int shotCount = 2;
        static float shootSpeed = 10f;
        static int nukeTime = 220;
        static int waitTime = 55; // make this divisible by the above
        static int firstShotDelay = 10;
        static int periodTime = (tpCount * tpTime + shotTime * shotCount + nukeTime + waitTime + firstShotDelay);
        public static void Update(NPC npc)
        {

            npc.HitSound = SoundID.NPCHit4;
            npc.velocity = Vector2.Zero;
            npc.ai[2]++;
            if (npc.ai[1] != -1 && npc.ai[3] != -1)
            {
                Teleport(npc);
            }
            int periodicTimer = (int)npc.ai[2] % periodTime;
            int periodCount = (int)npc.ai[2] / periodTime;

            if (periodicTimer < tpCount * tpTime)
            {
                if (periodicTimer % tpTime < tpAnimTime)
                {
                    npc.scale = ((periodicTimer % tpTime) / (float)tpAnimTime);
                    npc.scale = 0.5f - MathF.Cos(MathF.PI * npc.scale) * 0.5f;
                }
                else if (periodicTimer % tpTime > (tpTime - tpAnimTime) && periodicTimer < (tpCount - 1) * tpTime)
                {
                    npc.scale = (tpTime - (periodicTimer % tpTime)) / (float)tpAnimTime;
                    npc.scale = 0.5f - MathF.Cos(MathF.PI * npc.scale) * 0.5f;
                }
                else
                {
                    npc.scale = 1f;
                }
                if (npc.scale <= 0)
                {
                    npc.scale = 0.01f;
                }
                if (periodicTimer % tpTime == 0)
                {
                    SetupTeleport(npc);
                }
            }

            if (periodicTimer > tpCount * tpTime + firstShotDelay)
            {
                if (periodCount % 3 == 2)
                {
                    if (periodicTimer % rapidShotTime == 0 && periodicTimer < periodTime - nukeTime - waitTime)
                    {
                        Shoot(npc);
                    }


                }
                else
                {
                    if (periodicTimer % shotTime == 0)
                    {
                        Shoot(npc);
                    }
                }
                if (periodicTimer > periodTime - nukeTime - waitTime && periodCount % 3 != 2)
                {
                    npc.ai[2] += periodTime - periodicTimer;
                }
                else
                {
                    SpawnNukes(npc);



                }
            }
        }
        public static void Rotate(NPC npc)
        {
            float angle = 0;
            int periodicTimer = (int)npc.ai[2] % periodTime;
            int startTime = periodTime - nukeTime - waitTime;
            if (npc.ai[0] > 5)
            {
                if (periodicTimer > startTime)
                {

                    npc.rotation += (periodicTimer - startTime) / 400f;

                    //Vector2 test = new Vector2(periodicTimer - startTime, npc.rotation);
                    //Main.NewText(test);
                }
                else
                {

                    angle += TRAEMethods.PredictiveAimWithOffset(npc.Center, shootSpeed * 3, Main.player[npc.target].Center, Main.player[npc.target].velocity, npc.ai[1] == 0 ? 25 * 9 : 15 * 9) - MathF.PI / 2;

                    int periodCount = (int)npc.ai[2] / periodTime;
                    if (periodicTimer >= (tpCount - 1) * tpTime && periodCount % 3 == 2 && periodicTimer < tpCount * tpTime + shotTime * shotCount)
                    {
                        int timer = (tpCount * tpTime + shotTime * shotCount) - periodicTimer;
                        if (timer > (shotTime * shotCount))
                        {
                            timer = (shotTime * shotCount);
                        }
                        float ratio = (float)timer / (shotTime * shotCount);
                        float spread = MathF.PI;
                        angle += spread * ratio - spread / 2f;
                    }
                    float rotSpeed = 0.1f;
                    if (npc.ai[1] == 0)
                    {
                        rotSpeed *= 3;
                    }
                    if (npc.ai[0] == 0 && npc.ai[1] == 1)
                    {
                        npc.rotation += rotSpeed;
                    }
                    npc.rotation.SlowRotation(angle, rotSpeed);

                }
            }

        }

        public static void SpawnNukes(NPC npc)
        {
            int periodicTimer = (int)npc.ai[2] % periodTime;
            int startTime = periodTime - nukeTime - waitTime;

            if (periodicTimer >= startTime + nukeTime / 4 && periodicTimer < startTime + nukeTime)
            {
                int eyeSpawnTimer = (nukeTime - nukeTime / 4) / 3;

                if (periodicTimer % (float)eyeSpawnTimer == 0f)
                {
                    SoundEngine.PlaySound(SoundID.Item92, npc.Center);
                    float num30 = 5f;
                    Vector2 vector5 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                    float num31 = Main.rand.Next(-200, 200);
                    float num32 = Main.rand.Next(-200, 200);
                    float num33 = (float)Math.Sqrt(num31 * num31 + num32 * num32);
                    num33 = num30 / num33;
                    Vector2 vector6 = vector5;
                    Vector2 vector7 = default(Vector2);
                    vector7.X = num31 * num33;
                    vector7.Y = num32 * num33;
                    vector6.X += vector7.X * 10f;
                    vector6.Y += vector7.Y * 10f;
                    if (Main.netMode != 1)
                    {
                        Projectile p = Main.projectile[Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), vector5, Vector2.Zero, ModContent.ProjectileType<EyeNuke>(), npc.GetAttackDamage_ForProjectiles(45f, 40f), 0, 255)];
                        p.velocity.X = vector7.X;
                        p.velocity.Y = vector7.Y;

                    }

                }
            }


        }
        static void Teleport(NPC npc)
        {

            TeleportDust(npc.Center);
            npc.Center = new Vector2(npc.ai[1], npc.ai[3]);
            TeleportDust(npc.Center);
            SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 });
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            npc.rotation = (player.Center - npc.Center).ToRotation() - MathF.PI / 2;

            npc.ai[1] = npc.ai[3] = -1;
        }
        static void SetupTeleport(NPC npc)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.TargetClosest(false);
                Player player = Main.player[npc.target];
                float r = Main.rand.NextFloat() * MathF.PI * 2;
                Vector2 offset = TRAEMethods.PolarVector(320, r);
                offset.X *= 1.4f;
                Vector2 teleToHere = player.Center + offset;
                npc.ai[1] = teleToHere.X;
                npc.ai[3] = teleToHere.Y;
                npc.netUpdate = true;
            }
        }
        static void TeleportDust(Vector2 center, bool pullIn = false)
        {
            for (int i = 0; i < 100; i++)
            {
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                float radius = 160;
                Dust dust = Dust.NewDustPerfect(center + (pullIn ? TRAEMethods.PolarVector(radius, theta) : Vector2.Zero), DustID.SilverFlame, TRAEMethods.PolarVector((pullIn ? -1 : 1) * radius / 10f, theta));
                dust.color = Color.Red;
                dust.noGravity = true;
            }
        }
        static void Shoot(NPC npc)
        {
            //todo: fix laser spawning position
            Vector2 shootPos = npc.Center + TRAEMethods.PolarVector(25 * 9, npc.rotation + MathF.PI / 2);
            Vector2 shootVel = TRAEMethods.PolarVector(shootSpeed, npc.rotation + MathF.PI / 2);
            DeathLaserShootDust(shootVel, shootPos - Vector2.Normalize(shootVel) * 140);
            if (Main.netMode != 1)
            {

                int attackDamage_ForProjectiles3 = npc.GetAttackDamage_ForProjectiles(35f, 30f);
                Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), shootPos, shootVel, ProjectileID.DeathLaser, attackDamage_ForProjectiles3, 0f, Main.myPlayer);
            }
        }

        public static void Start(NPC npc)
        {
            if (npc.ai[0] == 4f)
            {
                npc.ai[2] += 0.005f;
                npc.ai[2] = MathF.Min(.5f, npc.ai[2]);
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
                    npc.ai[1] = -1;
                    npc.ai[3] = -1;
                    npc.netUpdate = true;
                }
                else
                {
                    for (int num477 = 0; num477 < 20; num477++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
                    }

                    Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoarPitched with { MaxInstances = 0 }, npc.Center);
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

        public static void Phase3Draw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int periodicTimer = (int)npc.ai[2] % periodTime;

            if (periodicTimer > tpCount * tpTime && (int)npc.ai[2] / periodTime % 3 != 2)
            {
                DrawLaser(npc, spriteBatch);
            }
            Texture2D texture = TextureAssets.Npc[npc.type].Value;
            Color color = drawColor;
            Vector2 halfSize = new Vector2(55f, 107f);
            float num35 = 0f;
            float num36 = Main.NPCAddHeight(npc);
            Texture2D effectTexture = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/RedEffect").Value;
            Vector2 Pos = new Vector2(
                npc.position.X - screenPos.X + npc.width / 2 - TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale,
                npc.position.Y - screenPos.Y + npc.height - TextureAssets.Npc[npc.type].Height() * npc.scale / Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35);
            float opacity = Utils.GetLerpValue(.25f, .85f, npc.scale, true);
            npc.Opacity = opacity;
            if (npc.ai[0] == 4f)
            {
                float prog = npc.ai[2] / 0.5f;
                for (int i = 0; i < 4; i++)
                {
                    float rot = (i / 4f) * 2f * MathF.PI;
                    rot += prog * MathF.PI / 2f;
                    float radius = (1f - prog) * 200;
                    float c = (1f * prog) * 0.3f;
                    Color color2 = new Color(c, c, c, c);
                    Vector2 effectPos = Pos + TRAEMethods.PolarVector(radius, rot);
                    spriteBatch.Draw(effectTexture, effectPos, npc.frame, color2 * opacity, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                }

            }
            if (npc.ai[0] == 5f)
            {
                float prog = npc.ai[2] / 0.5f;
                for (int i = 0; i < 16; i++)
                {
                    float rot = (i / 16f) * 2f * MathF.PI;
                    float radius = (1f - prog) * 8000;
                    float c = (1f * prog) * 0.3f;
                    Color color2 = new Color(c, c, c, c);
                    Vector2 effectPos = Pos + TRAEMethods.PolarVector(radius, rot);
                    spriteBatch.Draw(effectTexture, effectPos, npc.frame, color2 * opacity, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                }
            }
            if (((byte)npc.ai[0] > 5f))
            {
                for (int i = 0; i < npc.oldPos.Length; i++)
                {
                    float c = 255f * ((float)i / npc.oldPos.Length);
                    Color color2 = new Color(c, c, c, c) * 0.3f;
                    Vector2 effectPos = (Pos - npc.position) + npc.oldPos[i];
                    /* new Vector2(
                    npc.oldPos[i].X - screenPos.X + (float)(npc.width / 2) - (float)TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale, 
                    npc.oldPos[i].Y - screenPos.Y + (float)npc.height - (float)TextureAssets.Npc[npc.type].Height() * npc.scale / (float)Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35); */
                    spriteBatch.Draw(effectTexture, effectPos, npc.frame, color2 * opacity, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                }
            }


            spriteBatch.Draw(texture, Pos, npc.frame, color * opacity, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
        }
        public static void DrawLaser(NPC npc, SpriteBatch spriteBatch)
        {

            Vector2 retPosition = npc.Center - Main.screenPosition /*+ TRAEMethods.PolarVector(25 * 9, npc.rotation + MathF.PI / 2)*/;           
            int timer = (int)npc.ai[2] % periodTime;
            float opacity = Utils.GetLerpValue(202, 190, timer, true);
            Texture2D blankTexture = TextureAssets.Extra[178].Value;
            Vector2 texScale = new Vector2((retPosition).Length(), 1);//1/256, texture is 256x256
            Main.EntitySpriteDraw(blankTexture, retPosition, null, Color.Red * opacity, npc.rotation + MathF.PI / 2, new Vector2(0, 1), texScale, SpriteEffects.None);


        }
        static void DeathLaserShootDust(Vector2 shootVelocity, Vector2 origin)
        {
            shootVelocity.Normalize();
            int dustAmount = 12;//change to what you want, this is the line
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 dustVel = shootVelocity * Utils.Remap(i, 0, dustAmount, 2, 10);//arbitrary numbers, change 2 ad 10 to your liking
                float scale = Utils.Remap(i, 0, dustAmount, 2, 1);
                GlowyRedDust(origin, dustVel, scale);
            }
            dustAmount = 5;//change to what you want, this is the ring
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 offset = Utils.Remap(i, 0, dustAmount, 0, MathF.Tau).ToRotationVector2();
                offset.X *= .5f;
                offset = offset.RotatedBy(shootVelocity.ToRotation());
                GlowyRedDust(origin + offset, offset * 2, 2);
            }
            Vector2 sparkleScale = new(0.75f,1.5f);
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
            //dust = Dust.CloneDust(dust);
            //dust.color = Color.White with { A = 0 };
            //dust.scale *= .7f;
            //dust.customData = 1;
        }
    }
    public class EyeNuke : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Eye Nuke");
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 150;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.alpha = 255;
            //DrawOffsetX = 150;
            //DrawOriginOffsetX = 150;

        }
        void Explosion(Projectile projectile)
        {
            SoundEngine.PlaySound(SoundID.Item62 with { MaxInstances = 0 }, Projectile.position);
            for (int num731 = 0; num731 < 30; ++num731)
            {
                int num732 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default, 2f);
                Dust dust = Main.dust[num732];
                dust.velocity *= 2f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num732].scale = 0.5f;
                    Main.dust[num732].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num733 = 0; num733 < 30; ++num733)
            {
                int num734 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default, 3f);
                Main.dust[num734].noGravity = true;
                Dust dust = Main.dust[num734];
                dust.velocity *= 4f;
                num734 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default, 2f);
                dust = Main.dust[num734];
                dust.velocity *= 2f;
            }

            for (int i = 0; i < 80; i++)
            {
                float theta = Main.rand.NextFloat(-MathF.PI, MathF.PI);
                float radius = 250;
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.SilverFlame, TRAEMethods.PolarVector(radius / 10f, theta), Scale: 1.5f);
                dust.color = Color.Red;
                dust.noGravity = true;
                dust = Dust.NewDustPerfect(projectile.Center, DustID.SilverFlame, TRAEMethods.PolarVector(radius / 8f, theta), Scale: 1.5f);
                dust.color = Color.Red;
                dust.noGravity = true;
            }
        }
        public override void AI()
        {
            //fading in like the death laser does. looks nicer than it just suddenly popping into existence
            Projectile.alpha -= 50;
            Projectile.alpha = (int)MathF.Max(Projectile.alpha, 0);
            if (Projectile.timeLeft == 5)
            {
                TRAEMethods.Explode(Projectile, 180); Explosion(Projectile);

            }
            if (Projectile.timeLeft % 12 == 0)
            {
                Projectile.frame += 1;
            }
            if (Projectile.frame >= 3)
            {
                Projectile.frame = 0;
            }
            if (Projectile.timeLeft > 50)
            {
                if (Projectile.ai[0] < 0 || Projectile.ai[0] > 255)
                {
                    Projectile.timeLeft = 60;
                    return;
                }
                Player player = Main.player[(int)Projectile.ai[0]];
                if (!player.active || player.dead)
                {
                    Projectile.timeLeft = 60;
                    return;
                }
                float flytowards = (player.Center - Projectile.Center).ToRotation();
                float speedBonus = (player.Center - Projectile.Center).Length() / 100f;

                //this is an easing so they lose homing strength over time so it looks nicer. it is a nerf overall so I increased the homing duration by 10 extra frames to compensate
                float homingSpeedMultiplier = Utils.GetLerpValue(50, 90, Projectile.timeLeft, true);
                homingSpeedMultiplier = .5f - MathF.Cos(homingSpeedMultiplier * MathF.PI) * .5f;
                Projectile.rotation.SlowRotation(flytowards - MathF.PI / 2, MathF.PI / 45f * homingSpeedMultiplier);
                Projectile.velocity = TRAEMethods.PolarVector(6.25f + speedBonus, Projectile.rotation + MathF.PI / 2f);
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            TRAEMethods.Explode(Projectile, 180);
            Explosion(Projectile);
        }

        static void AABBLineVisualizer(Vector2 lineStart, Vector2 lineEnd, float lineWidth)
        {
            Texture2D blankTexture = Terraria.GameContent.TextureAssets.Extra[195].Value;
            Vector2 texScale = new Vector2((lineStart - lineEnd).Length(), lineWidth) * 0.00390625f;//1/256, texture is 256x256
            Main.EntitySpriteDraw(blankTexture, (lineStart) - Main.screenPosition, null, Color.Red, (lineEnd - lineStart).ToRotation(), new Vector2(0, 128), texScale, SpriteEffects.None);
        }

        public static Entity FindTarget(Projectile projectile)
        {
            Entity target = null;
            float maxRange = 10000;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro < maxRange)
                {
                    target = Main.player[i];
                    maxRange = (Main.player[i].Center - projectile.Center).Length() - Main.player[i].aggro;
                }
            }
            return target;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.timeLeft > 60)
            {
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(0, 60).RotatedBy(Projectile.rotation));
            }
            return null;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor *= Projectile.Opacity;
            if (Projectile.timeLeft > 1)
            {

                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Texture2D glowTexture = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/EyeNuke_Glow").Value;
                Vector2 offset = Vector2.Zero;
                if (Projectile.timeLeft < 60)
                {
                    offset = new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5));
                }
                int frameCount = Main.projFrames[Projectile.type];
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + offset,
                            new Rectangle(0, Projectile.frame * (texture.Height / frameCount), texture.Width, (texture.Height / frameCount)), lightColor, Projectile.rotation,
                            new Vector2(texture.Width * 0.5f, (texture.Height / frameCount) * 0.5f), 1f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(glowTexture, Projectile.Center - Main.screenPosition + offset,
                            new Rectangle(0, Projectile.frame * (texture.Height / frameCount), texture.Width, (texture.Height / frameCount)), Color.White, Projectile.rotation,
                            new Vector2(texture.Width * 0.5f, (texture.Height / frameCount) * 0.5f), 1f, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}