using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

public static class PrimeStats
{
    //Prime himself
    public const bool lockPhase3ToExpert = true;
    public const float primeSpinBaseSpeed = 3f; //vanilla value: 2 classic, 6 epxert
    public const float primeSpinBonusSpeedFromDist = 1.2f; // vanilla value: 1.1f

    //Prime vice
    public const int viceHealth = 3200; //value in classic mode x1.5 in expert, vanilla value: 9000
    //Prime saw
    public const int sawHealth = 2300; //value in classic mode x1.5 in expert, vanilla value: 9000
    public const float sawApproachSpeed = 17f; //Max speed of prime saw when in 'pursuit mode' vanilla value: 7f
    public const float sawApproachAcc = sawApproachSpeed / 120f; //the accekration on prime saw's pursuit mode, vanilla vaule 0.05f
    //Prime laser
    public const int laserHealth = 2250; //value in classic mode x1.5 in expert, vanilla value: 6000
    //Prime cannon
    public const int cannonHealth = 2400; //value in classic mode x1.5 in expert, vanilla value: 7000
    //Prime mace
    public const int macHealth = 2500; //value in classic mode x1.5 in expert
    public const int primeMaceSwingTime = 90; // how long it takes for mace to finish its elliptical swing
    public const int primeMaceNonRageCooldown = 180; //time between mace swings when prime is not raged.
    //Prime rail
    public const int railHealth = 2250; //value in classic mode x1.5 in expert
    public const float railVel = 8f; //compined with railExtraUpdates, determines the velocity of the rail shot
    public const int railExtraUpdates = 5;
    public const int railChargeTime = 600; //time between rail shots
    public const int railWarnTime = 180; //how long the rail's warning sight line lasts
    public const int railDamage = 60;
    //Prime launcher
    public const int launcherHealth = 2000; //value in classic mode x1.5 in expert
    public const int missileBurstSize = 10; //size of missile burst when prime isn't raged
    public const int missileBurstDelay = 10; //time between missiles in burst
    public const int missileBurstCooldown = 360; //cooldown between bursts
    public const int ragedMissileCooldown = 30; //when raged instead of periodicly doing bursts the launch will continously launch missiles using this cooldown
    public const int missileDamage = 35;


    public static void ArmGore(NPC npc)
    {
        Gore.NewGore(npc.GetSource_FromAI(), npc.position, npc.velocity, 147);
        Gore.NewGore(npc.GetSource_FromAI(), npc.position, npc.velocity, 148);
        for (int num784 = 0; num784 < 10; num784++) 
        {
            int num785 = Dust.NewDust(npc.position, npc.width, npc.height, 31, 0f, 0f, 100, default(Color), 1.5f);
            Dust dust = Main.dust[num785];
            dust.velocity *= 1.4f;
        }

        for (int num786 = 0; num786 < 5; num786++) 
        {
            int num787 = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 2.5f);
            Main.dust[num787].noGravity = true;
            Dust dust = Main.dust[num787];
            dust.velocity *= 5f;
            num787 = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 1.5f);
            dust = Main.dust[num787];
            dust.velocity *= 3f;
        }

        int num788 = Gore.NewGore(npc.GetSource_FromAI(), npc.position, default(Vector2), Main.rand.Next(61, 64));
        Gore gore2 = Main.gore[num788];
        gore2.velocity *= 0.4f;
        Main.gore[num788].velocity.X += 1f;
        Main.gore[num788].velocity.Y += 1f;
        num788 = Gore.NewGore(npc.GetSource_FromAI(), npc.position, default(Vector2), Main.rand.Next(61, 64));
        gore2 = Main.gore[num788];
        gore2.velocity *= 0.4f;
        Main.gore[num788].velocity.X -= 1f;
        Main.gore[num788].velocity.Y += 1f;
        num788 = Gore.NewGore(npc.GetSource_FromAI(), npc.position, default(Vector2), Main.rand.Next(61, 64));
        gore2 = Main.gore[num788];
        gore2.velocity *= 0.4f;
        Main.gore[num788].velocity.X += 1f;
        Main.gore[num788].velocity.Y -= 1f;
        num788 = Gore.NewGore(npc.GetSource_FromAI(), npc.position, default(Vector2), Main.rand.Next(61, 64));
        gore2 = Main.gore[num788];
        gore2.velocity *= 0.4f;
        Main.gore[num788].velocity.X -= 1f;
        Main.gore[num788].velocity.Y -= 1f;
    }

    public static void RenderBones(NPC NPC, SpriteBatch spriteBatch, Vector2 screenPos, int side)
    {
        int headIndex = (int)MathF.Abs(NPC.ai[1]);
        Vector2 vector7 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f - 5f * side, NPC.position.Y + 20f);
        for (int k = 0; k < 2; k++) 
        {
            float num22 = Main.npc[headIndex].position.X + (float)(Main.npc[headIndex].width / 2) - vector7.X;
            float num23 = Main.npc[headIndex].position.Y + (float)(Main.npc[headIndex].height / 2) - vector7.Y;
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
    }
}