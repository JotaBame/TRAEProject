﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.Common.ModPlayers;

namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    class OrichalcumHookbill : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 141f;
            stabStart = 119f;
            stabEnd = 40;
            swingAmount = (float)Math.PI / 24;
        }
        float origonalAim = 0;
        public override void OnStart()
        {
            origonalAim = aimDirection;
        }
        int maxInterupting = 0;
        bool soundPlayed = false;
        public override void Channeling()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.friendly = false;
            if (interupting < player.itemAnimationMax)
            {
                interupting++;
                maxInterupting = interupting;
            }
            else
            {
                if (player.autoReuseGlove)
                {
                    player.channel = false;
                }
                else
                {
                    if (!soundPlayed)
                    {

                        SoundEngine.PlaySound(25, player.Center, 0);
                        soundPlayed = true;
                    }
                }
            }
            for (int i = 0; i < 200; i++)
            {
                hitCount[i] = 0;
            }

            SetAimDirecition(out float amt);
            outAmount = 1f + amt / 2f;
        }
        public override void InteruptedAnimation()
        {
            Projectile.friendly = true;
            interupting -= 1;
            SetAimDirecition(out _);
            outAmount = 1.5f;
        }
        public override void SpearActive()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.channel && interupting > 0)
            {
                Projectile.extraUpdates = 2;
            }
            else
            {
                Projectile.extraUpdates = 0;
            }
        }
        void SetAimDirecition(out float amt)
        {
            Player player = Main.player[Projectile.owner];
            amt = ((float)interupting / (float)player.itemAnimationMax);
            amt *= amt;
            aimDirection = origonalAim + 5f * ((float)Math.PI / 6f) * player.direction * -1f * amt;
        }
        public override void SpearModfiyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (maxInterupting > 0)
            {
                Player player = Main.player[Projectile.owner];
                damage += (int)(damage * (3 * (float)maxInterupting / (float)player.itemAnimationMax));
            }
        }
        bool spawnedPetal = false;
        public override void SpearHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!spawnedPetal)
            {
                spawnedPetal = true;
                Player player = Main.player[Projectile.owner];
                int direction = player.direction;
                float k = Main.screenPosition.X;
                if (direction < 0)
                {
                    k += (float)Main.screenWidth;
                }
                float y2 = Main.screenPosition.Y;
                y2 += (float)Main.rand.Next(Main.screenHeight);
                Vector2 vector = new Vector2(k, y2);
                float num2 = target.Center.X - vector.X;
                float num3 = target.Center.Y - vector.Y;
                num2 += (float)Main.rand.Next(-50, 51) * 0.1f;
                num3 += (float)Main.rand.Next(-50, 51) * 0.1f;
                float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
                num4 = 24f / num4;
                num2 *= num4;
                num3 *= num4;
                Projectile.NewProjectile(player.GetSource_FromThis(), k, y2, num2, num3, 221, 36, 0f, player.whoAmI);
            }
        }
    }
    public class OrichalcumHookbillThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 141f;
            holdAt = 85f;
            maxSticks = 1;
            stickingDps = 0;
            floatTime = 10;
        }
        public override void SpearHitNPC(bool atMaxCharge, NPC target, int damage, float knockback, bool crit)
        {
            if (atMaxCharge)
            {
                Player player = Main.player[Projectile.owner];
                int direction = player.direction;
                float k = Main.screenPosition.X;
                if (direction < 0)
                {
                    k += (float)Main.screenWidth;
                }
                float y2 = Main.screenPosition.Y;
                y2 += (float)Main.rand.Next(Main.screenHeight);
                Vector2 vector = new Vector2(k, y2);
                float num2 = target.Center.X - vector.X;
                float num3 = target.Center.Y - vector.Y;
                num2 += (float)Main.rand.Next(-50, 51) * 0.1f;
                num3 += (float)Main.rand.Next(-50, 51) * 0.1f;
                float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
                num4 = 24f / num4;
                num2 *= num4;
                num3 *= num4;
                Projectile.NewProjectile(player.GetSource_FromAI(), k, y2, num2, num3, 221, 36, 0f, player.whoAmI);
            }
        }
    }
}
