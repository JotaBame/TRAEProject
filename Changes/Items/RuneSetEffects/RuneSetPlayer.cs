using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Items.RuneSetEffects.SpecificWeapons;
using static AssGen.Assets;

namespace TRAEProject.Changes.Items.RuneSetEffects
{
    public class RuneSetPlayer : ModPlayer
    {
        public int runeEffectUseCounter;
        public bool runeEffects;
        public const int MaxRuneEffects = 7;
        public static int[] RuneAffectedItems => [ItemID.CrystalSerpent, ItemID.CrystalStorm, ItemID.CrystalVileShard, ItemID.LaserRifle, ItemID.MeteorStaff, ItemID.FrostStaff, ItemID.SkyFracture];

        public static int IncreaseRuneEffectCounter(Player player)
        {
            RuneSetPlayer runePlr = player.GetModPlayer<RuneSetPlayer>();
            runePlr.runeEffectUseCounter++;
            runePlr.runeEffectUseCounter %= MaxRuneEffects;
            return runePlr.runeEffectUseCounter;
        }
        public override float UseTimeMultiplier(Item item)
        {
            if (!RuneAffectedItems.Contains(item.type))
            {
                return 1f;
            }
            if (RuneSetHelper.DoubleShootRate(Player, item))
            {
                return .5f;
            }
            return 1f;
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!runeEffects)
            {
                return true;
            }
            float ai2 = 0;

            switch (item.type)
            {
                case ItemID.CrystalSerpent:

                    RuneSetHelper.SetUpAI2(Player, item, ref ai2, out bool doubleFire, out float secondAi2, out float damageMult, out float secondProjVelMult, out int extraFlatDamage, out float globalvelocityMult);
                    damage = (int)(damage * damageMult);
                    damage += extraFlatDamage;
                    velocity *= globalvelocityMult;
                    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, ai2: ai2);
                    if (doubleFire)
                    {
                        Projectile.NewProjectile(source, position, velocity * secondProjVelMult, type, damage, knockback, ai2: secondAi2);
                    }
                    return false;
                case ItemID.CrystalStorm:
                    ai2 = 0;
                    RuneSetHelper.SetUpAI2(Player, item, ref ai2, out doubleFire, out secondAi2, out damageMult, out secondProjVelMult, out extraFlatDamage, out globalvelocityMult);
                    damage = (int)(damage * damageMult);
                    damage += extraFlatDamage;
                    velocity *= globalvelocityMult;
                    float velX = velocity.X;
                    float velY = velocity.Y;
                    velX += Main.rand.Next(-40, 41) * 0.04f;
                    velY += Main.rand.Next(-40, 41) * 0.04f;
                    Projectile.NewProjectile(source, position.X, position.Y, velX, velY, type, damage, knockback, Main.myPlayer, ai2: ai2);
                    if (doubleFire)
                    {
                        velX = velocity.X;
                        velY = velocity.Y;
                        velX += Main.rand.Next(-40, 41) * 0.04f;
                        velY += Main.rand.Next(-40, 41) * 0.04f;
                        velX *= secondProjVelMult;
                        velX *= secondProjVelMult;
                        Projectile.NewProjectile(source, position.X, position.Y, velX, velY, type, damage, knockback, Main.myPlayer, ai2: secondAi2);
                    }
                    return false;
                case ItemID.CrystalVileShard:
                    RuneSetCrystalVileShard.Shoot(item, Player, source, position, velocity, type, damage, knockback);
                    return false;
                case ItemID.LaserRifle:
                    RuneSetHelper.SetUpAI2(Player, item, ref ai2, out doubleFire, out secondAi2, out damageMult, out secondProjVelMult, out extraFlatDamage, out globalvelocityMult);
                    damage = (int)(damage * damageMult);
                    damage += extraFlatDamage;
                    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, 0, 0, ai2);
                    if (doubleFire)
                    {
                        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, 0, 0, secondAi2);
                    }
                    return false;
                case ItemID.MeteorStaff:
                    MeteorStaffShooting(item, source, position, velocity, type, damage, knockback);
                    return false;

            }

            return true;
        }

        private void MeteorStaffShooting(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai2 = 0;
            int meteorCount = 1;
            RuneSetHelper.SetUpAI2(Player, item, ref ai2, out bool doubleFire, out float secondAi2, out float damageMult, out float secondProjVelMult, out int extraFlatDamage, out float globalvelocityMult);
                int direction = Player.direction;
            for (int i = 0; i < meteorCount; i++)
            {

                Vector2 pointPosition = new Vector2(position.X + Player.width * 0.5f + (Main.rand.Next(201) * -direction) + ((float)Main.mouseX + Main.screenPosition.X - position.X), Player.MountedCenter.Y - 600f);
                pointPosition.X = (pointPosition.X + Player.Center.X) / 2f + Main.rand.Next(-200, 201);
                pointPosition.Y -= 100 * i;
                float velX = (float)Main.mouseX + Main.screenPosition.X - pointPosition.X + (float)Main.rand.Next(-40, 41) * 0.03f;
                float velY = (float)Main.mouseY + Main.screenPosition.Y - pointPosition.Y;
                if (Player.gravDir == -1f)
                {
                    velY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - pointPosition.Y;
                }
                if (velY < 0f)
                {
                    velY *= -1f;
                }
                if (velY < 20f)
                {
                    velY = 20f;
                }
                float normalizingFactor = MathF.Sqrt(velX * velX + velY * velY);
                normalizingFactor = velocity.Length() / normalizingFactor;
                velX *= normalizingFactor;
                velY *= normalizingFactor;
                float finalVelX = velX;
                float finalVelY = velY + Main.rand.Next(-40, 41) * 0.02f;
                float ai1 = 0.5f + (float)Main.rand.NextDouble() * 0.3f;
                Projectile.NewProjectile(source, pointPosition.X, pointPosition.Y, finalVelX * 0.75f, finalVelY * 0.75f, type + Main.rand.Next(3), damage, knockback, Main.myPlayer, 0f, ai1, ai2);
                if (doubleFire)
                {
                    Projectile.NewProjectile(source, pointPosition.X, pointPosition.Y, finalVelX * 0.75f, finalVelY * 0.75f, type + Main.rand.Next(3), damage, knockback, Main.myPlayer, 0f, ai1, secondAi2);
                }
            }
        }
    }
}
