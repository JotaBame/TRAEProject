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
        public static void Load()
        {
            Recipe BloodyTear = Recipe.Create(ItemID.BloodMoonStarter).AddIngredient(ItemID.Lens, 5).AddIngredient(ItemID.VilePowder, 50).AddIngredient(ItemID.Deathweed, 10).AddTile(TileID.DemonAltar);
            BloodyTear.Register();
            Recipe BloodyTear2 = Recipe.Create(ItemID.BloodMoonStarter).AddIngredient(ItemID.Lens, 5).AddIngredient(ItemID.ViciousPowder, 50).AddIngredient(ItemID.Deathweed, 10).AddTile(TileID.DemonAltar);
            BloodyTear2.Register(); 
            Recipe Sashimi = Recipe.Create(ItemID.Sashimi).AddIngredient(ItemID.NeonTetra, 1).AddTile(TileID.CookingPots);
            Sashimi.Register();
            Recipe Leather = Recipe.Create(ItemID.Leather).AddIngredient(ItemID.Vertebrae, 5).AddTile(TileID.Tables);
            Leather.Register();
     
            Recipe Wire = 
            Recipe.Create(ItemID.Wire, 3).
            AddIngredient(ItemID.CopperBar, 1).
            AddTile(TileID.Anvils);
            Wire.Register();
            Wire = 
            Recipe.Create(ItemID.Wire, 3).
            AddIngredient(ItemID.TinBar, 1).
            AddTile(TileID.Anvils);
            Wire.Register();

            Wire = 
            Recipe.Create(ItemID.Wrench, 1).
            AddIngredient(ItemID.CopperBar, 8).
            AddTile(TileID.Anvils);
            Wire.Register();
            Wire = 
            Recipe.Create(ItemID.Wrench, 1).
            AddIngredient(ItemID.TinBar, 8).
            AddTile(TileID.Anvils);
            Wire.Register();

            Wire = 
            Recipe.Create(ItemID.WireCutter, 1).
            AddIngredient(ItemID.CopperBar, 8).
            AddTile(TileID.Anvils);
            Wire.Register();
            Wire = 
            Recipe.Create(ItemID.WireCutter, 1).
            AddIngredient(ItemID.TinBar, 8).
            AddTile(TileID.Anvils);
            Wire.Register();
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
            if (recipe.HasResult(ItemID.RestorationPotion))
            {
                recipe.ReplaceResult(ItemID.RestorationPotion, 2);
            }
            if (recipe.HasResult(ItemID.WormFood))
            {
                recipe.TryGetIngredient(ItemID.RottenChunk, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.VilePowder, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.RottenChunk, 10);
                recipe.AddIngredient(ItemID.WormTooth, 10);
                recipe.AddIngredient(ItemID.VilePowder, 20);
            }
            if (recipe.HasResult(ItemID.BloodySpine))
            {
                recipe.TryGetIngredient(ItemID.Vertebrae, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.ViciousPowder, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Vertebrae, 10);
                recipe.AddIngredient(ItemID.WormTooth, 10);
                recipe.AddIngredient(ItemID.ViciousPowder, 20);
            }

            if (recipe.HasResult(ItemID.DeerThing))
            {
                recipe.TryGetIngredient(ItemID.FlinxFur, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.FlinxFur, 2);
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
            if (recipe.HasResult(ItemID.TeleportationPotion))
            {
                recipe.TryGetIngredient(ItemID.ChaosFish, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Deathweed, 1);
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
