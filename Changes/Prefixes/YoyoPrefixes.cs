using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TRAEProject.Changes.Prefixes
{
	//This class defines what yoyo prefixes can have
    public abstract class YoyoPrefix : ModPrefix
    {
        public override float RollChance(Item item)
        {
            return 1f;
        }
        public override bool CanRoll(Item item) => true;
        public override PrefixCategory Category => PrefixCategory.Custom;
        byte damage = 0;
        byte negDamage = 0;
        byte crit = 0;
        byte knockback = 0;
        byte negKnockback = 0;
        byte range = 0;
        byte negRange = 0;
        byte speed = 0;
        byte negSpeed = 0;
        public void SetPrefix(sbyte damage, byte crit, sbyte speed, sbyte range, sbyte knockback)
        {
            if (damage < 0)
            {
                this.negDamage = (byte)(-1 * damage);
            }
            else
            {
                this.damage = (byte)damage;
            }
            if (speed < 0)
            {
                this.negSpeed = (byte)(-1 * speed);
            }
            else
            {
                this.speed = (byte)speed;
            }
            if (range < 0)
            {
                this.negRange = (byte)(-1 * range);
            }
            else
            {
                this.range = (byte)range;
            }
            if (knockback < 0)
            {
                this.negKnockback = (byte)(-1 * knockback);
            }
            else
            {
                this.knockback = (byte)knockback;
            }
            this.crit = crit;
        }


        public override void ModifyValue(ref float valueMult)
        {
            float multiplier = 1f * (1 + damage * 0.004f)
                * (1 + crit * 0.004f)
                * (1 + speed * 0.002f)
                * (1 + range * 0.002f)
                * (1 + knockback * 0.001f)
                * (1 - negDamage * 0.004f)
                * (1 - negSpeed * 0.001f)
                * (1 - negRange * 0.001f)
                * (1 - negKnockback * 0.001f);
            valueMult *= multiplier;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            if (negDamage > 0)
            {
                damageMult = 1f + (-this.negDamage) * .01f;
            }
            else
            {
                damageMult = 1f + (this.damage) * .01f;
            }
            if (negKnockback > 0)
            {
                knockbackMult = 1f + (-this.negKnockback) * .01f;
            }
            else
            {
                knockbackMult = 1f + (this.knockback) * .01f;
            }

            critBonus = this.crit;
        }
        public override void Apply(Item item)
        {
            if(negSpeed > 0)
            {
				item.GetGlobalItem<YoyoStats>().speed = 1f + (-this.negSpeed) *.01f;
            }
            else
            {
                item.GetGlobalItem<YoyoStats>().speed = 1f + (this.speed) * .01f;
            }
            if(negRange > 0)
            {
                item.GetGlobalItem<YoyoStats>().range = 1f + (-this.negRange) * .01f;
            }
            else
            {
                item.GetGlobalItem<YoyoStats>().range = 1f + (this.range) * .01f;
            }
            base.Apply(item);
        }
    }
	//each class below is an idividual prefix
    public class Radical : YoyoPrefix
    {
        public override void SetStaticDefaults()
        {
            SetPrefix(15, 5, 10, 10, 15);
        }
    }
	public class Extreme : YoyoPrefix
	{
		public override void SetStaticDefaults()
		{
			SetPrefix(10, 3, 10, 10, 10);
		}
	}
	public class Pounding : YoyoPrefix
	{
		public override void SetStaticDefaults()
		{
			SetPrefix(5, 0, 20, 0, 35);
		}
	}
	public class Zoned : YoyoPrefix
	{
		public override void SetStaticDefaults()
		{
			SetPrefix(10, 0, -20, 30, 0);
		}
	}
	public class Relentless : YoyoPrefix
	{
		public override void SetStaticDefaults()
		{
			SetPrefix(10, 10, 30, -30, -15);
		}
	}
	public class Tricky : YoyoPrefix
    {
		public override void SetStaticDefaults()
		{
			SetPrefix(0, 0, 20, 20, 0);
		}
	}
	public class Extended : YoyoPrefix
	{
		public override void SetStaticDefaults()
		{
			SetPrefix(0, 0, 0, 20, 0);
		}
	}
	public class Bad : YoyoPrefix
	{
		public override void SetStaticDefaults()
		{
			SetPrefix(-30, 0, -20, -20, -35);
		}
	}
	public class YoyoStats : GlobalItem
    {
        public float range = 1f;
        public float speed = 1f;

        public override bool InstancePerEntity => true;
        public override void SetStaticDefaults()
        {
            IL.Terraria.Projectile.AI_099_2 += HookYoyo;
        }
		//rewrite yoyo AI so they can benifit from the rang and speed stats
        private void HookYoyo(ILContext il)
        {
            var c = new ILCursor(il);
			c.Emit(OpCodes.Ldarg_0);
			c.EmitDelegate<Action<Projectile>>((projectile) =>
            {
                #region vanlilla copy lazy
                bool flag = false;
				for (int i = 0; i < projectile.whoAmI; i++)
				{
					if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == projectile.type)
					{
						flag = true;
					}
				}
				if (projectile.owner == Main.myPlayer)
				{
					projectile.localAI[0] += 1f;
					if (flag)
					{
						projectile.localAI[0] += (float)Main.rand.Next(10, 31) * 0.1f;
					}
					float num = projectile.localAI[0] / 60f;
					num /= (1f + Main.player[projectile.owner].meleeSpeed) / 2f;
					float num2 = ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type];
					if (num2 != -1f && num > num2)
					{
						projectile.ai[0] = -1f;
					}
				}
				if (projectile.type == 603 && projectile.owner == Main.myPlayer)
				{
					projectile.localAI[1] += 1f;
					if (projectile.localAI[1] >= 6f)
					{
						float num3 = 400f;
						Vector2 vector = projectile.velocity;
						Vector2 vector2 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						vector2.Normalize();
						vector2 *= (float)Main.rand.Next(10, 41) * 0.1f;
						if (Main.rand.Next(3) == 0)
						{
							vector2 *= 2f;
						}
						vector *= 0.25f;
						vector += vector2;
						for (int j = 0; j < 200; j++)
						{
							if (Main.npc[j].CanBeChasedBy(projectile, false))
							{
								float num4 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
								float num5 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
								float num6 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num4) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num5);
								if (num6 < num3 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height))
								{
									num3 = num6;
									vector.X = num4;
									vector.Y = num5;
									vector -= projectile.Center;
									vector.Normalize();
									vector *= 8f;
								}
							}
						}
						vector *= 0.8f;
						Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X - vector.X, projectile.Center.Y - vector.Y, vector.X, vector.Y, 604, projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
						projectile.localAI[1] = 0f;
					}
				}
				bool flag2 = false;
				if (projectile.type >= 556 && projectile.type <= 561)
				{
					flag2 = true;
				}
				if (Main.player[projectile.owner].dead)
				{
					projectile.Kill();
					return;
				}
				if (!flag2 && !flag)
				{
					Main.player[projectile.owner].heldProj = projectile.whoAmI;
					Main.player[projectile.owner].SetDummyItemTime(2);
					if (projectile.position.X + (float)(projectile.width / 2) > Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2))
					{
						Main.player[projectile.owner].ChangeDir(1);
						projectile.direction = 1;
					}
					else
					{
						Main.player[projectile.owner].ChangeDir(-1);
						projectile.direction = -1;
					}
				}
				if (projectile.velocity.HasNaNs())
				{
					projectile.Kill();
				}
				projectile.timeLeft = 6;
				#endregion
				float num7 = ProjectileID.Sets.YoyosMaximumRange[projectile.type] * Main.player[projectile.owner].HeldItem.GetGlobalItem<YoyoStats>().range;
				float num8 = ProjectileID.Sets.YoyosTopSpeed[projectile.type] * Main.player[projectile.owner].HeldItem.GetGlobalItem<YoyoStats>().speed;
                #region more vanilla lazy copy
                
                if (projectile.type == 545)
				{
					if (Main.rand.Next(6) == 0)
					{
						int num9 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 0, default(Color), 1f);
						Main.dust[num9].noGravity = true;
					}
				}
				else if (projectile.type == 553 && Main.rand.Next(2) == 0)
				{
					int num10 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 0, default(Color), 1f);
					Main.dust[num10].noGravity = true;
					Main.dust[num10].scale = 1.6f;
				}
				if (Main.player[projectile.owner].yoyoString)
				{
					num7 = num7 * 1.25f + 30f;
				}
				num7 /= (1f + Main.player[projectile.owner].meleeSpeed * 3f) / 4f;
				num8 /= (1f + Main.player[projectile.owner].meleeSpeed * 3f) / 4f;
				float num11 = 14f - num8 / 2f;
				if (num11 < 1f)
				{
					num11 = 1f;
				}
				float num12 = 5f + num8 / 2f;
				if (flag)
				{
					num12 += 20f;
				}
				if (projectile.ai[0] >= 0f)
				{
					if (projectile.velocity.Length() > num8)
					{
						projectile.velocity *= 0.98f;
					}
					bool flag3 = false;
					bool flag4 = false;
					Vector2 vector3 = Main.player[projectile.owner].Center - projectile.Center;
					if (vector3.Length() > num7)
					{
						flag3 = true;
						if ((double)vector3.Length() > (double)num7 * 1.3)
						{
							flag4 = true;
						}
					}
					if (projectile.owner == Main.myPlayer)
					{
						if (!Main.player[projectile.owner].channel || Main.player[projectile.owner].stoned || Main.player[projectile.owner].frozen)
						{
							projectile.ai[0] = -1f;
							projectile.ai[1] = 0f;
							projectile.netUpdate = true;
						}
						else
						{
							Vector2 vector4 = Main.ReverseGravitySupport(Main.MouseScreen, 0f) + Main.screenPosition;
							float x = vector4.X;
							float y = vector4.Y;
							Vector2 vector5 = new Vector2(x, y) - Main.player[projectile.owner].Center;
							if (vector5.Length() > num7)
							{
								vector5.Normalize();
								vector5 *= num7;
								vector5 = Main.player[projectile.owner].Center + vector5;
								x = vector5.X;
								y = vector5.Y;
							}
							if (projectile.ai[0] != x || projectile.ai[1] != y)
							{
								Vector2 vector6 = new Vector2(x, y) - Main.player[projectile.owner].Center;
								if (vector6.Length() > num7 - 1f)
								{
									vector6.Normalize();
									vector6 *= num7 - 1f;
									Vector2 vector7 = Main.player[projectile.owner].Center + vector6;
									x = vector7.X;
									y = vector7.Y;
								}
								projectile.ai[0] = x;
								projectile.ai[1] = y;
								projectile.netUpdate = true;
							}
						}
					}
					if (flag4 && projectile.owner == Main.myPlayer)
					{
						projectile.ai[0] = -1f;
						projectile.netUpdate = true;
					}
					if (projectile.ai[0] >= 0f)
					{
						if (flag3)
						{
							num11 /= 2f;
							num8 *= 2f;
							if (projectile.Center.X > Main.player[projectile.owner].Center.X && projectile.velocity.X > 0f)
							{
								projectile.velocity.X = projectile.velocity.X * 0.5f;
							}
							if (projectile.Center.Y > Main.player[projectile.owner].Center.Y && projectile.velocity.Y > 0f)
							{
								projectile.velocity.Y = projectile.velocity.Y * 0.5f;
							}
							if (projectile.Center.X < Main.player[projectile.owner].Center.X && projectile.velocity.X < 0f)
							{
								projectile.velocity.X = projectile.velocity.X * 0.5f;
							}
							if (projectile.Center.Y < Main.player[projectile.owner].Center.Y && projectile.velocity.Y < 0f)
							{
								projectile.velocity.Y = projectile.velocity.Y * 0.5f;
							}
						}
						Vector2 vector8 = new Vector2(projectile.ai[0], projectile.ai[1]) - projectile.Center;
						if (flag3)
						{
							num11 = 1f;
						}
						projectile.velocity.Length();
						float num13 = vector8.Length();
						if (num13 > num12)
						{
							vector8.Normalize();
							float num14 = Math.Min(num13 / 2f, num8);
							if (flag3)
							{
								num14 = Math.Min(num14, num8 / 2f);
							}
							vector8 *= num14;
							projectile.velocity = (projectile.velocity * (num11 - 1f) + vector8) / num11;
						}
						else if (flag)
						{
							if ((double)projectile.velocity.Length() < (double)num8 * 0.6)
							{
								vector8 = projectile.velocity;
								vector8.Normalize();
								vector8 *= num8 * 0.6f;
								projectile.velocity = (projectile.velocity * (num11 - 1f) + vector8) / num11;
							}
						}
						else
						{
							projectile.velocity *= 0.8f;
						}
						if (flag && !flag3 && (double)projectile.velocity.Length() < (double)num8 * 0.6)
						{
							projectile.velocity.Normalize();
							projectile.velocity *= num8 * 0.6f;
						}
					}
				}
				else
				{
					num11 = (float)((int)((double)num11 * 0.8));
					num8 *= 1.5f;
					projectile.tileCollide = false;
					Vector2 vector9 = Main.player[projectile.owner].Center - projectile.Center;
					float num15 = vector9.Length();
					if (num15 < num8 + 10f || num15 == 0f || num15 > 2000f)
					{
						projectile.Kill();
					}
					else
					{
						vector9.Normalize();
						vector9 *= num8;
						projectile.velocity = (projectile.velocity * (num11 - 1f) + vector9) / num11;
					}
				}
				projectile.rotation += 0.45f;
				#endregion
			});
            c.Emit(OpCodes.Ret);
        }
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (speed != 1)
            {
                TooltipLine line = new TooltipLine(TRAEProj.Instance, "TRAEYoyoSpeed", (speed <1 ? "-" : "+") + (int)Math.Round((speed < 1 ? (1f - speed): (speed - 1f)) * 100) + "% yoyo speed");
                line.IsModifier = true;
                if(speed < 1)
                {
                    line.IsModifierBad = true;
                }
                int kbIndex = tooltips.FindIndex(TL => TL.Name == "PrefixKnockback");
                if (kbIndex != -1)
                {
                    tooltips.Insert(kbIndex, line);
                }
                else
                {
                    tooltips.Add(line);
                }
            }
            if (range != 1)
            {
                TooltipLine line = new TooltipLine(TRAEProj.Instance, "TRAEYoyoRange", (range < 1 ? "-" : "+") + (int)Math.Round((range < 1 ? (1f - range) : (range - 1f)) * 100) + "% range");
                line.IsModifier = true;
                if (range < 1)
                {
                    line.IsModifierBad = true;
                }
                int kbIndex = tooltips.FindIndex(TL => TL.Name == "PrefixKnockback");
                if (kbIndex != -1)
                {
                    tooltips.Insert(kbIndex, line);
                }
                else
                {
                    tooltips.Add(line);
                }
            }
        }
    }
}