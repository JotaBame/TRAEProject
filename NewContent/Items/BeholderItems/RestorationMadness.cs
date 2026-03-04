using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Projectiles.KinnaraFeather;

namespace TRAEProject.NewContent.Items.BeholderItems
{
   public class GreaterRestorationPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

            // DisplayName.SetDefault("Greater Restoration Potion");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 28;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.DefaultToHealingPotion(20, 28, 15);
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(silver: 20);
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(BuffID.PotionSickness))
            {
                return false;
            }
            return true;
        }
        public override void OnConsumeItem(Player player)
        {
            player.ClearBuff(BuffID.PotionSickness);

            int potionSickness = 3000;
            if (player.pStone == true)
            {
                potionSickness = 2250;
            }
            player.AddBuff(BuffID.PotionSickness, potionSickness);

            player.AddBuff(BuffType<Restoring2>(), 1500);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "HealLife")
                {
                    line.Text = "Restores 150 HP over time\nReduced Potion Cooldown";
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.GelBalloon, 2)
                .AddIngredient(ItemID.BottledWater, 1)
                .AddIngredient(ItemID.CrystalShard, 1)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }

    public class Restoring1 : ModBuff
    {
        public override void SetStaticDefaults()
        {
             Main.buffNoSave[Type] = true;
            // DisplayName.SetDefault("Restoring");
            // Description.SetDefault("Restoring health");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<RestorationEffect>().Heal(100, buffIndex);
        }
    }
    public class Restoring2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
             Main.buffNoSave[Type] = true;
            // DisplayName.SetDefault("Restoring");
            // Description.SetDefault("Restoring health");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<RestorationEffect>().Heal(150, buffIndex);
        }
    }              
 
    public class RestorationEffect: ModPlayer
    {
         int maxDuration = 0;
        int healingInterval = 30;
        float healthLeftover = 0;
        float percentageHealedForDebugging = 0f;
        int healthHealedSoFar = 0;
        public override void ResetEffects()
        {
            if (!Player.HasBuff<Restoring1>() && !Player.HasBuff<Restoring2>())
            {
                healthHealedSoFar = 0;
                percentageHealedForDebugging = 0;
            }
        }
        public void Heal(int healthToHeal, int buffIndex)
        {
             int duration = Player.buffTime[buffIndex];

         
            if (maxDuration == 0)
            {
             
                maxDuration = duration + 1;
            }
            float percentToHeal = 1;
            int healCount = (maxDuration - duration) / healingInterval;
            int maxHeals = maxDuration / healingInterval;
            if (duration % healingInterval == 0)
            {
                int firstPart = maxHeals / 4 - 1;
                int secondPart = maxHeals - firstPart;

                if (healCount - 1 <= firstPart)
                {
                    percentToHeal = (.9f - 0.6f * healCount / firstPart) / firstPart;
                }
                else if (healCount - 1 > firstPart)
                {

                    percentToHeal = (0.4f - 0.2f * (healCount - firstPart) / secondPart) / secondPart;
                }
            

                //Vector2 position4 = Vector2.Zero;
                //position4.X = Player.Center.X + Main.rand.Next(-10, 11) - 6;
                //position4.Y = Player.Center.Y + Main.rand.Next(-20, 21) - 6;
                //Gore.NewGore(Player.GetSource_FromThis(), Player.Center, new(1, 1), Mod.Find<ModGore>("RestorationHeart").Type, Main.rand.Next(80, 100) * 100f);

                float heal = healthToHeal * percentToHeal;

                healthLeftover += heal - (int)(heal);
                if (healthLeftover > 1)
                {
                    heal += (int)healthLeftover;
                    healthLeftover -= (int)healthLeftover;
                 }
                if (healCount == 1)
                    heal += 2; // important else the final value is off by 1
                if (healCount == 2 && healthToHeal == 150)
                    heal += 0.5f; // yay more bandaids
                if (heal >= 1)
                {

                 
                    Player.Heal((int)(heal));
                    healthHealedSoFar += (int)heal;

                }
                percentageHealedForDebugging += percentToHeal;

                //Main.NewText(healCount + " " +  duration + " " + percentageHealedForDebugging + " " + healthHealedSoFar);
 
                //Main.NewText("n: " + healCount + " p: " + percentToHeal + " heal: " + heal+ " leftover: " + healthLeftover);

            }



        }
 
    }
    public class RestorationPotRework : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.RestorationPotion)
            {

                item.width = 20;
                item.height = 28;
                item.consumable = true;
                item.maxStack = 9999;
                item.useTime = item.useAnimation = 17;
                item.healLife = 10;
                item.noMelee = true;
                item.noUseGraphic = true;
                item.useStyle = ItemUseStyleID.DrinkLiquid;
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (player.HasBuff(BuffID.PotionSickness) && item.type == ItemID.RestorationPotion)
            {
                return false;
            }
            return true;
        }
        public override void OnConsumeItem(Item item, Player player)
        {
            if (item.type == ItemID.RestorationPotion)
            {
                int potionSickness = 3000;
                if (player.pStone == true)
                {
                    potionSickness = 2250;
                }
                player.AddBuff(BuffID.PotionSickness, potionSickness);

                player.AddBuff(BuffType<Restoring1>(), 1500);
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.RestorationPotion:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "HealLife")
                        {
                            line.Text = "Heals 100 HP over time";
                        }
                    }
                    break;

            }
        }
    }       
    public class RestorationHeart : ModGore
    {
        public override string Texture => "TRAEProject/NewContent/NPCs/Gores/RestorationHeart";

        public override void OnSpawn(Gore gore, IEntitySource source)
        {
            
                UpdateType = 331;
        }
    }
}