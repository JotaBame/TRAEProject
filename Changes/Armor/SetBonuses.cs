using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.TRAEDebuffs;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace TRAEProject.Changes.Armor
{
    public class SetBonuses : ModPlayer
    {
        public int shadowArmorDodgeChance = 0;
        public bool PirateSet = false;
        //public bool HolyProtection = false;
        public bool TitaniumArmorOn = false;
        public bool whenHitDodge = false;
        public bool secretPearlwoodSetBonus = false;
        public bool TRAEPal = false;
        public int PalBuildup = 0;
        public const int PalMax = 40 * 120;
        public int PalCounter = 0;
        public override void ResetEffects()
        {
            TitaniumArmorOn = false;
            PirateSet = false;
            shadowArmorDodgeChance = 0;
            //HolyProtection = false;
            whenHitDodge = false;
            secretPearlwoodSetBonus = false;
            TRAEPal = false;
        }
        public override void UpdateDead()
        {
            TitaniumArmorOn = false;
            PirateSet = false;
            shadowArmorDodgeChance = 0;
            //HolyProtection = false;
            whenHitDodge = false;
            secretPearlwoodSetBonus = false;
            TRAEPal = false;
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (info.Damage > 1)
            {
                //Shadowdodge();
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PirateSet && (ProjectileID.Sets.IsAWhip[proj.type]))
            {
                TRAEDebuff.Apply<PirateTag>(target, 240, -1);
            }
        }

        void DarkDodge()
        {
            Player.immune = true;
            Player.immuneTime = 80;
            if (Player.longInvince)
                Player.immuneTime = Player.immuneTime + 40;
            for (int index = 0; index < Player.hurtCooldowns.Length; ++index)
                Player.hurtCooldowns[index] = Player.immuneTime;
            for (int i = 0; i < 80; i++)
            {
                int num = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, Main.rand.Next(new int[] { 65, 173 }), 0f, 0f, 100, default, 2f);
                Main.dust[num].position.X += Main.rand.Next(-20, 21);
                Main.dust[num].position.Y += Main.rand.Next(-20, 21);
                Main.dust[num].velocity *= 0.4f;
                Main.dust[num].scale *= 1f + Main.rand.Next(40) * 0.01f;
                Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(Player.cWaist, Player);
                Main.dust[num].noGravity = true;
                Main.dust[num].noLight = true;
                if (Main.rand.Next(2) == 0)
                {
                    Main.dust[num].scale *= 1f + Main.rand.Next(40) * 0.01f;
                    Main.dust[num].velocity *= 1.4f;
                }
            }
            if (Player.whoAmI == Main.myPlayer)
            {
                NetMessage.SendData(MessageID.Dodge, -1, -1, null, Player.whoAmI, 1f);
            }
        }
        /*
        void Shadowdodge()
        {
            if (HolyProtection && !whenHitDodge)
            {
                if (Player.shadowDodgeTimer == 0)
                {
                    Player.shadowDodgeTimer = 1500;
                    Player.AddBuff(BuffID.ShadowDodge, 1500, false);
                }
            }
        }
        */
        public override void UpdateBadLifeRegen()
        {
            PalCounter++;
            if(Player.onHitRegen)
            {
                Player.setBonus = "Provides 2hp/s life regen\nYour life regen is used to fill hearts\nThe hearts can store up to 40 life, once full normal regen is resumed\nWhen you or a teamate is damaged the hearts will attempt to heal them.";
                Player.onHitRegen = false;
                Player.lifeRegen += 4;
                TRAEPal = true;
                //Main.NewText("hey");
                if(PalBuildup < PalMax)
                {
                    int amt = Math.Min(PalMax - PalBuildup, Player.lifeRegen);
                    PalBuildup += amt;
                    Player.lifeRegen -= amt;
                }
            }
            if(!TRAEPal)
            {
                PalBuildup = 0;
            }
        }
        public override void PostHurt(Player.HurtInfo info)
        {
            if(!Player.dead && Player.active)
            {
                int healRequest = Player.statLifeMax2 - Player.statLife;
                for(int i = 0; i < Main.player.Length; i++)
                {
                    if(healRequest <= 0)
                    {
                        break;
                    }
                    if(Main.player[i].active && !Main.player[i].dead && Main.player[i].team == Player.team && Main.player[i].GetModPlayer<SetBonuses>().TRAEPal && (Main.player[i].Center - Player.Center).Length() < 3000)
                    {
                        int amtGivable = Main.player[i].GetModPlayer<SetBonuses>().PalBuildup / 120;
                        int amtGiven = (int)MathF.Min(amtGivable, healRequest);
                        if(amtGiven <= 0)
                        {
                            continue;
                        }
                        healRequest -= amtGiven;
                        Main.player[i].GetModPlayer<SetBonuses>().PalBuildup -= 120 * amtGiven;
                        int leftHeal = amtGiven / 2;
                        int rightHeal = amtGiven - leftHeal;
                        for(int k = 0; k < 2; k++)
                        {
                            Vector2 position = Main.player[i].Center + Vector2.UnitX * (k == 0 ? -1 : 1) * 25 + Vector2.UnitY * MathF.Sin(Main.player[i].GetModPlayer<SetBonuses>().PalCounter * MathF.PI / 30) * (k == 0 ? -1 : 1) * 10;
                            Projectile.NewProjectile(new EntitySource_Misc("Palladium"), position, (k == 0 ? -10 : 10) * Vector2.UnitX, ModContent.ProjectileType<PalProjectile>(), 0, 0, Main.player[i].whoAmI, Player.whoAmI, k == 0 ? leftHeal : rightHeal);
                        }
                    }
                }
            }
        }
    }
    public class PalLayer : PlayerDrawLayer
    {

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if(drawInfo.drawPlayer.TryGetModPlayer<SetBonuses>(out SetBonuses modPlayer))
            {
                if(modPlayer.TRAEPal)
                {
                    Texture2D texture = Request<Texture2D>("TRAEProject/Changes/Armor/PalHeart").Value;
                    Player drawPlayer = drawInfo.drawPlayer;
                    float scale = ((float)modPlayer.PalBuildup / SetBonuses.PalMax) * 0.5f + 0.5f;
                    for(int i = 0; i < 2; i++)
                    {
                        Vector2 position = drawPlayer.Center + Vector2.UnitX * (i == 0 ? -1 : 1) * 25 + Vector2.UnitY * MathF.Sin(modPlayer.PalCounter * MathF.PI / 30) * (i == 0 ? -1 : 1) * 10;
                        

                        DrawData drawData = 
                        new DrawData(texture, position - Main.screenPosition, null, drawInfo.colorArmorBody, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0)
                        {
                            shader = drawPlayer.dye[1].dye
                        };
                        drawInfo.DrawDataCache.Add(drawData);
                    }
                }
            }
        }
    }
    public class PalProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 22;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60 * 60;

        }
        public override void AI()
        {
            Player player = Main.player[(int)Projectile.ai[0]];
            if(player.dead || !player.active)
            {
                Projectile.Kill();
                return;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.scale = ((float)(Projectile.ai[1] * 120) / SetBonuses.PalMax) * 0.5f + 0.5f;
            //Main.NewText(Projectile.scale);
            if(Projectile.timeLeft < (60 * 60) - 10)
            {
                Projectile.rotation.SlowRotation((player.Center - Projectile.Center).ToRotation(), MathF.PI / 15f);
                Projectile.velocity = TRAEMethods.PolarVector(10, Projectile.rotation);
                if(Collision.CheckAABBvAABBCollision(Projectile.position, Projectile.Size, player.position, player.Size))
                {
                    player.statLife += (int)Projectile.ai[1];
                    player.HealEffect((int)Projectile.ai[1]);
                    Projectile.Kill();
                }
            }
        }
        public override void PostDraw(Color drawColor)
        {
            // As mentioned above, be sure not to forget this step.
            Player player = Main.player[Projectile.owner];
            int shader = player.dye[1].dye;
            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            int shader = player.dye[1].dye;
            if (shader != 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
            }
            return true;
        }
    }
}
