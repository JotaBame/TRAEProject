using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Recipes
{
    public static class ArmorRecipes
    {

        public static void Load(Mod mod)
        {
            Recipe AncientShadowHead = mod.CreateRecipe(ItemID.AncientShadowHelmet);
            AncientShadowHead.AddIngredient(ItemID.DemoniteBar, 15);
            AncientShadowHead.AddIngredient(ItemID.ShadowScale, 105);
            AncientShadowHead.AddTile(TileID.DemonAltar);
            AncientShadowHead.Register();
            Recipe AncientShadowLegs = mod.CreateRecipe(ItemID.AncientShadowGreaves);
            AncientShadowLegs.AddIngredient(ItemID.DemoniteBar, 20);
            AncientShadowLegs.AddIngredient(ItemID.ShadowScale, 15);
            AncientShadowLegs.AddTile(TileID.DemonAltar);
            AncientShadowLegs.Register();
            Recipe AncientShadowChest = mod.CreateRecipe(ItemID.AncientShadowHelmet);
            AncientShadowLegs.AddIngredient(ItemID.DemoniteBar, 25);
            AncientShadowLegs.AddIngredient(ItemID.ShadowScale, 20);
            AncientShadowChest.AddTile(TileID.DemonAltar);
            AncientShadowChest.Register();
        }
        public static void Modify(Recipe recipe)
        {
            Item ingredientToRemove;
            if (recipe.HasResult(ItemID.AncientArmorHat))
            {
                recipe.TryGetIngredient(ItemID.AncientCloth, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.AncientCloth, 2);
                recipe.AddRecipeGroup("Adamantite", 10);
                recipe.RemoveTile(TileID.Loom);
                recipe.AddTile(TileID.MythrilAnvil);
            }
            if (recipe.HasResult(ItemID.AncientArmorShirt))
            {
                recipe.TryGetIngredient(ItemID.AncientCloth, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.AncientCloth, 2);
                recipe.AddRecipeGroup("Adamantite", 20);
                recipe.RemoveTile(TileID.Loom);
                recipe.AddTile(TileID.MythrilAnvil);
            }
            if (recipe.HasResult(ItemID.AncientArmorPants))
            {
                recipe.TryGetIngredient(ItemID.AncientCloth, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.AncientCloth, 2);
                recipe.AddRecipeGroup("Adamantite", 15);
                recipe.RemoveTile(TileID.Loom);
                recipe.AddTile(TileID.MythrilAnvil);
            }
            if (recipe.HasResult(ItemID.CopperHelmet))
            {
                recipe.TryGetIngredient(ItemID.CopperBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.CopperBar, 10);
            }
            if (recipe.HasResult(ItemID.CopperChainmail))
            {
                recipe.TryGetIngredient(ItemID.CopperBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.CopperBar, 18);
            }
            if (recipe.HasResult(ItemID.CopperGreaves))
            {
                recipe.TryGetIngredient(ItemID.CopperBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.CopperBar, 12);
            }
            if (recipe.HasResult(ItemID.TinHelmet))
            {
                recipe.TryGetIngredient(ItemID.TinBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.TinBar, 10);
            }
            if (recipe.HasResult(ItemID.TinChainmail))
            {
                recipe.TryGetIngredient(ItemID.TinBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.TinBar, 18);
            }
            if (recipe.HasResult(ItemID.TinGreaves))
            {
                recipe.TryGetIngredient(ItemID.TinBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.TinBar, 12);
            }
            if (recipe.HasResult(ItemID.IronHelmet))
            {
                recipe.TryGetIngredient(ItemID.IronBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.IronBar, 10);
            }
            if (recipe.HasResult(ItemID.IronChainmail))
            {
                recipe.TryGetIngredient(ItemID.IronBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.IronBar, 18);
            }
            if (recipe.HasResult(ItemID.IronGreaves))
            {
                recipe.TryGetIngredient(ItemID.IronBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.IronBar, 12);
            }
            if (recipe.HasResult(ItemID.LeadHelmet))
            {
                recipe.TryGetIngredient(ItemID.LeadBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.LeadBar, 10);
            }
            if (recipe.HasResult(ItemID.LeadChainmail))
            {
                recipe.TryGetIngredient(ItemID.LeadBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.LeadBar, 18);
            }
            if (recipe.HasResult(ItemID.LeadGreaves))
            {
                recipe.TryGetIngredient(ItemID.LeadBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.LeadBar, 12);
            }
            if (recipe.HasResult(ItemID.SilverHelmet))
            {
                recipe.TryGetIngredient(ItemID.SilverBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.SilverBar, 10);
            }
            if (recipe.HasResult(ItemID.SilverChainmail))
            {
                recipe.TryGetIngredient(ItemID.SilverBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.SilverBar, 18);
            }
            if (recipe.HasResult(ItemID.SilverGreaves))
            {
                recipe.TryGetIngredient(ItemID.SilverBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.SilverBar, 12);
            }
            if (recipe.HasResult(ItemID.TungstenHelmet))
            {
                recipe.TryGetIngredient(ItemID.TungstenBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.TungstenBar, 10);
            }
            if (recipe.HasResult(ItemID.TungstenChainmail))
            {
                recipe.TryGetIngredient(ItemID.TungstenBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.TungstenBar, 18);
            }
            if (recipe.HasResult(ItemID.TungstenGreaves))
            {
                recipe.TryGetIngredient(ItemID.TungstenBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.TungstenBar, 12);
            }
            if (recipe.HasResult(ItemID.GoldHelmet))
            {
                recipe.TryGetIngredient(ItemID.GoldBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.GoldBar, 10);
            }
            if (recipe.HasResult(ItemID.GoldChainmail))
            {
                recipe.TryGetIngredient(ItemID.GoldBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.GoldBar, 18);
            }
            if (recipe.HasResult(ItemID.GoldGreaves))
            {
                recipe.TryGetIngredient(ItemID.GoldBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.GoldBar, 12);
            }
            if (recipe.HasResult(ItemID.PlatinumHelmet))
            {
                recipe.TryGetIngredient(ItemID.PlatinumBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.PlatinumBar, 10);
            }
            if (recipe.HasResult(ItemID.PlatinumChainmail))
            {
                recipe.TryGetIngredient(ItemID.PlatinumBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.PlatinumBar, 18);
            }
            if (recipe.HasResult(ItemID.PlatinumGreaves))
            {
                recipe.TryGetIngredient(ItemID.PlatinumBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.PlatinumBar, 12);
            }
        }
    }
}
