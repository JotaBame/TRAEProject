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
    class MushroomSpear : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 184f;
            stabStart = 147f;
            stabEnd = -10;
            swingAmount = (float)Math.PI / 24;
        }
        int timer = 0;
        public override void SpearActive()
        {
            Player player = Main.player[Projectile.owner];
            timer++;
            if(timer % (player.itemAnimationMax / 8) == 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, 131, Projectile.damage / 4, 0f, Projectile.owner);
            }
        }
    }
    public class MushroomSpearThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 184f;
            holdAt = 92f;
            floatTime = -1;
            Projectile.penetrate = -1;
        }
        float counter = 0;
        public override void ThrownUpdate()
        {
            counter += Projectile.velocity.Length();
            if(counter > 48f)
            {
                counter -= 48f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, 131, Projectile.damage / 4, 0f, Projectile.owner);
            }
        }
        int heldCounter = 0;
        public override void HeldUpdate()
        {
            heldCounter++;
            if(heldCounter % 6 ==0)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, 131, Projectile.damage / 4, 0f, Projectile.owner);
            }
        }
    }
}
