using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoStalker
{
    public class EchoStalkerBody1 : ModNPC
    {
        public override string Texture => "TRAEProject/NewContent/NPCs/Echosphere/EchoStalker/EchoStalkerBody";
        public virtual Texture2D RegularTexture => HairVariant ? EchoStalkerHead.BodyWithHair : EchoStalkerHead.Body;
        public virtual Texture2D GlowTexture => HairVariant ? EchoStalkerHead.BodyWithHairGlow : EchoStalkerHead.BodyGlow;
        public bool HairVariant { get => NPC.localAI[0] == 1; set => NPC.localAI[0] = value ? -1 : 1; }
        public int HeadIndex { get => (int)NPC.ai[0]; set => NPC.ai[0] = value; }
        public ref float PurpleGlowinessAmount => ref NPC.localAI[1];
        public bool InvalidHeadIndex => HeadIndex < 0 || HeadIndex >= Main.maxNPCs || !Main.npc[HeadIndex].active || Main.npc[HeadIndex].type != ModContent.NPCType<EchoStalkerHead>();
        //so I don't have to remember that it's localai1
        public static void SetPurpleGlowinessAmount(NPC segment, float normalizedAmount)
        {
            segment.localAI[1] = normalizedAmount;
        }
        public override void SetDefaults()
        {
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.Size = new(36);
            NPC.lifeMax = 1250;
            NPC.defense = 10;
            NPC.damage = 50;
            NPC.knockBackResist = 0;
        }

        public override void AI()
        {
            if (InvalidHeadIndex)
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
                return;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            InitializeHairVariantFlagIfNeeded();
            Texture2D texture = RegularTexture;
            if (NPC.Opacity != 1 && EchoStalkerHead.IsIdle(NPC.ai[0]))
            {
                EchosphereNPCHelper.SpectralDrawVerticalFlip(NPC, spriteBatch, screenPos, texture);
                return false;
            }
            drawColor *= NPC.Opacity;
            EchosphereNPCHelper.DrawEchoWormSegmentWithBlurOld(GlowTexture, RegularTexture, NPC.Center - screenPos, PurpleGlowinessAmount, NPC.rotation, RegularTexture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, NPC.Opacity, NPC.GetNPCColorTintedByBuffs(drawColor));
            // Main.EntitySpriteDraw(GlowTexture, NPC.Center - screenPos, null, drawColor, NPC.rotation, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
            return false;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return !EchoStalkerHead.IsHeadImmuneToItem(HeadIndex, player.whoAmI);
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return !EchoStalkerHead.IsHeadImmuneToProj(HeadIndex, projectile);
        }
        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            EchoStalkerHead.CopyProjIframesToOtherSegments(HeadIndex, NPC.whoAmI, projectile);
        }
        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            EchoStalkerHead.CopyItemIframesToOtherSegments(HeadIndex, NPC.whoAmI, player.whoAmI);
        }
        void InitializeHairVariantFlagIfNeeded()
        {
            if (NPC.localAI[0] == 0)
            {
                NPC.localAI[0] = Main.rand.NextBool() ? -1 : 1;
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override bool CheckActive()
        {
            return false;
        }
    }
    public class EchoStalkerBody2 : EchoStalkerBody1
    {
        public override Texture2D RegularTexture => HairVariant ? EchoStalkerHead.Body2WithHair : EchoStalkerHead.Body2;
        public override Texture2D GlowTexture => HairVariant ? EchoStalkerHead.Body2WithHairGlow : EchoStalkerHead.Body2Glow;
    }
    public class EchoStalkerTail : EchoStalkerBody1
    {
        public override Texture2D RegularTexture => HairVariant ? EchoStalkerHead.Tail : EchoStalkerHead.Tail;
        public override Texture2D GlowTexture => HairVariant ? EchoStalkerHead.TailGlow : EchoStalkerHead.TailGlow;
    }
}
