using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Sky.Griffin
{
    public class GriffinFlier : ModNPC
    {
        static float MaxVel => 12f;
        static float Acceleration => 0.4f;
        static float TransformationMinDist => 200f;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }
        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 60;
            NPC.lifeMax = 450;
            NPC.defense = 25;
            NPC.damage = 95;
            NPC.knockBackResist = 0.1f;
            NPC.noGravity = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            screenPos -= new Vector2(0, DrawOffsetY + NPC.gfxOffY);
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
        public override void AI()
        {
            //vampire bat 158
            //vampire walker 159
            BONK();
            NPC.TargetClosest();
            Movement();
            CheckForTransformation();
            SpeedUpMaybe();
        }

        private void SpeedUpMaybe()
        {
            NPC.ai[1] += 2f;
            if (NPC.ai[1] > 200f)
            {
                Player player = Main.player[NPC.target];
                if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                {
                    NPC.ai[1] = 0f;
                }
                //what is this??? (from vampire bat AI)
                //various speedup factors?
                float num211 = 0.2f;
                float num212 = 0.1f;
                float num213 = 4f;
                float num214 = 1.5f;


                if (NPC.ai[1] > 1000f)
                {
                    NPC.ai[1] = 0f;
                }
                NPC.ai[2] += 1f;
                if (NPC.ai[2] > 0f)
                {
                    if (NPC.velocity.Y < num214)
                    {
                        NPC.velocity.Y += num212;
                    }
                }
                else if (NPC.velocity.Y > 0f - num214)
                {
                    NPC.velocity.Y -= num212;
                }
                if (NPC.ai[2] < -150f || NPC.ai[2] > 150f)
                {
                    if (NPC.velocity.X < num213)
                    {
                        NPC.velocity.X += num211;
                    }
                }
                else if (NPC.velocity.X > 0f - num213)
                {
                    NPC.velocity.X -= num211;
                }
                if (NPC.ai[2] > 300f)
                {
                    NPC.ai[2] = -300f;
                }
            }
            NPC.spriteDirection = NPC.direction;
        }

        private void CheckForTransformation()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player player = Main.player[NPC.target];
                float dist = NPC.Distance(player.Center);
                if (dist < TransformationMinDist && NPC.position.Y + NPC.height < player.position.Y + player.height && Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                {
                    NPC.Transform(ModContent.NPCType<GriffinWalker>());
                }
            }
        }

        private void BONK()
        {
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
        }

        private void Movement()
        {
            if (NPC.direction == -1 && NPC.velocity.X > -MaxVel)
            {
                NPC.velocity.X -= Acceleration;
                if (NPC.velocity.X > MaxVel / 2)
                {
                    NPC.velocity.X -= Acceleration / 2;
                }
                else if (NPC.velocity.X > 0f)
                {
                    NPC.velocity.X += Acceleration / 4;
                }
                if (NPC.velocity.X < -MaxVel)
                {
                    NPC.velocity.X = -MaxVel;
                }
            }
            else if (NPC.direction == 1 && NPC.velocity.X < MaxVel)
            {
                NPC.velocity.X += Acceleration;
                if (NPC.velocity.X < -MaxVel / 2)
                {
                    NPC.velocity.X += Acceleration / 2;
                }
                else if (NPC.velocity.X < 0f)
                {
                    NPC.velocity.X -= Acceleration / 4;
                }
                if (NPC.velocity.X > MaxVel)
                {
                    NPC.velocity.X = MaxVel;
                }
            }
            if (NPC.directionY == -1 && NPC.velocity.Y > -MaxVel)
            {
                NPC.velocity.Y -= Acceleration;
                if (NPC.velocity.Y > MaxVel / 2)
                {
                    NPC.velocity.Y -= Acceleration / 2;
                }
                else if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y += Acceleration / 4;
                }
                if (NPC.velocity.Y < -MaxVel)
                {
                    NPC.velocity.Y = -MaxVel;
                }
            }
            else if (NPC.directionY == 1 && NPC.velocity.Y < MaxVel)
            {
                NPC.velocity.Y += Acceleration;
                if (NPC.velocity.Y < -MaxVel / 2)
                {
                    NPC.velocity.Y += Acceleration / 2;
                }
                else if (NPC.velocity.Y < 0f)
                {
                    NPC.velocity.Y -= Acceleration / 4;
                }
                if (NPC.velocity.Y > MaxVel)
                {
                    NPC.velocity.Y = MaxVel;
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //Souls of Flight (1, 50%/75% chance)
            //Steak(1, 3.33 % chance)
            npcLoot.Add(ItemDropRule.ExpertGetsRerolls(ItemID.SoulofFlight, 2, 1));//reroll once(?), making it 75 % on expert
            npcLoot.Add(ItemDropRule.Common(ItemID.Steak, 30));
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            int frameSpeed = 4;
            NPC.frame.Y = (int)NPC.frameCounter / frameSpeed % Main.npcFrameCount[Type] * frameHeight;
        }
    }
}
