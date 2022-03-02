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
    public static class WeaponRecipes
    {
        public static void Load(Mod mod)
        {
            Recipe StardustPortal = mod.CreateRecipe(ItemID.MoonlordTurretStaff).AddIngredient(3459, 18).AddTile(TileID.LunarCraftingStation);
            StardustPortal.Register();
            Recipe DarkLance = mod.CreateRecipe(ItemID.DarkLance);
            DarkLance.AddIngredient(ItemID.DemoniteBar, 10);
            DarkLance.AddIngredient(ItemID.ShadowScale, 5);
            DarkLance.AddTile(TileID.Anvils);
            DarkLance.Register();
            Recipe WaspGun = mod.CreateRecipe(ItemID.WaspGun);
            WaspGun.AddIngredient(ItemID.BeeGun, 1);
            WaspGun.AddIngredient(ItemID.SoulofFright, 20);
            WaspGun.AddTile(TileID.MythrilAnvil);
            WaspGun.Register(); 
            Recipe MagicDagger = mod.CreateRecipe(ItemID.MagicDagger);
            MagicDagger.AddIngredient(ItemID.ThrowingKnife, 1);
            MagicDagger.AddIngredient(ItemID.FallenStar, 5);
            MagicDagger.AddIngredient(ItemID.DemoniteBar, 10);
            MagicDagger.AddTile(TileID.Anvils);
            MagicDagger.Register(); 
            Recipe MagicDagger2 = mod.CreateRecipe(ItemID.MagicDagger);
            MagicDagger2.AddIngredient(ItemID.ThrowingKnife, 1);
            MagicDagger2.AddIngredient(ItemID.FallenStar, 5);
            MagicDagger2.AddIngredient(ItemID.CrimtaneBar, 10);
            MagicDagger2.AddTile(TileID.Anvils);
            MagicDagger2.Register();
            Recipe IceSickle = mod.CreateRecipe(ItemID.IceSickle);
            IceSickle.AddIngredient(ItemID.FrostCore, 1);
            IceSickle.AddIngredient(ItemID.SoulofNight, 15);
            IceSickle.AddIngredient(ItemID.Sickle, 1);
            IceSickle.AddTile(TileID.MythrilAnvil);
            IceSickle.Register();
            Recipe PulseBow = mod.CreateRecipe(ItemID.PulseBow);
            PulseBow.AddIngredient(ItemID.ShroomiteBar, 20);
            PulseBow.AddTile(TileID.Autohammer);
            PulseBow.Register(); 
            Recipe Rocket = mod.CreateRecipe(ItemID.RocketI, 50);
            Rocket.AddIngredient(ItemID.IronBar, 1);
            Rocket.AddIngredient(ItemID.ExplosivePowder, 2);
            Rocket.AddTile(TileID.Anvils);
            Rocket.Register();
            Recipe DestructiveRocket = mod.CreateRecipe(ItemID.RocketII, 100);
            DestructiveRocket.AddIngredient(ItemID.RocketI, 100);
            DestructiveRocket.AddIngredient(ItemID.ExplosivePowder, 1);
            DestructiveRocket.AddTile(TileID.Anvils);
            DestructiveRocket.Register();
            Recipe SuperRocket = mod.CreateRecipe(ItemID.RocketIII, 100);
            SuperRocket.AddIngredient(ItemID.RocketI, 100);
            SuperRocket.AddIngredient(ItemID.SoulofMight, 1);
            SuperRocket.AddTile(TileID.Anvils);
            SuperRocket.Register();
            Recipe DirectRocket = mod.CreateRecipe(ItemID.RocketIV, 100);
            DirectRocket.AddIngredient(ItemID.RocketI, 100);
            DirectRocket.AddIngredient(ItemID.Cog, 1);
            DirectRocket.AddTile(TileID.Anvils);
            DirectRocket.Register(); 
            Recipe ClusterRocket = mod.CreateRecipe(ItemID.ClusterRocketI, 100);
            ClusterRocket.AddIngredient(ItemID.RocketI, 100);
            ClusterRocket.AddIngredient(ItemID.ShroomiteBar, 1);
            ClusterRocket.AddTile(TileID.Autohammer);
            ClusterRocket.Register(); 
            Recipe HeavyRocket = mod.CreateRecipe(ItemID.ClusterRocketII, 100);
            HeavyRocket.AddIngredient(ItemID.RocketI, 100);
            HeavyRocket.AddIngredient(ItemID.Nanites, 1);
            HeavyRocket.AddTile(TileID.Autohammer);
            HeavyRocket.Register();

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
                recipe.TryGetIngredient(ItemID.WormTooth, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.WoodenArrow, 50);
                recipe.AddIngredient(ItemID.WormTooth, 1);
                recipe.ReplaceResult(ItemID.UnholyArrow, 50);
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
            if (recipe.HasResult(ItemID.Megashark))
            {
                recipe.TryGetIngredient(ItemID.SoulofMight, out ingredientToRemove);
                recipe.RemoveIngredient(ingredientToRemove);
                recipe.AddIngredient(ItemID.SoulofFright, 20);
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
                recipe.RemoveRecipe();
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
