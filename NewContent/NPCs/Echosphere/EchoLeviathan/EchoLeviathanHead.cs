﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.NPCs.Echosphere.EchoStalker;
using TRAEProject.NewContent.Projectiles;
using TRAEProject.NewContent.Projectiles.EchoLeviathanPortal;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoLeviathan
{
    internal class EchoLeviathanHead : ModNPC
    {
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("The magic that animates these statues can only remain active for a limited time. They perch in the islands in the sky, awaiting any intruders that come near.")
            });
        }
        static float WormMovementTopSpeed => 7;
        static float WormMovementBaseAcceleration => 0.3f;

        static float TimeSpentInWormMovement => 300;
        static float MaxSegmentTurn => .8f;//higher value = segments can bend more
        static float TurningRate => 0.1f;
        static int SonicWaveStartTime => 130;
        static int SonicWaveFireRate => 15;
        static int SonicWaveNumShots => 3;
        static int SonicWaveExtraWait => 60;
        static int SonicWaveStateDuration => SonicWaveStartTime + SonicWaveFireRate * SonicWaveNumShots + SonicWaveExtraWait;
        static int ShootingEndTime => SonicWaveStartTime + SonicWaveFireRate * SonicWaveNumShots;
        static int GlowFadeInTime => 25;
        static int GlowStayTimeBeforeShootingStart => 5;
        static int GlowStayTimeAfterShootingStart => 20;
        static int GlowFadeoutTime => 20;


        public static SoundStyle ShotSFXOld => new SoundStyle("TRAEProject/Assets/Sounds/SonicWave") with { Pitch = -0.5f, MaxInstances = 0 };//in case it is ever needed again
        public static SoundStyle ShotSFX => new("TRAEProject/NewContent/NPCs/Echosphere/EchoLeviathan/EchoLeviathanShot");
        public static SoundStyle DeathSFX => new("TRAEProject/NewContent/NPCs/Echosphere/EchoLeviathan/EchoLeviathanDeath");
        public static SoundStyle HitSFX => EchoStalkerHead.HitSFX.WithPitchOffset(-0.2f).WithVolumeScale(0.5f);
        enum AIState
        {
            Spawning = 0,
            Idle,
            WormMovement,
            SonicWave,
            MoveToPortal,
            EnteringPortal,
            ExitingPortal,
        }
        AIState State { get => (AIState)NPC.ai[0]; set => NPC.ai[0] = (float)value; }
        ref float Timer => ref NPC.ai[1];
        ref float OpacityCutoffFromFront => ref NPC.localAI[1];
        ref float OpacityCutoffFromBehind => ref NPC.localAI[2];
        ref float PortalExitingTimer => ref NPC.localAI[0];
        Vector2 PortalPos
        {
            get => new(NPC.ai[3], NPC.ai[2]); //this MUST be synced!!
            set
            {
                NPC.ai[3] = value.X;
                NPC.ai[2] = value.Y;
            }
        }
        static int[] SegmentWidths => new int[] { 60, 70, 70, 70, 60, 60, 110 };
        const float Phi = 1.61803398875f;
        public override void SetStaticDefaults()
        {
          //  NPCID.Sets.TrailCacheLength[Type] = 300;//12 per advance
          //  NPCID.Sets.TrailingMode[Type] = 0;//every three frames position is stored. counted with localai3!! Don't use localai3 in echo leviathan code!!
        }
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lifeMax = 30000;
            NPC.defense = 45;
            NPC.damage = 120;
            NPC.scale = 1.1f;
            NPC.width = NPC.height = 70;
            NPC.HitSound = SoundID.DD2_DrakinHurt;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0;
            NPC.noTileCollide = true;
            NPC.alpha = 255;
            NPC.DeathSound = DeathSFX;
            NPC.HitSound = HitSFX;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<EchoHeart>(), 2, 1, 1));
        }
        //REMEMBER, ONSPAWN IS ONLY CALLED SERVER SIDE
        public override void OnSpawn(IEntitySource source)
        {

            int body1 = ModContent.NPCType<EchoLeviathanBody1>();
            int body2 = ModContent.NPCType<EchoLeviathanBody2>();
            int body3 = ModContent.NPCType<EchoLeviathanBody3>();
            int tail = ModContent.NPCType<EchoLeviathanTail>();

            int[] types = new int[] { ModContent.NPCType<EchoLeviathanBody1>(), ModContent.NPCType<EchoLeviathanBody1>(),
               ModContent.NPCType<EchoLeviathanBody2>(), ModContent.NPCType<EchoLeviathanBody2>(),
             ModContent.NPCType<EchoLeviathanBody3>(), ModContent.NPCType<EchoLeviathanBody3>(), ModContent.NPCType<EchoLeviathanTail>() };
            //List<int> typeslist = new List<int>(30);
            //for (int i = 0; i < 10; i++)
            //{
            //    typeslist.Add(body1);
            //}
            //for (int i = 0; i < 10; i++)
            //{
            //    typeslist.Add(body2);
            //}
            //for (int i = 0; i < 10; i++)
            //{
            //    typeslist.Add(body3);
            //}
            //typeslist.Add(tail);
            //int[] types = typeslist.ToArray();
            for (int i = 1; i < types.Length + 1; i++)
            {
                NPC.NewNPC(Terraria.Entity.GetSource_NaturalSpawn(), (int)NPC.Center.X - i * 40, (int)NPC.Center.Y, types[i - 1], NPC.whoAmI, NPC.whoAmI, i * 11, i);
            }
            SpawnPortal(NPC.Center, 200);
        }


        public static bool EchoLeviIsIdle(float leviWhoAmI)
        {
            int index = (int)leviWhoAmI;
            if (index < 0 || index >= Main.maxNPCs)
            {
                return false;
            }
            NPC leviNPC = Main.npc[index];
            if (leviNPC.ModNPC is EchoLeviathanHead head)
            {
                return head.State == AIState.Idle;
            }
            return false;
        }
        public override bool PreAI()
        {
            if (State == AIState.Spawning && Timer++ < 100)//100 is time taken to come out of the portal
            {
                NPC.position -= NPC.velocity;
                return false;
            }
            return true;
        }
        public override void AI()
        {
            if (State != AIState.SonicWave)//don't change or update targets while firing
            {
                EchosphereNPCHelper.SearchForSpaceLayerPlayers(NPC);
            }
            if (State == AIState.Spawning)
            {
                State_Spawning();
                return;
            }
            if (State != AIState.EnteringPortal && State != AIState.ExitingPortal && !NPC.HasValidTarget)
            {
                State = AIState.Idle;
            }
            //exit out of idle
            if (NPC.HasValidTarget && State == AIState.Idle)
            {
                Timer = 0;
                NPC.Opacity = 1;
                State = AIState.WormMovement;
            }
            switch (State)
            {

                case AIState.Idle:
                    State_Idle();
                    break;
                case AIState.WormMovement:
                    State_WormMovement();
                    break;
                case AIState.SonicWave:
                    State_SonicWave();
                    break;
                case AIState.MoveToPortal:
                    State_MoveToPortal();
                    break;
                case AIState.EnteringPortal:
                    State_EnteringPortal();
                    break;
                case AIState.ExitingPortal:
                    State_ExitingPortal();
                    break;
            }
            NPC[] segments = SearchForBodySegments();
            for (int i = 0; i < segments.Length; i++)
            {
                NPC segment = segments[i];
                segment.dontTakeDamage = segment.Opacity < .6f;
            }
            NPC.dontTakeDamage = NPC.Opacity < .6f;
        }

        private float PurpleGlowinessAmount => State == AIState.SonicWave ? Utils.GetLerpValue(SonicWaveStartTime - GlowFadeInTime - GlowStayTimeBeforeShootingStart, SonicWaveStartTime - GlowStayTimeBeforeShootingStart, Timer, true) *
            Utils.GetLerpValue(ShootingEndTime + GlowFadeoutTime + GlowStayTimeAfterShootingStart, ShootingEndTime + GlowStayTimeAfterShootingStart, Timer, true) : 0;

        void State_SonicWave()
        {
            Timer++;

            ExitPortal();

            int start = SonicWaveStartTime;
            int fireRate = SonicWaveFireRate;
            int numShots = SonicWaveNumShots;
            int extraWait = SonicWaveExtraWait;
            Player player = Main.player[NPC.target];
            float moveSpeed = Timer >= start && Timer < start + fireRate * numShots ? 4 : 7;

            Vector2 chaseDir = GetChaseDirection(1f);
            NPC.velocity = Slerp(NPC.velocity, chaseDir * moveSpeed, TurningRate);
            if (Timer >= start && (Timer - start) % fireRate == 0 && Timer < start + fireRate * numShots)
            {
                if (Timer == start)
                {
                    SoundEngine.PlaySound(ShotSFX with { MaxInstances = 0 }, NPC.Center);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float shootAngle = NPC.velocity.ToRotation().AngleLerp(chaseDir.ToRotation(), .5f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootAngle.ToRotationVector2() * 12, ModContent.ProjectileType<EchoStalkerSonicWave>(), 75 / 2, 0, Main.myPlayer, 1);
                }
            }
            if (Timer > start + fireRate * numShots + extraWait)
            {
                Timer = 0;
                State = AIState.WormMovement;
            }
            SetSegmentPositionRotationSpriteDirectionAndOpacity();
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);

        }
        private void State_Spawning()
        {
            int totalWidth = SegmentWidths.Sum();
            OpacityCutoffFromFront = totalWidth;
            NPC.direction = 1;
            if (NPC.velocity == default)
            {
                NPC.velocity = new Vector2(0, 7);
            }
            if (NPC.oldPosition != default)
            {
                NPC.Opacity += (NPC.position - NPC.oldPosition).Length() * .05f;
                if (NPC.Opacity == 1)
                    OpacityCutoffFromBehind += (NPC.position - NPC.oldPosition).Length();
            }
            else
            {

                Vector2 spawnDirectionTarget = GetChaseDirection(1);
                NPC.velocity = spawnDirectionTarget * 10;
                PositionSegmentsInLine(-spawnDirectionTarget);
            }
            Timer++;
            if (OpacityCutoffFromBehind > totalWidth)
            {
                Timer = 0;
                State = AIState.WormMovement;
                EchosphereNPCHelper.SearchForSpaceLayerPlayers(NPC);
            }
            SetSegmentPositionRotationSpriteDirectionAndOpacity();
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
            if (NPC.spriteDirection == 0)
                NPC.spriteDirection = -1;

        }
        private void State_MoveToPortal()
        {
            float distToPortal = NPC.Distance(PortalPos);
            if (distToPortal > 20)
            {
                Vector2 direction = NPC.DirectionTo(PortalPos);
                NPC.velocity = Slerp(NPC.velocity, direction * 7, Utils.Remap(distToPortal, 20, 200, .8f, .05f));
            }
            else
            {
                State = AIState.EnteringPortal;
                Timer = 0;
                OpacityCutoffFromFront = SegmentWidths.Sum();
            }
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
            SetSegmentPositionRotationSpriteDirectionAndOpacity();
        }
        private void State_EnteringPortal()
        {
            if (OpacityCutoffFromFront < 0)
            {
                Timer = 0;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int indexForTarget = NPC.target;
                    if (indexForTarget < 0)
                    {
                        indexForTarget = ClosestPlayerConsiderAggro();
                    }
                    Player player = Main.player[indexForTarget];
                    State = AIState.ExitingPortal;
                    State = AIState.SonicWave;
                    Vector2 portalPos = player.Center + player.velocity.SafeNormalize(Vector2.UnitX * NPC.spriteDirection) * 600;
                    SpawnPortal(portalPos, 200);
                    PortalPos = portalPos;
                    Timer = 0;
                    NPC.Center = portalPos;
                    NPC.netUpdate = true;
                }
                OpacityCutoffFromFront = SegmentWidths.Sum();
                OpacityCutoffFromBehind = 0;
            }
            NPC.Opacity -= (NPC.velocity.Length() / SegmentWidths[0]) * 2;
            if (NPC.Opacity == 0)
            {
                OpacityCutoffFromFront -= NPC.velocity.Length();
            }
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
            SetSegmentPositionRotationSpriteDirectionAndOpacity();

        }
        void State_ExitingPortal()
        {
            Timer++;
            if (Timer < 100)
            {
                Vector2 chaseDir = GetChaseDirection(1);
                PositionSegmentsInLine(-chaseDir);
                NPC.velocity = chaseDir * 8;
                NPC.Opacity = 0;
                NPC.position -= NPC.velocity;//don't actually move but don't set velocity to zero
                return;
            }
            if (NPC.Opacity == 0)
            {
                NPC.velocity = Slerp(NPC.velocity, GetChaseDirection(8f), TurningRate);
                NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
            }
            int[] segmentWidths = SegmentWidths;
            NPC.Opacity += NPC.velocity.Length() / segmentWidths[0];
            int totalWidth = segmentWidths.Sum();
            if (NPC.Opacity == 1)
            {
                OpacityCutoffFromBehind += NPC.velocity.Length();
            }
            NPC.rotation = NPC.velocity.ToRotation();
            SetSegmentPositionRotationSpriteDirectionAndOpacity();
            if (OpacityCutoffFromBehind >= totalWidth)
            {
                Timer = 0;
                State = AIState.SonicWave;
            }
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
        }
        void ExitPortal()
        {
            if (Timer < 100)
            {
                Vector2 chaseDir = GetChaseDirection(1);
                PositionSegmentsInLine(-chaseDir);
                NPC.velocity = chaseDir * 8;
                NPC.Opacity = 0;
                NPC.position -= NPC.velocity;//don't actually move but don't set velocity to zero
                return;
            }
            if (NPC.Opacity == 0)
            {
                NPC.velocity = Slerp(NPC.velocity, GetChaseDirection(8f), TurningRate);
                NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
            }
            int[] segmentWidths = SegmentWidths;
            NPC.Opacity += NPC.velocity.Length() / segmentWidths[0];
            if (NPC.Opacity == 1)
            {
                OpacityCutoffFromBehind += NPC.velocity.Length();
            }
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
        }
        private void State_Idle()
        {
            SetSegmentPositionRotationSpriteDirectionAndOpacity();
            NPC[] segments = SearchForBodySegments();
            foreach (NPC segment in segments)
            {
                segment.dontTakeDamage = true;
                segment.Opacity = .5f;
            }
            NPC.dontTakeDamage = true;
            NPC.Opacity = .5f;
            Timer += .01f;
            NPC.velocity = Slerp(NPC.velocity, new Vector2(MathF.Sin(Timer), MathF.Cos(Timer * Phi) * .75f) * 5, TurningRate);
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
        }
        private void State_WormMovement()
        {
            Player player = Main.player[NPC.target];
            Timer++;
            WormMovement(player, player.Center, WormMovementTopSpeed, WormMovementBaseAcceleration);
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
            SetSegmentPositionRotationSpriteDirectionAndOpacity();

            if (Timer >= TimeSpentInWormMovement)
            {
                OpacityCutoffFromBehind = 9999;
                OpacityCutoffFromFront = 9999;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float moveSpeed = 7;
                    int teleportPortalDist = 500;
                    Vector2 portalPos = NPC.Center + Vector2.Normalize(NPC.velocity) * teleportPortalDist;
                    SpawnPortal(portalPos, (int)(NPC.Distance(portalPos) / moveSpeed + 100));//100 to account for the rest of the segments
                    State = AIState.MoveToPortal;
                    PortalPos = portalPos;
                    NPC.netUpdate = true;
                }
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                EchosphereNPCHelper.EchosphereEnemyDeathDust(NPC, 0.7f);
            }
        }
        Vector2 GetChaseDirection(float magnitude)
        {
            if (!NPC.HasValidTarget)
            {
                return new Vector2(0, -magnitude);//go up if no targets
            }
            return Main.player[NPC.target].DirectionFrom(NPC.Center);
        }
        int ClosestPlayerConsiderAggro()
        {
            int target = Main.maxPlayers;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (!player.active || player.dead || target != Main.maxPlayers && player.DistanceSQ(NPC.Center) + player.aggro >= Main.player[target].Distance(NPC.Center) + Main.player[target].aggro)
                {
                    continue;
                }
                target = i;
            }
            return target;
        }
        static Vector2 Slerp(Vector2 from, Vector2 to, float t)
        {
            float fromLen = from.Length();
            float toLen = to.Length();

            if (fromLen < 1e-6f || toLen < 1e-6f)
                return Vector2.Lerp(from, to, t);

            Vector2 fromNorm = from / fromLen;
            Vector2 toNorm = to / toLen;

            float dot = Vector2.Dot(fromNorm, toNorm);
            dot = MathHelper.Clamp(dot, -1f, 1f);
            float theta = (float)Math.Acos(dot);

            Vector2 direction;

            if (theta < 1e-5f)
            {
                direction = Vector2.Lerp(fromNorm, toNorm, t).SafeNormalize(Vector2.UnitX);
            }
            else
            {
                float sinTheta = (float)Math.Sin(theta);
                float a = (float)Math.Sin((1 - t) * theta) / sinTheta;
                float b = (float)Math.Sin(t * theta) / sinTheta;
                direction = a * fromNorm + b * toNorm;
            }

            float length = MathHelper.Lerp(fromLen, toLen, t);
            return direction * length;
        }
        NPC[] SearchForBodySegments()
        {
            NPC[] segments = new NPC[SegmentWidths.Length];
            int segmentsFound = 0;
            List<int> types = new() { ModContent.NPCType<EchoLeviathanBody1>(), ModContent.NPCType<EchoLeviathanBody2>(), ModContent.NPCType<EchoLeviathanBody3>(), ModContent.NPCType<EchoLeviathanTail>() };
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.active && types.Contains(npc.type) && npc.ai[0] == NPC.whoAmI)
                {
                    npc.realLife = npc.whoAmI;
                    npc.dontTakeDamage = false;
                    segments[segmentsFound] = npc;
                    segmentsFound++;
                }
            }
            NPC[] result = new NPC[segments.Length];
            for (int i = 0; i < segmentsFound; i++)
            {
                result[(int)segments[i].ai[2] - 1] = segments[i];
            }
            return result;
        }
        void PositionSegmentsInLine(Vector2 direction)
        {
            NPC[] segments = SearchForBodySegments();

            int segmentCount = segments.Length;
            int[] segmentWidths = SegmentWidths;
            int lengthAcross = 0;
            for (int i = 0; i < segmentCount; i++)
            {
                NPC curSegment = segments[i];
                lengthAcross += segmentWidths[i];
                curSegment.Center = NPC.Center + direction * lengthAcross;
            }
        }


        void SetSegmentPositionRotationSpriteDirectionAndOpacity()
        {
            NPC[] segments = SearchForBodySegments();
            float minAngleDelta = MaxSegmentTurn; // max angle change per segment in radians
            int segmentCount = segments.Length;
            int[] segmentWidths = SegmentWidths;
            float prevRotation = NPC.rotation;
            int lengthAcross = 0;
            float glowiness = PurpleGlowinessAmount;
            int maxLength = segmentWidths.Sum();//sum of all the elements

            for (int i = 0; i < segmentCount; i++)
            {
                int segmentWidth = segmentWidths[i];
                NPC curSegment = segments[i];
                NPC prevSegment = (i == 0) ? NPC : segments[i - 1];

                Vector2 desiredDirection = (prevSegment.Center - curSegment.Center).SafeNormalize(Vector2.UnitY);
                float desiredRotation = desiredDirection.ToRotation();

                float angleDifference = MathHelper.WrapAngle(desiredRotation - prevRotation);
                angleDifference = MathHelper.Clamp(angleDifference, -minAngleDelta, minAngleDelta);
                float constrainedRotation = prevRotation + angleDifference;

                Vector2 offsetDir = constrainedRotation.ToRotationVector2();
                Vector2 segmentCenter = prevSegment.Center - offsetDir * segmentWidth;

                curSegment.Center = segmentCenter;
                curSegment.rotation = constrainedRotation;
                curSegment.spriteDirection = (segmentCenter.X >= prevSegment.Center.X) ? -1 : 1;

                prevRotation = constrainedRotation;


                //for handling opacity and fading from going in and out of portals
                float fadeDuration = segmentWidth - 8;
                float behindOpacity = Utils.GetLerpValue(lengthAcross, lengthAcross + fadeDuration, OpacityCutoffFromBehind, true);
                float frontOpacity = Utils.GetLerpValue(lengthAcross, lengthAcross - fadeDuration, maxLength - OpacityCutoffFromFront - segmentWidths[0], true);
                segments[i].Opacity = behindOpacity * frontOpacity;
                lengthAcross += segmentWidths[i];
                EchoLeviathanBody1.SetPurpleGlowinessAmount(segments[i], glowiness);
            }
            NPC.localAI[3] = glowiness;

        }




        void SetSegmentPositionRotationSpriteDirectionAndOpacity_Old2()
        {
            NPC[] segments = SearchForBodySegments();
            int segmentCount = segments.Length;
            Vector2 lastSegmentCenter = NPC.Center;
            int[] segmentWidths = SegmentWidths;
            int maxLength = segmentWidths.Sum();//sum of all the elements
            int lengthAcross = 0;
            float glowiness = PurpleGlowinessAmount;
            for (int i = 0; i < segmentCount; i++)
            {
                int segmentWidth = segmentWidths[i];
                NPC curSegment = segments[i];
                NPC prevSegment;
                if (i == 0)
                {
                    prevSegment = NPC;
                }
                else
                {
                    prevSegment = segments[i - 1];
                }

                Vector2 offsetDir = (prevSegment.Center - curSegment.Center).SafeNormalize(Vector2.UnitY);
                //need to constrain the direction to avoid issues from it turning too tightly
                Vector2 segmentCenter = prevSegment.Center - offsetDir * segmentWidth;
                float rotation = (lastSegmentCenter - segmentCenter).ToRotation();

                int spriteDir = segmentCenter.X >= lastSegmentCenter.X ? -1 : 1;//    -1 is flip vertically in the drawing code
                segments[i].Center = segmentCenter;
                segments[i].spriteDirection = spriteDir;
                segments[i].rotation = rotation;


                //for handling opacity and fading from going in and out of portals
                float fadeDuration = segmentWidth - 8;
                float behindOpacity = Utils.GetLerpValue(lengthAcross, lengthAcross + fadeDuration, OpacityCutoffFromBehind, true);
                float frontOpacity = Utils.GetLerpValue(lengthAcross, lengthAcross - fadeDuration, maxLength - OpacityCutoffFromFront - segmentWidths[0], true);
                segments[i].Opacity = behindOpacity * frontOpacity;
                lengthAcross += segmentWidths[i];
                lastSegmentCenter = segmentCenter;
                EchoLeviathanBody1.SetPurpleGlowinessAmount(segments[i], glowiness);
            }
            NPC.localAI[3] = glowiness;
        }




        static Vector2 ConstrainAngle(float minAngle, Vector2 angleCenter, Vector2 angleToLimit)
        {
            float dot = Vector2.Dot(angleCenter, angleToLimit);
            float dotNormal = Vector2.Dot(angleToLimit, new Vector2(-angleCenter.Y, angleCenter.X));
            float dotThreshold = MathF.Cos(minAngle * .5f);

            if (dot < dotThreshold)
            {
                return angleToLimit;
            }
            if (dotNormal < 0)
            {
                return angleCenter.RotatedBy(-minAngle * .5f);
            }
            return angleCenter.RotatedBy(minAngle * .5f);
        }


        void SetSegmentPositionRotationSpriteDirectionAndOpacity_Old()
        {
            NPC[] segments = SearchForBodySegments();

            int segmentCount = segments.Length;
            Vector2 lastSegmentCenter = NPC.Center;
            int[] segmentWidths = SegmentWidths;
            int maxLength = segmentWidths.Sum();//sum of all the elements
            int lengthAcross = 0;
            float glowiness = PurpleGlowinessAmount;
            for (int i = 0; i < segmentCount; i++)
            {
                int segmentWidth = segmentWidths[i];
                Vector2 segmentCenter = NPC.oldPos[(int)segments[i].ai[1]] + NPC.Size / 2f;
                float rotation = (lastSegmentCenter - segmentCenter).ToRotation();
                segmentCenter = lastSegmentCenter - rotation.ToRotationVector2() * segmentWidth;
                rotation = (lastSegmentCenter - segmentCenter).ToRotation() + MathF.PI / 2f;
                Vector2 snapPos = segmentCenter;
                int spriteDir = segmentCenter.X >= lastSegmentCenter.X ? -1 : 1;//    -1 should be flip vertically remember!!
                segments[i].Center = snapPos;
                segments[i].spriteDirection = spriteDir;
                segments[i].rotation = rotation - MathF.PI / 2;

                float fadeDuration = segmentWidth - 8;
                float behindOpacity = Utils.GetLerpValue(lengthAcross, lengthAcross + fadeDuration, OpacityCutoffFromBehind, true);
                float frontOpacity = Utils.GetLerpValue(lengthAcross, lengthAcross - fadeDuration, maxLength - OpacityCutoffFromFront - segmentWidths[0], true);
                segments[i].Opacity = behindOpacity * frontOpacity;
                // Main.NewText($"behind opacity: {behindOpacity}, cutoffFromBehind: {OpacityCutoffFromBehind}");
                //Main.NewText($"front opacity: {frontOpacity}, cutoffFromFront: {OpacityCutoffFromFront}");
                lengthAcross += segmentWidths[i];
                lastSegmentCenter = segmentCenter;
                EchoLeviathanBody1.SetPurpleGlowinessAmount(segments[i], glowiness);
            }
            NPC.localAI[3] = glowiness;
        }
        public static void SpectralDraw(NPC NPC, SpriteBatch spriteBatch, Vector2 screenPos, Texture2D texture)
        {
            float opacity = NPC.Opacity * .5f;
            if(opacity <= 0)
            {
                return;
            }
            Color drawColor = new Color(255, 52, 242, 0) * opacity;
            for (int i = 0; i < 4; i++)
            {
                float rotation = Main.GlobalTimeWrappedHourly * 5 + i * .25f * MathF.Tau;
                Vector2 offset = rotation.ToRotationVector2() * 2;
                if (MathF.Abs(offset.X) > MathF.Abs(offset.Y))
                {
                    offset.X = MathF.Max(MathF.Abs(offset.X), 2) * MathF.Sign(offset.X);
                }
                else
                {
                    offset.Y = MathF.Max(MathF.Abs(offset.Y), 2) * MathF.Sign(offset.Y);
                }
                offset = offset.RotatedBy(NPC.rotation);
                spriteBatch.Draw(texture, NPC.Center - screenPos + offset, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
        }
        public static void SpectralDrawVerticalFlip(NPC NPC, SpriteBatch spriteBatch, Vector2 screenPos, Texture2D texture)
        {
            float opacity = NPC.Opacity * .5f;
            if (opacity <= 0)
            {
                return;
            }
            Color drawColor = new Color(255, 52, 242, 0) * opacity;
            for (int i = 0; i < 4; i++)
            {
                float rotation = Main.GlobalTimeWrappedHourly * 5 + i * .25f * MathF.Tau;
                Vector2 offset = rotation.ToRotationVector2() * 2;
                if (MathF.Abs(offset.X) > MathF.Abs(offset.Y))
                {
                    offset.X = MathF.Max(MathF.Abs(offset.X), 2) * MathF.Sign(offset.X);
                }
                else
                {
                    offset.Y = MathF.Max(MathF.Abs(offset.Y), 2) * MathF.Sign(offset.Y);
                }
                offset = offset.RotatedBy(NPC.rotation);
                spriteBatch.Draw(texture, NPC.Center - screenPos + offset, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoLeviathan/EchoLeviathanJaw").Value;
            if (NPC.Opacity != 1 && State == AIState.Idle)
            {
                SpectralDrawVerticalFlip(NPC, spriteBatch, screenPos, texture);
                texture = TextureAssets.Npc[Type].Value;
                SpectralDrawVerticalFlip(NPC, spriteBatch, screenPos, texture);
            }
            else
            {
                drawColor *= NPC.Opacity;
                Vector2 origin = texture.Size() / 2;
                GetHeadRotationOffset(out float headRot, out float jawRot, out Vector2 jawOffset, out Vector2 headOffset, origin);
                Vector2 offset = (NPC.rotation - MathF.PI * 0.5f * NPC.spriteDirection).ToRotationVector2() * 6;
                Main.EntitySpriteDraw(texture, NPC.Center - screenPos + offset + jawOffset, null, drawColor, NPC.rotation + headRot, origin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
                texture = TextureAssets.Npc[Type].Value;
                Main.EntitySpriteDraw(texture, NPC.Center - screenPos + offset + headOffset, null, drawColor, NPC.rotation + jawRot, origin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
                if (State == AIState.SonicWave)
                {
                    texture = ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoLeviathan/EchoLeviathanHeadGlow").Value;
                    float glowiness = NPC.localAI[3];// one minus??? wasn't working properly without it for some reason.
                    glowiness *= NPC.Opacity;
                    EchosphereNPCHelper.DrawEchoWormBlur(texture, NPC.Center - screenPos + offset + headOffset, glowiness, NPC.rotation + jawRot, origin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
                }
            }
            return false;
        }
        void GetHeadRotationOffset(out float headRot, out float jawRot, out Vector2 jawOffset, out Vector2 headOffset, Vector2 origin)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;

            Vector2 pivot = new(24, 50);
            if (NPC.spriteDirection == -1)
            {
                pivot.Y = texture.Height - pivot.Y;
            }
            headRot = 0;
            jawRot = 0;
            float animationProgress;
            if (State != AIState.SonicWave)
            {
                jawOffset = default;
                headOffset = default;
                return;
            }
            float mouthOpenAnimationDuration = 10;
            float mouthCloseAnimationDuration = 20;
            float readyShotDuration = MathF.Max(1, SonicWaveStartTime - mouthOpenAnimationDuration);
            if (Timer <= readyShotDuration)//ready shot progress
            {
                animationProgress = 1 + Timer / readyShotDuration;
            }
            else if (Timer <= SonicWaveStartTime)//mouth open progress
            {
                animationProgress = Utils.Remap(Timer, readyShotDuration, SonicWaveStartTime, 2, 3);
            }
            else if (Timer <= SonicWaveStartTime + SonicWaveFireRate * SonicWaveNumShots)//wobble progress
            {
                animationProgress = Utils.Remap(Timer, SonicWaveStartTime, SonicWaveStartTime + SonicWaveFireRate * SonicWaveNumShots, 3, 4);
            }
            else if (Timer < SonicWaveStartTime + SonicWaveFireRate * SonicWaveNumShots + mouthCloseAnimationDuration)//mouth close progress
            {
                animationProgress = Utils.Remap(Timer, SonicWaveStartTime + SonicWaveFireRate * SonicWaveNumShots, SonicWaveStartTime + SonicWaveFireRate * SonicWaveNumShots + mouthCloseAnimationDuration, 4, 5);
            }
            else
            {
                animationProgress = 5;
            }
            float maxMouthOpen = .35f;
            float maxMouthClose = 0.05f;
            switch ((int)animationProgress)
            {
                case 1://ready shot
                    animationProgress %= 1;
                    animationProgress = EaseInOut(animationProgress);
                    animationProgress *= maxMouthClose;
                    headRot += animationProgress * -NPC.spriteDirection;
                    jawRot -= animationProgress * -NPC.spriteDirection;

                    break;
                case 2://open mouth
                    headRot += maxMouthClose * -NPC.spriteDirection;
                    jawRot -= maxMouthClose * -NPC.spriteDirection;
                    if (animationProgress > 2.75f)
                    {
                        animationProgress %= 1;
                        animationProgress = Utils.Remap(animationProgress, 0.75f, 1, 0, 1);
                        animationProgress = EaseInOut(animationProgress) * maxMouthOpen;
                        headRot -= animationProgress * -NPC.spriteDirection;
                        jawRot += animationProgress * -NPC.spriteDirection;
                    }
                    else
                    {
                        animationProgress %= 1;
                        animationProgress += maxMouthClose;
                    }
                    break;
                case 3://wobble during shooting
                    animationProgress = WobblyEffect(animationProgress * 2f % 1) * .65f + maxMouthOpen;
                    headRot -= animationProgress * -NPC.spriteDirection;
                    jawRot += animationProgress * -NPC.spriteDirection;
                    break;
                case 4://close mouth
                    animationProgress = (1 - EaseInOut(animationProgress % 1)) * maxMouthOpen;
                    headRot -= animationProgress * -NPC.spriteDirection;
                    jawRot += animationProgress * -NPC.spriteDirection;
                    break;
                default:
                    animationProgress = 0;
                    break;
            }
            //jawOffset = originToPivot.RotatedBy(jawRot);
            //headOffset = originToPivot.RotatedBy(headRot);
            //Vector2 moveToProperPosition = -originToPivot;
            //jawOffset += moveToProperPosition;
            //moveToProperPosition = -originToPivot;
            //headOffset += moveToProperPosition;

            Vector2 pivotOffset = (pivot - origin).RotatedBy(NPC.rotation);
            Vector2 rotatedOffset = pivotOffset.RotatedBy(jawRot);
            jawOffset = rotatedOffset - pivotOffset;
            rotatedOffset = pivotOffset.RotatedBy(headRot);
            headOffset = rotatedOffset - pivotOffset;

        }
        static float WobblyEffect(float progress)
        {
            progress = MathF.Sin(10f * progress / MathF.PI);
            return progress * progress * 0.25f;
        }
        static float EaseInOut(float progress)
        {
            return -MathF.Cos(progress * MathF.PI) * 0.5f + 0.5f;
        }
        static float Magnitude(Vector2 vec)
        {
            return MathF.Abs(vec.X) + MathF.Abs(vec.Y);
        }
        void WormMovement(Player player, Vector2 targetPos, float topSpeed = 5, float acceleration = .3f)
        {
            Vector2 npcTilePos = new(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float maxSpeedX = player.position.X + player.width / 2;
            float maxSpeedY = player.position.Y + player.height / 2;
            maxSpeedX = (int)(maxSpeedX / 16f) * 16;
            maxSpeedY = (int)(maxSpeedY / 16f) * 16;
            npcTilePos.X = (int)(npcTilePos.X / 16f) * 16;
            npcTilePos.Y = (int)(npcTilePos.Y / 16f) * 16;
            maxSpeedX -= npcTilePos.X;
            maxSpeedY -= npcTilePos.Y;
            float distLength = (float)Math.Sqrt(maxSpeedX * maxSpeedX + maxSpeedY * maxSpeedY);
            float num71 = Math.Abs(maxSpeedX);
            float num72 = Math.Abs(maxSpeedY);
            float normalizingFactor = topSpeed / distLength;
            maxSpeedX *= normalizingFactor;
            maxSpeedY *= normalizingFactor;

            bool goDown = false;
            if ((NPC.velocity.X > 0f && maxSpeedX < 0f || NPC.velocity.X < 0f && maxSpeedX > 0f || NPC.velocity.Y > 0f && maxSpeedY < 0f || NPC.velocity.Y < 0f && maxSpeedY > 0f) && Magnitude(NPC.velocity) > acceleration / 2f && distLength < 300f)
            {
                goDown = true;
                if (Magnitude(NPC.velocity) < topSpeed)
                {
                    NPC.velocity *= 1.1f;
                }
            }
            if (NPC.position.Y > targetPos.Y || targetPos.Y / 16f > Main.worldSurface || player.dead)
            {
                goDown = true;
                if (Math.Abs(NPC.velocity.X) < topSpeed / 2f)
                {
                    if (NPC.velocity.X == 0f)
                    {
                        NPC.velocity.X -= NPC.direction;
                    }
                    NPC.velocity.X *= 1.1f;
                }
                else if (NPC.velocity.Y > 0f - topSpeed)
                {
                    NPC.velocity.Y -= acceleration;
                }
            }


            if (!goDown)
            {
                if (NPC.velocity.X > 0f && maxSpeedX > 0f || NPC.velocity.X < 0f && maxSpeedX < 0f || NPC.velocity.Y > 0f && maxSpeedY > 0f || NPC.velocity.Y < 0f && maxSpeedY < 0f)
                {
                    if (NPC.velocity.X < maxSpeedX)
                    {
                        NPC.velocity.X += acceleration;
                    }
                    else if (NPC.velocity.X > maxSpeedX)
                    {
                        NPC.velocity.X -= acceleration;
                    }
                    if (NPC.velocity.Y < maxSpeedY)
                    {
                        NPC.velocity.Y += acceleration;
                    }
                    else if (NPC.velocity.Y > maxSpeedY)
                    {
                        NPC.velocity.Y -= acceleration;
                    }
                    if (Math.Abs(maxSpeedY) < topSpeed * 0.2 && (NPC.velocity.X > 0f && maxSpeedX < 0f || NPC.velocity.X < 0f && maxSpeedX > 0f))
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y += acceleration * 2f;
                        }
                        else
                        {
                            NPC.velocity.Y -= acceleration * 2f;
                        }
                    }
                    if (Math.Abs(maxSpeedX) < topSpeed * 0.2 && (NPC.velocity.Y > 0f && maxSpeedY < 0f || NPC.velocity.Y < 0f && maxSpeedY > 0f))
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X += acceleration * 2f;
                        }
                        else
                        {
                            NPC.velocity.X -= acceleration * 2f;
                        }
                    }
                }
                else if (num71 > num72)
                {
                    if (NPC.velocity.X < maxSpeedX)
                    {
                        NPC.velocity.X += acceleration * 1.1f;
                    }
                    else if (NPC.velocity.X > maxSpeedX)
                    {
                        NPC.velocity.X -= acceleration * 1.1f;
                    }
                    if (Magnitude(NPC.velocity) < topSpeed * 0.5)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y += acceleration;
                        }
                        else
                        {
                            NPC.velocity.Y -= acceleration;
                        }
                    }
                }
                else
                {
                    if (NPC.velocity.Y < maxSpeedY)
                    {
                        NPC.velocity.Y += acceleration * 1.1f;
                    }
                    else if (NPC.velocity.Y > maxSpeedY)
                    {
                        NPC.velocity.Y -= acceleration * 1.1f;
                    }
                    if (Magnitude(NPC.velocity) < topSpeed * 0.5)
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X += acceleration;
                        }
                        else
                        {
                            NPC.velocity.X -= acceleration;
                        }
                    }
                }
            }
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.alpha < 240 && target.Hitbox.Intersects(Utils.CenteredRectangle(NPC.Center, new Vector2(50)));//hitbox of width and height 50 for damaging players
        }
        void SpawnPortal(Vector2 position, int duration)
        {
            Projectile.NewProjectile(NPC.GetSource_FromAI(), position, default, ModContent.ProjectileType<EchoLeviathanPortal>(), -1, 0, -1, duration);
        }
        public override void Load()
        {
            On_NPC.NewNPC += ChangeSpawnPosition;
        }

        private int ChangeSpawnPosition(On_NPC.orig_NewNPC orig, IEntitySource source, int X, int Y, int Type, int Start, float ai0, float ai1, float ai2, float ai3, int Target)
        {
            if (Type != ModContent.NPCType<EchoLeviathanHead>())
            {
                return orig(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
            }
            if (Target < 0 || Target >= Main.maxPlayers)//if no target when spawning, search for airborne players
            {
                Target = EchosphereNPCHelper.SearchForSpaceLayerPlayers(new Vector2(X, Y));
            }
            if (Target < 0 || Target >= Main.maxPlayers)//if no space target detected, check for valid targets that aren't necessarily on the space
            {
                Vector2 from = new(X, Y);
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (!player.active || player.dead || (Target >= 0 && Target < Main.maxPlayers) && player.DistanceSQ(from) + player.aggro < Main.player[Target].DistanceSQ(from) + Main.player[Target].aggro)
                    {
                        continue;
                    }
                    Target = i;
                }
            }
            if (Target < 0 || Target >= Main.maxPlayers)//if all target searches failed, spawn with no position override
            {
                return orig(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
            }

            //override spawning position to be around player
            Player plr = Main.player[Target];
            X = (int)plr.Center.X;
            Y = (int)plr.Center.Y;
            X += Main.rand.Next(90, 120) * (Main.rand.Next(0, 2) * 2 - 1);
            Y += Main.rand.Next(90, 120) * (Main.rand.Next(0, 2) * 2 - 1);
            return orig(source, X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
        }
        public static bool IsHeadImmuneToItem(float headNPCIndex, int playerIndex)
        {
            int index = (int)headNPCIndex;
            if (!Main.npc.IndexInRange(index))
            {
                return true;
            }
            NPC npc = Main.npc[index];
            if (npc.type != ModContent.NPCType<EchoLeviathanHead>())
            {
                return true;
            }
            Player player = Main.player[playerIndex];
            return player.meleeNPCHitCooldown[index] > 0;
        }




        public override void ModifyHoverBoundingBox(ref Rectangle boundingBox)
        {
            if (NPC.alpha >= 254)
            {
                boundingBox.X = -1000;//put it out of bounds of the map so it is never displayed
            }
        }
    }
}
