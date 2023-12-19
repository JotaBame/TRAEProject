using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Buffs
{
	public class StrongerPoisoned : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			// DisplayName.SetDefault("Stronger Poisoned");
			// Description.SetDefault("Yes, I'm fully aware that this is the exact opposite of what a Bezoar is for.");
		}
 
    }
	public class StrongerPoisonedEffect : GlobalNPC
	{
                public override bool InstancePerEntity => true;


        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff(BuffType<StrongerPoisoned>()))
            {
                if (npc.HasBuff(BuffID.Poisoned))
                {
                    npc.lifeRegen -= 18;
                    damage *= 3;
                    damage /= 2;
                }
                if (npc.HasBuff(BuffID.Venom))
                {
                    npc.lifeRegen -= 80;
                    damage *= 3;
                    damage /= 2;

                }

            }
        }
    }
    public class Bezoar : ModPlayer
    {
        public bool bezoar = false;

        public override void ResetEffects()
        {
            bezoar = false;
        }
        public override void UpdateDead()
        {
            bezoar = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (bezoar && (target.HasBuff(BuffID.Poisoned) || target.HasBuff(BuffID.Venom)))
            {
                target.AddBuff(BuffType<StrongerPoisoned>(), 300);
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (bezoar && (target.HasBuff(BuffID.Poisoned) || target.HasBuff(BuffID.Venom)))
            {
                target.AddBuff(BuffType<StrongerPoisoned>(), 300);
            }
        }
    }
}