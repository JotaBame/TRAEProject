using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Buffs;
using TRAEProject.Changes.Items;
using TRAEProject.Changes;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TRAEProject.Changes.Accesory;
using System.Collections.Generic;

namespace TRAEProject
{
    public class PlayerBuffChanges : GlobalBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[BuffID.Bewitched] = false;
            Main.persistentBuff[BuffID.Bewitched] = true;
            Main.buffNoSave[BuffID.AmmoBox] = false;
            Main.persistentBuff[BuffID.AmmoBox] = true;
            Main.buffNoSave[BuffID.Sharpened] = false;
            Main.persistentBuff[BuffID.Sharpened] = true;
            Main.buffNoSave[BuffID.Clairvoyance] = false;
            Main.persistentBuff[BuffID.Clairvoyance] = true;
            Main.buffNoSave[BuffID.WarTable] = false;
            Main.persistentBuff[BuffID.WarTable] = true;
        }
        public override void Update(int type, Player player, ref int buffIndex)
        {
            switch (type)
            {

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
                    player.GetModPlayer<OnHitEffects>().newthorns += 0.33f;
                    return;
                case BuffID.Rabies:
                    player.AddBuff(BuffType<NeoFeralBite>(), player.buffTime[buffIndex], false);
                    player.DelBuff(buffIndex);
                    return;
                case BuffID.Inferno:
                    player.infernoCounter = 0;
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
                    player.moveSpeed -= 0.15f;
                    return;
                case BuffID.Sunflower:
                case BuffID.SugarRush:
                    player.moveSpeed -= 0.1f;
                    return;
                case BuffID.Panic:
                    player.moveSpeed -= 0.6f;
                    return;
                case BuffID.WaterWalking:
                    player.GetModPlayer<Mobility>().TRAEwaterwalk = true;
                    break;
                case BuffID.RapidHealing:
                    if (player.HeldItem.type == ItemID.PalladiumSword ||
                        player.HeldItem.type == ItemID.PalladiumPike ||
                        player.HeldItem.type == ItemID.PalladiumRepeater
                        )
                        player.lifeRegenCount += 1;
                    break;
            }
        }

        public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
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
                    tip = "40% increased movement speed";
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
                    tip = Mobility.swiftSpeed + "% increased movement speed";
                    return;
                case BuffID.StarInBottle:
                    tip = "Increased max mana by 20";
                    return;
                case BuffID.SugarRush:
                    tip = "10% increased movement speed and 20% increased mining speed";
                    return;
    
            }
        }
    }
    public class InfernoProjectiles : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        int InfernoHits = 0;
        int infernoTimer = 0;
        public override void AI(Projectile projectile)
        {
            if (InfernoHits > 0)
            {
                infernoTimer++;
                if (infernoTimer >= 10)
                {
                    infernoTimer = 0;
                    InfernoHits -= 1;
                }    
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner]; 
            if (player.inferno && InfernoHits < 3)
            {
                InfernoHits += 1;
                Lighting.AddLight((int)(target.Center.X / 16f), (int)(target.Center.Y / 16f), 0.65f, 0.4f, 0.1f);
                int OnFireID = 24;
                float range = 100f;
                int RingDamage = damageDone / 10;
                if (RingDamage < 1)
                {
                    RingDamage = 1;
                }
                int dustsToMake = 5 + damageDone / 10;
                if(dustsToMake > 600)
                {
                    dustsToMake = 600;
                }
                for (int i = 0; i < dustsToMake; i++)
                {
                    float radius = range / 62.5f;
                    // Why 62.5f and not 41.67?
                    // This is 150% of 41.67, because below the extra dusts get increased distance, with a max of 50% more.
                    // Therefore, the circle of flames more or less accurately represents the radius of the fire ring.
                    Vector2 speed = Main.rand.NextVector2CircularEdge(radius, radius);
                    Dust d = Dust.NewDustPerfect(target.Center, DustID.Torch, speed * 5, Scale: 3f);
                    if (Main.rand.NextBool(3))
                    {
                        d.scale *= Main.rand.NextFloat(1.25f, 1.5f);
                        d.velocity *= Main.rand.NextFloat(1.25f, 1.5f);
                    }
                    d.noGravity = true;
                }
                    int NPCLimit = 0;
                for (int k = 0; k < 200; k++)
                {
                    NPC nPC = Main.npc[k];
                    if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(target.Center, nPC.Center) <= range)
                    {
                        ++NPCLimit;
                        if (NPCLimit < 3)
                        {
                            float finalDefense = nPC.defense - player.GetArmorPenetration(DamageClass.Generic);
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
                            RingDamage += (int)finalDefense / 2;
                            player.ApplyDamageToNPC(nPC, RingDamage, 0f, 0, crit: false);
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
        public override void PostUpdateBuffs()
        {
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

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (npc.HasBuff(BuffID.WitheredWeapon))
            {
                modifiers.FinalDamage *= 0.84f;
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
    }
 
}

        