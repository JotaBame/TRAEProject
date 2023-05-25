using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using TRAEProject.Changes.Prefixes;
using TRAEProject.Common;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Armor.Joter
{

    // JOTTIES
    // fix this on enemies with high defense
    class JoterTrident : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Feed the Joter");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            // DisplayName.SetDefault("Joter Trident");
        }
        // STATS IN SpearItems.cs
    }
    class JoterTridentSpear : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 99f;
            stabStart = 79;
            stabEnd = -10;
            swingAmount = MathF.PI / 32;

        }
        public override void SpearHitNPCMelee(NPC target, NPC.HitInfo hit) // if more on kills are ever added, make this a method in ProjectileStats
        {
            Player player = Main.player[Projectile.owner];

            if (target.life <= 0) // this hook is run directly after the damage has been dealt, so you can just make it 0
            {
                JoterEat(player, target, hit.Damage);
            }
        }
        public void JoterEat(Player player, NPC target, int damage)
        {
            int duration = target.lifeMax;
            if (duration > 60 * 60)
                duration = 3600;
            player.AddBuff(BuffID.HeartyMeal, duration);
            if (target.type == NPCID.MoonLordHead)
                player.QuickSpawnItem(target.GetSource_FromThis(), ItemType<FinalBoss>());
            SoundEngine.PlaySound(SoundID.Item2 with { MaxInstances = 0 }, Projectile.position);
            int JottieCount = target.lifeMax / 50;
            if (JottieCount > 25)
                JottieCount = 25;
            for (int i = 0; i < JottieCount; ++i)
            {
                float velX = Main.rand.Next(-35, 36) * 0.1f;
                float velY = Main.rand.Next(-35, 36) * 0.1f;
                Projectile.NewProjectile(player.GetSource_FromAI(), player.Center.X, player.Center.Y, velX, velY, ProjectileType<Jottie>(), damage, 0f, Main.myPlayer, 0f, 0f);
            }
        }
    }
    public class JoterTridentThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 99f;
            holdAt = 45f;
            maxSticks = 3;
            stickingDps = 0;
            DustOnDeath = DustID.t_Flesh;
        }
        int targetlife = 0;

        public override void SpearHitNPC(bool atMaxCharge, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            if (target.life <= 0) // this hook is run directly after the damage has been dealt, so you can just make it 0
            {
                int duration = target.lifeMax;
                if (duration > 60 * 60)
                    duration = 3600;
                player.AddBuff(BuffID.HeartyMeal, duration);
                if (target.type == NPCID.MoonLordHead)
                    player.QuickSpawnItem(target.GetSource_FromThis(), ItemType<FinalBoss>());
                SoundEngine.PlaySound(SoundID.Item2 with { MaxInstances = 0 }, Projectile.position);
                int JottieCount = target.lifeMax / 50;
                if (JottieCount > 25)
                    JottieCount = 25;
                for (int i = 0; i < JottieCount; ++i)
                {
                    float velX = Main.rand.Next(-35, 36) * 0.1f;
                    float velY = Main.rand.Next(-35, 36) * 0.1f;
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center.X, player.Center.Y, velX, velY, ProjectileType<Jottie>(), damageDone, 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath1 with { MaxInstances = 0 }, Projectile.position);
            Vector2 usePos = Projectile.position; // Position to use for dusts

            // Please note the usage of MathHelper, please use this!
            // We subtract 90 degrees as radians to the rotation vector to offset the sprite as its default rotation in the sprite isn't aligned properly.
            Vector2 rotVector = (Projectile.rotation + MathHelper.ToRadians(135f * Projectile.direction)).ToRotationVector2(); // rotation vector to use for dust velocity
            usePos += rotVector * 16f;

            // Declaring a constant in-line is fine as it will be optimized by the compiler
            // It is however recommended to define it outside method scope if used elswhere as well
            // They are useful to make numbers that don't change more descriptive


            // Spawn some dusts upon javelin death
            for (int i = 0; i < 20; i++)
            {
                // Create a new dust
                Dust dust = Dust.NewDustDirect(usePos, Projectile.width, Projectile.height, DustOnDeath);
                dust.position = (dust.position + Projectile.Center) / 2f;
                dust.velocity += rotVector * 2f;
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                usePos -= rotVector * 8f;
            }
        }
    }
    public class Jottie : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.TinyEater);
            AIType = ProjectileID.TinyEater;
            Projectile.width = 26;
            Projectile.height = 52;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            if (target.life <= 0) // this hook is run directly after the damage has been dealt, so you can just make it 0
            {
                int duration = target.lifeMax;
                if (duration > 60 * 60)
                    duration = 3600;
                player.AddBuff(BuffID.HeartyMeal, duration);
                if (target.type == NPCID.MoonLordHead)
                    player.QuickSpawnItem(target.GetSource_FromThis(), ItemType<FinalBoss>());
                SoundEngine.PlaySound(SoundID.Item2 with { MaxInstances = 0 }, Projectile.position);
                int JottieCount = target.lifeMax / 100;
                if (JottieCount > 25)
                    JottieCount = 25;
                for (int i = 0; i < JottieCount; ++i)
                {
                    float velX = Main.rand.Next(-35, 36) * 0.1f;
                    float velY = Main.rand.Next(-35, 36) * 0.1f;
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center.X, player.Center.Y, velX, velY, ProjectileType<Jottie>(), damageDone, 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }
    }
}
