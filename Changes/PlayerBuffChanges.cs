using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject;
using TRAEProject.NewContent.Buffs;
using TRAEProject.Changes.Items;
using TRAEProject.Common;
using TRAEProject.Changes;
using TRAEProject.Changes.Weapon.Summon.Minions;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace ChangesBuffs
{
    public class PlayerBuffChanges : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            switch (type)
            {
                case BuffID.ThornWhipPlayerBuff:
                    player.meleeSpeed -= 0.05f;
                    player.autoReuseGlove = true;
                    return;
                case BuffID.SwordWhipPlayerBuff:
                    player.meleeSpeed -= 0.1f;
                    player.autoReuseGlove = true; 
                    return;
                case BuffID.ScytheWhipPlayerBuff:
                    player.meleeSpeed -= 0.15f;
                    player.autoReuseGlove = true;
                    return;
                case BuffID.ObsidianSkin:
                    player.buffImmune[BuffID.OnFire] = false;
                    player.buffImmune[BuffID.Burning] = true;
                    player.fireWalk = false;
                    return;
                case BuffID.StarInBottle:
                    player.statManaMax2 += 20;
                    return;
                case BuffID.Titan:
                    player.meleeScaleGlove = true;
                    return;
                case BuffID.Thorns:
                    player.thorns -= 1f;
                    player.GetModPlayer<OnHitItems>().newthorns += 0.33f;
                    return;
                case BuffID.Archery:
                    player.arrowDamage -= 0.08f; //  because Archery Potion uses a unique multiplier -8% makes it effectively +10% arrow damage 
                    return;
                case BuffID.Rabies:
                    player.AddBuff(BuffType<NeoFeralBite>(), player.buffTime[buffIndex], false);
                    player.DelBuff(buffIndex);
                    return;

                case BuffID.ManaRegeneration:
                    player.GetModPlayer<Mana>().manaRegenBoost += 0.2f;
                    return;
                case BuffID.ManaSickness:               
                    player.manaSickReduction = 0f;
                    return;
                case BuffID.WellFed:
                    player.moveSpeed -= 0.15f;
                    return;
                case BuffID.WellFed2:
                    player.moveSpeed -= 0.24f;
                    return;
                case BuffID.WellFed3:
                    player.moveSpeed -= 0.33f;
                    return;
                case BuffID.Swiftness:
                    player.moveSpeed -= 0.10f;
                    return;
                case BuffID.Sunflower:
                case BuffID.SugarRush:
                    player.moveSpeed -= 0.1f;
                    return;
                case BuffID.Panic:
                    player.moveSpeed -= 0.6f;
                    return; 
            }
        }

        public override void ModifyBuffTip(int type, ref string tip, ref int rare)
        {
            switch (type)
            {
                case BuffID.StardustDragonMinion:
                    tip = "The Lunar Dragon will fight for you";
                    return;
                case BuffID.BeetleEndurance1:
                    tip = "Damage taken reduced by 10%";
                    return;
                case BuffID.BeetleEndurance2:
                    tip = "Damage taken reduced by 20%";
                    return;
                case BuffID.BeetleEndurance3:
                    tip = "Damage taken reduced by 30%";
                    return;
                case BuffID.Panic:
                    tip = "Greatly increased movement capabilities";
                    return;
                case BuffID.Archery:
                    tip = "10% increased arrow damage, 20% increased arrow speed";
                    return;
                case BuffID.Titan:
                    tip = "50% increased knockback and 10% increased melee weapon size";
                    return;
                case BuffID.AmmoReservation:
                    tip = "Prevents basic ammo consumption";
                    return;
                case BuffID.Inferno:
                    tip = "Your attacks cause fiery explosions";
                    return;
                case BuffID.WeaponImbueNanites:
                    tip = "Melee attacks confuse enemies and increase health regeneration";
                    return;
                case BuffID.ManaRegeneration:
                    tip = "Increases mana regeneration by 20%";
                    return;
	            case BuffID.ManaSickness:
                    tip = "Can't drink another mana potion";
                    return;
                case BuffID.Swiftness:
                    tip = "15% increased movement speed";
                    return;
            }
        }
    }
    public class BuffChangesModPlayer : ModPlayer
    {

        public bool Celled = false;

        public override void ResetEffects()
        {
            Celled = false;
        }
        public override void UpdateDead()
        {
            Celled = false;
        }

        public override void UpdateBadLifeRegen()
        {
            if (Player.HasBuff(BuffID.Bleeding) && Main.expertMode)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegen -= 4;
            }
            if (Celled)
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen = 0;
                }
                Player.lifeRegen -= 20;
            }
        }

        public override void UpdateLifeRegen()
        {
            if (Player.HasBuff(BuffID.Regeneration))
            {
                if (Player.lifeRegen > 0)
                {
                    Player.lifeRegen -= 2;
                }
            }
            if (Player.HasBuff(BuffType<NanoHealing>()))
            {
                if (Player.lifeRegen < 0)
                {
                    Player.lifeRegen += 8; // used only to negate up to 4 damage over time
                    if (Player.lifeRegen > 0)
                    {
                        Player.lifeRegen = 0;
                    }
                }
                Player.lifeRegenTime += 3; // makes it tick up four times faster
                Player.lifeRegenCount += 2; // adds 1 hp/s
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Celled && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " was consumed by cells");
                return true;
            }
            if (Player.HasBuff(BuffID.Bleeding) && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.name + " bled to death");
                return true;
            }
            return true;
        }


        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (Player.inferno)
            {
                Lighting.AddLight((int)(target.Center.X / 16f), (int)(target.Center.Y / 16f), 0.65f, 0.4f, 0.1f);
                int OnFireID = 24;
                float range = 100f;
                int RingDamage = damage / 10;
                if (RingDamage < 1)
                {
                    RingDamage = 1;
                }
                Vector2 spinningpoint1 = ((float)Main.rand.NextDouble() * 6.283185f).ToRotationVector2();
                Vector2 spinningpoint2 = spinningpoint1;
                float RandomNumberBetweenSixAndTen = Main.rand.Next(3, 5) * 2;
                int Twenty = 20;
                float OneOrMinusOne = Main.rand.Next(2) == 0 ? 1f : -1f; // one in three chance of it being 1
                bool flag = true;
                for (int i = 0; i < Twenty * RandomNumberBetweenSixAndTen; ++i) // makes 120 or 240 dusts total
                {
                    if (i % Twenty == 0)
                    {
                        spinningpoint2 = spinningpoint2.RotatedBy(OneOrMinusOne * (6.28318548202515 / RandomNumberBetweenSixAndTen), default);
                        spinningpoint1 = spinningpoint2;
                        flag = !flag;
                    }
                    else
                    {
                        float num4 = 6.283185f / (Twenty * RandomNumberBetweenSixAndTen);
                        spinningpoint1 = spinningpoint1.RotatedBy(num4 * OneOrMinusOne * 3.0, default);
                    }
                    float num5 = MathHelper.Lerp(7.5f, 60f, i % Twenty / Twenty);
                    int index2 = Dust.NewDust(new Vector2(target.Center.X, target.Center.Y), 6, 6, 127, 0.0f, 0.0f, 100, default, 3f);
                    Dust dust1 = Main.dust[index2];
                    dust1.velocity = Vector2.Multiply(dust1.velocity, 0.1f);
                    Dust dust2 = Main.dust[index2];
                    dust2.velocity = Vector2.Add(dust2.velocity, Vector2.Multiply(spinningpoint1, num5));
                    if (flag)
                        Main.dust[index2].scale = 0.9f;
                    Main.dust[index2].noGravity = true;
                }
                int NPCLimit = 0;
                for (int k = 0; k < 200; k++)
                {
                    NPC nPC = Main.npc[k];
                    if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(target.Center, nPC.Center) <= range)
                    {
                        ++NPCLimit;
                        if (NPCLimit < 5)
                        {
                            int finalDefense = nPC.defense - Player.armorPenetration;
                            nPC.ichor = false;
                            nPC.betsysCurse = false;
                            if (finalDefense < 0)
                            {
                                finalDefense = 0;
                            }
                            if (finalDefense > 100)
                            {
                                finalDefense = 100;
                            }
                            RingDamage += finalDefense / 2;
                            Player.ApplyDamageToNPC(nPC, RingDamage, 0f, 0, crit: false);
                            if (nPC.FindBuffIndex(OnFireID) == -1)
                            {
                                nPC.AddBuff(OnFireID, 120);
                            }
                        }
                    }
                }
            }
            return;
        }
    }
}

        