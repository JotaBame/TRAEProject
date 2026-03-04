using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;

namespace TRAEProject.NewContent.TRAEDebuffs
{
    public class ObsidianSkulledStacks : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int stacks = 0;
        public override void ResetEffects(NPC npc)
        {
            stacks = 0; 
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += stacks * 3;
 
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += stacks * 3;
        }


    }
    public class ObsidianSkulled : TRAEDebuff
    { 
        public override void Update(NPC npc)
        {
            npc.GetGlobalNPC<ObsidianSkulledStacks>().stacks++;
             if (Main.rand.NextBool(6))
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(npc.Center, DustID.ShadowbeamStaff, speed * 5, Scale: 1.5f);
                d.noGravity = true;
            }
           
        }
    }
}
