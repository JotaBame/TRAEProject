using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Projectiles;
using TRAEProject.NewContent.Projectiles.EchoLeviathanPortal;

namespace TRAEProject.NewContent.NPCs.EchoLeviathan
{
    /// <summary>
    /// UNFINISHED!!
    /// </summary>
    internal class EchoLeviathanHead : ModNPC
    {
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
        ref float SpawnTimer => ref NPC.localAI[0];
        Vector2 PortalPos
        {
            get => new Vector2(NPC.ai[3], NPC.ai[2]); //this MUST be synced!!
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
            NPCID.Sets.TrailCacheLength[Type] = 300;//12 per advance
            NPCID.Sets.TrailingMode[Type] = 0;//every three frames position is stored. counted with localai3!! Don't use localai3 in echo leviathan code!!
        }
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lifeMax = 18000;
            NPC.defense = 25;
            NPC.damage = 120;
            NPC.width = NPC.height = 70;
            NPC.HitSound = SoundID.DD2_BetsyHurt;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0;
            NPC.noTileCollide = true;
            NPC.alpha = 255;
        }

        //REMEMBER, THIS ONLY CALLED ON SIDE THAT SPAWNED THE NPC
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
                NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)NPC.Center.X - i * 40, (int)NPC.Center.Y, types[i - 1], NPC.whoAmI, NPC.whoAmI, i * 11, i);
            }
            SpawnPortal(NPC.Center, 200);
        }
        public override bool PreAI()
        {
            if (SpawnTimer++ < 100)
            {
                NPC.position -= NPC.velocity;
                return false;
            }
            return true;
        }
        public override void AI()
        {
            SearchForPlayersConsiderAggro();
            if (State == AIState.Spawning)
            {
                State_Spawning();
                return;
            }
            if (State != AIState.EnteringPortal && State != AIState.ExitingPortal && (NPC.target < 0 || NPC.target >= Main.maxNPCs))
            {
                State = AIState.Idle;
            }
            //exit out of idle
            if (NPC.target != -1 && State == AIState.Idle)
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

        void State_SonicWave()
        {
            Timer++;
            int start = 30;
            int fireRate = 10;
            int numShots = 3;
            int extraWait = 50;
            Player player = Main.player[NPC.target];
            float moveSpeed = Timer >= start && Timer < start + fireRate * numShots ? 4 : 7;
            NPC.velocity = Vector2.Lerp(NPC.velocity, NPC.DirectionTo(player.Center) * moveSpeed, 0.1f);
            if (Timer >= start && (Timer - start) % fireRate == 0 && Timer < start + fireRate * numShots)
            {
                if (Timer == start)
                {
                    SoundEngine.PlaySound(new SoundStyle("TRAEProject/Assets/Sounds/SonicWave") with { MaxInstances = 0, Pitch = -.5f }, NPC.Center);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float shootAngle = NPC.velocity.ToRotation().AngleLerp((player.Center - NPC.Center).ToRotation(), .5f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootAngle.ToRotationVector2() * 10, ModContent.ProjectileType<EchoStalkerSonicWave>(), 75 / 2, 0, Main.myPlayer, 1);
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
                Vector2 spawnDirectionTarget = NPC.DirectionTo(Main.player[ClosestPlayerConsiderAggro()].Center);
                NPC.velocity = spawnDirectionTarget * 10;
            }
            Timer++;
            if (OpacityCutoffFromBehind > totalWidth)
            {
                Timer = 0;
                State = AIState.WormMovement;
                SearchForPlayersConsiderAggro();
            }
            SetSegmentPositionRotationSpriteDirectionAndOpacity();
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
        }
        private void State_MoveToPortal()
        {
            float distToPortal = NPC.Distance(PortalPos);
            if (distToPortal > 20)
            {
                Vector2 direction = NPC.DirectionTo(PortalPos);
                NPC.velocity = Vector2.Lerp(NPC.velocity, direction * 7, Utils.Remap(distToPortal, 20, 200, .8f, .05f));
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
                    if ( indexForTarget < 0)
                    {
                        indexForTarget = ClosestPlayerConsiderAggro();
                    }
                    Player player = Main.player[indexForTarget];
                    State = AIState.ExitingPortal;
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
            NPC.Opacity -= NPC.velocity.Length() * 2;
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
                NPC.Opacity = 0;
                NPC.position -= NPC.velocity;
                return;
            }
            if (NPC.Opacity == 0)
            {
                if (NPC.target != -1)
                {
                    NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * 8;
                }
                else
                {
                    Player plr = Main.player[ClosestPlayerConsiderAggro()];
                    NPC.velocity = NPC.DirectionTo(plr.Center - new Vector2(0, 350)) * 8;
                }
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
            NPC.velocity = Vector2.Lerp(NPC.velocity, new Vector2(MathF.Sin(Timer), MathF.Cos(Timer * Phi) * .75f) * 5, .1f);
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
        }
        private void State_WormMovement()
        {
            Player player = Main.player[NPC.target];
            Timer++;
            WormMovement(player, player.Center, 7);
            NPC.rotation = NPC.velocity.ToRotation();
            NPC.spriteDirection = MathF.Sign(NPC.velocity.X);
            SetSegmentPositionRotationSpriteDirectionAndOpacity();

            if (Timer >= 460)
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
        int ClosestPlayerConsiderAggro()
        {
            int target = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (!player.active || player.dead || target != -1 && player.DistanceSQ(NPC.Center) + player.aggro >= Main.player[target].Distance(NPC.Center) + Main.player[target].aggro)
                {
                    continue;
                }
                target = i;
            }
            return target;
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
        void SearchForPlayersConsiderAggro()
        {
            if (Main.npc.IndexInRange(NPC.target))
            {
                Player player = Main.player[NPC.target];
                if (!Collision.SolidTiles(player.BottomLeft, player.width, 16))//current player is valid
                {
                    return;
                }
            }
            int target = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                //readability!!!!!!!!!!!!!!!!!!!!!!!
                if (!player.active || player.dead || Collision.SolidTiles(player.BottomLeft, player.width, 16) || target != -1 && player.DistanceSQ(NPC.Center) + player.aggro < Main.player[target].Distance(NPC.Center) + Main.player[target].aggro)
                {
                    continue;
                }
                target = i;
            }
            NPC.target = target;
        }
        void SetSegmentPositionRotationSpriteDirectionAndOpacity()
        {
            NPC[] segments = SearchForBodySegments();

            int segmentCount = segments.Length;
            Vector2 lastSegmentCenter = NPC.Center;
            int[] segmentWidths = SegmentWidths;
            int maxLength = segmentWidths.Sum();//sum of all the elements
            int lengthAcross = 0;
            for (int i = 0; i < segmentCount; i++)
            {
                int segmentWidth = segmentWidths[i];
                Vector2 segmentCenter = NPC.oldPos[(int)segments[i].ai[1]] + NPC.Size / 2f;
                float rotation = (lastSegmentCenter - segmentCenter).ToRotation();
                segmentCenter = lastSegmentCenter - rotation.ToRotationVector2() * segmentWidth;
                rotation = (lastSegmentCenter - segmentCenter).ToRotation() + MathF.PI / 2f;
                Vector2 snapPos = segmentCenter;
                int spriteDir = (segmentCenter.X >= lastSegmentCenter.X) ? -1 : 1;//    -1 should be flip vertically remember!!
                segments[i].Center = snapPos;
                segments[i].spriteDirection = spriteDir;
                segments[i].rotation = rotation - MathF.PI / 2;

                float fadeDuration = 30;
                float behindOpacity = Utils.GetLerpValue(lengthAcross, lengthAcross + fadeDuration, OpacityCutoffFromBehind, true);
                float frontOpacity = Utils.GetLerpValue(lengthAcross, lengthAcross - fadeDuration, maxLength - OpacityCutoffFromFront - segmentWidths[0], true);
                segments[i].Opacity = behindOpacity * frontOpacity;
                // Main.NewText($"behind opacity: {behindOpacity}, cutoffFromBehind: {OpacityCutoffFromBehind}");
                //Main.NewText($"front opacity: {frontOpacity}, cutoffFromFront: {OpacityCutoffFromFront}");
                lengthAcross += segmentWidths[i];
                lastSegmentCenter = segmentCenter;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            drawColor *= NPC.Opacity;
            Main.EntitySpriteDraw(texture, NPC.Center - screenPos, null, drawColor, NPC.rotation, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
            texture = ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/EchoLeviathan/EchoLeviathanJaw").Value;
            Main.EntitySpriteDraw(texture, NPC.Center - screenPos, null, drawColor, NPC.rotation, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);

            return false;
        }
        static float Magnitude(Vector2 vec)
        {
            return MathF.Abs(vec.X) + MathF.Abs(vec.Y);
        }
        void WormMovement(Player player, Vector2 targetPos, float topSpeed = 5, float acceleration = .3f)
        {
            Vector2 vector5 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float maxSpeedX = player.position.X + player.width / 2;
            float maxSpeedY = player.position.Y + player.height / 2;
            maxSpeedX = (int)(maxSpeedX / 16f) * 16;
            maxSpeedY = (int)(maxSpeedY / 16f) * 16;
            vector5.X = (int)(vector5.X / 16f) * 16;
            vector5.Y = (int)(vector5.Y / 16f) * 16;
            maxSpeedX -= vector5.X;
            maxSpeedY -= vector5.Y;
            float num68 = (float)Math.Sqrt(maxSpeedX * maxSpeedX + maxSpeedY * maxSpeedY);
            float num71 = Math.Abs(maxSpeedX);
            float num72 = Math.Abs(maxSpeedY);
            float normalizingFactor = topSpeed / num68;
            maxSpeedX *= normalizingFactor;
            maxSpeedY *= normalizingFactor;

            bool goDown = false;
            if (((NPC.velocity.X > 0f && maxSpeedX < 0f) || (NPC.velocity.X < 0f && maxSpeedX > 0f) || (NPC.velocity.Y > 0f && maxSpeedY < 0f) || (NPC.velocity.Y < 0f && maxSpeedY > 0f)) && Magnitude(NPC.velocity) > acceleration / 2f && num68 < 300f)
            {
                goDown = true;
                if (Magnitude(NPC.velocity) < topSpeed)
                {
                    NPC.velocity *= 1.1f;
                }
            }
            if (NPC.position.Y > targetPos.Y || (targetPos.Y / 16f) > Main.worldSurface || player.dead)
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
                if ((NPC.velocity.X > 0f && maxSpeedX > 0f) || (NPC.velocity.X < 0f && maxSpeedX < 0f) || (NPC.velocity.Y > 0f && maxSpeedY > 0f) || (NPC.velocity.Y < 0f && maxSpeedY < 0f))
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
                    if (Math.Abs(maxSpeedY) < topSpeed * 0.2 && ((NPC.velocity.X > 0f && maxSpeedX < 0f) || (NPC.velocity.X < 0f && maxSpeedX > 0f)))
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
                    if (Math.Abs(maxSpeedX) < topSpeed * 0.2 && ((NPC.velocity.Y > 0f && maxSpeedY < 0f) || (NPC.velocity.Y < 0f && maxSpeedY > 0f)))
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
                    if ((Magnitude(NPC.velocity)) < topSpeed * 0.5)
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
                    if ((Magnitude(NPC.velocity)) < topSpeed * 0.5)
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
            return target.Hitbox.Intersects(Utils.CenteredRectangle(NPC.Center, new Vector2(50)));//hitbox of width and height 50 for damaging players
        }
        void SpawnPortal(Vector2 position, int duration)
        {
            Projectile.NewProjectile(NPC.GetSource_FromAI(), position, default, ModContent.ProjectileType<EchoLeviathanPortal>(), -1, 0, -1, duration);
        }
    }
}
