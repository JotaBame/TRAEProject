using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoLeviathan
{
    public class EchoLeviathanBody1 : ModNPC
    {
        protected virtual string GlowPathTexture => "TRAEProject/NewContent/NPCs/Echosphere/EchoLeviathan/EchoLeviathanBody1Glow";
        public ref float PurpleGlowinessAmount => ref NPC.localAI[1];
        //so I don't have to remember it's localai1, and makes it easier to read for others
        public static void SetPurpleGlowinessAmount(NPC npc, float amount)
        {
            npc.localAI[1] = amount;
        }
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lifeMax = 18000;
            NPC.defense = 40;
            NPC.damage = 80;
            NPC.width = NPC.height = 70;
            NPC.HitSound = SoundID.DD2_DrakinHurt;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;//initially invincible
            NPC.alpha = 255;//initially invisible
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void AI()
        {
            int parent = (int)NPC.ai[0];
            if (parent < 0 || parent >= Main.maxNPCs || !Main.npc[parent].active || Main.npc[parent].type != ModContent.NPCType<EchoLeviathanHead>())
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
                return;
            }
            NPC.realLife = parent;
        }
        //public override bool? CanBeHitByItem(Player player, Item item)
        //{
        //    return !EchoLeviathanHead.IsHeadImmuneToItem(NPC.ai[0], player.whoAmI);
        //}
        //public override bool? CanBeHitByProjectile(Projectile projectile)
        //{
        //    return !EchoLeviathanHead.IsHeadImmuneToProj(NPC.ai[0], projectile);
        //}
        //public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        //{
        //    EchoLeviathanHead.CopyProjIframesToOtherSegments(NPC.ai[0], NPC.whoAmI, projectile);
        //}
        //public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        //{
        //    EchoLeviathanHead.CopyItemIframesToOtherSegments(NPC.ai[0], NPC.whoAmI, player.whoAmI);
        //}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            if (NPC.Opacity != 1 && EchoLeviathanHead.EchoLeviIsIdle(NPC.ai[0]))
            {
                EchosphereHelper.SpectralDrawVerticalFlip(NPC, spriteBatch, screenPos, texture);
                return false;
            }
            drawColor *= NPC.Opacity;
            Texture2D blurTexture = ModContent.Request<Texture2D>(GlowPathTexture).Value;
            EchosphereHelper.DrawEchoWormSegmentWithBlur(blurTexture, texture, NPC.Center - screenPos, PurpleGlowinessAmount * NPC.Opacity, NPC.rotation, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, NPC.Opacity, NPC.GetNPCColorTintedByBuffs(drawColor));
            //Main.EntitySpriteDraw(texture, NPC.Center - screenPos, null, drawColor, NPC.rotation, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
            return false;
        }
        protected virtual int SegmentWidth => 80;
        public override void ModifyHoverBoundingBox(ref Rectangle boundingBox)
        {
            if (NPC.alpha >= 254)
            {
                boundingBox.X = -1000;//put it out of bounds of the map so it is never displayed
            }
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.alpha < 240 && target.Hitbox.Intersects(Utils.CenteredRectangle(NPC.Center, new Vector2(50)));//hitbox of width and height 50 for damaging players
        }
    }
    public class EchoLeviathanBody2 : EchoLeviathanBody1
    {
        protected override string GlowPathTexture => "TRAEProject/NewContent/NPCs/Echosphere/EchoLeviathan/EchoLeviathanBody2Glow";

    }
    public class EchoLeviathanBody3 : EchoLeviathanBody1
    {
        protected override string GlowPathTexture => "TRAEProject/NewContent/NPCs/Echosphere/EchoLeviathan/EchoLeviathanBody3Glow";
    }
}
