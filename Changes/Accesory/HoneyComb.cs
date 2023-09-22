using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using TRAEProject.NewContent.Projectiles;
using static Terraria.ModLoader.ModContent;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace TRAEProject.Changes.Accesory
{
    public class HoneyComb : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            switch (item.type)
            {
                case ItemID.HoneyBalloon:
                case ItemID.BalloonHorseshoeHoney:
                case ItemID.HoneyComb:
                case ItemID.SweetheartNecklace:
                case ItemID.StingerNecklace:
                    player.GetModPlayer<HoneyCombPlayer>().combs += 1; 

                    player.honeyCombItem = null;
                    break;
                case ItemID.BeeCloak:
                    player.honeyCombItem = null;
                    player.starCloakItem = null; 
                    player.GetModPlayer<HoneyCombPlayer>().combs += 1;
                    player.GetModPlayer<HoneyCombPlayer>().NewbeesOnHit = true;
                    break;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.BeeCloak:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Causes buzzy stars to fall and douses you in honey when damaged\nSlightly increases life regeneration";
                        }
                    }
                    break;
                case ItemID.HoneyBalloon:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Releases bees and douses you in honey when damaged\nSlightly increases life regeneration";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Increases jump height";
                        }
                    }
                    return;
                case ItemID.BalloonHorseshoeHoney:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Releases bees and douses you in honey when damaged\nSlightly increases life regeneration";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases jump height, prevents fall damage and allows fast fall";
                        }
                    }
                    return;
                case ItemID.HoneyComb:
                case ItemID.SweetheartNecklace:
                case ItemID.StingerNecklace:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text += "\nSlightly increases life regeneration";
                        }
                    }
                    break;
            }
        }
    }
    public class HoneyCombPlayer : ModPlayer
    {
       public int combs = 0;
        public bool NewbeesOnHit = false; 
        
        int beedamage = 1;
        public override void ResetEffects()
        {
            combs = 0; beedamage = 1;NewbeesOnHit = false; 
        }
        public override void UpdateDead()
        {
            combs = 0; beedamage = 1;NewbeesOnHit = false; 
        }
        public override void UpdateLifeRegen()
        {
           Player.lifeRegen += 1 * combs;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (combs > 0 && hurtInfo.Damage > 0)
            {
                if (NewbeesOnHit)
                {
                    int duration = hurtInfo.Damage * 6;
                    duration += duration / 2 * (combs - 1);
                    if (duration < 150)
                        duration = 150;
                    Player.AddBuff(BuffID.Honey, duration);
                    if (!Player.HasBuff(BuffID.ShadowDodge))
                    {
                        beedamage = hurtInfo.Damage;
                        if (beedamage > 200)
                            beedamage = 200;
                    }
                    int[] spread = { 1, 2 };
                    int count = (beedamage / 33);
                    if (count < 1)
                        count = 1;
                    count += count / 2 * (combs - 1);
                    TRAEMethods.SpawnProjectilesFromAbove(Player, Player.position, count, 400, 600, spread, 20, ProjectileType<BuzzyStar>(), beedamage, 2f, Player.whoAmI);
                }
                else
                    combOnHit(hurtInfo.Damage, npc);
            }
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {

        

   if (combs > 0 && hurtInfo.Damage > 0)
            {
                if (NewbeesOnHit)
                {
                    int duration = hurtInfo.Damage * 6;
                    duration += duration / 2 * (combs - 1);
                    if (duration < 150)
                        duration = 150;
                    Player.AddBuff(BuffID.Honey, duration);
                    if (!Player.HasBuff(BuffID.ShadowDodge))
                    {
                        beedamage = hurtInfo.Damage;
                        if (beedamage > 200)
                            beedamage = 200;
                    }
                    int[] spread = { 1, 2 };
                    int count = (beedamage / 33);
                    if (count < 1)
                        count = 1;
                    count += count / 2 * (combs - 1);
                    TRAEMethods.SpawnProjectilesFromAbove(Player, Player.position, count, 400, 600, spread, 20, ProjectileType<BuzzyStar>(), beedamage, 2f, Player.whoAmI);
                }
                else
                    combOnHit(hurtInfo.Damage, proj);
            }
        }

        public void combOnHit(int damage, Entity attack)
        {

            int bees = damage / 20;
            if (bees > 16)
                bees = 16;
            bees += bees / 2 * (combs - 1); // 50% more bees per comb
            int duration = damage * 6;
            duration += duration / 2 * (combs - 1);
            if (duration < 150)
                duration = 150;
            Player.AddBuff(BuffID.Honey, duration);
            int BeeDamage = 18;
            for (int i = 0; i < bees; i++)
            {
                int Bee = ProjectileID.Bee;
                if (Main.rand.NextBool(5))
                {
                    Bee = ProjectileID.GiantBee;
                }
                float speedX = Main.rand.Next(-35, 36) * 0.02f;
                float speedY = Main.rand.Next(-35, 36) * 0.02f;
                Projectile.NewProjectile(Player.GetSource_OnHurt(attack), Player.Center.X, Player.Center.Y, speedX, speedY, Bee, BeeDamage, 1, Main.myPlayer);
            }        
        }
    }
}
