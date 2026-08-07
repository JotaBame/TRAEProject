using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.NPCs.Miniboss.Santa;
using TRAEProject.Changes.Weapon.Summon.Minions;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.Armor.IceArmor;
using TRAEProject.NewContent.Items.Weapons.Summoner.AbsoluteZero;
using TRAEProject.NewContent.Items.Weapons.Summoner.Echolalia;
using TRAEProject.NewContent.NPCs.Echosphere.EchoStalker;
using static System.Net.Mime.MediaTypeNames;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Buffs
{

	public class AdvancedWhipTags : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
            if (target.HasBuff<EcholaliaSecondTag>() && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]) && projectile.type != ProjectileID.StardustGuardian && projectile.type != ProjectileType<MadFlocko>())
            {
                EcholaliaHitEffect(target);
                modifiers.SetCrit();
            }
            if (target.HasBuff<AZSecondTag>() && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]) && projectile.type != ProjectileID.StardustGuardian && projectile.type != ProjectileType<MadFlocko>())
			{
				modifiers.SourceDamage *= 3;
			}
		}
		bool firstHit = false;
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)

        {
            if (projectile.type == ProjectileType<AbsoluteZeroP>())
                FirecrackerLikeTag(target, BuffType<AZSecondTag>());

            if (projectile.type == ProjectileType<EcholaliaP>())
               target.AddBuff(BuffType<EcholaliaSecondTag>(), 240);




            if ((projectile.minion || ProjectileID.Sets.MinionShot[projectile.type])
                && projectile.type != ProjectileID.StardustGuardian && projectile.type != ProjectileType<MadFlocko>())// you dont want these two minions you cant turn off to set off the tags
            {
                 if (target.HasBuff(BuffType<AZSecondTag>()))
                {
                    SoundEngine.PlaySound(SoundID.Item28 with { Pitch = -0.5f, MaxInstances = 0 }, target.Center);

                    int buffIndex = target.FindBuffIndex(BuffType<AZSecondTag>());
                    if (buffIndex != -1)
                    {
                        target.DelBuff(buffIndex);
                    }
                    target.GetGlobalNPC<Freeze>().FreezeMe(target, 120);


                    float distance = 160f;

                    foreach (NPC enemy in Main.npc)
                    {
                        int finalDamage = damageDone;

                        if (!enemy.dontTakeDamage && enemy.active && !enemy.friendly && !enemy.immortal && enemy.whoAmI != target.whoAmI
                            && enemy.type != NPCType<EchoStalkerBody1>() && enemy.type != NPCType<EchoStalkerBody2>() && enemy.type != NPCType<EchoStalkerTail>())
                        {
                            Vector2 newMove = enemy.Center - target.Center;

                            float distanceTo = MathF.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);// could simplify this using Vector2.Length?
                            if (distanceTo < distance)
                            {
                                finalDamage += ignoreDefense(enemy, Main.player[projectile.owner]);
                                
                                Main.player[projectile.owner].ApplyDamageToNPC(enemy, finalDamage, 0f, 0, crit: false);

                                target.GetGlobalNPC<Freeze>().FreezeMe(target, 60);


                            }



                        }
                    }
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

                }
                if (target.HasBuff(BuffType<EcholaliaSecondTag>()))
                {

                    int buffIndex = target.FindBuffIndex(BuffType<EcholaliaSecondTag>());
                    if (buffIndex != -1)
                    {
                        target.DelBuff(buffIndex);
                    }

                    foreach (NPC enemy in Main.npc)
                    {

                        if (!enemy.dontTakeDamage && enemy.active && !enemy.friendly && !enemy.immortal && enemy.whoAmI != target.whoAmI
                        && enemy.type != NPCType<EchoStalkerBody1>() && enemy.type != NPCType<EchoStalkerBody2>() && enemy.type != NPCType<EchoStalkerTail>()

                         && enemy.HasBuff(BuffType<EcholaliaSecondTag>()))
                        {
                            int finalDamage = damageDone / 2; // divide by two because it always crits
                          

                            finalDamage += ignoreDefense(enemy, Main.player[projectile.owner]);
                            Main.player[projectile.owner].ApplyDamageToNPC(enemy, finalDamage, hit.Knockback, hit.HitDirection, crit: true);

                            buffIndex = enemy.FindBuffIndex(BuffType<EcholaliaSecondTag>());
                            if (buffIndex != -1)
                            {
                                enemy.DelBuff(buffIndex);
                            }
                            EcholaliaHitEffect(enemy);

                           
                        }

                    }




                }
                return;
            }
        }
 
        void FirecrackerLikeTag(NPC target, int Buff)

        {
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.whoAmI != target.whoAmI && npc.HasBuff(Buff))
                {

                    int buffIndex = npc.FindBuffIndex(Buff);
                    if (buffIndex != -1)
                    {
                        npc.DelBuff(buffIndex);
                    }
                }
            }

            target.AddBuff(Buff, 240);
            return;
        }
        void EcholaliaHitEffect(NPC enemy)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath7 with { Pitch = -0.5f, MaxInstances = 4 }, enemy.Center);


            for (int j = 0; j < 30; j++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(4f, 4f);
                Dust d = Dust.NewDustPerfect(enemy.Center, DustID.PinkTorch, speed * 2, Scale: 1.5f);
                d.noGravity = true;


            }
        }
        int ignoreDefense(NPC enemy, Player player)
        {
            float defense = enemy.defense - player.GetArmorPenetration(DamageClass.Generic);
            enemy.ichor = false;
            enemy.betsysCurse = false;
            if (defense < 0)
            {
                defense = 0;
            }
            if (defense > 100)
            {
                defense = 100;
            }
             return ((int)defense / 2);
        }
    }
    public class AZSecondTag : ModBuff
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
    public class EcholaliaSecondTag : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.IsATagBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            // DisplayName.SetDefault("KaleidoscopeNewDebuff");
            // Description.SetDefault(""); 
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            for (int i = 0; i < 2; i++)
            {
                int num2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.PinkTorch, 0f, 0f, 100, default, 1f);

                Main.dust[num2].noGravity = true;
            }
         }
    }

}