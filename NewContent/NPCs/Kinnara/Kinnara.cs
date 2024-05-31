using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.NPCs.Boss.Plantera;
using TRAEProject.NewContent.Projectiles.KinnaraFeather;

namespace TRAEProject.NewContent.NPCs.Kinnara
{
    public class Kinnara : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;

        }
        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 34;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 1000f;
            NPC.aiStyle = -1;
            AIType = 0;
            NPC.defense = 22;
            NPC.lifeMax = 300;
            NPC.damage = 80;
            NPC.knockBackResist = 0.4f;
            NPC.noGravity = true;
        }
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (!NPC.confused && player.Distance(NPC.Center) < 9000)//attack minimum distance
            {
                NPC.ai[0]++;
                int firerate = 45;
                int rounds = 4;
                float feathersPerRound = 5;
                int extraWaitTime = 3;
                if (NPC.ai[0] % firerate == 0 && NPC.ai[0] < firerate * rounds)
                { 
                    for (int i = 0; i < feathersPerRound; i++)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            //ca
                            SpreadShot_VampireKnivesCode(player, i);
                        }
                    }
                }
                NPC.ai[0] %= firerate * rounds + extraWaitTime;
            }
            else
            {
                NPC.ai[0] = 0;
            }

            Movement();
            NPC.rotation = NPC.velocity.X * .1f;
            NPC.spriteDirection = -NPC.direction;

        }

        private void SpreadShot_SimpleRandom(Player player)
        {
            Vector2 velocity = NPC.DirectionTo(player.Center).RotatedByRandom(1.2f) * 5;//5 is shootspeed, 1.2f is max spread
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<KinnaraFeather>(), 40, 0);
        }
        private void SpreadShot_VampireKnivesCode(Player player, int i)
        {
            float posX = NPC.Center.X;
            float posY = NPC.Center.Y;
            float speedX = player.Center.X - posX;
            float speedY = player.Center.Y - posY;
            float random = i;
            speedX += Main.rand.Next(-35, 36) * random;
            speedY += Main.rand.Next(-35, 36) * random;
            float normalizingFactor = MathF.Sqrt(speedX * speedX + speedY * speedY);
            normalizingFactor = 10 / normalizingFactor;//10 is shootspeed
            speedX *= normalizingFactor;
            speedY *= normalizingFactor;
            Projectile.NewProjectile(NPC.GetSource_FromAI(), posX, posY, speedX, speedY, ModContent.ProjectileType<KinnaraFeather>(), 40, 0);
        }
        private void SpreadShot_Old(float feathersPerRound, int i)
        {
            Player player = Main.player[NPC.target];
            float spread = 1;
            Vector2 velocity = NPC.DirectionTo(player.Center);
            float t = Utils.GetLerpValue(0, (feathersPerRound - 1) / 2, i, true) * Utils.GetLerpValue(feathersPerRound - 1, (feathersPerRound - 1) / 2, i, true);
            t = 1 - t;
            t *= t;
            t = 1 - t;
            velocity *= MathHelper.Lerp(5, 8, t);//shootspeed
            velocity = velocity.RotatedBy(Utils.Remap(i, 0, feathersPerRound - 1, -spread / 2, spread / 2));
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<KinnaraFeather>(), 40, 0);
        }
        private void SpreadShot_OldRandom(float feathersPerRound)
        {
            float i = Main.rand.NextFloat(feathersPerRound - 1);
            Player player = Main.player[NPC.target];
            float spread = 1;
            Vector2 velocity = NPC.DirectionTo(player.Center);
            float t = Utils.GetLerpValue(0, (feathersPerRound - 1) / 2, i, true) * Utils.GetLerpValue(feathersPerRound - 1, (feathersPerRound - 1) / 2, i, true);
            t = 1 - t;
            t *= t;
            t = 1 - t;
            velocity *= MathHelper.Lerp(5, 8, t);//shootspeed
            velocity = velocity.RotatedBy(Utils.Remap(i, 0, feathersPerRound - 1, -spread / 2, spread / 2));
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<KinnaraFeather>(), 40, 0);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneNormalSpace)
            {
                return float.PositiveInfinity;//placeholder value
            }
            return 0;
        }
        private void Movement()
        {
            float maxVelX = 9;
            float accelX = .3f;
            float maxVelY = 5;
            float accelY = 0.4f;
            if (NPC.collideX)
            {
                NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
                if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
                {
                    NPC.velocity.X = 2f;
                }
                if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
                {
                    NPC.velocity.X = -2f;
                }
            }
            if (NPC.collideY)
            {
                NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
                if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
                {
                    NPC.velocity.Y = 1f;
                }
                if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
                {
                    NPC.velocity.Y = -1f;
                }
            }
            if (NPC.confused)
            {
                NPC.direction *= -1;
            }
            if (NPC.direction == -1 && NPC.velocity.X > -maxVelX)
            {
                NPC.velocity.X -= accelX;
                if (NPC.velocity.X > maxVelX)
                {
                    NPC.velocity.X -= accelX;
                }
                else if (NPC.velocity.X > 0f)
                {
                    NPC.velocity.X -= accelX * .5f;
                }
                if (NPC.velocity.X < -maxVelX)
                {
                    NPC.velocity.X = -maxVelX;
                }
            }
            else if (NPC.direction == 1 && NPC.velocity.X < maxVelX)
            {
                NPC.velocity.X += 0.1f;
                if (NPC.velocity.X < -maxVelX)
                {
                    NPC.velocity.X += accelX;
                }
                else if (NPC.velocity.X < 0f)
                {
                    NPC.velocity.X += accelX * .5f;
                }
                if (NPC.velocity.X > maxVelX)
                {
                    NPC.velocity.X = maxVelX;
                }
            }
            int directionY = NPC.directionY;
            if (NPC.confused)
            {
                directionY *= -1;
            }
            if (directionY == -1 && NPC.velocity.Y > -maxVelY)
            {
                NPC.velocity.Y -= accelY;
                if (NPC.velocity.Y > maxVelY)
                {
                    NPC.velocity.Y -= accelY * 1.25f;
                }
                else if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y += accelY * .75f;
                }
                if (NPC.velocity.Y < -maxVelY)
                {
                    NPC.velocity.Y = -maxVelY;
                }
            }
            else if (directionY == 1 && NPC.velocity.Y < maxVelY)
            {
                NPC.velocity.Y += accelY;
                if (NPC.velocity.Y < -maxVelY)
                {
                    NPC.velocity.Y += accelY * 1.25f; ;
                }
                else if (NPC.velocity.Y < 0f)
                {
                    NPC.velocity.Y -= accelY * .75f;
                }
                if (NPC.velocity.Y > maxVelY)
                {
                    NPC.velocity.Y = maxVelY;
                }
            }

            if (NPC.wet)
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y *= 0.95f;
                }
                NPC.velocity.Y -= 0.5f;
                if (NPC.velocity.Y < -4f)
                {
                    NPC.velocity.Y = -4f;
                }
                NPC.TargetClosest();
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            int frameSpeed = 5;
            int frameY = (int)(NPC.frameCounter / frameSpeed) % Main.npcFrameCount[Type] * frameHeight;
            NPC.frame.Y = frameY;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Feather, 2));
            npcLoot.Add(new DropBasedOnExpertMode(new CommonDrop(ItemID.SoulofFlight, 100, 1, 1, 33), new CommonDrop(ItemID.SoulofFlight, 100, 1, 1, 44)));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Main.EntitySpriteDraw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            return false;
        }
    }
}
