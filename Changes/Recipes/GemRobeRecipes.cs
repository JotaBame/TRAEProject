using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Accesory;
using TRAEProject.Changes.Armor;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Recipes
{
    public static class GemRobesRecipes
    {

 
        public static void Modify(Recipe recipe)
        {
            Item ingredientToRemove;
            if (recipe.HasResult(ItemID.AmethystRobe))
            {
                recipe.TryGetIngredient(ItemID.Amethyst, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Amethyst, 6);
            }
            if (recipe.HasResult(ItemID.TopazRobe))
            {
                recipe.TryGetIngredient(ItemID.Topaz, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Topaz, 6);
            }
            if (recipe.HasResult(ItemID.SapphireRobe))
            {
                recipe.TryGetIngredient(ItemID.Sapphire, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Sapphire, 8);
            }
            if (recipe.HasResult(ItemID.EmeraldRobe))
            {
                recipe.TryGetIngredient(ItemID.Emerald, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Emerald, 8);
            }
            if (recipe.HasResult(ItemID.RubyRobe))
            {
                recipe.TryGetIngredient(ItemID.Ruby, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Ruby, 8);
            }

            if (recipe.HasResult(ItemID.AmethystStaff))
            {
                recipe.TryGetIngredient(ItemID.Amethyst, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Amethyst, 4);
            }
            if (recipe.HasResult(ItemID.TopazStaff))
            {
                recipe.TryGetIngredient(ItemID.Topaz, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Topaz, 4);
            }
            if (recipe.HasResult(ItemID.SapphireStaff))
            {
                recipe.TryGetIngredient(ItemID.Sapphire, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Sapphire, 6);
            }
            if (recipe.HasResult(ItemID.EmeraldStaff))
            {
                recipe.TryGetIngredient(ItemID.Emerald, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Emerald, 6);
            }

        }
    }
    public class GemRobes : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void SetDefaults(Item item)
        {

            switch (item.type)
            {
                case ItemID.AmethystRobe:
                    item.defense = 1;
                    item.rare = 1;
                    return;
                case ItemID.TopazRobe:
                    item.defense = 1;
                    break;
                case ItemID.SapphireRobe:
                    item.defense = 1;
                    return;
                case ItemID.RubyRobe:
                    item.defense = 1;
                    return;
            }
        }
        public override void UpdateEquip(Item item, Player player)
        {
            switch (item.type)
            {
 
                case ItemID.AmethystRobe:
                    player.statManaMax2 += 20;
                     player.manaCost -= 0.05f;
                    break;
                case ItemID.TopazRobe:
                    player.statManaMax2 -= 20;
                    player.manaCost -= 0.04f;
                    break;
                case ItemID.SapphireRobe:
                    player.manaCost -= 0.03f;
                    break;
                case ItemID.EmeraldRobe:
                    player.statManaMax2 -= 20;
                    player.manaCost -= 0.02f;
                    break;
                case ItemID.RubyRobe:
                case ItemID.AmberRobe:
                    player.manaCost -= 0.01f;
                    break;

            }
        }
 
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
 
                case ItemID.AmethystRobe:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases maximum mana by 20";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "10% reduced mana costs";
                        }
                    }
                    return;
                case ItemID.TopazRobe:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases maximum mana by 20";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "11% reduced mana costs";
                        }
                    }
                    return;
                case ItemID.SapphireRobe:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "12% reduced mana costs";
                        }
                    }
                    return;
                case ItemID.EmeraldRobe:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Increases maximum mana by 40";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "13% reduced mana costs";
                        }
                    }
                    return;
                case ItemID.RubyRobe:
                    foreach (TooltipLine line in tooltips)
                    {
 
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "14% reduced mana costs";
                        }
                    }
                    return;
                case ItemID.AmberRobe:
                    foreach (TooltipLine line in tooltips)
                    {
           
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "14% reduced mana costs";
                        }
                    }
                    return;
            }
        }
     }
}
