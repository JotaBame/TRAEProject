using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.ID;
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes;
using System.Collections.Generic;
using TRAEProject.Changes.Accesory;
using TRAEProject.NewContent.Items.Accesories;

namespace TRAEProject
{
    public class Defense : ModPlayer
    {
        public int DamageAfterDefenseAndDR = 0;

        public bool WormScarf = false;
        public bool newBrain = false;
        public bool EndurancePot = false;
        public bool IceBarrier = false;
        public bool pocketMirror = false;
        public float mirrorReflectDamage = 0;
        public bool RoyalGel = false;
        public int RoyalGelCooldown = 0;
        public int FlatDamageReduction = 0;

        public override void ResetEffects()
        {
            RoyalGel = false;
            DamageAfterDefenseAndDR = 0;
            IceBarrier = false;
            newBrain = false;
            EndurancePot = false;
            WormScarf = false;
            pocketMirror = false;
            FlatDamageReduction = 0;

        }
        public override void UpdateDead()
        {
            RoyalGelCooldown = 0;
            RoyalGel = false;
            DamageAfterDefenseAndDR = 0;
            IceBarrier = false;
            newBrain = false;
            EndurancePot = false;
            WormScarf = false;
            pocketMirror = false;
            FlatDamageReduction = 0;
        }
        public override void PostUpdate()
        {
             if (RoyalGelCooldown > 0)
            {
                Player.drippingSlime = true;
                RoyalGelCooldown--;
            }
        }
 
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (newBrain && Main.rand.NextBool(6) && Player.FindBuffIndex(321) == -1)
            {
                Player.BrainOfConfusionDodge();
                for (int i = 0; i < 200; i++)
                {
                    if (!Main.npc[i].active || Main.npc[i].friendly)
                    {
                        continue;
                    }
                    float enemyPositionToThePlayer = (Main.npc[i].Center - Player.Center).Length();
                    float MaxRange = 900;
                    if (enemyPositionToThePlayer < MaxRange)
                    {
                        float duration = 150;
                        Main.npc[i].AddBuff(31, (int)duration);
                    }

                }
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center.X + (float)Main.rand.Next(-40, 40), Player.Center.Y - (float)Main.rand.Next(20, 60), Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, 565, 0, 0f, Player.whoAmI);
                return true;
            }
            return false;
        }
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (GetInstance<TRAEConfig>().DefenseRework)
            {
                Player.DefenseEffectiveness *= 0f;
                float defense = Player.statDefense;
                float DefenseDamageReduction = defense / (defense + 80); // Formula for defense
                modifiers.FinalDamage *= 1 - DefenseDamageReduction;
            }
            
            if (RoyalGel && RoyalGelCooldown == 0)
            {
                RoyalGelCooldown = 30 * 60;
                modifiers.SourceDamage.Flat -= 25;
                SoundEngine.PlaySound(SoundID.NPCDeath1 with { MaxInstances = 0 });
                for (int i = 0; i < 25; ++i)
                {
                    Vector2 position10 = new Vector2(Player.position.X, Player.position.Y);
                    Dust dust = Dust.NewDustDirect(position10, Player.width, Player.height, DustID.t_Slime, 0f, 0f, 100, default, 2.5f);
                    dust.velocity *= 3f;
                }
            }

            modifiers.SourceDamage.Flat -= FlatDamageReduction;
            if (EndurancePot)
            {
                modifiers.FinalDamage *= 0.90f;
            }
            if (WormScarf)
            {
                modifiers.FinalDamage *= 0.83f;
            }
            if (IceBarrier)
            {
                modifiers.FinalDamage *= 0.75f;
            }

            if (Player.beetleDefense)
            {
                float beetleEndurance = (1 - 0.15f * Player.beetleOrbs) / (1 - 0.10f * Player.beetleOrbs);

                modifiers.FinalDamage *= beetleEndurance;
            }
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (GetInstance<TRAEConfig>().DefenseRework)
            {
                Player.DefenseEffectiveness *= 0f;
                float defense = Player.statDefense;
                float DefenseDamageReduction = defense / (defense + 80); // Formula for defense

                modifiers.FinalDamage *= 1 - DefenseDamageReduction;
            }
            if (RoyalGel && RoyalGelCooldown == 0)
            {
                RoyalGelCooldown = 30 * 60;
                modifiers.SourceDamage.Flat -= 25;
                SoundEngine.PlaySound(SoundID.NPCDeath1 with { MaxInstances = 0 });
                for (int i = 0; i < 25; ++i)
                {
                    Vector2 position10 = new Vector2(Player.position.X, Player.position.Y);
                    Dust dust = Dust.NewDustDirect(position10, Player.width, Player.height, DustID.t_Slime, 0f, 0f, 100, default, 2.5f);
                    dust.velocity *= 3f;
                }
            }

            modifiers.SourceDamage.Flat -= FlatDamageReduction;
            if (EndurancePot)
            {
                modifiers.FinalDamage *= 0.90f;
            }
            if (WormScarf)
            {
                modifiers.FinalDamage *= 0.83f;
            }
            if (IceBarrier)
            {
                modifiers.FinalDamage *= 0.75f;
            }
   
            if (Player.beetleDefense)
            {
                float beetleEndurance = (1 - 0.15f * Player.beetleOrbs) / (1 - 0.10f * Player.beetleOrbs);

                modifiers.FinalDamage *= beetleEndurance;
            }
            if (pocketMirror)
            {
                modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
                {
                    float damageReduction = info.Damage / 1000f;
                    if (damageReduction > 0.25f)
                        damageReduction = 0.25f;
                    mirrorReflectDamage = info.Damage * damageReduction;

                    info.Damage = (int)(info.Damage - mirrorReflectDamage);
                    mirrorReflectDamage = MathF.Round(mirrorReflectDamage, 0);
                    if (Player.GetModPlayer<NazarDebuffs>().NazarMirror)
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ProjectileType<MirrorShotEvil>(), (int)(mirrorReflectDamage * 10), 5f);
                    else
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ProjectileType<MirrorShot>(), (int)(mirrorReflectDamage * 10), 5f);

                };
            }
        }
    }
    public class DRAccessories : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (GetInstance<TRAEConfig>().DefenseRework)
            {
                switch (item.type)
                {
                    case ItemID.BrainOfConfusion:
                        player.brainOfConfusionItem = null;
                        player.GetModPlayer<Defense>().newBrain = true;
                        return;
                    case ItemID.WormScarf:
                        player.GetModPlayer<Defense>().WormScarf = true;
                        player.endurance -= 0.17f;
                        return;
                }
            }
                switch (item.type)
            {
 
                case ItemID.PocketMirror:
                     player.GetModPlayer<Defense>().pocketMirror = true;
                    return;
                case ItemID.ReflectiveShades:
                    player.GetModPlayer<Defense>().pocketMirror = true; 
                    player.GetCritChance<GenericDamageClass>() += 8;

                    return;
                case ItemID.RoyalGel:
                    player.GetModPlayer<Defense>().RoyalGel = true;
                    return;
            }

            return;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.BrainOfConfusion && GetInstance<TRAEConfig>().DefenseRework)
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "Has a chance to dodge an attack using illusions";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                    {
                        line.Text = "Temporarily increase critical strike chance and confuse nearby enemies after a dodge";
                    }
                    if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                    {
                        line.Text = "";
                    }
                }
            }
            switch (item.type)
            {
 
                case ItemID.PocketMirror:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Projectiles deal 1% less damage for every 10 damage\nMaxes out at 25% damage reduction\nReduced damage is reflected at the enemy\nGrants immunity to Petrified";
                        }
                    }
                    break;
                case ItemID.ReflectiveShades:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Projectiles deal 1% less damage for every 10 damage\nMaxes out at 25% damage reduction\nReduced damage is reflected at the enemy\nGrants immunity to Petrified\n8% increased critical strike chance";
                        }
               
                    }
                    break;
   
            }
        }
    }
    public class DRBuffs : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            if (GetInstance<TRAEConfig>().DefenseRework)
            {
                switch (type)
                {
                    case BuffID.Endurance:
                        player.GetModPlayer<Defense>().EndurancePot = true; player.endurance -= 0.1f;
                        return;
                    case BuffID.IceBarrier:
                        player.GetModPlayer<Defense>().IceBarrier = true; player.endurance -= 0.25f;
                        return;

                }
            }
            return;
        }

    }
}
    




