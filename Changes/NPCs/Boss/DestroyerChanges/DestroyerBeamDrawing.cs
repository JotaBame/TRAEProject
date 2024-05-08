
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.DestroyerChanges
{
    public static class DestroyerBeamDrawing
    {
        public static bool Draw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.TheDestroyerTail || npc.type == NPCID.TheDestroyerBody)
            {
                if (npc.dontTakeDamage)
                    return false;
                Rectangle frame = npc.type switch
                {
                    NPCID.TheDestroyer => new Rectangle(2, 0, 46, 74),//head
                    NPCID.TheDestroyerBody => new Rectangle(0, 28, 50, 46),
                    _ => new Rectangle(6, 28, 38, 64),//tail
                };
                if (npc.type != NPCID.TheDestroyer)
                {
                    NPC nextVisibleSegment = Main.npc[(int)Main.npc[(int)Main.npc[(int)npc.ai[1]].ai[1]].ai[1]];//walk forward 3 npcs on the chain
                    float nextVisibleSegmentFrameHeight = nextVisibleSegment.type switch
                    {
                        NPCID.TheDestroyer => 72,//head
                        NPCID.TheDestroyerBody => 42,
                        _ => 56//tail
                    };
                    nextVisibleSegmentFrameHeight -= 8;//account for the laser head thing
                    DestroyerBeam.AddLightningToCache(npc.Center - new Vector2(0, frame.Height - 8).RotatedBy(npc.rotation) + npc.velocity * 2,
                        nextVisibleSegment.Center + new Vector2(0, nextVisibleSegmentFrameHeight).RotatedBy(nextVisibleSegment.rotation) + nextVisibleSegment.velocity * 2, Color.Red, 1, 4, npc.whoAmI * 200,
                        (npc.rotation).ToRotationVector2(), nextVisibleSegment.rotation.ToRotationVector2());
                }
               
                Texture2D texture = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/DestroyerChanges/DestroyerSegmentEdge").Value;
                Vector2 offset = (npc.rotation + MathF.PI / 2).ToRotationVector2() * (npc.type == NPCID.TheDestroyer ? 54 : 35);
                if (npc.type != NPCID.TheDestroyerTail)
                {
                    Main.EntitySpriteDraw(texture, npc.Center + offset - screenPos, null, drawColor, npc.rotation + MathF.PI / 2f, texture.Size() / 2, npc.scale, SpriteEffects.None);
                }
                if (npc.type != NPCID.TheDestroyer)
                {
                    offset = (npc.rotation + MathF.PI / 2).ToRotationVector2() * (npc.type == NPCID.TheDestroyerTail ? 47 : 35);
                    Main.EntitySpriteDraw(texture, npc.Center - offset - screenPos, null, drawColor, npc.rotation + MathF.PI / 2f, texture.Size() / 2, npc.scale, SpriteEffects.FlipHorizontally);
                }
                texture = TextureAssets.Npc[npc.type].Value;
                Main.EntitySpriteDraw(texture, npc.Center - screenPos, frame, drawColor, npc.rotation, frame.Size() / 2, npc.scale, SpriteEffects.None);

                return false;
            }
            return true;
        }

    }
}
