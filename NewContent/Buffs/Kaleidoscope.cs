using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
namespace TRAEProject.NewContent.Buffs
{
	public class KaleidoscopeSecondTag: ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			BuffID.Sets.IsATagBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			// DisplayName.SetDefault("KaleidoscopeNewDebuff");
			// Description.SetDefault(""); 
		}
	}
	public class KaleidoscopeSecondTagOnHit : GlobalProjectile
	{
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (target.HasBuff<KaleidoscopeSecondTag>() && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]) && projectile.type != ProjectileID.StardustGuardian)
			{
				modifiers.SourceDamage *= 5;
			}
		}
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)

		{
			if (projectile.type == ProjectileID.RainbowWhip)
			{
				target.AddBuff(BuffType<KaleidoscopeSecondTag>(), 240);
                
			}
			if (target.HasBuff(BuffType<KaleidoscopeSecondTag>()) && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]) && projectile.type != ProjectileID.StardustGuardian)
			{
				int buffIndex = target.FindBuffIndex(BuffType<KaleidoscopeSecondTag>());
				if (buffIndex != -1)
				{ 
					target.DelBuff(buffIndex); 
				}
				float distance = 500f;

				for (int i = 0; i < 50; i++)
				{
					Vector2 speed = Main.rand.NextVector2CircularEdge(12f, 12f);
					Dust dust7 = Dust.NewDustDirect(target.Center, target.width, target.height, 267, speed.X * 5, speed.Y * 5, 0, Main.hslToRgb(Main.player[projectile.owner].miscCounterNormalized * 9f % 1f, 1f, 0.5f), 1.3f);
					dust7.noGravity = true;
					dust7.scale = 0.9f + Main.rand.NextFloat() * 0.9f;
					dust7.fadeIn = Main.rand.NextFloat() * 0.9f;
					if (dust7.dustIndex != 6000)
					{
						Dust dust8 = Dust.CloneDust(dust7);
						dust8.scale /= 2f;
						dust8.fadeIn *= 0.85f;
						dust8.color = new Color(255, 255, 255, 255);
					}
				}
				foreach (NPC enemy in Main.npc)
				{
					Vector2 newMove = enemy.Center - target.Center;
					float distanceTo = MathF.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);// could simplify this using Vector2.Length?
					if (!enemy.dontTakeDamage && enemy.active && !enemy.friendly && !enemy.immortal && distanceTo < distance && enemy.whoAmI != target.whoAmI)
					{
						Main.player[projectile.owner].ApplyDamageToNPC(enemy, damageDone, 0f, 0, crit: false);
					}
				}
			}
			return;
		}

	}
}