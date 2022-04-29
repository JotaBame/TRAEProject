using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    class GhastlyGlaive : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 165f;
            stabStart = 119f;
            stabEnd = -10;
            swingAmount = (float)Math.PI / 12;
        }
        public override void SpearActive()
        {
            if (Projectile.localAI[0] > 0f)
            {
                Projectile.localAI[0] -= 1f;
            }
        }
        public override void SpearHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SummonMonkGhast();
        }
        void SummonMonkGhast()
        {
            if (Projectile.localAI[0] > 0f)
            {
                return;
            }
            Projectile.localAI[0] = 1000f;
            List<NPC> list = new List<NPC>();
            for (int i = 0; i < 200; i++)
            {
                NPC nPC = Main.npc[i];
                if (nPC.CanBeChasedBy(this) && Projectile.Distance(nPC.Center) < 800f)
                {
                    list.Add(nPC);
                }
            }
            Vector2 center = Projectile.Center;
            Vector2 value = Vector2.Zero;
            if (list.Count > 0)
            {
                NPC nPC2 = list[Main.rand.Next(list.Count)];
                center = nPC2.Center;
                value = nPC2.velocity;
            }
            int num = Main.rand.Next(2) * 2 - 1;
            Vector2 velocity = new Vector2((float)num * (4f + (float)Main.rand.Next(3)), 0f);
            Vector2 vector = center + new Vector2(-num * 120, 0f);
            velocity += (center + value * 15f - vector).SafeNormalize(Vector2.Zero) * 2f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), vector, velocity, 700, Projectile.damage / 2, 0f, Projectile.owner);
        }
    }
    class GhastlyGlaiveThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 165f;
            holdAt = 102f;
            floatTime = 120;
            maxSticks = 1;
            stickingDps = 0;
        }
        public override void SpearHitNPC(bool atMaxCharge, NPC target, int damage, float knockback, bool crit)
        {
            if (atMaxCharge)
            {
                SummonMonkGhast();
            }
        }
        int timer = 1;
        public override void StuckEffects(NPC victim)
        {
            timer++;
            if (timer % 90 == 0)
            {
                SummonMonkGhast();
            }
        }
        void SummonMonkGhast()
        {
            Projectile.localAI[0] = 1000f;
            List<NPC> list = new List<NPC>();
            for (int i = 0; i < 200; i++)
            {
                NPC nPC = Main.npc[i];
                if (nPC.CanBeChasedBy(this) && Projectile.Distance(nPC.Center) < 800f)
                {
                    list.Add(nPC);
                }
            }
            Vector2 center = Projectile.Center;
            Vector2 value = Vector2.Zero;
            if (list.Count > 0)
            {
                NPC nPC2 = list[Main.rand.Next(list.Count)];
                center = nPC2.Center;
                value = nPC2.velocity;
            }
            int num = Main.rand.Next(2) * 2 - 1;
            Vector2 velocity = new Vector2((float)num * (4f + (float)Main.rand.Next(3)), 0f);
            Vector2 vector = center + new Vector2(-num * 120, 0f);
            velocity += (center + value * 15f - vector).SafeNormalize(Vector2.Zero) * 2f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), vector, velocity, 700, Projectile.damage / 2, 0f, Projectile.owner);
        }
    }
}
