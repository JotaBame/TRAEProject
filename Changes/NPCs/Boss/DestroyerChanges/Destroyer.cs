using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss.DestroyerChanges
{
    public class Destroyer : GlobalNPC
    {

        //segmentation
        const int ActiveSegmentDenominator = 3; //when set to n every nth segment will be active
        const int ActiveSegmentShifter = 0; //move all active segments back or forward on destroyer

        //movement
        const float DestroyerGravity = 0.15f; //When not burrowing the Destroyer's head acts like a gravity obeying projectile, this determines the gravity, Vanilla value: 0.15f
        const int PlayerDetectionRegionSize = 800; //if the Destroyer's head is not close enough to the player it will burrow regardless of blocks, this is how close it has to be to not set burrow, vanilla value: 1000
        const float BaseTerminalVelocity = 12f; //Determines various max speed values, vanilla value: 16f
        const float BaseAccelration = 0.1f * 2f; //the acceration destroyer uses for moving, gets a 20% more on some seeds, vanilla value: 0.1f
        const float BaseBurrowAccelration = 0.15f * 2f; //extra acceration destroyer uses for moving when burrowing, gets a 20% more on some seeds, vanilla value: 0.15f
        const float OverPlayerBurrowTarget = 160f; //In vanilla destroyer attempts to burrow directly at the player, however when close enough gravity will take over causing it to miss, with the the destoryer will target above the player making it more of a threat
        const float GravityReductionWhenBelowPlayer = 0.1f; //added feature, Destroyer's gravity is reduced when below the player trying to move toward them

        //lasers
        const int AimedLaserWarnTime = 90; //how long the visual warning for aimed lasers are about to fire lasts, this is also effectively a minimum cooldown on lasers
        const int PerpiLaserWarnTime = 180; //how long the visual warning for perpi lasers are about to fire lasts
        const int BaseLaserCooldown = 360; //the time between laser shots on neutral mood
        const float MaxLaserRange = 2000; //maximun distance a segment can be from the player in order to fire a laser at them
        const float MinLaserRange = 600; // minimum distance a segment can be from the player in order to fire a laser at them
        const int FiringSegmentDenominator = 5; //when set to n every nth active segment will fire aimed lasers
        const int PerpFiringSegmentDenominator = 3; //when set to n every nth active segment will fire perpi lasers
        const int PerpiFireFrequency = 3; //every nth shot will be perpindicular shots instead of aimed shots 


        //mood
        const float MoodPeriod = 900; //how long it takes to go through destroyer's mood cycle
        const float LaserMoodCooldownBonus = 4f; //on positive mood it fires up to this many times less often, on negetive mood it fire up to this many times more often
        const float SpeedMoodSpeedBonus = 2f; //on positive mood it moves up to this many times faster, on negetive mood it moves up to this many times slower

        //probes
        const int probesToSpawn = 20; //how many probes the tail and head each spawn of the course of the fight

        // master (unused, for now)
        const int MasterAimedLaserWarnTime = 75;
        const int MasterPerpiLaserWarnTime = 120;
        const int MasterBaseLaserCooldown = 300;

        //don't mess with these
        const int segmentActive = 2;
        const int segmentInactive = 1;
        public override void SetDefaults(NPC npc)
        {
            if (GetInstance<TRAEConfig>().DestroyerRework && !Main.zenithWorld)
            {
                if (npc.type == NPCID.TheDestroyer && Main.expertMode)
                {
                    npc.damage = 50; // expert quadruples damage, this ends up being 200;
                }
                if (npc.type == NPCID.Probe && Main.masterMode)
                {
                    npc.lifeMax = 134; // probes have too much hp in master
                }
                if (npc.type == NPCID.TheDestroyer)
                {
                    npc.lifeMax = (int)(npc.lifeMax * ((float)50000 / 80000));

                }
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.TheDestroyer && GetInstance<TRAEConfig>().DestroyerRework && !Main.zenithWorld)
            {
                //recreated modified vanilla movement
                //Main.NewText("AI0: " + npc.ai[0] + " AI1: " + npc.ai[1] + " AI2: " + npc.ai[2] + " AI3: " + npc.ai[2] + " LAI0: " + npc.localAI[0] + " LAI1: " + npc.localAI[1] + " LAI2: " + npc.localAI[2] + " LAI3: " + npc.localAI[2] );
                float hpRatio = Main.expertMode ? (1f - (float)npc.life / npc.lifeMax) * 0.4f + 0.5f : 1f;
                npc.ai[2] += MathF.PI * 2f / MoodPeriod;
                float currentMood = MathF.Sin(npc.ai[2]) * hpRatio;
                //Main.NewText("Mood: " + MathF.Round(currentMood, 2) + "/" + MathF.Round(hpRatio, 2));
                float currentSpeedMultiplier = 1f;
                float laserCooldownModifer = 1f;
                float moodModifer = currentMood * hpRatio;

                if (moodModifer > 0)
                {
                    currentSpeedMultiplier = 1 + moodModifer * (SpeedMoodSpeedBonus - 1);
                    laserCooldownModifer = 1f / (1 + moodModifer * (LaserMoodCooldownBonus - 1));
                }
                else
                {
                    currentSpeedMultiplier = 1f / (1 + Math.Abs(moodModifer) * (SpeedMoodSpeedBonus - 1));
                    laserCooldownModifer = 1 + Math.Abs(moodModifer) * (LaserMoodCooldownBonus - 1);
                }

                npc.localAI[3]++;

                if (npc.localAI[3] > Math.Max(0, BaseLaserCooldown / laserCooldownModifer - AimedLaserWarnTime))
                {
                    if (npc.localAI[2] % PerpiFireFrequency == PerpiFireFrequency - 1)
                    {
                        npc.localAI[3] = -1 * PerpiLaserWarnTime;
                    }
                    else
                    {
                        npc.localAI[3] = -1 * AimedLaserWarnTime;

                    }
                }
                if (npc.localAI[3] == 0)
                {
                    npc.localAI[2]++;
                }
                if (npc.localAI[2] % PerpiFireFrequency == PerpiFireFrequency - 1)
                {
                    if (npc.localAI[3] == -1)
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            float r = npc.rotation - MathF.PI / 2f + i * MathF.PI / 8f;
                            Vector2 laserVel = TRAEMethods.PolarVector(11f, r);
                            Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, laserVel, 100, npc.GetAttackDamage_ForProjectiles(22f, 18f), 0, Main.myPlayer);
                            RetPhase3.DeathLaserShootDust(laserVel, npc.Center);
                        }
                    }
                }
                AI_Destroyer_Head(npc, currentSpeedMultiplier);
                if (npc.ai[1] * (1f / probesToSpawn) < 1f - (float)npc.life / npc.lifeMax)
                {
                    npc.ai[1]++;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int num763 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), 139);
                        if (Main.netMode == NetmodeID.Server && num763 < 200)
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num763);

                        npc.netUpdate = true;
                    }
                }
                return false;
            }
            return base.PreAI(npc);
        }
        public override void AI(NPC npc)
        {
            if (GetInstance<TRAEConfig>().DestroyerRework && !Main.zenithWorld)
            {
                if (npc.type == NPCID.TheDestroyerTail)
                {
                    if (npc.localAI[3] * (1f / probesToSpawn) < 1f - (float)npc.life / npc.lifeMax)
                    {
                        npc.localAI[3]++;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int num763 = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), 139);
                            if (Main.netMode == NetmodeID.Server && num763 < 200)
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num763);

                            npc.netUpdate = true;
                        }
                    }
                }
                if (npc.type == NPCID.TheDestroyerBody)
                {
                    //stop vanilla shooting behavior
                    npc.localAI[0] = 0;

                    if (npc.localAI[3] == 0)
                    {
                        npc.localAI[3] = FindIDByRecursion(npc);
                    }
                    //make only every few segments active
                    //in vanilla ai[2] = 0 means it hasn't launched a probe, ai[2] = 1 means it has, we use 2 for active and 1 for innactive to prevent probe spawning
                    npc.ai[2] = (int)(npc.localAI[3] + ActiveSegmentShifter) % ActiveSegmentDenominator == 0 ? segmentActive : segmentInactive;
                    if (npc.ai[2] == segmentActive)
                    {
                        Lighting.AddLight(npc.Center, TorchID.Red);
                        npc.TargetClosest();
                        int activeSegID = (int)npc.localAI[3] / ActiveSegmentDenominator;
                        if ((int)Main.npc[(int)npc.ai[3]].localAI[2] % PerpiFireFrequency == PerpiFireFrequency - 1)
                        {
                            if (Main.npc[(int)npc.ai[3]].localAI[3] == -1 && (activeSegID + (int)Main.npc[(int)npc.ai[3]].localAI[2]) % PerpFiringSegmentDenominator == 0)
                            {
                                //Vector2 laserVel = (Main.player[npc.target].Center - npc.Center) / 80;
                                Vector2 laserVel = TRAEMethods.PolarVector(11f, npc.rotation);
                                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, laserVel, 100, npc.GetAttackDamage_ForProjectiles(22f, 18f), 0, Main.myPlayer);
                                RetPhase3.DeathLaserShootDust(laserVel, npc.Center);

                                laserVel = TRAEMethods.PolarVector(-11f, npc.rotation);
                                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, laserVel, 100, npc.GetAttackDamage_ForProjectiles(22f, 18f), 0, Main.myPlayer);
                                RetPhase3.DeathLaserShootDust(laserVel, npc.Center);
                            }
                        }
                        else if ((Main.player[npc.target].Center - npc.Center).Length() < MaxLaserRange && (Main.player[npc.target].Center - npc.Center).Length() > MinLaserRange)
                        {
                            //(Main.player[npc.target].Center - npc.Center).SafeNormalize(-Vector2.UnitY) * 8f
                            if (Main.npc[(int)npc.ai[3]].localAI[3] == -1 && (activeSegID + (int)Main.npc[(int)npc.ai[3]].localAI[2]) % FiringSegmentDenominator == 0)
                            {
                                Vector2 laserVel = (Main.player[npc.target].Center - npc.Center) / 80;
                                //Vector2 laserVel = (Main.player[npc.target].Center - npc.Center).SafeNormalize(-Vector2.UnitY) * 11f;
                                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, laserVel, 100, npc.GetAttackDamage_ForProjectiles(22f, 18f), 0, Main.myPlayer);
                                RetPhase3.DeathLaserShootDust(laserVel, npc.Center);
                            }
                        }
                    }
                    else
                    {
                        //Dust.NewDustPerfect(npc.Center, DustID.MagnetSphere, Vector2.Zero, 0, Color.White, 2);

                        npc.dontTakeDamage = true;
                    }
                }
            }
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            if (GetInstance<TRAEConfig>().DestroyerRework && !Main.zenithWorld)
            {

                if (npc.type == NPCID.TheDestroyer)
                {
                    if (npc.localAI[2] % PerpiFireFrequency == PerpiFireFrequency - 1)
                    {
                        if (npc.localAI[3] < 0)
                        {
                            for (int i = -1; i < 2; i++)
                            {
                                float r = npc.rotation - MathF.PI / 2f + i * MathF.PI / 8f;
                                DrawPointedLaser(npc, spriteBatch, r, Math.Max(0.1f, 1f - Main.npc[(int)npc.ai[3]].localAI[3] / -PerpiLaserWarnTime), 100);
                            }
                        }
                    }
                }
                if (npc.type == NPCID.TheDestroyerBody)
                {
                    if (npc.ai[2] == segmentActive)
                    {
                        int activeSegID = (int)npc.localAI[3] / ActiveSegmentDenominator;
                        if ((int)Main.npc[(int)npc.ai[3]].localAI[2] % PerpiFireFrequency == PerpiFireFrequency - 1)
                        {
                            if (Main.npc[(int)npc.ai[3]].localAI[3] < 0 && (activeSegID + (int)Main.npc[(int)npc.ai[3]].localAI[2]) % PerpFiringSegmentDenominator == 0)
                            {
                                DrawPointedLaser(npc, spriteBatch, npc.rotation, Math.Max(0.1f, 1f - Main.npc[(int)npc.ai[3]].localAI[3] / -PerpiLaserWarnTime), 100);
                                DrawPointedLaser(npc, spriteBatch, npc.rotation + MathF.PI, Math.Max(0.1f, 1f - Main.npc[(int)npc.ai[3]].localAI[3] / -PerpiLaserWarnTime), 100);
                            }
                        }
                        else if ((Main.player[npc.target].Center - npc.Center).Length() < MaxLaserRange && (Main.player[npc.target].Center - npc.Center).Length() > MinLaserRange)
                        {

                            if (Main.npc[(int)npc.ai[3]].localAI[3] < 0 && (activeSegID + (int)Main.npc[(int)npc.ai[3]].localAI[2]) % FiringSegmentDenominator == 0)
                            {
                                DrawLaser(npc, spriteBatch, (Main.player[npc.target].Center - npc.Center).ToRotation(), Math.Max(0.1f, 1f - Main.npc[(int)npc.ai[3]].localAI[3] / -AimedLaserWarnTime), (Main.player[npc.target].Center - npc.Center).Length());
                            }
                        }
                    }

                }
            }
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public static void DrawLaser(NPC npc, SpriteBatch spriteBatch, float dir, float opacity, float length)
        {
            Vector2 segPos = npc.Center - Main.screenPosition;
            Texture2D blankTexture = TextureAssets.Extra[178].Value;
            Vector2 texScale = new Vector2(length, 6 * opacity);
            spriteBatch.Draw(blankTexture, segPos, new Rectangle(0, 0, 1, 1), Color.Red * opacity * 0.75f, dir, new Vector2(0, 0.5f), texScale, SpriteEffects.None, 0);
        }
        public static void DrawPointedLaser(NPC npc, SpriteBatch spriteBatch, float dir, float opacity, float length)
        {
            Vector2 segPos = npc.Center - Main.screenPosition;
            Texture2D blankTexture = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/PointedWarning").Value;
            Vector2 texScale = new Vector2(length / 10, 6 * opacity / 9);
            spriteBatch.Draw(blankTexture, segPos, new Rectangle(0, 0, 10, 9), Color.Red * opacity * 0.75f, dir, new Vector2(0, 4.5f), texScale, SpriteEffects.None, 0);
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
            if (npc.type == NPCID.TheDestroyerBody && GetInstance<TRAEConfig>().DestroyerRework && !Main.zenithWorld)
            {
                //we manually set frameY = 0 when on 2 to for it to use the not probe launched frame for active segments
                if (npc.ai[2] == segmentActive)
                {
                    npc.frame.Y = 0;
                }
                else
                {
                    npc.frame.Y = frameHeight;
                }

            }
            base.FindFrame(npc, frameHeight);
        }
        //Using whoAmI might lead to multiplayer desyncs so will come up with a better implimentation later
        static int FindIDByWhoAmI(NPC npc)
        {
            int segmentId = 1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.TheDestroyerBody)
                {
                    if (i == npc.whoAmI)
                    {
                        break;
                    }
                    else
                    {
                        segmentId++;
                    }
                }
            }
            return segmentId;
        }
        //ai[1] is the segment its attached to, ai[3] is the head, by using recussion we can work our way up the worm and count the segment's position
        static int FindIDByRecursion(NPC npc)
        {
            int segmentId = 1;
            int attachedTo = (int)npc.ai[1];
            for (int i = 0; i < 200; i++)
            {
                if (attachedTo == (int)npc.ai[3])
                {
                    break;
                }
                segmentId++;
                attachedTo = (int)Main.npc[attachedTo].ai[1];
            }
            return segmentId;
        }
        //copied and modified from vanilla
        private void AI_Destroyer_Head(NPC npc, float speedBonus)
        {
            
            if (npc.ai[3] > 0f)
                npc.realLife = (int)npc.ai[3];

            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead)
                npc.TargetClosest();

            if (npc.alpha != 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.TheDestroyer, 0f, 0f, 100, default, 2f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].noLight = true;
                }
            }
            npc.alpha -= 42;
            if (npc.alpha < 0)
                npc.alpha = 0;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.ai[0] == 0f)
                {
                    npc.ai[3] = npc.whoAmI;
                    npc.realLife = npc.whoAmI;
                    int prevSegIndex = npc.whoAmI;
                    int num4 = 80;
                    if (Main.getGoodWorld)
                        num4 *= 2;

                    for (int j = 0; j <= num4; j++)
                    {
                        int segType = 135;
                        if (j == num4)
                            segType = 136;

                        int segIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)(npc.position.X + npc.width / 2), (int)(npc.position.Y + npc.height), segType, npc.whoAmI);
                        Main.npc[segIndex].ai[3] = npc.whoAmI;
                        Main.npc[segIndex].realLife = npc.whoAmI;
                        Main.npc[segIndex].ai[1] = prevSegIndex;
                        Main.npc[prevSegIndex].ai[0] = segIndex;
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, segIndex);
                        prevSegIndex = segIndex;
                    }
                }
            }

            //detect tile region
            int tileToLeft = (int)(npc.position.X / 16f) - 1;
            int tileToRight = (int)((npc.position.X + npc.width) / 16f) + 2;
            int tileAbove = (int)(npc.position.Y / 16f) - 1;
            int tileBelow = (int)((npc.position.Y + npc.height) / 16f) + 2;
            if (tileToLeft < 0)
                tileToLeft = 0;

            if (tileToRight > Main.maxTilesX)
                tileToRight = Main.maxTilesX;

            if (tileAbove < 0)
                tileAbove = 0;

            if (tileBelow > Main.maxTilesY)
                tileBelow = Main.maxTilesY;

            bool isBurrowing = false;
            if (!isBurrowing)
            {
                Vector2 vector2 = default;
                for (int k = tileToLeft; k < tileToRight; k++)
                {
                    for (int l = tileAbove; l < tileBelow; l++)
                    {
                        if (Main.tile[k, l] != null && (Main.tile[k, l].HasUnactuatedTile && (Main.tileSolid[Main.tile[k, l].TileType] || Main.tileSolidTop[Main.tile[k, l].TileType] && Main.tile[k, l].TileFrameY == 0) || Main.tile[k, l].LiquidAmount > 64))
                        {
                            vector2.X = k * 16;
                            vector2.Y = l * 16;
                            if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16f && npc.position.Y + npc.height > vector2.Y && npc.position.Y < vector2.Y + 16f)
                            {
                                isBurrowing = true;
                                break;
                            }
                        }
                    }
                }
            }

            //detect player proximity region
            if (!isBurrowing)
            {

                Lighting.AddLight((int)((npc.position.X + npc.width / 2) / 16f), (int)((npc.position.Y + npc.height / 2) / 16f), 0.3f, 0.1f, 0.05f);

                npc.localAI[1] = 1f;
                Rectangle rectangle = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                bool airBurrow = true;
                if (npc.position.Y > Main.player[npc.target].position.Y)
                {
                    for (int m = 0; m < 255; m++)
                    {
                        if (Main.player[m].active)
                        {
                            Rectangle rectangle2 = new Rectangle((int)Main.player[m].position.X - PlayerDetectionRegionSize, (int)Main.player[m].position.Y - PlayerDetectionRegionSize, PlayerDetectionRegionSize * 2, PlayerDetectionRegionSize * 2);
                            if (rectangle.Intersects(rectangle2))
                            {
                                airBurrow = false;
                                break;
                            }
                        }
                    }

                    if (airBurrow)
                        isBurrowing = true;
                }

            }
            else
            {
                npc.localAI[1] = 0f;
            }

            float terminalVelocity = BaseTerminalVelocity * speedBonus;
            if (Main.dayTime || Main.player[npc.target].dead)
            {
                isBurrowing = false;
                npc.velocity.Y += 1f;
                if (npc.position.Y > Main.worldSurface * 16.0)
                {
                    npc.velocity.Y += 1f;
                    terminalVelocity = BaseTerminalVelocity * 2;
                }

                if (npc.position.Y > Main.rockLayer * 16.0)
                {
                    for (int n = 0; n < 200; n++)
                    {
                        if (Main.npc[n].aiStyle == npc.aiStyle)
                            Main.npc[n].active = false;
                    }
                }
            }

            float acceleration = BaseAccelration * speedBonus;
            float extraBurrowAccleration = BaseBurrowAccelration * speedBonus;
            if (Main.getGoodWorld)
            {
                acceleration *= 1.2f;
                extraBurrowAccleration *= 1.2f;
            }

            Vector2 center = npc.Center;
            float distToPlayerX = Main.player[npc.target].position.X + Main.player[npc.target].width / 2;
            float distToPlayerY = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - OverPlayerBurrowTarget;
            distToPlayerX = (int)(distToPlayerX / 16f) * 16;
            distToPlayerY = (int)(distToPlayerY / 16f) * 16;
            center.X = (int)(center.X / 16f) * 16;
            center.Y = (int)(center.Y / 16f) * 16;
            distToPlayerX -= center.X;
            distToPlayerY -= center.Y;
            float distToPlayer = (float)Math.Sqrt(distToPlayerX * distToPlayerX + distToPlayerY * distToPlayerY);


            if (!isBurrowing)
            {
                npc.TargetClosest();
                npc.velocity.Y += speedBonus * DestroyerGravity * (npc.Center.Y > Main.player[npc.target].Center.Y && npc.velocity.Y < 0 ? GravityReductionWhenBelowPlayer : 1f);
                if (npc.velocity.Y > terminalVelocity)
                    npc.velocity.Y = terminalVelocity;

                if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)terminalVelocity * 0.4)
                {
                    if (npc.velocity.X < 0f)
                        npc.velocity.X -= acceleration * 1.1f;
                    else
                        npc.velocity.X += acceleration * 1.1f;
                }
                else if (npc.velocity.Y == terminalVelocity)
                {
                    if (npc.velocity.X < distToPlayerX)
                        npc.velocity.X += acceleration;
                    else if (npc.velocity.X > distToPlayerX)
                        npc.velocity.X -= acceleration;
                }
                else if (npc.velocity.Y > 4f)
                {
                    if (npc.velocity.X < 0f)
                        npc.velocity.X += acceleration * 0.9f;
                    else
                        npc.velocity.X -= acceleration * 0.9f;
                }
            }
            else
            {
                if (npc.soundDelay == 0)
                {
                    float num23 = distToPlayer / 40f;
                    if (num23 < 10f)
                        num23 = 10f;

                    if (num23 > 20f)
                        num23 = 20f;

                    npc.soundDelay = (int)num23;
                    SoundEngine.PlaySound(SoundID.WormDig, npc.position);
                }

                float num24 = Math.Abs(distToPlayerX);
                float num25 = Math.Abs(distToPlayerY);
                float multiplier = terminalVelocity / distToPlayer;
                float maxSpeedX = distToPlayerX * multiplier;
                float maxSpeedY = distToPlayerY * multiplier;
                //distToPlayerX *= num26;
                //distToPlayerY *= num26;
                if ((npc.velocity.X > 0f && maxSpeedX > 0f || npc.velocity.X < 0f && maxSpeedX < 0f) && (npc.velocity.Y > 0f && maxSpeedY > 0f || npc.velocity.Y < 0f && maxSpeedY < 0f))
                {
                    if (npc.velocity.X < maxSpeedX)
                        npc.velocity.X += extraBurrowAccleration;
                    else if (npc.velocity.X > maxSpeedX)
                        npc.velocity.X -= extraBurrowAccleration;

                    if (npc.velocity.Y < maxSpeedY)
                        npc.velocity.Y += extraBurrowAccleration;
                    else if (npc.velocity.Y > maxSpeedY)
                        npc.velocity.Y -= extraBurrowAccleration;
                }

                if (npc.velocity.X > 0f && maxSpeedX > 0f || npc.velocity.X < 0f && maxSpeedX < 0f || npc.velocity.Y > 0f && maxSpeedY > 0f || npc.velocity.Y < 0f && maxSpeedY < 0f)
                {
                    if (npc.velocity.X < maxSpeedX)
                        npc.velocity.X += acceleration;
                    else if (npc.velocity.X > maxSpeedX)
                        npc.velocity.X -= acceleration;

                    if (npc.velocity.Y < maxSpeedY)
                        npc.velocity.Y += acceleration;
                    else if (npc.velocity.Y > maxSpeedY)
                        npc.velocity.Y -= acceleration;

                    if ((double)Math.Abs(maxSpeedY) < (double)terminalVelocity * 0.2 && (npc.velocity.X > 0f && maxSpeedX < 0f || npc.velocity.X < 0f && maxSpeedX > 0f))
                    {
                        if (npc.velocity.Y > 0f)
                            npc.velocity.Y += acceleration * 2f;
                        else
                            npc.velocity.Y -= acceleration * 2f;
                    }

                    if ((double)Math.Abs(maxSpeedX) < (double)terminalVelocity * 0.2 && (npc.velocity.Y > 0f && maxSpeedY < 0f || npc.velocity.Y < 0f && maxSpeedY > 0f))
                    {
                        if (npc.velocity.X > 0f)
                            npc.velocity.X += acceleration * 2f;
                        else
                            npc.velocity.X -= acceleration * 2f;
                    }
                }
                else if (num24 > num25)
                {
                    if (npc.velocity.X < maxSpeedX)
                        npc.velocity.X += acceleration * 1.1f;
                    else if (npc.velocity.X > maxSpeedX)
                        npc.velocity.X -= acceleration * 1.1f;

                    if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)terminalVelocity * 0.5)
                    {
                        if (npc.velocity.Y > 0f)
                            npc.velocity.Y += acceleration;
                        else
                            npc.velocity.Y -= acceleration;
                    }
                }
                else
                {
                    if (npc.velocity.Y < maxSpeedY)
                        npc.velocity.Y += acceleration * 1.1f;
                    else if (npc.velocity.Y > maxSpeedY)
                        npc.velocity.Y -= acceleration * 1.1f;

                    if ((double)(Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) < (double)terminalVelocity * 0.5)
                    {
                        if (npc.velocity.X > 0f)
                            npc.velocity.X += acceleration;
                        else
                            npc.velocity.X -= acceleration;
                    }
                }
            }

            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;

            if (isBurrowing)
            {
                if (npc.localAI[0] != 1f)
                    npc.netUpdate = true;

                npc.localAI[0] = 1f;
            }
            else
            {
                if (npc.localAI[0] != 0f)
                    npc.netUpdate = true;

                npc.localAI[0] = 0f;
            }

            if ((npc.velocity.X > 0f && npc.oldVelocity.X < 0f || npc.velocity.X < 0f && npc.oldVelocity.X > 0f || npc.velocity.Y > 0f && npc.oldVelocity.Y < 0f || npc.velocity.Y < 0f && npc.oldVelocity.Y > 0f) && !npc.justHit)
                npc.netUpdate = true;
        }
        public override bool PreKill(NPC npc)
        {
            if (npc.type == NPCID.Probe && Main.expertMode)
            {
                NPCLoader.blockLoot.Add(ItemID.Heart);
                return false;
            }
            return true;
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (GetInstance<TRAEConfig>().DestroyerRework && !Main.zenithWorld)
                return DestroyerBeamDrawing.Draw(npc, spriteBatch, screenPos, drawColor);
            return true;
        }
    }
    public class DestroyerProjectiles : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileID.PinkLaser && GetInstance<TRAEConfig>().DestroyerRework && !Main.zenithWorld)
            {
                projectile.scale *= 2f;
            }
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            if (projectile.type == ProjectileID.PinkLaser && NPC.AnyNPCs(NPCID.Probe) && GetInstance<TRAEConfig>().DestroyerRework && !Main.zenithWorld)
            {
                modifiers.FinalDamage *= 0.88f; // with this, probe lasers match the damage of body lasers
            }
        }
    }
}