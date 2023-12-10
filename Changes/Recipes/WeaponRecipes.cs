using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Materials;

namespace TRAEProject.Changes.Recipes
{
    public static class WeaponRecipes
    {
        public static void Load()
        {
            Recipe StardustPortal = Recipe.Create(ItemID.MoonlordTurretStaff).AddIngredient(3459, 18).AddTile(TileID.LunarCraftingStation);
            StardustPortal.Register();
            Recipe DarkLance = Recipe.Create(ItemID.DarkLance);
            DarkLance.AddIngredient(ItemID.DemoniteBar, 10);
            DarkLance.AddIngredient(ItemID.ShadowScale, 5);
            DarkLance.AddTile(TileID.Anvils);
            DarkLance.Register();
            Recipe WaspGun = Recipe.Create(ItemID.WaspGun);
            WaspGun.AddIngredient(ItemID.BeeGun, 1);
            WaspGun.AddIngredient(ItemID.SoulofMight, 15);
            WaspGun.AddTile(TileID.MythrilAnvil);
            WaspGun.Register(); 
            Recipe MagicDagger = Recipe.Create(ItemID.MagicDagger);
            MagicDagger.AddIngredient(ItemID.ThrowingKnife, 100);
            MagicDagger.AddIngredient(ItemID.DemoniteBar, 10);
            MagicDagger.AddTile(TileID.Anvils);
            MagicDagger.Register(); 
            Recipe MagicDagger2 = Recipe.Create(ItemID.MagicDagger);
            MagicDagger2.AddIngredient(ItemID.ThrowingKnife, 100);
            MagicDagger2.AddIngredient(ItemID.CrimtaneBar, 10);
            MagicDagger2.AddTile(TileID.Anvils);
            MagicDagger2.Register(); 
            Recipe MagicMissile = Recipe.Create(ItemID.MagicMissile);
            MagicMissile.AddRecipeGroup("GoldBar", 10);
            MagicMissile.AddIngredient(ItemID.FallenStar, 5);
            MagicMissile.AddIngredient(ItemID.ShadowScale, 20);
            MagicMissile.AddTile(TileID.Anvils);
            MagicMissile.Register();
            Recipe MagicMissile2 = Recipe.Create(ItemID.MagicMissile);
            MagicMissile2.AddRecipeGroup("GoldBar", 10);
            MagicMissile2.AddIngredient(ItemID.FallenStar, 5);
            MagicMissile2.AddIngredient(ItemID.TissueSample, 20);
            MagicMissile2.AddTile(TileID.Anvils);
            MagicMissile2.Register();
            Recipe IceSickle = Recipe.Create(ItemID.IceSickle);
            IceSickle.AddIngredient(ItemID.FrostCore, 1);
            IceSickle.AddIngredient(ItemID.SoulofNight, 15);
            IceSickle.AddIngredient(ItemID.Sickle, 1);
            IceSickle.AddTile(TileID.MythrilAnvil);
            IceSickle.Register();
            Recipe PulseBow = Recipe.Create(ItemID.PulseBow);
            PulseBow.AddIngredient(ItemID.ShroomiteBar, 20);
            PulseBow.AddTile(TileID.Autohammer);
            PulseBow.Register(); 
            Recipe Rocket = Recipe.Create(ItemID.RocketI, 50);
            Rocket.AddIngredient(ItemID.IronBar, 1);
            Rocket.AddIngredient(ItemID.ExplosivePowder, 2);
            Rocket.AddTile(TileID.Anvils);
            Rocket.Register();
			Recipe Rocket1 = Recipe.Create(ItemID.RocketI, 50);
            Rocket1.AddIngredient(ItemID.LeadBar, 1);
            Rocket1.AddIngredient(ItemID.ExplosivePowder, 2);
            Rocket1.AddTile(TileID.Anvils);
            Rocket1.Register();
            Recipe DestructiveRocket = Recipe.Create(ItemID.RocketII, 100);
            DestructiveRocket.AddIngredient(ItemID.RocketI, 100);
            DestructiveRocket.AddIngredient(ItemID.ExplosivePowder, 1);
            DestructiveRocket.AddTile(TileID.Anvils);
            DestructiveRocket.Register();
            Recipe SuperRocket = Recipe.Create(ItemID.RocketIII, 100);
            SuperRocket.AddIngredient(ItemID.RocketI, 100);
            SuperRocket.AddIngredient(ItemID.CobaltBar, 1);
            SuperRocket.AddTile(TileID.Anvils);
            SuperRocket.Register(); 
			Recipe SuperRocket2 = Recipe.Create(ItemID.RocketIII, 100);
            SuperRocket2.AddIngredient(ItemID.RocketI, 100);
            SuperRocket2.AddIngredient(ItemID.PalladiumBar, 1);
            SuperRocket2.AddTile(TileID.Anvils);
            SuperRocket2.Register();
            Recipe DirectRocket = Recipe.Create(ItemID.RocketIV, 100);
            DirectRocket.AddIngredient(ItemID.RocketI, 100);
            DirectRocket.AddIngredient(ItemID.Cog, 1);
            DirectRocket.AddTile(TileID.Anvils);
            DirectRocket.Register(); 
            Recipe ClusterRocket = Recipe.Create(ItemID.ClusterRocketI, 100);
            ClusterRocket.AddIngredient(ItemID.RocketI, 100);
            ClusterRocket.AddIngredient(ItemID.ShroomiteBar, 1);
            ClusterRocket.AddTile(TileID.Autohammer);
            ClusterRocket.Register(); 
            Recipe HeavyRocket = Recipe.Create(ItemID.ClusterRocketII, 100);
            HeavyRocket.AddIngredient(ItemID.RocketI, 100);
            HeavyRocket.AddIngredient(ItemID.Nanites, 1);
            HeavyRocket.AddTile(TileID.Autohammer);
            HeavyRocket.Register(); 
            Recipe StarWrath = Recipe.Create(ItemID.StarWrath, 1);

            StarWrath.AddIngredient(ItemID.FragmentSolar, 18);
            StarWrath.AddTile(TileID.LunarCraftingStation);
            StarWrath.Register();

            Recipe TentacleSpike = Recipe.Create(ItemID.WormTooth, 12);
            TentacleSpike.AddIngredient(ItemID.Leather, 2);
            TentacleSpike.AddTile(TileID.Anvils);
            TentacleSpike.Register();

            Recipe Yelets = Recipe.Create(ItemID.Yelets, 1);
            Yelets.AddIngredient(ItemID.ChlorophyteBar, 12);
            Yelets.AddTile(TileID.MythrilAnvil);
            Yelets.Register();

            Recipe HelFire = Recipe.Create(ItemID.HelFire, 1);
            HelFire.AddIngredient(ModContent.ItemType<DriedRose>(), 3);
            HelFire.AddIngredient(ModContent.ItemType<MagicalAsh>(), 3);
            HelFire.AddIngredient(ModContent.ItemType<ObsidianScale>(), 3);
            HelFire.AddIngredient(ModContent.ItemType<SalamanderTail>(), 3);
            HelFire.AddTile(TileID.MythrilAnvil);
            HelFire.Register();
        }
        public static void Modify(Recipe recipe)
        {
            Item ingredientToRemove;
            if (recipe.HasResult(ItemID.SilverBullet))
            {
                recipe.ReplaceResult(ItemID.SilverBullet, 100);
                recipe.TryGetIngredient(ItemID.MusketBall, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.MusketBall, 100);
            }
            if (recipe.HasResult(ItemID.TungstenBullet))
            {
                recipe.ReplaceResult(ItemID.TungstenBullet, 100);
                recipe.TryGetIngredient(ItemID.MusketBall, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.MusketBall, 100);
            }

            if (recipe.HasResult(ItemID.MeteorShot))
            {
                recipe.ReplaceResult(ItemID.MeteorShot, 100);
                recipe.TryGetIngredient(ItemID.MusketBall, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.MusketBall, 100);
            }
            if (recipe.HasResult(ItemID.ExplodingBullet))
            {
                recipe.TryGetIngredient(ItemID.EmptyBullet, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.MusketBall, 50);
            }
            if (recipe.HasResult(ItemID.MoonlordBullet))
            {
                recipe.ReplaceResult(ItemID.MoonlordBullet, 500);
            }
            if (recipe.HasResult(ItemID.MoonlordArrow))
            {
                recipe.ReplaceResult(ItemID.MoonlordArrow, 500);
            }
            if (recipe.HasResult(ItemID.VenomArrow))
            {
                recipe.ReplaceResult(ItemID.VenomArrow, 100);
                recipe.TryGetIngredient(ItemID.WoodenArrow, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.WoodenArrow, 100);
            }
            if (recipe.HasResult(ItemID.UnholyArrow))
            {
                recipe.TryGetIngredient(ItemID.WoodenArrow, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.Vertebrae, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.WormTooth, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.WoodenArrow, 33);
                recipe.AddIngredient(ItemID.WormTooth, 1); 
                recipe.AddIngredient(ItemID.VilePowder, 1);
                recipe.ReplaceResult(ItemID.UnholyArrow, 33);
            }
            if (recipe.HasResult(ItemID.JestersArrow))
            {
                recipe.TryGetIngredient(ItemID.WoodenArrow, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.FallenStar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.WoodenArrow, 25);
                recipe.AddIngredient(ItemID.FallenStar, 1);
                recipe.ReplaceResult(ItemID.JestersArrow, 25);
            }
            if (recipe.HasResult(ItemID.HellfireArrow))
            {
                recipe.TryGetIngredient(ItemID.HellstoneBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.ExplosivePowder, 1);
            }
            if (recipe.HasResult(ItemID.MiniNukeII))
            {
                recipe.TryGetIngredient(ItemID.RocketIV, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.RocketII, 100);
            }
            if (recipe.HasResult(ItemID.TrueExcalibur))
            {
                recipe.TryGetIngredient(ItemID.ChlorophyteBar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.ChlorophyteSaber, 1);
                recipe.AddIngredient(ItemID.SoulofMight, 20);
                recipe.AddIngredient(ItemID.SoulofLight, 20);
            }
            if (recipe.HasResult(ItemID.TrueNightsEdge))
            {
                recipe.TryGetIngredient(ItemID.SoulofFright, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.TryGetIngredient(ItemID.SoulofMight, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.ChlorophyteClaymore, 1);
                recipe.AddIngredient(ItemID.SoulofNight, 20);
            }
            if (recipe.HasResult(ItemID.Flamethrower))
            {
                recipe.TryGetIngredient(ItemID.SoulofFright, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.SlimeGun, 1);
                recipe.RemoveTile(TileID.MythrilAnvil);
                recipe.AddTile(TileID.Anvils);
            }
            if (recipe.HasResult(ItemID.StardustDragonStaff))
            {
                recipe.DisableRecipe();
            }

            if (recipe.HasResult(ItemID.SpiritFlame))
            {
                recipe.DisableRecipe();
            }
            if (recipe.HasResult(ItemID.BoneJavelin))
            {
                recipe.ReplaceResult(ItemID.BoneJavelin, 1);
                recipe.TryGetIngredient(3380, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(3380, 10);
            }
            if (recipe.HasResult(ItemID.BoneJavelin))
            {
                recipe.ReplaceResult(ItemID.BoneJavelin, 1);
                recipe.TryGetIngredient(3380, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(3380, 10);
            }
            if (recipe.HasResult(ItemID.EnchantedBoomerang))
            {
                recipe.TryGetIngredient(ItemID.FallenStar, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.FallenStar, 5);
                recipe.AddIngredient(ItemID.Ruby, 1);
            }
        }
    }
}
