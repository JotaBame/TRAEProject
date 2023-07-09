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
            Player.endurance = 0;
            if (RoyalGelCooldown > 0)
            {
                Player.drippingSlime = true;
                RoyalGelCooldown--;
            }
        }
        public override void PostUpdateEquips()
        {
            Player.endurance = 0;
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
            Player.DefenseEffectiveness *= 0f;

            float defense = Player.statDefense;
            float DefenseDamageReduction = defense / (defense + 80); // Formula for defense

            modifiers.FinalDamage *= 1 - DefenseDamageReduction;
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
            DamageAfterDefenseAndDR += (int)(modifiers.FinalDamage.Flat);
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            Player.DefenseEffectiveness *= 0f;

            float defense = Player.statDefense;
            float DefenseDamageReduction = defense / (defense + 80); // Formula for defense
            modifiers.FinalDamage *= 1 - DefenseDamageReduction;
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
            if (pocketMirror)
            {
                modifiers.FinalDamage *= 0.88f;
            }
            if (Player.beetleDefense)
            {
                float beetleEndurance = (1 - 0.15f * Player.beetleOrbs) / (1 - 0.10f * Player.beetleOrbs); 

                modifiers.FinalDamage *= beetleEndurance;
            }
            DamageAfterDefenseAndDR += (int)(modifiers.FinalDamage.Flat);
        }
    }
    public class DRAccessories : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
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
                case ItemID.PocketMirror:
                    player.GetModPlayer<Defense>().pocketMirror = true;
                    return;
                case ItemID.RoyalGel:
                    player.GetModPlayer<Defense>().RoyalGel = true;
                    return;
            }

            return;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.WormScarf:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Reduces damage taken by 17%";
                        }
                    }
                    return;
                case ItemID.FrozenTurtleShell:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Puts a shell around the owner when below 50% life that reduces damage by 25%";
                        }
                    }
                    return;
                case ItemID.FrozenShield:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Puts a shell around the owner when below 50% life that reduces damage by 25%";
                        }
                    }
                    return;
                case ItemID.PocketMirror:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "12% reduced damage from projectiles\nGrants immunity to Petrified";
                        }
                    }
                    return;
                case ItemID.BrainOfConfusion:
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
                    return;
            }
        }
    }
    public class DRBuffs : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            switch (type)
            {
                case BuffID.Endurance:
                    player.GetModPlayer<Defense>().EndurancePot = true; player.endurance -= 0.1f;
                    return;
                case BuffID.IceBarrier:
                    player.GetModPlayer<Defense>().IceBarrier = true; player.endurance -= 0.25f;
                    return;
                case BuffID.CatBast:
                    player.GetModPlayer<Defense>().FlatDamageReduction += 5;
                    player.statDefense -= 5;
                    return;
            }
            return;
        }
        public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
        {
            switch (type)
            {
                case BuffID.IceBarrier:
                    tip = "Reduces damage taken by 20%";
                    return;
                case BuffID.CatBast:
                    tip = "Damage Reduced by 5";
                    return;
            }
        }
    }
}
    




