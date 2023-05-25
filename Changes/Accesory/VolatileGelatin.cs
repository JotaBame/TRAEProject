using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Accesory
{
    public class VolatileGelatin : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if(item.type == ItemID.VolatileGelatin)
            {
                player.GetModPlayer<VolatileGelatinPlayer>().VolatileGelatinNew = true;
                player.volatileGelatin = false;
                player.volatileGelatinCounter = 0;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.VolatileGelatin:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Critical strikes temporarily raise defense\nWhen hit, all stored defense is released as damaging gel\nLimited to 25 extra defense";
                        }
                    }
                    break;
            }
}
    }
    public class VolatileGelatinPlayer : ModPlayer
    {
        public bool VolatileGelatinNew = false;
        int gelTime = 0;
        int dustChance = 26;
        int cooldown = 0;
        int TimeBeforeLosingStack = 120;
        int gelStored = 0;

        public override void ResetEffects()
        {
            VolatileGelatinNew = false;
        }
        public override void UpdateDead()
        {

            VolatileGelatinNew = false;
            gelTime = 0;
            gelStored = 0;
        }
        public override void PostUpdateEquips()
        {
            if (cooldown > 0)
                cooldown--;
            if (gelStored > 25)
            {
                gelStored = 25;
            }
            if (gelStored > 0)
            {
                dustChance = 26 - gelStored;
                if (Main.rand.Next(dustChance) == 0)
                {
                    int DustColor = Main.rand.NextFromList(DustID.BlueTorch, DustID.PinkTorch, DustID.PurpleTorch);
                    Rectangle r3 = Utils.CenteredRectangle(Player.Center, Vector2.One * Player.width);
                    int num3 = Dust.NewDust(r3.TopLeft(), r3.Width, r3.Height, DustColor, 0f, 0f, 150, default, 0.3f);
                    Main.dust[num3].fadeIn = 1f;
                    Main.dust[num3].velocity *= 0.2f;
                    Main.dust[num3].noLight = true;
                    Main.dust[num3].noGravity = true;
                }
                gelTime++;
                if (gelTime >= TimeBeforeLosingStack)
                {
                    gelStored--;
                    gelTime = 0;
                }
                Player.statDefense += gelStored;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && VolatileGelatinNew && cooldown == 0)
            {
                cooldown = 15;
                gelStored += damageDone / 50;
                if (gelStored > 25)
                {
                    gelStored = 25;
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit && VolatileGelatinNew && cooldown == 0)
            {
                cooldown = 15;
                gelStored += damageDone / 50;
                if (gelStored > 25)
                {
                    gelStored = 25;
                }
            }
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (gelStored > 0)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath1 with { MaxInstances = 0 });
                int GelID = ProjectileID.VolatileGelatinBall;
                float num852 = ((float)Math.PI * 2f);
                float gelCount = 59.167f * 6;
                for (float c = 0f; c < 1f; c += gelCount / (678f * (float)Math.PI))
                {
                    float f2 = num852 + c * ((float)Math.PI * 2f);
                    Vector2 velocity = f2.ToRotationVector2() * (8f + Main.rand.NextFloat() * 3f);
                    velocity += Vector2.UnitY * -1f;
                    int num854 = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, velocity, GelID, 50, 0f, Player.whoAmI);
                    Projectile pRojectile = Main.projectile[num854];
                    Projectile projectile2 = pRojectile;
                    projectile2.timeLeft = 40;
                }
                gelStored = 0;

            }
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (gelStored > 0)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath1 with { MaxInstances = 0 }); int GelID = ProjectileID.VolatileGelatinBall;
                float num852 = ((float)Math.PI * 2f);
                float gelCount = 59.167f * 6;
                for (float c = 0f; c < 1f; c += gelCount / (678f * (float)Math.PI))
                {
                    float f2 = num852 + c * ((float)Math.PI * 2f);
                    Vector2 velocity = f2.ToRotationVector2() * (4f + Main.rand.NextFloat() * 2f);
                    velocity += Vector2.UnitY * -1f;
                    int num854 = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, velocity, GelID, 50, 0f, Player.whoAmI);
                    Projectile pRojectile = Main.projectile[num854];
                    Projectile projectile2 = pRojectile;
                    projectile2.timeLeft = 40;
                }
                gelStored = 0;
            }
        }
    }
}
