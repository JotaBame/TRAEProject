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
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    public class ObsidianSwordfish : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 88f;
            stabStart = 55f;
            stabEnd = -10;
            swingAmount = (float)Math.PI / 32;
        }
        public override void OnMaxReach(float direction)
        {
            if (Projectile.wet)
            {
                Player player = Main.player[Projectile.owner];
                Projectile p = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, TRAEMethods.PolarVector(player.HeldItem.shootSpeed * player.GetModPlayer<MeleeStats>().meleeVelocity * (1 / player.GetAttackSpeed(DamageClass.Melee)), direction), ProjectileType<ObsidianSwordfishThrow>(), (int)(Projectile.damage * 0.6f), Projectile.knockBack, Projectile.owner)];
                ((SpearThrow)p.ModProjectile).chargeAmt = 1f;
                SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
                Projectile.Kill();
            }
        }
    }
    public class ObsidianSwordfishThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 88f;
            holdAt = 15.5f;
            Projectile.penetrate = -1;
            //Projectile.ignoreWater = true;
        }
        public override void ThrownUpdate()
        {

            if (Projectile.wet)
            {
                Projectile.extraUpdates = 1;
                floatTime = -1;
                NPC target = null;
                if (TRAEMethods.ClosestNPC(ref target, 1000, Projectile.Center, false, -1, delegate (NPC possibleTarget) { return Projectile.localNPCImmunity[possibleTarget.whoAmI] != -1; }))
                {
                    float vel = Projectile.velocity.Length();
                    float dir = Projectile.velocity.ToRotation();
                    dir = dir.AngleLerp((target.Center - Projectile.Center).ToRotation(), (float)Math.PI / 15);
                    Projectile.velocity = TRAEMethods.PolarVector(vel, dir);
                    float length = Projectile.velocity.Length();
                    Projectile.velocity += (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY) * 2f;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= length;
                }
            }
            else
            {
                Projectile.extraUpdates = 0;
                floatTime = 60;
            }
        }
    }
}
