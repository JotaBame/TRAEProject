
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories.ShadowflameCharm;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.Items.Accesories;
using TRAEProject.NewContent.Items.Accesories.MobilityJumps;

namespace TRAEProject.Changes.Recipes
{
    public static class AccesoryRecipes
    {

        public static void Load()
        { 
            Recipe MoltenSkullRoseMagmaSkull = Recipe.Create(ItemID.MoltenSkullRose).AddIngredient(ItemID.MagmaStone, 1).AddIngredient(ItemID.ObsidianSkull, 1).AddIngredient(ItemID.ObsidianRose).AddTile(TileID.TinkerersWorkbench);
            MoltenSkullRoseMagmaSkull.Register();
            Recipe YoyoBagAlt = Recipe.Create(ItemID.YoyoBag).AddIngredient(ItemID.YoYoGlove, 1).AddIngredient(ItemType<CounterweightString>()).AddTile(TileID.TinkerersWorkbench);
            YoyoBagAlt.Register();
            Recipe HermesBoots = Recipe.Create(ItemID.HermesBoots).AddIngredient(ItemID.Aglet, 1).AddIngredient(ItemID.Silk, 20).AddTile(TileID.Loom);
            HermesBoots.Register(); 
            Recipe Mantra = Recipe.Create(ItemID.CountercurseMantra).AddIngredient(ItemID.Nazar, 1).AddIngredient(ItemID.AnkhCharm, 1).AddTile(TileID.TinkerersWorkbench);
            Mantra.Register();
            Recipe Waders = Recipe.Create(ItemID.LavaWaders).AddIngredient(ItemID.WaterWalkingBoots, 1).AddIngredient(ItemID.LavaCharm, 1).AddTile(TileID.TinkerersWorkbench);
            Waders.Register();
            Recipe BoBrecipe = Recipe.Create(ItemID.BundleofBalloons);
            BoBrecipe.AddRecipeGroup("CloudBalloon");
            BoBrecipe.AddRecipeGroup("BlizzardJump");
            BoBrecipe.AddRecipeGroup("SandstormJump");
            BoBrecipe.AddTile(TileID.TinkerersWorkbench);
            BoBrecipe.Register();
            Recipe BoBrecipe1 = Recipe.Create(ItemID.BundleofBalloons);
            BoBrecipe1.AddRecipeGroup("BlizzardBalloon");
            BoBrecipe1.AddRecipeGroup("CloudJump");
            BoBrecipe1.AddRecipeGroup("SandstormJump");
            BoBrecipe1.AddTile(TileID.TinkerersWorkbench);
            BoBrecipe1.Register();
            Recipe BoBrecipe2 = Recipe.Create(ItemID.BundleofBalloons);
            BoBrecipe2.AddRecipeGroup("SandstormBalloon");
            BoBrecipe2.AddRecipeGroup("CloudJump");
            BoBrecipe2.AddRecipeGroup("BlizzardJump");
            BoBrecipe2.AddTile(TileID.TinkerersWorkbench);
            BoBrecipe2.Register();
            Recipe SolarWings = Recipe.Create(ItemID.WingsSolar);
            SolarWings.AddIngredient(ItemID.FragmentSolar, 12);
            SolarWings.AddIngredient(ItemType<LuminiteFeather>(), 1);
            SolarWings.AddTile(TileID.LunarCraftingStation);
            SolarWings.Register();
            Recipe NebulaWings = Recipe.Create(ItemID.WingsNebula);
            NebulaWings.AddIngredient(ItemID.FragmentNebula, 12);
            NebulaWings.AddIngredient(ItemType<LuminiteFeather>(), 1);
            NebulaWings.AddTile(TileID.LunarCraftingStation);
            NebulaWings.Register();

            Recipe VortexWings = Recipe.Create(ItemID.WingsVortex);
            VortexWings.AddIngredient(ItemID.FragmentVortex, 12);
            VortexWings.AddIngredient(ItemType<LuminiteFeather>(), 1);
            VortexWings.AddTile(TileID.LunarCraftingStation);
            VortexWings.Register();
            Recipe StardustWings = Recipe.Create(ItemID.WingsStardust);
            StardustWings.AddIngredient(ItemID.FragmentStardust, 12);
            StardustWings.AddIngredient(ItemType<LuminiteFeather>(), 1);
            StardustWings.AddTile(TileID.LunarCraftingStation);
            StardustWings.Register();
        }
        public static void Modify(Recipe recipe)
        {
            Item ingredientToRemove;
            if (recipe.HasResult(ItemID.HorseshoeBundle) && !recipe.HasIngredient(ItemID.BundleofBalloons))
            {
                recipe.DisableRecipe();
            }
            if (recipe.HasResult(ItemID.MechanicalGlove))
            {
                recipe.TryGetIngredient(ItemID.PowerGlove, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.FeralClaws, 1);
            }
            if (recipe.HasResult(ItemID.PapyrusScarab))
            {
                recipe.TryGetIngredient(ItemID.HerculesBeetle, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemType<SkeletonBeetle>(), 1);
            }
            if (recipe.HasResult(ItemID.FireGauntlet))
            {
 
                recipe.AddIngredient(ItemType<SalamanderTail>(), 1);

            }

            if (recipe.HasResult(ItemID.FrostsparkBoots))
            {
                if (GetInstance<TRAEConfig>().TerrasparkLineRework)
                {
                    recipe.TryGetIngredient(ItemID.LightningBoots, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.AddIngredient(ItemID.FlurryBoots, 1);
                }
            }
            if (recipe.HasResult(ItemID.LightningBoots))
            {
                if (GetInstance<TRAEConfig>().TerrasparkLineRework)
                {
                    recipe.TryGetIngredient(ItemID.Aglet, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                }
            }
            if (recipe.HasResult(ItemID.LavaWaders) && recipe.HasIngredient(ItemID.ObsidianRose) || recipe.HasIngredient(ItemID.MoltenCharm) || recipe.HasIngredient(ItemID.MoltenSkullRose) || recipe.HasIngredient(ItemID.ObsidianWaterWalkingBoots))

            {
                recipe.TryGetIngredient(ItemID.ObsidianWaterWalkingBoots, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.ObsidianRose, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.MoltenCharm, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.MoltenSkullRose, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.WaterWalkingBoots, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.LavaCharm, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);

                recipe.DisableRecipe();

            }
            if (GetInstance<TRAEConfig>().MobilityRework)
            {
                if (recipe.HasResult(ItemID.TerrasparkBoots))
                {
                    if (GetInstance<TRAEConfig>().TerrasparkLineRework)
                    {
                        recipe.TryGetIngredient(ItemID.FrostsparkBoots, out ingredientToRemove);
                        recipe.RemoveIngredient(ingredientToRemove);
                        recipe.TryGetIngredient(ItemID.LavaWaders, out ingredientToRemove);
                        recipe.RemoveIngredient(ingredientToRemove);
                        recipe.AddIngredient(ItemID.LightningBoots, 1);
                        recipe.AddIngredient(ItemID.Tabi, 1);
                        recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
                    }


                }
                if (recipe.HasResult(ItemID.ObsidianWaterWalkingBoots))
                {
                    recipe.TryGetIngredient(ItemID.WaterWalkingBoots, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.TryGetIngredient(ItemID.ObsidianSkull, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.AddIngredient(ItemID.RocketBoots, 1);
                    recipe.AddIngredient(ItemID.LuckyHorseshoe, 1);
                }

                if (recipe.HasResult(ItemID.FrogGear))
                {
                    recipe.TryGetIngredient(ItemID.FrogWebbing, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.TryGetIngredient(ItemID.FrogFlipper, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.TryGetIngredient(ItemID.Flipper, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.TryGetIngredient(ItemID.TigerClimbingGear, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.AddIngredient(ItemID.FrogLeg, 1);
                    recipe.AddIngredient(ItemID.Tabi, 1);
                }
                if (recipe.HasResult(ItemID.ObsidianHorseshoe))
                {
                    recipe.TryGetIngredient(ItemID.ObsidianSkull, out ingredientToRemove);
                    recipe.RemoveIngredient(ingredientToRemove);
                    recipe.AddIngredient(ItemID.GravityGlobe, 1);
                }
            }
        

            if (recipe.HasResult(ItemID.MoonShell))
            {
                recipe.AddIngredient(ItemID.FrozenTurtleShell, 1);
            }
            if (recipe.HasResult(ItemID.CelestialShell))
            {
                recipe.TryGetIngredient(ItemID.MoonShell, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.NeptunesShell, 1);
            }
            if (recipe.HasResult(ItemID.MoltenCharm))
            {
                recipe.TryGetIngredient(ItemID.LavaCharm, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.ObsidianSkull, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.MagmaStone);
                recipe.AddIngredient(ItemType<ShadowflameCharmItem>());
            }
            if (recipe.HasResult(ItemID.MagicCuffs))
            {
                recipe.TryGetIngredient(ItemID.ManaRegenerationBand, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.BandofStarpower, 1);
            }
            if (recipe.HasResult(ItemID.AnkhCharm))
            {
                recipe.DisableRecipe();
            }
            if (recipe.HasResult(ItemID.ThePlan))
            {
                recipe.DisableRecipe();
            }
            if (recipe.HasResult(ItemID.BundleofBalloons) && GetInstance<TRAEConfig>().MobilityRework)
            {
                recipe.AddIngredient(ItemID.SoulofFlight, 20);
            }
            if (recipe.HasResult(ItemType<BundleOfWeirdBalloons>()) && !GetInstance<TRAEConfig>().MobilityRework)
            {
                recipe.TryGetIngredient(ItemID.SoulofFlight, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
             }
            if (recipe.HasResult(ItemID.SniperScope))
            {
                recipe.TryGetIngredient(ItemID.DestroyerEmblem, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.EyeoftheGolem, 1);
            }
            if (recipe.HasResult(ItemID.ReconScope))
            {
                recipe.TryGetIngredient(ItemID.PutridScent, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.SniperScope, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.RifleScope, 1); recipe.AddIngredient(ItemID.SoulofSight, 10);

                recipe.AddIngredient(ItemID.MagicQuiver, 1);
            }
            if (recipe.HasResult(ItemID.AnkhShield))
            {
                recipe.TryGetIngredient(ItemID.ObsidianShield, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.CobaltShield, 1);
            }
            if (recipe.HasResult(ItemID.HeroShield))
            {
                recipe.TryGetIngredient(ItemID.PaladinsShield, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.CobaltShield, 1);
            }
            if (recipe.HasResult(ItemID.ObsidianSkull))
            {
                recipe.AddIngredient(ItemID.Bone, 20);
            }
            if (recipe.HasResult(3999)) // MAGMA SKULL
            {
                recipe.TryGetIngredient(ItemID.LavaCharm, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.MagmaStone, 1);
            }
            if (recipe.HasResult(ItemID.MoltenSkullRose)) 
            {
                recipe.TryGetIngredient(ItemID.LavaCharm, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);

            }
            if (recipe.HasResult(ItemID.MoltenSkullRose) && recipe.HasIngredient(ItemID.ObsidianSkullRose) && !recipe.HasIngredient(3999))
            {
                recipe.AddIngredient(ItemID.MagmaStone);
            }
    
            if (recipe.HasResult(ItemID.FartinaJar))
            {
                recipe.TryGetIngredient(ItemID.CloudinaBottle, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.Bottle);
            }
            if (recipe.HasResult(ItemID.AvengerEmblem))
            {
                recipe.TryGetIngredient(ItemID.SoulofFright, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.SoulofMight, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.SoulofSight, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.SoulofMight, 15);

            }
         
        }
    }
}
