using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.Common.ModPlayers;
using TRAEProject.NewContent.TRAEDebuffs;

namespace TRAEProject.Changes.Accesory
{
    public class ObsidianShield : GlobalItem
    { 
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            switch (item.type)
            {
                case ItemID.ObsidianShield:
                    player.GetModPlayer<ObsidianSkullEffect>().shieldRange += 300f;
                    break;
                case ItemID.MagmaStone:
                case ItemID.FireGauntlet:
                    player.GetModPlayer<ObsidianSkullEffect>().magmas += 1;
                    break;
                case 3999: // Magma Skull
                    player.GetModPlayer<ObsidianSkullEffect>().magmaSkull += 1;
                    break;
                case ItemID.ObsidianSkull:
                    player.GetModPlayer<ObsidianSkullEffect>().skull += 1;
                    break;
                case ItemID.MoltenSkullRose:
                    player.GetModPlayer<ObsidianSkullEffect>().moltenskullrose += 1;
                    player.GetModPlayer<CritDamage>().critDamage += 0.09f;
                    
                    break;
               
                case ItemID.MoltenQuiver:
                    player.GetModPlayer<ObsidianSkullEffect>().arrowsburn += 1;
                    break;
                case ItemID.ObsidianRose:
                    player.GetModPlayer<CritDamage>().magicCritDamage += 0.10f;
                    break;
                case ItemID.ObsidianSkullRose:
                    player.GetModPlayer<ObsidianSkullEffect>().roseskull += 1;
                    player.GetModPlayer<CritDamage>().magicCritDamage += 0.10f;
                    player.GetModPlayer<CritDamage>().rangedCritDamage += 0.10f;
                    break;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.MoltenQuiver:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Your arrows will bounce towards nearby enemies, losing 33% damage in the process";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Arrows inflict fire damage, with a very low chance to incinerate on a critical strike";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "Lights wooden arrows ablaze";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip3")
                        {
                            line.Text = "Quiver in fear!";
                        }
                    }
                    break;
                case ItemID.MagmaStone:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text += "\nMelee critical strikes have have a very low chance to incinerate, higher on stronger hits";
                        }
                    }
                    break;                    
                case ItemID.MoltenSkullRose:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Attacks inflict fire damage";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "9% increased critical strike damage";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "Critical strikes lower defense by 3, up to 9";
                        }
                    }
                    break;
                case ItemID.ObsidianSkullRose:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "10% increased Magic and Ranged critical strike damage";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Magic and Ranged critical strikes lower defense by 3, up to 9";
                        }
                    }
                    break;
                case 3999: // Magma Skull
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Melee and Ranged attacks inflict fire damage";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Melee and Ranged critical strikes lower defense by 3, up to 9";
                        }
                    }
                    break;
                case ItemID.ObsidianSkull:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Ranged critical strikes lower defense by 3, up to 9";
                        }
                    }
                    break;
              
                case ItemID.ObsidianRose:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "10% increased Magic critical strike damage";
                        }
                    }
                    break;
                case ItemID.ObsidianShield:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Hitting nearby enemies lowers their defense by 3, up to 9";
                        }
                    }
                    break;
            }
        }
    }
    public class ObsidianSkullEffect : ModPlayer
    {
        public int moltenskullrose = 0;
        public int skull = 0;
        public int roseskull = 0;
        public int arrowsburn = 0;
        public int magmas = 0;
        public int magmaSkull = 0;
        public float shieldRange = 0f;
        public override void ResetEffects()
        {
            magmas = 0; 
            magmaSkull = 0;
            arrowsburn = 0;
            moltenskullrose = 0; 
            skull = 0; 
            roseskull = 0;
            shieldRange = 0f;
        }
        public override void UpdateDead()
        {
            magmas = 0;
            magmaSkull = 0;
            arrowsburn = 0;
            moltenskullrose = 0; 
            skull = 0; roseskull = 0;
            shieldRange = 0f;
        }
        public override void PostUpdateEquips()
        {
            if (Player.fireWalk)
            {
                Player.fireWalk = false;  
			}
            if (Player.lavaRose)
            {
                Player.lavaRose = false;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            if ((target.Center - Player.Center).Length() < shieldRange)
            {
                TRAEDebuff.Apply<ObsidianSkulled>(target, 180, 3);
            }
            if (hit.Crit && (moltenskullrose > 0 || magmaSkull > 0)) // you need to have molten skull for this to work on true melee, no point on doing other checks
            {
                int duration = damageDone / Main.rand.Next(3, 6) * (skull + moltenskullrose + magmaSkull);              
                TRAEDebuff.Apply<ObsidianSkulled>(target, duration, 3);
            }
            if (hit.Crit && Player.magmaStone)
            {
                int chance = 1600 / (damageDone / 2 * magmas);
                if (chance <= 1)
                    chance = 1;
                if (Main.rand.NextBool(chance))
                {
                    if (!target.HasBuff(BuffID.Daybreak))
                    {
                        SoundEngine.PlaySound(SoundID.Item45 with { MaxInstances = 0 });
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 3f);
                        Dust d = Dust.NewDustPerfect(target.Center, DustID.Torch, speed * 3, Scale: 1.5f);
                        d.noGravity = true;
                    }
                    int duration = 180;
                    target.AddBuff(BuffID.Daybreak, duration);
                }
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if ((target.Center - Player.Center).Length() < shieldRange)
            {
                TRAEDebuff.Apply<ObsidianSkulled>(target, 180, 3);
            }
            if (moltenskullrose > 0 || magmaSkull > 0)
            {
                target.AddBuff(BuffID.OnFire3, Main.rand.Next(120, 360));
            }
            if (hit.Crit && Player.magmaStone)
            {
                if (proj.CountsAsClass<MeleeDamageClass>())
                {
                    int chance = 1600 / (damageDone / 2 * magmas);
                    if (chance <= 1)
                        chance = 1;
                    if (Main.rand.NextBool(chance))
                    {
                        if (!target.HasBuff(BuffID.Daybreak))
                        {
                            SoundEngine.PlaySound(SoundID.Item45 with { MaxInstances = 0 });
                        }
                        for (int i = 0; i < 20; i++)
                        {
                            Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 3f);
                            Dust d = Dust.NewDustPerfect(target.Center, DustID.Torch, speed * 3, Scale: 1.5f);
                            d.noGravity = true;
                        }
                        int duration = 60 + 60 * magmas;
                        target.AddBuff(BuffID.Daybreak, duration);
                    }
                }
            }
         
            if (hit.Crit
                && (skull > 0 && proj.CountsAsClass<RangedDamageClass>())
                || moltenskullrose > 0 
                || (proj.CountsAsClass<MagicDamageClass>() && roseskull > 0)
                || (proj.CountsAsClass<MeleeDamageClass>() && magmaSkull > 0)
               )
            {
                int duration = damageDone * Main.rand.Next(3, 6) * (skull + moltenskullrose + roseskull);
                TRAEDebuff.Apply<ObsidianSkulled>(target, duration, 4);
            }
            if (proj.arrow && arrowsburn > 0)
            {
                target.AddBuff(BuffID.OnFire3, Main.rand.Next(120, 360));
                if (hit.Crit)
                {
                    int chance = 1600 / (damageDone / 2 * arrowsburn);
                    if (Main.rand.NextBool(chance))
                    {
                        if (target.HasBuff(BuffID.Daybreak))
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 3f);
                                Dust d = Dust.NewDustPerfect(target.Center, DustID.Torch, speed * 3, Scale: 1.5f);
                                d.noGravity = true;
                                SoundEngine.PlaySound(SoundID.Item45 with { MaxInstances = 0 }, target.position);
                            }
                        }
                        int duration = 180;
                        target.AddBuff(BuffID.Daybreak, duration);
                    }
                }
            }
        }
 
    }
}
