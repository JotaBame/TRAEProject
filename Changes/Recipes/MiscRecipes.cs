using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Recipes
{
    public static class MiscRecipes
    {
        public static void Load(Mod mod)
        {
            Recipe BloodyTear = mod.CreateRecipe(ItemID.BloodMoonStarter).AddIngredient(ItemID.Lens, 5).AddIngredient(ItemID.VilePowder, 50).AddIngredient(ItemID.Deathweed, 10).AddTile(TileID.DemonAltar);
            BloodyTear.Register();
            Recipe BloodyTear2 = mod.CreateRecipe(ItemID.BloodMoonStarter).AddIngredient(ItemID.Lens, 5).AddIngredient(ItemID.ViciousPowder, 50).AddIngredient(ItemID.Deathweed, 10).AddTile(TileID.DemonAltar);
            BloodyTear2.Register(); 
            Recipe Sashimi = mod.CreateRecipe(ItemID.Sashimi).AddIngredient(ItemID.NeonTetra, 1).AddTile(TileID.CookingPots);
            Sashimi.Register();
        }
        public static void Modify(Recipe recipe)
        {
            Item ingredientToRemove;
            if (recipe.HasResult(ItemID.HeartreachPotion))
            {
                recipe.TryGetIngredient(ItemID.Daybloom, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.CrimsonTigerfish, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Waterleaf, 1);
                recipe.AddIngredient(ItemID.PrincessFish, 1);
            }
            if (recipe.HasResult(ItemID.SeafoodDinner))
            {
                if (recipe.HasIngredient(ItemID.SpecularFish))
                {
                    recipe.TryGetIngredient(ItemID.SpecularFish, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.AddIngredient(ItemID.SpecularFish, 6);
                }
                if (recipe.HasIngredient(ItemID.CrimsonTigerfish))
                {
                    recipe.TryGetIngredient(ItemID.CrimsonTigerfish, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.AddIngredient(ItemID.CrimsonTigerfish, 6);
                }
                if (recipe.HasIngredient(ItemID.PrincessFish))
                {
                    recipe.TryGetIngredient(ItemID.PrincessFish, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.AddIngredient(ItemID.PrincessFish, 6);
                }
                if (recipe.HasIngredient(ItemID.NeonTetra))
                {
                    recipe.TryGetIngredient(ItemID.NeonTetra, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.RemoveRecipe();
                }
  
            }
            if (recipe.HasResult(ItemID.ThornsPotion))
            {
                recipe.TryGetIngredient(ItemID.Stinger, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.WormTooth, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
            }
            if (recipe.HasResult(ItemID.InfernoPotion))
            {
                recipe.TryGetIngredient(ItemID.Obsidifish, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
            }
            if (recipe.HasResult(ItemID.TitanPotion))
            {
                recipe.TryGetIngredient(ItemID.Bone, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.Deathweed, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.CrimsonTigerfish, 1);
            }
            if (recipe.HasResult(ItemID.SuperManaPotion))
            {
                recipe.TryGetIngredient(ItemID.FallenStar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.SoulofSight, 1);
            }
            if (recipe.HasResult(ItemID.PickaxeAxe) || recipe.HasResult(ItemID.Drax))
            {
                recipe.TryGetIngredient(ItemID.SoulofSight, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.SoulofFright, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.SoulofMight, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.SoulofFright, 20);

            }
        }
    }
}
