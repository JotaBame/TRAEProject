using Microsoft.Xna.Framework;
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
    class MythrilHalberd : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 141f;
            stabStart = 119f;
            stabEnd = 40f;
            swingAmount = MathF.PI / 24;
            Projectile.GetGlobalProjectile<TRAEProject.Common.ProjectileStats>().heavyCritter = true;
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
                    if(!soundPlayed)
                    {

                        SoundEngine.PlaySound(SoundID.MaxMana with { MaxInstances = 0 }, player.Center);
                        soundPlayed = true;
                    }
                }
            }
            for(int i =0; i < 200; i++)
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
            aimDirection = origonalAim + 5f * (MathF.PI / 6f) * player.direction * -1f * amt;
        }
        public override void SpearModfiyHitNPCMelee(NPC target, ref NPC.HitModifiers modifiers)
        {
            if(maxInterupting > 0)
            {
                Player player = Main.player[Projectile.owner];
                modifiers.FinalDamage.Base *= 3 * (float)maxInterupting / (float)player.itemAnimationMax;
            }
        }
    }
    public class MythrilHalberdThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 141f;
            holdAt = 85f;
            maxSticks = 1;
            stickingDps = 0;
            floatTime = 10;
            DustOnDeath = DustID.Mythril;
            DustOnDeathCount = 35;
            Projectile.GetGlobalProjectile<TRAEProject.Common.ProjectileStats>().heavyCritter = true;
        }
    }
}
