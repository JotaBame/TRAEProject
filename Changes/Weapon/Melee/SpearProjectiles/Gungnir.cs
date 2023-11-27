using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ID;
using Terraria;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    public class Gungnir : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 164f;
            stabStart = 123f;
            stabEnd = -10;
            swingAmount = MathF.PI / 32;
        }
    }
    public class GungnirThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 164f;
            holdAt = 120f;
            Projectile.penetrate = 3;
            floatTime = -1;
            DustOnDeath = DustID.HallowedWeapons;
        }
        public override void ThrownUpdate()
        {

            NPC target = null;
            if (TRAEMethods.ClosestNPC(ref target, 400, Projectile.Center, false, -1, delegate (NPC possibleTarget) { return Projectile.localNPCImmunity[possibleTarget.whoAmI] != -1; }))
            {
                float vel = Projectile.velocity.Length();
                float dir = Projectile.velocity.ToRotation();
                dir = dir.AngleLerp((target.Center - Projectile.Center).ToRotation(), MathF.PI / 40);
                Projectile.velocity = TRAEMethods.PolarVector(vel, dir);
                float length = Projectile.velocity.Length();
                Projectile.velocity += (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY) * 2f;
                Projectile.velocity.Normalize();
                Projectile.velocity *= length;
            }
        }
    }
}
