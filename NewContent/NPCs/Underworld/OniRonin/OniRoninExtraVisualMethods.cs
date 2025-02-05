using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace TRAEProject.NewContent.NPCs.Underworld.OniRonin
{
    public class OniRoninExtraVisualMethods
    {
        public static void PetalDrawing(Projectile projectile)
        {
            Texture2D blackBall = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/GlowBallSmallPremultiplied").Value;
            Main.EntitySpriteDraw(blackBall, projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), null, Color.Black * 0.6f, projectile.rotation, blackBall.Size() / 2, projectile.scale * 1.07f, projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            Rectangle frame = Utils.Frame(texture, 1, 4, 0, projectile.frame);
            bool fire = projectile.type == ModContent.ProjectileType<OniPetalSpiralingFire>() || projectile.type == ModContent.ProjectileType<OniPetalCirclingFire>();
            SpriteEffects projDir = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (fire)
                for (float i = 0; i < 1; i += 0.25f)
                {
                    Vector2 drawOffset = new Vector2(3).RotatedBy(i * MathF.Tau + Main.GlobalTimeWrappedHourly);
                    Main.EntitySpriteDraw(texture, projectile.Center + drawOffset - Main.screenPosition, frame, Color.OrangeRed * 0.75f, projectile.rotation, frame.Size() / 2, projectile.scale, projDir);
                }
            for (int i = projectile.oldPos.Length - 1; i >= 0; i--)
            {
                float opacity = 1f - (float)i / projectile.oldPos.Length;
                opacity *= fire ? 1 : 0.75f;
                Vector2 drawPos = projectile.oldPos[i] + projectile.Size * 0.5f - Main.screenPosition;
                Main.EntitySpriteDraw(texture, drawPos, frame, Color.White * opacity, projectile.oldRot[i], frame.Size() / 2, projectile.scale * opacity * 0.5f + 0.5f, projDir);

            }
        }
        /// <summary>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="effectType"> 
        /// 0: circling petals
        /// 1: petal rapid fire
        /// 2: teleport
        /// 3: idle feet dust
        /// </param>
        /// <param name="direction"></param>
        /// <param name="progress"></param>
        public static void OniDustEffects(Vector2 position, int effectType, float direction, float progress)
        {
            switch (effectType)
            {
                case 0:
                    for (float i = 0; i < 1; i += 0.05f)
                    {
                        Vector2 posOffset = new Vector2(4).RotatedBy(i * MathF.Tau);
                        posOffset.Y *= 0.5f;
                        Vector2 velocity = Vector2.UnitY * progress * -3 + Vector2.UnitY * -3;
                        Dust.NewDustPerfect(position + posOffset, DustID.Torch, posOffset * 0.15f + velocity, 0, default, 1.6f).noGravity = true;
                    }
                    break;
                case 1:
                    {          
                        for (float i = 0; i < 1; i += 0.25f)
                        {
                            Vector2 posOffset = new Vector2(2).RotatedBy(i * MathF.Tau + Main.timeForVisualEffects * 0.2f);
                            posOffset.X *= 0.5f;
                            Dust.NewDustPerfect(position + posOffset * 10, DustID.Torch, -posOffset * 0.9f, 0, default, 1.6f).noGravity = true;
                        }         
                        for (int i = 0; i < 2; i++)
                        {
                            Vector2 posOffset = new Vector2(Main.rand.NextFloat() * 10);
                            posOffset.Y *= 0.5f;
                            Vector2 velocity = new Vector2(Main.rand.NextFloat() * 8 + 4, 0).RotatedBy(direction + Main.rand.NextFloat() * 0.1f - 0.05f);
                            Dust.NewDustPerfect(position + posOffset, DustID.Torch, velocity, 0, default, 1.5f).noGravity = true;
                        }
                    }
                    break;
                case 2:
                    {
                        for (float i = 0; i < 1; i += 0.033f)//circle
                        {
                            Vector2 posOffset = new Vector2(30, 0).RotatedBy(MathF.Tau * i);
                            Dust.NewDustPerfect(position + posOffset, DustID.Torch, direction.ToRotationVector2() * progress, 100, Color.White, 2).noGravity = true;
                        }
                        for (float i = 0; i < 1; i+= 0.2f)
                        {
                            for (float j = 0; j < 1; j+= 0.066f)
                            {
                                Vector2 posOffset = new Vector2(j * 60 - 30, 8).RotatedBy(MathF.Tau * i);
                                Dust.NewDustPerfect(position + posOffset, DustID.Torch, direction.ToRotationVector2() * progress, 100, Color.White, 2).noGravity = true;
                            }
                        }
                    }
                    break;
                case 3:
                    {
                        if (direction == 0)
                            direction = -1;
                        Vector2 dustPosOffset = Vector2.Lerp(new Vector2(-7 * direction, -7), Vector2.Zero, Main.rand.NextFloat());
                        if(Main.rand.NextBool(2))
                        Dust.NewDustPerfect(dustPosOffset + position, DustID.Torch, Vector2.Zero, 100);
                        dustPosOffset = Vector2.Lerp(new Vector2(6 * direction, -6), new Vector2(2 * direction, -3), Main.rand.NextFloat());
                        if (Main.rand.NextBool(2))
                            Dust.NewDustPerfect(dustPosOffset + position, DustID.Torch, Vector2.Zero, 100);
                    }
                    break;
            }
        }
    }
    public struct SmearTeleportEffect
    {
        public Vector2 TeleportStart { get; set; }
        public Vector2 TeleportEnd { get; set; }
        public float TimeLeft { get; set; }
        /// <summary>
        /// animation usually lasts TimeLeft. This multiplies the speed of the animation
        /// </summary>
        public float SmearSpeed { get; set; }

        public SmearTeleportEffect(Vector2 start, Vector2 end, float fadeOutDuration, float smearSpeed = 3)
        {
            TeleportStart = start;
            TeleportEnd = end;
            TimeLeft = fadeOutDuration;
            SmearSpeed = smearSpeed;
        }   
        public void DrawAndUpdateTeleportEffect(NPC npc)
        {
            if (!Main.gamePaused)
                TimeLeft--;
            float increment = 1 / (MathF.Floor(Vector2.Distance(TeleportEnd, TeleportStart)) * 0.5f);
            Main.instance.LoadNPC(npc.type);
            Texture2D texture = TextureAssets.Npc[npc.type].Value;
            float fadeOut = TimeLeft / 20;        
            float afterImgCancel = (1 - fadeOut);
            for (float i = 1; i > 0; i-= increment)
            {
                float opacity = MathF.Acos(MathF.Cos(MathF.Tau * i)) / MathF.PI;
                opacity = 1 - MathF.Pow(1 - opacity, 2);
                opacity *= 0.15f;
                float space = i * afterImgCancel * SmearSpeed;
                if (space > 1)
                    continue;
                Vector2 drawPos = Vector2.Lerp(TeleportStart, TeleportEnd , space);
                drawPos -= Main.screenPosition;
                Main.EntitySpriteDraw(texture, drawPos, npc.frame, Color.OrangeRed * opacity * fadeOut, npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
        }
    }
}
