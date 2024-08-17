using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;

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
            Item.DefaultToHealingPotion(20, 28, 3);
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

            int potionSickness = 2700;
            if (player.pStone == true)
            {
                potionSickness = 2025;
            }
            player.AddBuff(BuffID.PotionSickness, potionSickness);

            player.AddBuff(BuffType<Restoring2>(), 1800);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "HealLife")
                {
                    line.Text = "Restores 150 HP over 30 seconds\nReduced Potion Cooldown";
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
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            // DisplayName.SetDefault("Restoring");
            // Description.SetDefault("Restoring health");
        }

    }
    public class Restoring2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            // DisplayName.SetDefault("Restoring");
            // Description.SetDefault("Restoring health");
        }
    }
    public class RestorationEffect: ModPlayer
    {

        int timer = 0;
        public override void PostUpdateBuffs()
        {
            if (Player.HasBuff<Restoring1>() || Player.HasBuff<Restoring2>())
            {
                timer += 1;
                if (timer >= 40)
                {
                    Vector2 position4 = Vector2.Zero;
                    position4.X = Player.Center.X + Main.rand.Next(-10, 11) - 6;
                    position4.Y = Player.Center.Y + Main.rand.Next(-20, 21) - 6;
                    Gore.NewGore(Player.GetSource_None(), position4, new Vector2(Main.rand.Next(-5, 6) * 0.1f, Main.rand.Next(-10, -5) * 0.1f), Mod.Find<ModGore>("RestorationHeart").Type, Main.rand.Next(80, 100) * 0.01f);
                    if (Player.HasBuff<Restoring2>())
                    {
                        Player.HealEffect(3, true);
                        Player.statLife += 3;
                    }
                    if (Player.HasBuff<Restoring1>())
                    {
                        Player.HealEffect(2, true);
                        Player.statLife += 2;
                    }
                    timer = 0;
                }
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
                item.healLife = 2;
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
                player.AddBuff(BuffType<Restoring1>(), 1800);
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
                            line.Text = "Heals 90 HP over 30 seconds";
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