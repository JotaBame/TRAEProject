using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using TRAEProject.Changes.Prefixes;
using TRAEProject.Common.Verlet;
using TRAEProject.NewContent.Structures.Echosphere.Generation;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoSprite
{
    public class EchoSpriteGoreBody : ModGore
    {
        public override bool Update(Gore gore) => EchosphereNPCHelper.EchosphereEnemyGoreUpdate(gore);
        public override Color? GetAlpha(Gore gore, Color lightColor) => EchosphereNPCHelper.EchosphereEnemyGoreGetAlpha(gore, lightColor);
    }
    public class EchoSpriteGoreTail : ModGore
    {
        VerletSimulator trail;
        public override void Load()
        {
            On_Main.DrawGore += EchoSpriteTailRendering;
        }
        public static void Spawn(VerletSimulator echoSpriteTail, NPC echoSprite, Vector2 position)
        {
            Gore g = Gore.NewGoreDirect(echoSprite.GetSource_Death(), position, echoSprite.velocity, ModContent.GoreType<EchoSpriteGoreTail>());
            if (g != null && g.ModGore is EchoSpriteGoreTail tailGore)
            {
                Vector2[] dotDeltaPos = new Vector2[echoSpriteTail.dots.Length];
                float velocityLength = 10f;
                for (int i = 0; i < dotDeltaPos.Length; i++)
                {
                    dotDeltaPos[i] = (MathF.Tau * (float)i / dotDeltaPos.Length).ToRotationVector2() * velocityLength;
                }
                float randRot = Main.rand.NextFloat(MathF.Tau);
                for (int i = 0; i < dotDeltaPos.Length; i++)
                {
                    dotDeltaPos[i] = dotDeltaPos[i].RotatedBy(randRot);
                }
                TRAEMethods.Shuffle(ref dotDeltaPos);

                for (int i = 0; i < echoSpriteTail.dots.Length; i++)
                {
                    Dot dot = echoSpriteTail.dots[i];
                    dot.locked = false;
                    for (int j = 0; j < dot.connections.Length; j++)
                    {
                        dot.connections[j].length = 2;
                    }

                    dot.pos -= dotDeltaPos[i];
                    dot.oldPos = dot.pos + dotDeltaPos[i];
                }
                echoSpriteTail.iterations = 1;
                tailGore.trail = echoSpriteTail;
            }
        }
        public override Color? GetAlpha(Gore gore, Color lightColor)
        {
            return Color.Transparent;
        }
        public override bool Update(Gore gore)
        {
            if(gore.timeLeft == Gore.goreTime)
            {
            }
            trail.AddForce(gore.velocity * 16f);
            trail.Simulate();
            return EchosphereNPCHelper.EchosphereEnemyGoreUpdate(gore);
        }
        void DrawTrail(SpriteBatch sb, Vector2 screenPos, Color drawColor, Gore gore)
        {
            Texture2D outer = EchoSprite.trailOuter.Value;
            Texture2D inner = EchoSprite.trailInner.Value;
            Vector2[] positions = trail.GetPositions();
            Vector2 origin = outer.Size() / 2;
            float rotation = gore.rotation;
            SpriteEffects fx = SpriteEffects.None;
           
            for (int i = 0; i < positions.Length; i++)
            {
                sb.Draw(outer, positions[i] - screenPos, null, drawColor, rotation, origin, Vector2.One, fx, 0f);
            }
            origin = inner.Size() / 2;
            for (int i = 0; i < positions.Length; i++)
            {
                sb.Draw(inner, positions[i] - screenPos, null, drawColor, rotation, origin, Vector2.One, fx, 0f);
            }
            //wobble the positions with a sine function before drawing them
        }
        private void EchoSpriteTailRendering(On_Main.orig_DrawGore orig, Main self)
        {
            for (int i = 0; i < Main.maxGore; i++)
            {
                Gore gore = Main.gore[i];
                if (gore.active && gore.type == ModContent.GoreType<EchoSpriteGoreTail>())
                {
                    DrawTrail(Main.spriteBatch, Main.screenPosition, EchosphereNPCHelper.EchosphereEnemyGoreGetAlpha(gore, Color.White).Value, gore);   
                }
            }
        }
    }
}
