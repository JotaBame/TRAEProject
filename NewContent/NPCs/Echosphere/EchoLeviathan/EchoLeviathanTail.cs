﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.NPCs.Echosphere.EchoLeviathan
{
    internal class EchoLeviathanTail : ModNPC
    {
        public ref float PurpleGlowinessAmount => ref NPC.localAI[1];
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.lifeMax = 18000;
            NPC.defense = 34;
            NPC.damage = 70;
            NPC.width = NPC.height = 70;
            NPC.HitSound = EchoLeviathanHead.HitSFX;
            NPC.DeathSound = EchoLeviathanHead.DeathSFX;
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
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            if (NPC.Opacity != 1 && EchoLeviathanHead.EchoLeviIsIdle(NPC.ai[0]))
            {
                EchosphereNPCHelper.SpectralDrawVerticalFlip(NPC, spriteBatch, screenPos, texture);
                return false;
            }
            drawColor *= NPC.Opacity;
            Texture2D blurTexture = ModContent.Request<Texture2D>("TRAEProject/NewContent/NPCs/Echosphere/EchoLeviathan/EchoLeviathanTailGlow").Value;
            float opacity = NPC.Opacity;
            opacity = Utils.GetLerpValue(0.75f, 1f, opacity, true);
            EchosphereNPCHelper.DrawEchoWormSegmentWithBlur(blurTexture, texture, NPC.Center - screenPos, PurpleGlowinessAmount * opacity, NPC.rotation, texture.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, NPC.Opacity, NPC.GetNPCColorTintedByBuffs(drawColor));
            return false;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.alpha < 240 && target.Hitbox.Intersects(Utils.CenteredRectangle(NPC.Center, new Vector2(50)));//hitbox of width and height 50 for damaging players
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                EchosphereNPCHelper.EchosphereEnemyDeathDust(NPC, 0.7f);
            }
        }
        public override bool CheckDead()
        {
            int parent = (int)NPC.ai[0];
            return parent < 0 || parent >= Main.maxNPCs || !Main.npc[parent].active || Main.npc[parent].type != ModContent.NPCType<EchoLeviathanHead>() || Main.npc[parent].life <= 0;
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
