using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Weapons.Summoner.AbsoluteZero;
using TRAEProject.Common;
using Terraria.Audio;
using TRAEProject.NewContent.Items.Armor.IceArmor;

namespace TRAEProject.NewContent.Buffs
{
	public class AZSecondTag: ModBuff
	{
		public override void SetStaticDefaults() 
		{
			Main.debuff[Type] = true;
			BuffID.Sets.IsATagBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			// DisplayName.SetDefault("KaleidoscopeNewDebuff");
			// Description.SetDefault(""); 
		}
	}
	public class AZSecondTagOnHit : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (target.HasBuff<AZSecondTag>() && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]) && projectile.type != ProjectileID.StardustGuardian && projectile.type != ProjectileType<MadFlocko>())
			{
				modifiers.SourceDamage *= 3;
			}
		}
		bool firstHit = false;
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)

		{
			if (projectile.type == ProjectileType<AbsoluteZeroP>())
            {

                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
 
                    if (npc.whoAmI != target.whoAmI && npc.HasBuff(BuffType<AZSecondTag>()))
                    {
 
                        int buffIndex = npc.FindBuffIndex(BuffType<AZSecondTag>());
                        if (buffIndex != -1)
                        {
                            npc.DelBuff(buffIndex);
                        }
                     }
                }

                target.AddBuff(BuffType<AZSecondTag>(), 240);
                
			}
			if (target.HasBuff(BuffType<AZSecondTag>()) && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]) && projectile.type != ProjectileID.StardustGuardian && projectile.type != ProjectileType<MadFlocko>())
			{
				int buffIndex = target.FindBuffIndex(BuffType<AZSecondTag>());
				if (buffIndex != -1)
				{ 
					target.DelBuff(buffIndex);
                }
                float distance = 160f;

                target.GetGlobalNPC<Freeze>().FreezeMe(target, 120);
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC nPC = Main.npc[i];
                    if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(target.Center, nPC.Center) <= distance)
                    {
                        target.GetGlobalNPC<Freeze>().FreezeMe(target, 60);

                    }
                }
				SoundEngine.PlaySound(SoundID.Item28 with { Pitch = -0.5f, MaxInstances = 0 }, target.Center) ; 

                for (int i = 0; i < 25; i++)
                {

                    Vector2 speed = Main.rand.NextVector2CircularEdge(3.8f, 3.8f);
					  int num = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, 16, speed.X * 5, speed.Y * 5, 0, default, 1.3f);
                    int num2 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Ice, speed.X * 5, speed.Y * 5, 0, default, 1.3f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].noLight = true;
                    Main.dust[num2].noGravity = true;
                    Main.dust[num2].noLight = true;
					Dust dust7 = Dust.NewDustDirect(target.Center, target.width, target.height, DustID.IceTorch, speed.X * 5, speed.Y * 5, 0, default, 1.3f);
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
                    float finalDefense = enemy.defense - Main.player[projectile.owner].GetArmorPenetration(DamageClass.Generic);
                    enemy.ichor = false;
                    enemy.betsysCurse = false;
                    if (finalDefense < 0)
                    {
                        finalDefense = 0;
                    }
                    if (finalDefense > 100)
                    {
                        finalDefense = 100;
                    }
                    int damage = damageDone + (int)finalDefense / 2;
                    if (!enemy.dontTakeDamage && enemy.active && !enemy.friendly && !enemy.immortal && distanceTo < distance && enemy.whoAmI != target.whoAmI)
					{
						Main.player[projectile.owner].ApplyDamageToNPC(enemy, damage, 0f, 0, crit: false);
					}
				}
			}
			return;
		}

	}
}